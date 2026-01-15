add-migration Initial_ShopDB -Context ApplicationDbContext -Project Infrastructure -StartupProject Api  -o Data\Migrations

update-database -Context ApplicationDbContext

add-migration add-payment-in-order-tbl -Context ApplicationDbContext -Project Infrastructure -StartupProject Api  -o Data\Migrations
