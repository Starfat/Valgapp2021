using Microsoft.EntityFrameworkCore.Migrations;

namespace Valgapplikasjon.Migrations.KandidaturDb
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kandidat",
                columns: table => new
                {
                    KandidatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrukerId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Sjekkboks = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kandidat", x => x.KandidatId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kandidat");
        }
    }
}
