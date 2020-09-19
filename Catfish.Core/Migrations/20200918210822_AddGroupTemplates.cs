using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class AddGroupTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catfish_Groups_Catfish_Entities_EntityTemplateId",
                table: "Catfish_Groups");

            migrationBuilder.DropIndex(
                name: "IX_Catfish_Groups_EntityTemplateId",
                table: "Catfish_Groups");

            migrationBuilder.DropColumn(
                name: "EntityTemplateId",
                table: "Catfish_Groups");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Catfish_Groups",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Catfish_GroupTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    EntityTemplateId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_GroupTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catfish_GroupTemplates_Catfish_Entities_EntityTemplateId",
                        column: x => x.EntityTemplateId,
                        principalTable: "Catfish_Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Catfish_GroupTemplates_Catfish_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Catfish_Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_GroupTemplates_EntityTemplateId",
                table: "Catfish_GroupTemplates",
                column: "EntityTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_GroupTemplates_GroupId",
                table: "Catfish_GroupTemplates",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catfish_GroupTemplates");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Catfish_Groups");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityTemplateId",
                table: "Catfish_Groups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_Groups_EntityTemplateId",
                table: "Catfish_Groups",
                column: "EntityTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catfish_Groups_Catfish_Entities_EntityTemplateId",
                table: "Catfish_Groups",
                column: "EntityTemplateId",
                principalTable: "Catfish_Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
