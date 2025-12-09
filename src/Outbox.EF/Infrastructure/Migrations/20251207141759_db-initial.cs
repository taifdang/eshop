using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Outbox.EF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dbinitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PollingOutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayloadType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false)
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
