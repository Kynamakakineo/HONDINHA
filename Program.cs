using Microsoft.EntityFrameworkCore;
using AutoManagerApi.Data; // Certifique-se que o nome da pasta do seu Contexto é esse mesmo
 
var builder = WebApplication.CreateBuilder(args);
 
// 1. Adiciona os Controllers COM A CORREÇÃO DO CICLO INFINITO
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
 
// 2. Adiciona o Swagger (o gerador da documentação)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
// 3. Pega a senha do banco e Configura o MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
 
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
 
// 4. Configuração do CORS (Para o Front funcionar)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
 
// 5. Adiciona Autorização
builder.Services.AddAuthorization();
 
var app = builder.Build();
 
// --- AQUI ESTAVA O PROBLEMA: O SWAGGER PRECISA ESTAR AQUI ---
// Configura o HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Gera o JSON do Swagger
    app.UseSwaggerUI(); // Gera a TELA bonita do Swagger
}
// -----------------------------------------------------------
 
app.UseHttpsRedirection();
 
// Aplica o CORS antes de tudo
app.UseCors("PermitirTudo");
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 