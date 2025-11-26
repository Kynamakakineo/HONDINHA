using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddConcessionariaToCarro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConcessionariaId",
                table: "Carros",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carros_ConcessionariaId",
                table: "Carros",
                column: "ConcessionariaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carros_Concessionarias_ConcessionariaId",
                table: "Carros",
                column: "ConcessionariaId",
                principalTable: "Concessionarias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carros_Concessionarias_ConcessionariaId",
                table: "Carros");

            migrationBuilder.DropIndex(
                name: "IX_Carros_ConcessionariaId",
                table: "Carros");

            migrationBuilder.DropColumn(
                name: "ConcessionariaId",
                table: "Carros");
        }
    }
}
