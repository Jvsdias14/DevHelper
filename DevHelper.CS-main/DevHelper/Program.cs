using DevHelper.Data.Interface;
using DevHelper.Data.Interfaces;
using DevHelper.Data.Model;
using DevHelper.Data.Repository;
using DevHelper.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var config = builder.Configuration;

// Configuração do DbContext
builder.Services.AddDbContext<DBdevhelperContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DBdevhelperConnection")));

// Configuração de repositórios
builder.Services.AddScoped<iProblemaRepository, ProblemaRepository>();
builder.Services.AddScoped<iProblemaRepositoryAsync, ProblemaRepository>();

builder.Services.AddScoped<iUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<iUsuarioRepositoryAsync, UsuarioRepository>();

// Configuração de autenticação com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Index";
        options.LogoutPath = "/Account/Logout";
    });

// Registrar o serviço de login
builder.Services.AddScoped<Login>();

// Registrar o IPasswordHasher
builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

var app = builder.Build();

// Configuração do pipeline de requisições
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Autenticação e autorização
app.UseAuthentication(); // Deve ser colocado antes do UseAuthorization()
app.UseAuthorization();

app.MapRazorPages();

app.Run();
