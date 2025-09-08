using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitCheck.Models;

namespace UnitCheck.Repository
{
    public interface IEquipeRepository
    {
        EquipeModel adicionar(EquipeModel equipe);
        EquipeModel atualizar(EquipeModel equipe);
        EquipeModel? buscarId(int id);
        void AdicionarColaboradoresNaEquipe(int equipeId, List<int> colaboradorIds);

        bool deletar(int id);
        List<EquipeModel> ListarEquipes();
        List<ColaboradorModel> ListarColaboradoresDeUmaEquipe(int equipeId);

    }
}