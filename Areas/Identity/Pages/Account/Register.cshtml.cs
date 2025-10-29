using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
// Adicionando o using para usar o RoleManager
using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace UnitCheck.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager; // <-- 1. CAMPO ADICIONADO

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager) // <-- 2. PARÂMETRO ADICIONADO AO CONSTRUTOR
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager; // <-- 3. ATRIBUIÇÃO ADICIONADA
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O email é obrigatório.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "A senha é obrigatória.")]
            [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme a Senha")]
            [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não correspondem.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Novo usuário criado com sucesso.");

                    // #############################################################
                    // # 4. LÓGICA DE ATRIBUIÇÃO DA ROLE "ADMIN" ADICIONADA AQUI #
                    // #############################################################
                    
                    // Verifica se a Role "Admin" existe
                    if (await _roleManager.RoleExistsAsync("Admin"))
                    {
                        // Atribui a Role "Admin" ao novo usuário
                        await _userManager.AddToRoleAsync(user, "Admin");
                        _logger.LogInformation($"Usuário {user.UserName} criado e atribuído à Role 'Admin'.");
                    }
                    else
                    {
                        _logger.LogError("Falha ao atribuir Role: A Role 'Admin' não foi encontrada.");
                    }
                    
                    // #############################################################

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}