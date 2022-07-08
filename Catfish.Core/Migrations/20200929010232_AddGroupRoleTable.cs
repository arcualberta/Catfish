using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class AddGroupRoleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catfish_GroupRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_GroupRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catfish_GroupRoles_Catfish_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Catfish_Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_GroupRoles_GroupId",
                table: "Catfish_GroupRoles",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catfish_GroupRoles");
        }
    }
}
