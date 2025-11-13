using Microsoft.EntityFrameworkCore;
using UnitCheck.Data;
using UnitCheck.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddScoped<IEventoRepository, EventoRepository>();

string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultDatabase");

builder.Services.AddDbContext<BancoContext>(opt =>
{
    opt.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));
});


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<BancoContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Caminho para onde redirecionar se o usuário NÃO estiver logado e tentar acessar uma página protegida
    options.LoginPath = "/Identity/Account/Login"; 
    
    // Caminho para onde redirecionar o usuário após o logout bem-sucedido
    options.LogoutPath = "/Identity/Account/Login"; 
    
    // Opcional: Caminho para onde redirecionar em caso de Acesso Negado (403)
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});


builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<IEquipeRepository, EquipeRepository>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Chama a classe SeedData
        await SeedData.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Um erro ocorreu durante o Seed de dados do Identity.");
    }
}

app.Run();
