using UnitCheck.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace UnitCheck.Models
{
    // ViewModel usado para a tela de geração de uma nova lista de presença
    public class ListGenerationViewModel
    {
        // Campos do Formulário
        
        [Display(Name = "Modelo de Lista")]
        [Required(ErrorMessage = "O modelo de lista é obrigatório.")]
        public int ListTemplateId { get; set; }

        [Display(Name = "Responsável/Setor (Opcional)")]
        // Permite sobrescrever o responsável padrão do template
        public string? Responsible { get; set; }

        [Display(Name = "Equipes Incluídas")]
        [Required(ErrorMessage = "Selecione pelo menos uma equipe.")]
        public List<int> SelectedEquipeIds { get; set; } = new List<int>();

        [Display(Name = "Tipo de Uso")]
        [Required]
        public ListType GeneratedType { get; set; }

        // Dados de Suporte para a View
        
        // Lista de templates disponíveis (popula um Dropdown)
        public List<ListTemplate> AvailableTemplates { get; set; } = new List<ListTemplate>();
        
        // Lista de equipes disponíveis (popula Checkboxes)
        public List<EquipeModel> AvailableTeams { get; set; } = new List<EquipeModel>();
    }
}