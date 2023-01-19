using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class AddAssociationEntityTemplateAndWorkflowDbRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EntityTemplateId",
                table: "CF_Repo_Workflows",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CF_Repo_Workflows_EntityTemplateId",
                table: "CF_Repo_Workflows",
                column: "EntityTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CF_Repo_Workflows_CF_Repo_EntityTemplates_EntityTemplateId",
                table: "CF_Repo_Workflows",
                column: "EntityTemplateId",
                principalTable: "CF_Repo_EntityTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CF_Repo_Workflows_CF_Repo_EntityTemplates_EntityTemplateId",
                table: "CF_Repo_Workflows");

            migrationBuilder.DropIndex(
                name: "IX_CF_Repo_Workflows_EntityTemplateId",
                table: "CF_Repo_Workflows");

            migrationBuilder.DropColumn(
                name: "EntityTemplateId",
                table: "CF_Repo_Workflows");
        }
    }
}
