using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace UnitCheck.Models
{
    public class EventoModel
    {
        public int Id { get; set; }
        public String Nome { get; set; } = string.Empty;
        public String Tipo { get; set; } = string.Empty;
        public int nivelPrioridade { get; set; } //0=sem data definida / 1=baixa / 2=media / 3=alta
        public DateTime data_criacao { get; set; }
        public DateTime? data_finalizacao { get; set; }

        [Display(Name = "Equipe Associada")]
        public int EquipeId { get; set; } // Chave Estrangeira
        public bool Status { get; set; } //Pendente ou concluido
        public EquipeModel? Equipe { get; set; }

    }
}