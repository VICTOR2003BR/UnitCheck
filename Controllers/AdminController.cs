using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UnitCheck.Models;

namespace UnitCheck.Controllers
{
    // Apenas usuários com a Role "Admin" podem acessar este Controller
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Admin/CreateLeader
        public IActionResult CreateLeader()
        {
            return View();
        }

        // POST: /Admin/CreateLeader
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLeader(LeaderCreationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Cria o novo usuário
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 2. Atribui a Role "LiderDeEquipe"
                    await _userManager.AddToRoleAsync(user, "LiderDeEquipe");
                    
                    // Sucesso: Redireciona para uma tela de sucesso ou lista
                    TempData["SuccessMessage"] = $"Líder de Equipe {model.Email} criado com sucesso!";
                    return RedirectToAction("Index", "Home"); 
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
    }
}