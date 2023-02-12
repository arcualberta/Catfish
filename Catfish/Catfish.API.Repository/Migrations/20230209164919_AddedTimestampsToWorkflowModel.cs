using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class AddedTimestampsToWorkflowModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "eTriggerType",
                table: "CF_Repo_Triggers",
                newName: "Type");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CF_Repo_Workflows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "CF_Repo_Workflows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateId",
                table: "CF_Repo_Triggers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CF_Repo_Recipient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailType = table.Column<int>(type: "int", nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MetadataFormId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MetadataFeildId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CF_Repo_Recipient");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "CF_Repo_Workflows");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "CF_Repo_Workflows");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "CF_Repo_Triggers");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "CF_Repo_Triggers",
                newName: "eTriggerType");
        }
    }
}
