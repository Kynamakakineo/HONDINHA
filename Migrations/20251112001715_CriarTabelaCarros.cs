using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaCarros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Configura o charset padrão do banco para utf8mb4 (suporta acentos e emojis)
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            // Criação da tabela "Carros"
            migrationBuilder.CreateTable(
                name: "Carros",
                columns: table => new
                {
                    // Coluna ID (chave primária), preenchida automaticamente (auto incremento)
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),

                    // Coluna Marca (string, até 50 caracteres, não aceita nulo)
                    Marca = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),

                    // Coluna Modelo (string, até 50 caracteres)
                    Modelo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),

                    // Coluna Ano (inteiro)
                    Ano = table.Column<int>(type: "int", nullable: false),

                    // Coluna Cor (texto longo)
                    Cor = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),

                    // Coluna Preço (valor decimal)
                    Preco = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    // Define o ID como chave primária da tabela
                    table.PrimaryKey("PK_Carros", x => x.Id);
                })
                // Define o charset da tabela como utf8mb4
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Caso a migration seja revertida, exclui a tabela Carros
            migrationBuilder.DropTable(
                name: "Carros");
        }
    }
}
