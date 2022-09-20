using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class UpdatedEntityTemplateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CF_Repo_EntityTemplates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SerializedEntityTemplateSettings",
                table: "CF_Repo_EntityTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "CF_Repo_EntityTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "CF_Repo_EntityTemplates",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "CF_Repo_EntityTemplates");

            migrationBuilder.DropColumn(
                name: "SerializedEntityTemplateSettings",
                table: "CF_Repo_EntityTemplates");

            migrationBuilder.DropColumn(
                name: "State",
                table: "CF_Repo_EntityTemplates");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "CF_Repo_EntityTemplates");
        }
    }
}
