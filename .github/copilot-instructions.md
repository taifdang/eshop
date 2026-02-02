# Eshop Codebase Instructions

## Architecture Overview

This is a .NET 9 e-commerce application using **Clean Architecture** with CQRS pattern and Domain-Driven Design.

### Layer Structure
- **Domain** (`src/Domain`): Core entities, value objects, domain events. No external dependencies.
- **Application** (`src/Application`): Commands/queries with MediatR handlers, interfaces, DTOs, validators
- **Infrastructure** (`src/Infrastructure`): External integrations (payments, email, storage), EventBus configuration
- **Persistence** (`src/Persistence`): EF Core DbContext, entity configurations, interceptors, repositories
- **Outbox.EF** (`src/Outbox.EF`): Outbox pattern implementation with polling service
- **EventBus** + **EventBus.RabbitMQ** (`src/EventBus*`): Event bus abstraction and RabbitMQ implementation
- **Api** (`src/Api`): Minimal APIs organized in `Endpoints/` folder (e.g., `CatalogApi.cs`, `OrderApi.cs`)
- **AppHost** (`src/AppHost`): .NET Aspire orchestration for local development

### Dependency Flow
Dependencies point inward: Api → Application → Domain ← Infrastructure ← Persistence

## Key Patterns & Practices

### 1. CQRS with MediatR
All business logic flows through MediatR requests:
- **Commands**: `*Command.cs` + `*CommandHandler.cs` (e.g., `CreateOrderCommand`)
- **Queries**: `*Query.cs` + `*QueryHandler.cs` (e.g., `GetOrderByIdQuery`)
- **Validators**: `*CommandValidator.cs` using FluentValidation (not queries)
- Place in feature folders: `Application/{Feature}/{Commands|Queries}/{Name}/`

**Pipeline behaviors** (executed in order):
1. `ActivityBehavior` - Metrics & distributed tracing via OpenTelemetry
2. `LoggingBehavior` - Request/response logging
3. `ValidationBehavior` - FluentValidation (auto-discovered via reflection)
4. `TxBehavior` - Database transactions ONLY for commands (not queries) with EF retry strategy

### 2. Domain-Driven Design
- **Aggregates** inherit from `Aggregate<T>` (see `Domain/Entities/Order.cs`)
- **Domain Events** track state changes (e.g., `OrderCreatedDomainEvent`)
  - Raised by calling `aggregate.AddDomainEvent(event)` within aggregate methods
  - Automatically dispatched by `DispatchDomainEventInterceptor` after SaveChanges
  - Handled by `INotificationHandler<TDomainEvent>` in `Application/{Feature}/DomainEventHandlers/`
- **Value Objects** for type safety (e.g., `Money`, `Address` in `Domain/ValueObject/`)
- Aggregates encapsulate business rules; use factory methods like `Order.Create()`
- **Interceptors**: 
  - `AuditableEntityInterceptor` - Auto-sets CreatedDate, UpdatedDate, CreatedBy, UpdatedBy
  - `DispatchDomainEventInterceptor` - Publishes domain events to MediatR after SaveChanges

**Domain Event → Integration Event Flow**:
1. Aggregate raises domain event (e.g., `OrderCreatedDomainEvent`)
2. `DispatchDomainEventInterceptor` publishes to MediatR after SaveChanges
3. Domain event handler converts to integration event and saves to Outbox table
4. `TransactionalOutboxPollingService` publishes to EventBus (RabbitMQ) every 5s

### 3. Outbox Pattern for Reliability
Events aren't published directly—they're saved to `PollingOutboxMessage` table:
```csharp
var message = new PollingOutboxMessage {
    PayloadType = typeof(OrderCreatedIntegrationEvent).FullName,
    Payload = JsonSerializer.Serialize(integrationEvent),
    CreateDate = DateTime.UtcNow
};
await outboxRepository.AddAsync(message);
await outboxRepository.SaveChangesAsync();
```
`TransactionalOutboxPollingService` background service polls every 5s and publishes to EventBus.
- Ensures at-least-once delivery and no message loss
- Retry mechanism with max retry count
- Separate `OutboxDbContext` for outbox messages (migrated alongside `ApplicationDbContext`)

### 4. Payment Gateway Factory Pattern
Multiple payment providers (Vnpay, Stripe) implement `IPaymentGateway`:
```csharp
var gateway = _factory.Resolve(request.Provider);  // Factory resolves by enum
var result = await gateway.CreatePaymentUrl(request);
```
- Add new providers in `Infrastructure/ExternalServices/Payments/`
- Register in `Infrastructure/DependencyInjection.cs`
- Update `PaymentGatewayFactory.Resolve()` switch statement

### 5. Observability
Commands/queries automatically tracked with:
- **Metrics**: `CommandHandlerMetrics`/`QueryHandlerMetrics` (counters, histograms)
- **Tracing**: `CommandHandlerActivity`/`QueryHandlerActivity` (OpenTelemetry spans)
- Tags include handler name/type discovered via reflection
- Registered in `ApplicationServiceExtensions.cs` as singletons
- Default Aspire observability includes distributed tracing, logs aggregation, and metrics dashboards

## Development Workflows

### Build & Run
```powershell
# Build entire solution
dotnet build

# Run API only (port 6005)
dotnet run --project src/Api/Api.csproj

# Run with Aspire (recommended - orchestrates all services)
aspire run --project ./eshop.sln

# Docker Compose (full stack with postgres, rabbitmq, nginx)
docker-compose -f infra/docker-compose/docker-compose.dev.yml up --build
```

### Aspire Development
Aspire orchestrates the entire stack locally:
- **AppHost** (`src/AppHost/Program.cs`) defines service dependencies
- Services: API, IdentityService, Postgres, RabbitMQ, React frontend (Vite)
- Auto-configured connections and service discovery
- Built-in dashboard for monitoring services at `http://localhost:15888`

### Testing
```powershell
dotnet test tests/UnitTests/UnitTests.csproj
dotnet test tests/IntegrationTests/IntegrationTests.csproj
```
- **Unit tests**: Use xUnit + Moq for mocking
- **Integration tests**: Use `Aspire.Hosting.Testing` for full stack testing

### Database Migrations
Migrations run automatically on startup via `MigrateAndSeedDataAsync()` in `Program.cs`.
Both `ApplicationDbContext` and `OutboxDbContext` are migrated.

To create new migrations:
```powershell
dotnet ef migrations add MigrationName -p src/Persistence
```

## Common Tasks

### Adding New Command/Query
1. Create folder: `Application/{Feature}/{Commands|Queries}/{Name}/`
2. Define request: `public record MyCommand(...) : IRequest<MyResult>;`
3. Create handler: `public class MyCommandHandler : IRequestHandler<MyCommand, MyResult>`
4. (Optional) Add validator: `public class MyCommandValidator : AbstractValidator<MyCommand>`
   - Validators only for commands, not queries
   - Auto-discovered by FluentValidation via reflection
   - Run in `ValidationBehavior` pipeline before handler execution

### Adding New Entity
1. Create in `Domain/Entities/`, inherit `Aggregate<TId>` or `Entity<TId>`
2. Add `DbSet<Entity>` to `ApplicationDbContext` in `Persistence/`
3. Create configuration in `Persistence/Configurations/` implementing `IEntityTypeConfiguration<T>`
4. Generate migration: `dotnet ef migrations add AddEntity -p src/Persistence`

### Adding Minimal API Endpoint
Add methods to existing `*Api.cs` files in `src/Api/Endpoints/`:
```csharp
group.MapPost("/", async (ISender sender, MyCommand cmd) => 
    await sender.Send(cmd))
    .RequireAuthorization()
    .WithName("CreateItem")
    .Produces<MyResult>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest);
```
Map in `Program.cs`: `app.MapMyApi();`

### Integration Events
For cross-boundary communication:
1. Define event in `Contracts/IntegrationEvents/`
2. Create handler in `Application/{Feature}/IntegrationEventHandlers/`
3. Register subscription in `Infrastructure/DependencyInjection.cs`:
   ```csharp
   .AddSubscription<MyEvent, MyEventHandler>()
   ```

## Configuration

- **Development**: `src/Api/appsettings.Development.json`
- **Docker**: `src/Api/appsettings.Docker.json`
- Connection strings use Aspire format: `builder.AddNpgsqlDbContext<AppDbContext>("shopdb")`
- Payment configs in `VnpayConf`/`StripeConf` sections
- Test environment uses `NullEventPublisher` to skip RabbitMQ during testing

## Important Conventions

- **Naming**: Commands end with `Command`, queries with `Query`, handlers with `Handler`, validators with `Validator`
- **Transaction scope**: Only commands wrapped in transactions (queries are read-only)
- **Async all the way**: Use `async/await` consistently
- **Dependency Injection**: Register services via extension methods in `*DependencyInjection.cs` or `Extensions.cs`
- **Validation**: Use FluentValidation, not data annotations
- **Event sourcing**: Domain events for internal state changes, integration events for external communication
- **Feature folders**: Group by feature (Order, Catalog, etc.) not by layer (Commands, Queries)

## Frontend

React + Vite app in `src/Web/ClientApp/` communicates with backend via HTTP APIs.
