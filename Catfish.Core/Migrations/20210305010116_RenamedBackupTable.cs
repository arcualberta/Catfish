using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class RenamedBackupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Backups",
                table: "Backups");

            migrationBuilder.RenameTable(
                name: "Backups",
                newName: "Catfish_Backup");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catfish_Backup",
                table: "Catfish_Backup",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Catfish_Backup",
                table: "Catfish_Backup");

            migrationBuilder.RenameTable(
                name: "Catfish_Backup",
                newName: "Backups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Backups",
                table: "Backups",
                column: "Id");
        }
    }
}
