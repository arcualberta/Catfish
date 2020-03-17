using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class CreateBasicDataModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catfish_Entities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(type: "xml", nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_Entities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Catfish_Relationships",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(nullable: false),
                    ObjctId = table.Column<Guid>(nullable: false),
                    Predicate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_Relationships", x => new { x.SubjectId, x.ObjctId });
                    table.ForeignKey(
                        name: "FK_Catfish_Relationships_Catfish_Entities_ObjctId",
                        column: x => x.ObjctId,
                        principalTable: "Catfish_Entities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Catfish_Relationships_Catfish_Entities_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Catfish_Entities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catfish_Relationships_ObjctId",
                table: "Catfish_Relationships",
                column: "ObjctId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catfish_Relationships");

            migrationBuilder.DropTable(
                name: "Catfish_Entities");
        }
    }
}
