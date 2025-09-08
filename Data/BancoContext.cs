using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnitCheck.Models;

namespace UnitCheck.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        public DbSet<ColaboradorModel> Colaboradores { get; set; }
        public DbSet<EquipeModel> Equipes { get; set; }
        //public DbSet<ListaDePresencaModel> ListasDePresenca { get; set; }
        //public DbSet<ColaboradorPresencaModel> ColaboradoresPresenca { get; set; }



    }
}

//dotnet ef migrations add migration_1
//dotnet ef database update
