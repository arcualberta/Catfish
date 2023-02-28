using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class RemovedTriggersFromDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CF_Repo_Recipient");

            migrationBuilder.DropTable(
                name: "CF_Repo_Triggers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CF_Repo_Triggers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CF_Repo_Triggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CF_Repo_Recipient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailType = table.Column<int>(type: "int", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MetadataFeildId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MetadataFormId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkflowTriggerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CF_Repo_Recipient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CF_Repo_Recipient_CF_Repo_Triggers_WorkflowTriggerId",
                        column: x => x.WorkflowTriggerId,
                        principalTable: "CF_Repo_Triggers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CF_Repo_Recipient_WorkflowTriggerId",
                table: "CF_Repo_Recipient",
                column: "WorkflowTriggerId");
        }
    }
}
