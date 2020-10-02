using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class UpdatedUserGroupRoleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catfish_UserGroupRoles_Catfish_Groups_GroupId",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.DropIndex(
                name: "IX_Catfish_UserGroupRoles_GroupId",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupRoleId",
                table: "Catfish_UserGroupRoles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_UserGroupRoles_GroupRoleId",
                table: "Catfish_UserGroupRoles",
                column: "GroupRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catfish_UserGroupRoles_Catfish_GroupRoles_GroupRoleId",
                table: "Catfish_UserGroupRoles",
                column: "GroupRoleId",
                principalTable: "Catfish_GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catfish_UserGroupRoles_Catfish_GroupRoles_GroupRoleId",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.DropIndex(
                name: "IX_Catfish_UserGroupRoles_GroupRoleId",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.DropColumn(
                name: "GroupRoleId",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Catfish_UserGroupRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Catfish_UserGroupRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_UserGroupRoles_GroupId",
                table: "Catfish_UserGroupRoles",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catfish_UserGroupRoles_Catfish_Groups_GroupId",
                table: "Catfish_UserGroupRoles",
                column: "GroupId",
                principalTable: "Catfish_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
