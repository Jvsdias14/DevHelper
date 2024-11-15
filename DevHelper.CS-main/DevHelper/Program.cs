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

// Configura��o do DbContext
builder.Services.AddDbContext<DBdevhelperContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DBdevhelperConnection")));

// Configura��o de reposit�rios
builder.Services.AddScoped<iProblemaRepository, ProblemaRepository>();
builder.Services.AddScoped<iProblemaRepositoryAsync, ProblemaRepository>();

builder.Services.AddScoped<iUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<iUsuarioRepositoryAsync, UsuarioRepository>();

// Configura��o de autentica��o com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Index";
        options.LogoutPath = "/Account/Logout";
    });

// Registrar o servi�o de login
builder.Services.AddScoped<Login>();

// Registrar o IPasswordHasher
builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

var app = builder.Build();

// Configura��o do pipeline de requisi��es
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Autentica��o e autoriza��o
app.UseAuthentication(); // Deve ser colocado antes do UseAuthorization()
app.UseAuthorization();

app.MapRazorPages();

app.Run();
