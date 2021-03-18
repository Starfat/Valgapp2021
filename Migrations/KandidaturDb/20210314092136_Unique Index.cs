using Microsoft.EntityFrameworkCore.Migrations;

namespace Valgapplikasjon.Migrations.KandidaturDb
{
    public partial class UniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Kandidat_BrukerId",
                table: "Kandidat",
                column: "BrukerId",
                unique: true,
                filter: "[BrukerId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Kandidat_BrukerId",
                table: "Kandidat");
        }
    }
}
