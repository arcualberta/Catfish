using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class UpdateRepoDbContextWorkflowToWorkfloDbRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CF_Repo_Triggers_CF_Repo_Workflows_WorkflowId",
                table: "CF_Repo_Triggers");

            migrationBuilder.DropIndex(
                name: "IX_CF_Repo_Triggers_WorkflowId",
                table: "CF_Repo_Triggers");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "CF_Repo_Triggers");

            migrationBuilder.AddColumn<string>(
                name: "SerializedWorkflow",
                table: "CF_Repo_Workflows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedWorkflow",
                table: "CF_Repo_Workflows");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowId",
                table: "CF_Repo_Triggers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CF_Repo_Triggers_WorkflowId",
                table: "CF_Repo_Triggers",
                column: "WorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_CF_Repo_Triggers_CF_Repo_Workflows_WorkflowId",
                table: "CF_Repo_Triggers",
                column: "WorkflowId",
                principalTable: "CF_Repo_Workflows",
                principalColumn: "Id");
        }
    }
}
