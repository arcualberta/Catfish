using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class CreatedUserGrouping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catfish_Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityTemplateId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catfish_Groups_Catfish_Entities_EntityTemplateId",
                        column: x => x.EntityTemplateId,
                        principalTable: "Catfish_Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Catfish_UserGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_UserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catfish_UserGroups_Catfish_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Catfish_Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_Groups_EntityTemplateId",
                table: "Catfish_Groups",
                column: "EntityTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_UserGroups_GroupId",
                table: "Catfish_UserGroups",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catfish_UserGroups");

            migrationBuilder.DropTable(
                name: "Catfish_Groups");
        }
    }
}
