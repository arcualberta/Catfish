using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class AddTriggerAndWorkflowClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CF_Repo_Workflows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CF_Repo_Workflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CF_Repo_Triggers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eTriggerType = table.Column<int>(type: "int", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CF_Repo_Triggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CF_Repo_Triggers_CF_Repo_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "CF_Repo_Workflows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CF_Repo_Triggers_WorkflowId",
                table: "CF_Repo_Triggers",
                column: "WorkflowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CF_Repo_Triggers");

            migrationBuilder.DropTable(
                name: "CF_Repo_Workflows");
        }
    }
}
