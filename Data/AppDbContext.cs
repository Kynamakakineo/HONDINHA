using AutoManagerApi.Models; // Importar seus modelos!
using Microsoft.EntityFrameworkCore;

namespace AutoManagerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        // Mapeia o modelo Concessionaria para a tabela "Concessionarias"
        public DbSet<Concessionaria> Concessionarias { get; set; } 
        
        // Mapeia o modelo Carro para a tabela "Carros"
        public DbSet<Carro> Carros { get; set; } 

        // --- FIM DA ADIÇÃO ---


        // Esta função configura os relacionamentos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Boa prática chamar o base

            // Configurar a relação 1-para-N (Uma Concessionaria -> Muitos Carros)
            modelBuilder.Entity<Concessionaria>()
                .HasMany(c => c.Carros) // Uma Concessionária TEM MUITOS Carros
                .WithOne(car => car.ConcessionariaAtual) // Um Carro TEM UMA ConcessionariaAtual
                .HasForeignKey(car => car.ConcessionariaId); // A chave é ConcessionariaId
        }
    }
}