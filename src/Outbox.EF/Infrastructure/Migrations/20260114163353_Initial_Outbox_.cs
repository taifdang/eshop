using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Outbox.EF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Outbox_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PollingOutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    PayloadType = table.Column<string>(type: "text", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollingOutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollingOutboxMessages_CreateDate_RetryCount_ProcessedDate",
                table: "PollingOutboxMessages",
                columns: new[] { "CreateDate", "RetryCount", "ProcessedDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PollingOutboxMessages");
        }
    }
}
