using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace UnitCheck.Areas.Identity.Pages.Account
{
    // O usuário precisa estar logado para chegar aqui
    [AllowAnonymous] 
    public class AccessDeniedModel : PageModel
    {
        // O método OnGet é o ponto de entrada da página
        public void OnGet()
        {
            // Apenas retorna a View.
        }
    }
}