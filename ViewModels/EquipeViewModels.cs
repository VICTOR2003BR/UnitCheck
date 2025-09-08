namespace UnitCheck.ViewModels
{
    using System.Collections.Generic;
    using UnitCheck.Models;

    public class GerenciarColaboradoresViewModel
    {
        public EquipeModel Equipe { get; set; } = new EquipeModel();
        public List<CheckboxItem> TodosColaboradores { get; set; } = new List<CheckboxItem>();
    }

    public class CheckboxItem
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
    }

    public class CriarEquipeViewModel
    {
        public EquipeModel Equipe { get; set; } = new EquipeModel();
        public List<ColaboradorModel> ColaboradoresDisponiveis { get; set; } = new List<ColaboradorModel>();
        public int? LiderIdSelecionado { get; set; }
    }

    public class EditarEquipeViewModel
    {
        public EquipeModel Equipe { get; set; } = new EquipeModel(); // Equipe a ser editada
        public List<ColaboradorModel> ColaboradoresDisponiveis { get; set; } = new List<ColaboradorModel>(); // Para o dropdown
        public int? LiderIdSelecionado { get; set; } // O ID do líder selecionado (ou a ser selecionado)
    }


}