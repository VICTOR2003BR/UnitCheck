using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitCheck.Data;
using UnitCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace UnitCheck.Repository
{
    public class EquipeRepository : IEquipeRepository
    {

        private readonly BancoContext equipe_context;

        public EquipeRepository(BancoContext BancoContext)
        {
            equipe_context = BancoContext;
        }

        public EquipeModel adicionar(EquipeModel equipe)
        {
            equipe_context.Add(equipe);
            equipe_context.SaveChanges();
            return equipe;
        }

        public void AdicionarColaboradoresNaEquipe(int equipeId, List<int> colaboradorIds)
        {
            var equipe = equipe_context.Equipes
                                       .Include(e => e.Colaboradores)
                                       .FirstOrDefault(e => e.Id == equipeId);
            if (equipe == null)
            {
                throw new Exception("Equipe não encontrada para adicionar/remover colaboradores.");
            }
            var colaboradoresAtuais = equipe.Colaboradores.ToList();
            foreach (var colaboradorAtual in colaboradoresAtuais)
            {
                if (!colaboradorIds.Contains(colaboradorAtual.Id))
                {
                    equipe.Colaboradores.Remove(colaboradorAtual);
                }
            }
            var novosColaboradoresIds = colaboradorIds
                                        .Where(id => !equipe.Colaboradores.Any(ec => ec.Id == id))
                                        .ToList();
            if (novosColaboradoresIds.Any())
            {
                var colaboradoresParaAdicionar = equipe_context.Colaboradores
                                                              .Where(c => novosColaboradoresIds.Contains(c.Id))
                                                              .ToList();
                foreach (var col in colaboradoresParaAdicionar)
                {
                    equipe.Colaboradores.Add(col);
                }
            }
            equipe_context.SaveChanges();
        }


        // public EquipeModel? buscarId(int id)
        // {
        //     return equipe_context.Equipes.FirstOrDefault(x => x.Id == id);
        // }

        public EquipeModel? buscarId(int id)
        {
            //Include(e => e.Colaboradores) para carregar a coleção de colaboradores
            return equipe_context.Equipes.Include(e => e.Colaboradores).FirstOrDefault(x => x.Id == id);
        }

        public EquipeModel atualizar(EquipeModel equipe)
        {
            EquipeModel? equipeBD = buscarId(equipe.Id);
            if (equipeBD == null) throw new Exception("Equipe não encontrada para atualização.");

            equipeBD.Nome = equipe.Nome;
            equipeBD.Local = equipe.Local;
            equipeBD.Descricao = equipe.Descricao;

            equipeBD.Lider = equipe.Lider;
            equipeBD.LiderId = equipe.LiderId;

            equipe_context.Equipes.Update(equipeBD);
            equipe_context.SaveChanges();
            return equipeBD;
        }

        public bool deletar(int id)
        {
            EquipeModel? equipeBD = buscarId(id);
            if (equipeBD == null) throw new Exception("Equipe não encontrada");
            equipe_context.Equipes.Remove(equipeBD);
            equipe_context.SaveChanges();
            return true;
        }

        public List<ColaboradorModel> ListarColaboradoresDeUmaEquipe(int equipeId)
        {
            var equipe = equipe_context.Equipes
                                       .Include(e => e.Colaboradores)
                                       .FirstOrDefault(e => e.Id == equipeId);
            if (equipe == null)
            {
                return new List<ColaboradorModel>();
            }

            return equipe.Colaboradores.ToList();
        }

        public List<EquipeModel> ListarEquipes()
        {
            return equipe_context.Equipes.Include(e => e.Colaboradores).ToList();
        }

    }
}