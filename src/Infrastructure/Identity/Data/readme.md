add-migration Initial_Identity -Context AppIdentityDbContext -Project Infrastructure -StartupProject Api  -o Identity\Data\Migrations

update-database -Context AppIdentityDbContext