using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitCheck.Data;
using UnitCheck.Models;

namespace UnitCheck.Repository
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly BancoContext colaborador_context;

        public ColaboradorRepository(BancoContext BancoContext)
        {
            colaborador_context = BancoContext;   
        }

        public ColaboradorModel adicionar(ColaboradorModel colaborador)
        {
            colaborador_context.Add(colaborador);
            colaborador_context.SaveChanges();
            return colaborador;
        }
        
        public ColaboradorModel? buscarId(int id)
        {
            return colaborador_context.Colaboradores.FirstOrDefault(x => x.Id == id);
        }

        public ColaboradorModel atualizar(ColaboradorModel colaborador)
        {
            ColaboradorModel? colaboradorBD = buscarId(colaborador.Id);
            if (colaboradorBD == null) throw new Exception("Colaborador não encontrado");
            colaboradorBD.Nome = colaborador.Nome;
            colaboradorBD.matricula = colaborador.matricula;
            colaboradorBD.cpf = colaborador.cpf;
            colaboradorBD.funcao = colaborador.funcao;

            colaborador_context.Colaboradores.Update(colaboradorBD);
            colaborador_context.SaveChanges();
            return colaboradorBD;
        }

        public List<ColaboradorModel> ListarColaboradores()
        {
            return colaborador_context.Colaboradores.ToList();
        }

        public bool deletar(int id)
        {
            ColaboradorModel? colaboradorBD = buscarId(id);
            if (colaboradorBD == null) throw new Exception("Colaborador não encontrado");
            colaborador_context.Colaboradores.Remove(colaboradorBD);
            colaborador_context.SaveChanges();
            return true;
        }
    }
}