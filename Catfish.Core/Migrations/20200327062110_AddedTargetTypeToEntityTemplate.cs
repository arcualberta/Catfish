using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class AddedTargetTypeToEntityTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "Catfish_Entities");

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                table: "Catfish_Entities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "Catfish_Entities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "Catfish_Entities");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "Catfish_Entities");

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "Catfish_Entities",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
