using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    public partial class AddedMessageToJobRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "CF_Repo_JobRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "CF_Repo_JobRecords");
        }
    }
}
