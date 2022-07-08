using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class IsEditableColumnRemovedFromSystemStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "Catfish_SystemStatuses");

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_Entities_TemplateId",
                table: "Catfish_Entities",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catfish_Entities_Catfish_Entities_TemplateId",
                table: "Catfish_Entities",
                column: "TemplateId",
                principalTable: "Catfish_Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catfish_Entities_Catfish_Entities_TemplateId",
                table: "Catfish_Entities");

            migrationBuilder.DropIndex(
                name: "IX_Catfish_Entities_TemplateId",
                table: "Catfish_Entities");

            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "Catfish_SystemStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
