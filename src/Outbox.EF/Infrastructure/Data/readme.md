add-migration db-initial -Context OutboxDbContext -Project Outbox.EF -StartupProject Api  -o Infrastructure\Migrations

update-database -Context OutboxDbContext
