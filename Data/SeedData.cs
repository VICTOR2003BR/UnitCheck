using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace UnitCheck.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Criar a Role "Admin" se ela não existir
            string adminRole = "Admin";
            if (await roleManager.FindByNameAsync(adminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // 2. Criar o primeiro usuário Administrador se ele não existir
            string adminEmail = "admin@unitcheck.com.br"; 
            string adminPassword = "AdminPassword123*"; // MUDE ESTA SENHA DE SEGURANÇA!

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Cria o usuário com a senha
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Atribui a role "Admin" ao novo usuário
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}