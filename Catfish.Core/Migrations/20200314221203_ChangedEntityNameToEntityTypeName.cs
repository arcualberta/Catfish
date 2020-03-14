using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class ChangedEntityNameToEntityTypeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Catfish_Entities");

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "Catfish_Entities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "Catfish_Entities");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Catfish_Entities",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
