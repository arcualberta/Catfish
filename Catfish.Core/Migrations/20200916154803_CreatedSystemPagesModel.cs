using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class CreatedSystemPagesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catfish_XmlModels");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Catfish_Entities");

            migrationBuilder.CreateTable(
                name: "Catfish_SystemPages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<Guid>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false),
                    PageKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_SystemPages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catfish_SystemPages");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Catfish_Entities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Catfish_XmlModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "xml", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_XmlModels", x => x.Id);
                });
        }
    }
}
