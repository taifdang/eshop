FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["Api/Api.csproj", "Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["ServiceDefaults/ServiceDefaults.csproj", "ServiceDefaults/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["EventBus.InMemory/EventBus.InMemory.csproj", "EventBus.InMemory/"]
COPY ["Outbox/Outbox.csproj", "Outbox/"]
COPY ["Outbox.EF/Outbox.EF.csproj", "Outbox.EF/"]

RUN dotnet restore "Api/Api.csproj"

# copy everything else and build
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS http://+:80
ENV ASPNETCORE_ENVIRONMENT Docker

EXPOSE 80

ENTRYPOINT ["dotnet", "Api.dll"]
