using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class AddNormalizedStatusAndEntityTemplateIdToSystemStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "Catfish_SystemStatuses");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityTemplateId",
                table: "Catfish_SystemStatuses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "NormalizedStatus",
                table: "Catfish_SystemStatuses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityTemplateId",
                table: "Catfish_SystemStatuses");

            migrationBuilder.DropColumn(
                name: "NormalizedStatus",
                table: "Catfish_SystemStatuses");

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "Catfish_SystemStatuses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
