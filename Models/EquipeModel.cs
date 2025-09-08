using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnitCheck.Models
{
    public class EquipeModel
    {
        public int Id { get; set; }
        public String Nome { get; set; }  = string.Empty;
        public String Local { get; set; } = string.Empty;
        public String Descricao { get; set; } = string.Empty;
        public String? Lider { get; set; } //= string.Empty;
        public int? LiderId { get; set; }
        public List<ColaboradorModel> Colaboradores { get; set; } = new List<ColaboradorModel>();
    }
}