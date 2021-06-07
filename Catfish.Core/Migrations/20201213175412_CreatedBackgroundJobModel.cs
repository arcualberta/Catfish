using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class CreatedBackgroundJobModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catfish_BackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    HangfireJobId = table.Column<long>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: true),
                    SourceTaskId = table.Column<Guid>(nullable: true),
                    Task = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_BackgroundJobs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catfish_BackgroundJobs");
        }
    }
}
