using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitCheck.Models;

namespace UnitCheck.Repository
{
    public interface IColaboradorRepository
    {
        ColaboradorModel adicionar(ColaboradorModel colaborador);
        ColaboradorModel atualizar(ColaboradorModel colaborador);
        ColaboradorModel? buscarId(int id);
        bool deletar(int id);
        List<ColaboradorModel> ListarColaboradores();
    }
}