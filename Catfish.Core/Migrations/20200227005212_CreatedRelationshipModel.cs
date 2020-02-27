using Microsoft.EntityFrameworkCore.Migrations;

namespace Catfish.Core.Migrations
{
    public partial class CreatedRelationshipModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    SubjectId = table.Column<int>(nullable: false),
                    ObjctId = table.Column<int>(nullable: false),
                    Predicate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => new { x.SubjectId, x.ObjctId });
                    table.ForeignKey(
                        name: "FK_Relationships_Catfish_XmlModel_ObjctId",
                        column: x => x.ObjctId,
                        principalTable: "Catfish_XmlModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Relationships_Catfish_XmlModel_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Catfish_XmlModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_ObjctId",
                table: "Relationships",
                column: "ObjctId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relationships");
        }
    }
}
