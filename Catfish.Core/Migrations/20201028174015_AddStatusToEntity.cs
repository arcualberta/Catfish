using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class AddStatusToEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Catfish_Entities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_Entities_StatusId",
                table: "Catfish_Entities",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catfish_Entities_Catfish_SystemStatuses_StatusId",
                table: "Catfish_Entities",
                column: "StatusId",
                principalTable: "Catfish_SystemStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catfish_Entities_Catfish_SystemStatuses_StatusId",
                table: "Catfish_Entities");

            migrationBuilder.DropIndex(
                name: "IX_Catfish_Entities_StatusId",
                table: "Catfish_Entities");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Catfish_Entities");
        }
    }
}
