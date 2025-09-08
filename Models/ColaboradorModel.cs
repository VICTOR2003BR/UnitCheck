using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace UnitCheck.Models
{
    public class ColaboradorModel
    {
        public int Id { get; set; }
        public String Nome { get; set; } = string.Empty;
        public String Email { get; set; } = string.Empty;

        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "O telefone deve conter 10 ou 11 dígitos numéricos.")] // Ex: (XX) XXXX-XXXX ou (XX) XXXXX-XXXX
        [StringLength(11, MinimumLength = 10, ErrorMessage = "O telefone deve ter entre 10 e 11 dígitos.")]
        public String? Telefone { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números.")]
        public String cpf { get; set; } = string.Empty;
        public String funcao { get; set; } = string.Empty;
        public String matricula { get; set; } = string.Empty;
        public DateTime data_admissao { get; set; }
        public bool Status { get; set; }
        public List<EquipeModel> Equipes { get; set; } = new List<EquipeModel>();

        //public List<ColaboradorPresencaModel> ColaboradoresPresenca { get; set; } = new List<ColaboradorPresencaModel>();

    }
}