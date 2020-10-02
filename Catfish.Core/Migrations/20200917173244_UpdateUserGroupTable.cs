using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class UpdateUserGroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catfish_UserGroups_Catfish_Groups_GroupId",
                table: "Catfish_UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Catfish_UserGroups",
                table: "Catfish_UserGroups");

            migrationBuilder.RenameTable(
                name: "Catfish_UserGroups",
                newName: "Catfish_UserGroupRoles");

            migrationBuilder.RenameIndex(
                name: "IX_Catfish_UserGroups_GroupId",
                table: "Catfish_UserGroupRoles",
                newName: "IX_Catfish_UserGroupRoles_GroupId");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Catfish_UserGroupRoles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catfish_UserGroupRoles",
                table: "Catfish_UserGroupRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Catfish_UserGroupRoles_Catfish_Groups_GroupId",
                table: "Catfish_UserGroupRoles",
                column: "GroupId",
                principalTable: "Catfish_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catfish_UserGroupRoles_Catfish_Groups_GroupId",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Catfish_UserGroupRoles",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Catfish_UserGroupRoles");

            migrationBuilder.RenameTable(
                name: "Catfish_UserGroupRoles",
                newName: "Catfish_UserGroups");

            migrationBuilder.RenameIndex(
                name: "IX_Catfish_UserGroupRoles_GroupId",
                table: "Catfish_UserGroups",
                newName: "IX_Catfish_UserGroups_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catfish_UserGroups",
                table: "Catfish_UserGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Catfish_UserGroups_Catfish_Groups_GroupId",
                table: "Catfish_UserGroups",
                column: "GroupId",
                principalTable: "Catfish_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
