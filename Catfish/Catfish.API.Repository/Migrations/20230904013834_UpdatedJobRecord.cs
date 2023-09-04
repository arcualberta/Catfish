using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class UpdatedJobRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalIterations",
                table: "CF_Repo_JobRecords",
                newName: "ProcessedDataRows");

            migrationBuilder.RenameColumn(
                name: "CompletedIterations",
                table: "CF_Repo_JobRecords",
                newName: "ExpectedDataRows");

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "CF_Repo_JobRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "CF_Repo_JobRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Started",
                table: "CF_Repo_JobRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "CF_Repo_JobRecords");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "CF_Repo_JobRecords");

            migrationBuilder.DropColumn(
                name: "Started",
                table: "CF_Repo_JobRecords");

            migrationBuilder.RenameColumn(
                name: "ProcessedDataRows",
                table: "CF_Repo_JobRecords",
                newName: "TotalIterations");

            migrationBuilder.RenameColumn(
                name: "ExpectedDataRows",
                table: "CF_Repo_JobRecords",
                newName: "CompletedIterations");
        }
    }
}
