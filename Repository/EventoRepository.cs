using UnitCheck.Data;
using UnitCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace UnitCheck.Repository
{
    public class EventoRepository : IEventoRepository
    {
        private readonly BancoContext _context;

        public EventoRepository(BancoContext context)
        {
            _context = context;
        }

        public EventoModel Adicionar(EventoModel evento)
        {
            _context.Eventos.Add(evento);
            _context.SaveChanges();
            return evento;
        }

        public bool Apagar(int id)
        {
            EventoModel evento = BuscarPorId(id);

            if (evento == null)
            {
                return false;
            }
            _context.Eventos.Remove(evento);
            _context.SaveChanges();

            return true;
        }

        public EventoModel Atualizar(EventoModel evento)
        {
            _context.Eventos.Update(evento);
            _context.SaveChanges();
            return evento;
        }

        public EventoModel BuscarPorId(int id)
        {
            return _context.Eventos
        .Include(e => e.Equipe)
        .FirstOrDefault(e => e.Id == id);
        }

        public List<EventoModel> BuscarTodos()
        {
            // Inclui a Equipe para que a View possa mostrar o nome da equipe
            return _context.Eventos.Include(e => e.Equipe).ToList();
        }

        public List<EventoModel> BuscarPendentes()
        {
            return _context.Eventos
                .Where(e => e.Status == false) // Filtra onde Status é Pendente
                .Include(e => e.Equipe)
                .ToList();
        }

        public int ContarEventosPendentes()
        {
            // Filtra os eventos onde Status é false (Pendente) e conta o total.
            // Status: true = Concluído, false = Pendente
            return _context.Eventos.Count(e => e.Status == false);
        }


    }
}