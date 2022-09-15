using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class CreatedEntityModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CF_Repo_EntityTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CF_Repo_EntityTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CF_Repo_Entities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    SerializedData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CF_Repo_Entities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CF_Repo_Entities_CF_Repo_EntityTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "CF_Repo_EntityTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CF_Repo_EntityTemplateForm",
                columns: table => new
                {
                    EntityTemplatesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CF_Repo_EntityTemplateForm", x => new { x.EntityTemplatesId, x.FormsId });
                    table.ForeignKey(
                        name: "FK_CF_Repo_EntityTemplateForm_CF_Repo_EntityTemplates_EntityTemplatesId",
                        column: x => x.EntityTemplatesId,
                        principalTable: "CF_Repo_EntityTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CF_Repo_EntityTemplateForm_CF_Repo_Forms_FormsId",
                        column: x => x.FormsId,
                        principalTable: "CF_Repo_Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CF_Repo_Relationships",
                columns: table => new
                {
                    SubjectEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObjectEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CF_Repo_Relationships", x => x.SubjectEntityId);
                    table.ForeignKey(
                        name: "FK_CF_Repo_Relationships_CF_Repo_Entities_ObjectEntityId",
                        column: x => x.ObjectEntityId,
                        principalTable: "CF_Repo_Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CF_Repo_Relationships_CF_Repo_Entities_SubjectEntityId",
                        column: x => x.SubjectEntityId,
                        principalTable: "CF_Repo_Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CF_Repo_Entities_TemplateId",
                table: "CF_Repo_Entities",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CF_Repo_EntityTemplateForm_FormsId",
                table: "CF_Repo_EntityTemplateForm",
                column: "FormsId");

            migrationBuilder.CreateIndex(
                name: "IX_CF_Repo_Relationships_ObjectEntityId",
                table: "CF_Repo_Relationships",
                column: "ObjectEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CF_Repo_EntityTemplateForm");

            migrationBuilder.DropTable(
                name: "CF_Repo_Relationships");

            migrationBuilder.DropTable(
                name: "CF_Repo_Entities");

            migrationBuilder.DropTable(
                name: "CF_Repo_EntityTemplates");
        }
    }
}
