using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddImagemUrlToCarros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemUrl",
                table: "Carros",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemUrl",
                table: "Carros");
        }
    }
}
