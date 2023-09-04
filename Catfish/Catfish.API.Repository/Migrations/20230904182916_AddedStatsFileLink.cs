using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class AddedStatsFileLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DownloadStatsFileLink",
                table: "CF_Repo_JobRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadStatsFileLink",
                table: "CF_Repo_JobRecords");
        }
    }
}
