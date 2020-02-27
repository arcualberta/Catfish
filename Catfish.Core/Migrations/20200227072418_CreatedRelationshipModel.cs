using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class CreatedRelationshipModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catfish_Relationships",
                columns: table => new
                {
                    SubjectId = table.Column<int>(nullable: false),
                    ObjctId = table.Column<int>(nullable: false),
                    Predicate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catfish_Relationships", x => new { x.SubjectId, x.ObjctId });
                    table.ForeignKey(
                        name: "FK_Catfish_Relationships_Catfish_XmlModels_ObjctId",
                        column: x => x.ObjctId,
                        principalTable: "Catfish_XmlModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Catfish_Relationships_Catfish_XmlModels_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Catfish_XmlModels",
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
        }
    }
}
