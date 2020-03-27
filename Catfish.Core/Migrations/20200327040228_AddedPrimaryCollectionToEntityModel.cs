using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class AddedPrimaryCollectionToEntityModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PrimaryCollectionId",
                table: "Catfish_Entities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_Entities_PrimaryCollectionId",
                table: "Catfish_Entities",
                column: "PrimaryCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catfish_Entities_Catfish_Entities_PrimaryCollectionId",
                table: "Catfish_Entities",
                column: "PrimaryCollectionId",
                principalTable: "Catfish_Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catfish_Entities_Catfish_Entities_PrimaryCollectionId",
                table: "Catfish_Entities");

            migrationBuilder.DropIndex(
                name: "IX_Catfish_Entities_PrimaryCollectionId",
                table: "Catfish_Entities");

            migrationBuilder.DropColumn(
                name: "PrimaryCollectionId",
                table: "Catfish_Entities");
        }
    }
}
