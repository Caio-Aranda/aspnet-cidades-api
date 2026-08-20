using Projeto.Repository;
using Projeto.Services;
using Projeto.Repository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(); 


builder.Services.AddScoped<MySqlDbContext>();
builder.Services.AddScoped<CidadeRepository>();
builder.Services.AddScoped<CidadeServices>();
builder.Services.AddScoped<AlunoRepository>();
builder.Services.AddScoped<AlunoServices>();
builder.Services.AddHttpContextAccessor(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); 
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();