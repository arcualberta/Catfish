using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class AddedUserEmailToEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Catfish_Entities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Catfish_Entities");
        }
    }
}
