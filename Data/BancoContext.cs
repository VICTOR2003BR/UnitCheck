using Microsoft.EntityFrameworkCore;
using UnitCheck.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace UnitCheck.Data
{
    public class BancoContext : IdentityDbContext<IdentityUser>
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        // Seus DbSets existentes
        public DbSet<ColaboradorModel> Colaboradores { get; set; }
        public DbSet<EquipeModel> Equipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

//dotnet ef migrations add migration_1
//dotnet ef database update
