using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class CreatedIndexingHitory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Catfish_Entities",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Catfish_IndexingHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ObjectId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastIndexedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_IndexingHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catfish_IndexingHistory");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Catfish_Entities");
        }
    }
}
