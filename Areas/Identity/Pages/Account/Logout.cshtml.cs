using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace UnitCheck.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // Método chamado quando o usuário tenta acessar a página GET (geralmente só para confirmar)
        public void OnGet()
        {
        }

        // Método chamado quando o formulário POST é enviado (ao clicar em Logout)
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Executa a desautenticação
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            // **CORREÇÃO DE REDIRECIONAMENTO**
            // Se houver um returnUrl, ele usa.
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // Se não houver returnUrl, força o redirecionamento para a página de Login.
                return RedirectToPage("./Login"); 
            }
        }
    }
}