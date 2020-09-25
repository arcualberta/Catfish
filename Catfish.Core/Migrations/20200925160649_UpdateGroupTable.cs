using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class UpdateGroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Catfish_Groups");

            migrationBuilder.AddColumn<int>(
                name: "GroupStatus",
                table: "Catfish_Groups",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupStatus",
                table: "Catfish_Groups");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Catfish_Groups",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
