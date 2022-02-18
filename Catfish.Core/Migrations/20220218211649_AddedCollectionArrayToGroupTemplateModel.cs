using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class AddedCollectionArrayToGroupTemplateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupTemplateId",
                table: "Catfish_Entities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_Entities_GroupTemplateId",
                table: "Catfish_Entities",
                column: "GroupTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catfish_Entities_Catfish_GroupTemplates_GroupTemplateId",
                table: "Catfish_Entities",
                column: "GroupTemplateId",
                principalTable: "Catfish_GroupTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catfish_Entities_Catfish_GroupTemplates_GroupTemplateId",
                table: "Catfish_Entities");

            migrationBuilder.DropIndex(
                name: "IX_Catfish_Entities_GroupTemplateId",
                table: "Catfish_Entities");

            migrationBuilder.DropColumn(
                name: "GroupTemplateId",
                table: "Catfish_Entities");
        }
    }
}
