using System.ComponentModel.DataAnnotations.Schema;

namespace AutoManagerApi.Models
{



    //Classe de carineo
    public class Carro
    {
        public int? Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int? Ano { get; set; }
        public string Cor { get; set; }

        [Column(TypeName = "decimal(10, 2)")] // Boa prática para dinheiro
        public decimal Preco { get; set; }

        // --- ADIÇÕES PARA LOGÍSTICA ---
        
        // Chave estrangeira que aponta para a tabela Concessionaria
        public int? ConcessionariaId { get; set; } 
        
        // Propriedade de navegação (o link para o objeto "Concessionaria")
        public Concessionaria? ConcessionariaAtual { get; set; } 
    }
}