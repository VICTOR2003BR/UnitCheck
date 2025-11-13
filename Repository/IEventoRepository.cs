using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitCheck.Models;

namespace UnitCheck.Repository
{
    public interface IEventoRepository
    {
        List<EventoModel> BuscarTodos();
        EventoModel Adicionar(EventoModel evento);
        EventoModel Atualizar(EventoModel evento);
        EventoModel BuscarPorId(int id);
        bool Apagar(int id);
    }
}