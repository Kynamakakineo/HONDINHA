namespace AutoManagerApi.Models
{

    //Olha a tabela d concessionarias
    public class Concessionaria
    {
        public int Id { get; set; } 
        public string Nome { get; set; } = null!;
        public string Endereco { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public string Cnpj { get; set; } = null!;
        public string? ImageUrl { get; set; }

        // chave gringa
        
        public List<Carro> Carros { get; set; } = new();
    }
}
