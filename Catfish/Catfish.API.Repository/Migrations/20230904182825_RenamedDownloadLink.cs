using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class RenamedDownloadLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DownloadLink",
                table: "CF_Repo_JobRecords",
                newName: "DownloadDataFileLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DownloadDataFileLink",
                table: "CF_Repo_JobRecords",
                newName: "DownloadLink");
        }
    }
}
