using UnitCheck.Models;
using System.Collections.Generic;
using System.Linq;

namespace UnitCheck.Models.ViewModels
{
    // ViewModel para agrupar todos os dados necessários para a View de Detalhes da Equipe.
    public class EquipeDetalhesViewModel
    {
        public EquipeModel ?Equipe { get; set; }
        
        // Lista de colaboradores associados à equipe.
        public List<ColaboradorModel> ?Colaboradores { get; set; }

        // Lista de eventos/atividades associados à equipe.
        public List<EventoModel> ?Eventos { get; set; }
    }
}