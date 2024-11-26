using DevHelper.Data.Interface;
using DevHelper.Data.Interfaces;
using DevHelper.Data.Model;
using DevHelper.Data.Repository;
using DevHelper.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

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

builder.Services.AddScoped<iArquivoProblemaRepository, ArquivoProblemaRepository>();
builder.Services.AddScoped<iArquivoProblemaRepositoryAsync, ArquivoProblemaRepository>();

builder.Services.AddScoped<iArquivoSolucaoRepository, ArquivoSolucaoRepository>();
builder.Services.AddScoped<iArquivoSolucaoRepositoryAsync, ArquivoSolucaoRepository>();

builder.Services.AddScoped<iUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<iUsuarioRepositoryAsync, UsuarioRepository>();

builder.Services.AddScoped<iSolucaoRepository, SolucaoRepository>();
builder.Services.AddScoped<iSolucaoRepositoryAsync, SolucaoRepository>();

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

// Registrar o IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Adicionar configurações de sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Adicionar configuração de limite de tamanho para uploads de arquivos
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB, ajuste conforme necessário
});

var app = builder.Build();

// Configuração do pipeline de requisições
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/uploads"
});
app.UseRouting();


// Usar sessão antes de autenticação e autorização
app.UseSession();

// Autenticação e autorização
app.UseAuthentication(); // Deve ser colocado antes do UseAuthorization()
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var userRepository = context.RequestServices.GetRequiredService<iUsuarioRepositoryAsync>();
            var user = await userRepository.SelecionaPelaChaveAsync(int.Parse(userId));
            if (user == null)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Session.Clear();
            }
        }
    }
    await next.Invoke();
});

app.MapRazorPages();

app.Run();
