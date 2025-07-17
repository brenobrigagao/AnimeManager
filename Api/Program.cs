using Api;
using Application.Services.Anime;
using Application.Services.Auth;
using Application.Services.Avaliação;
using Application.Services.Avaliacao;
using Application.Services.Estudio;
using Application.Services.Genero;
using Application.Services.Senha;
using Application.Services.Token;
using Application.Services.Usuario;
using Infra.Data.Context;
using Infra.Repositories;
using Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAnimeService, AnimeService>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<IEstudioService, EstudioService>();
builder.Services.AddScoped<IGeneroService, GeneroService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISenhaService, SenhaService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<IEstudioRepository, EstudioRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthentication();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();
