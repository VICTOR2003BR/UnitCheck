using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UnitCheck.Models;
using UnitCheck.Repository;
using System.Collections.Generic;
using System.Linq;
using UnitCheck.ViewModels;
using Microsoft.Extensions.Logging;

namespace UnitCheck.Controllers;

public class EquipeController : Controller
{
    private readonly ILogger<EquipeController> _logger;
    private readonly IEquipeRepository _equipeRepository;
    private readonly IColaboradorRepository _colaboradorRepository;

    //---------------------------------------------------------------------------

    public EquipeController(IEquipeRepository equipeRepository,
                                 IColaboradorRepository colaboradorRepository,
                                 ILogger<EquipeController> logger)
    {
        _equipeRepository = equipeRepository;
        _colaboradorRepository = colaboradorRepository;
        _logger = logger;
    }

    //---------------------------------------------------------------------------


    // public IActionResult Index()
    // {
    //     List<EquipeModel> equipe = _equipeRepository.ListarEquipes();
    //     return View(equipe);
    // }

    public IActionResult Index(string busca)
    {
        List<EquipeModel> equipes = _equipeRepository.ListarEquipes();

        if (!string.IsNullOrEmpty(busca))
        {
            equipes = equipes.Where(e => e.Nome.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                                         (e.Lider != null && e.Lider.Contains(busca, StringComparison.OrdinalIgnoreCase)))
                             .ToList();
        }

        return View(equipes);
    }

    //---------------------------------------------------------------------------

    public IActionResult Criar()
    {
        var todosColaboradores = _colaboradorRepository.ListarColaboradores(); // Obtém todos os colaboradores
        var viewModel = new CriarEquipeViewModel
        {
            Equipe = new EquipeModel(), // Inicializa um novo objeto EquipeModel
            ColaboradoresDisponiveis = todosColaboradores // Passa a lista para a ViewModel
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Criar(CriarEquipeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (viewModel.LiderIdSelecionado.HasValue)
            {
                var liderSelecionado = _colaboradorRepository.buscarId(viewModel.LiderIdSelecionado.Value);
                if (liderSelecionado != null)
                {
                    viewModel.Equipe.Lider = liderSelecionado.Nome;
                }
                else
                {

                    ModelState.AddModelError("LiderIdSelecionado", "Líder selecionado não encontrado.");
                    viewModel.ColaboradoresDisponiveis = _colaboradorRepository.ListarColaboradores();
                    return View(viewModel);
                }
            }
            else
            {

                viewModel.Equipe.Lider = null;
            }

            _equipeRepository.adicionar(viewModel.Equipe);
            TempData["MensagemSucesso"] = "Equipe criada com sucesso!";
            return RedirectToAction("Index");
        }
        viewModel.ColaboradoresDisponiveis = _colaboradorRepository.ListarColaboradores();
        return View(viewModel);
    }

    //---------------------------------------------------------------------------

    [HttpGet]
    public IActionResult Editar(int id)
    {
        EquipeModel? equipe = _equipeRepository.buscarId(id);
        if (equipe == null)
        {
            return NotFound();
        }

        // Obtém todos os colaboradores para o dropdown
        var todosColaboradores = _colaboradorRepository.ListarColaboradores();

        // Cria a ViewModel de edição e popula
        var viewModel = new EditarEquipeViewModel
        {
            Equipe = equipe, // Atribui a equipe encontrada
            ColaboradoresDisponiveis = todosColaboradores,
            // Define o líder selecionado para o ID atual da equipe, se houver
            LiderIdSelecionado = equipe.LiderId
        };

        return View(viewModel);
    }

    [HttpPost]
    [ActionName("Editar")] // Usa o mesmo nome da rota da View para o POST
    public IActionResult Editar(EditarEquipeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            // Lógica para definir o Lider (Nome e Id)
            if (viewModel.LiderIdSelecionado.HasValue)
            {
                var liderSelecionado = _colaboradorRepository.buscarId(viewModel.LiderIdSelecionado.Value);
                if (liderSelecionado != null)
                {
                    viewModel.Equipe.Lider = liderSelecionado.Nome;
                    viewModel.Equipe.LiderId = liderSelecionado.Id; // Atualiza o LiderId
                }
                else
                {
                    ModelState.AddModelError("LiderIdSelecionado", "Líder selecionado não encontrado.");
                    viewModel.ColaboradoresDisponiveis = _colaboradorRepository.ListarColaboradores();
                    return View(viewModel);
                }
            }
            else
            {
                viewModel.Equipe.Lider = null;
                viewModel.Equipe.LiderId = null;
            }
            try
            {
                _equipeRepository.atualizar(viewModel.Equipe);
                TempData["MensagemSucesso"] = "Equipe atualizada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar equipe com ID {viewModel.Equipe.Id}.");
                ModelState.AddModelError("", $"Ocorreu um erro ao atualizar a equipe: {ex.Message}. Por favor, tente novamente.");
                // Re-popula a lista de colaboradores caso precise retornar à View com erro
                viewModel.ColaboradoresDisponiveis = _colaboradorRepository.ListarColaboradores();
                return View(viewModel); // Retorna a View de edição com a ViewModel e os erros
            }
        }
        viewModel.ColaboradoresDisponiveis = _colaboradorRepository.ListarColaboradores();
        return View(viewModel);
    }

    //---------------------------------------------------------------------------

    [HttpGet]
    public IActionResult Deletar(int id)
    {
        EquipeModel? equipe = _equipeRepository.buscarId(id);
        if (equipe == null)
        {
            return NotFound();
        }
        return View(equipe);
    }

    [HttpPost]
    [ActionName("Deletar")]
    public IActionResult ConfirmarDelecao(int id)
    {
        _equipeRepository.deletar(id);
        return RedirectToAction("Index");
    }

    //---------------------------------------------------------------------------

    public IActionResult AdicionarColaborador()
    {
        return View();
    }
    //---------------------------------------------------------------------------

    public IActionResult GerenciarColaboradores(int id)
    {
        var equipe = _equipeRepository.buscarId(id); // Este método agora inclui colaboradores
        if (equipe == null)
        {
            return NotFound();
        }

        // Obter todos os colaboradores disponíveis
        var todosColaboradores = _colaboradorRepository.ListarColaboradores();

        // ViewModel para passar dados para a View
        var viewModel = new GerenciarColaboradoresViewModel
        {
            Equipe = equipe,
            TodosColaboradores = todosColaboradores.Select(c => new CheckboxItem // Crie uma classe auxiliar para checkboxes
            {
                Id = c.Id,
                Nome = c.Nome,
                IsChecked = equipe.Colaboradores.Any(ec => ec.Id == c.Id) // Marca os que já estão na equipe
            }).ToList()
        };

        return View(viewModel);
    }
    //---------------------------------------------------------------------------

    [HttpPost]
    public IActionResult SalvarColaboradores(int equipeId, List<int> selecionadosIds)
    {
        selecionadosIds = selecionadosIds ?? new List<int>();

        try
        {
            _equipeRepository.AdicionarColaboradoresNaEquipe(equipeId, selecionadosIds);

            TempData["MensagemSucesso"] = "Colaboradores da equipe atualizados com sucesso!";
            return RedirectToAction("DetalhesEquipe", new { id = equipeId }); // Melhor redirecionar para os Detalhes
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao atualizar colaboradores da equipe com ID: {equipeId}");
            TempData["MensagemErro"] = $"Erro ao atualizar colaboradores da equipe: {ex.Message}";
            // Se houver erro, redireciona de volta para GerenciarColaboradores para exibir a mensagem
            return RedirectToAction("GerenciarColaboradores", new { id = equipeId });
        }
    }
    //---------------------------------------------------------------------------

    public IActionResult DetalhesEquipe(int id)
    {
        try
        {
            var equipe = _equipeRepository.buscarId(id); // Já carrega os colaboradores
            if (equipe == null)
            {
                TempData["MensagemErro"] = "Equipe não encontrada.";
                return RedirectToAction("Index"); // Redireciona para a lista de equipes
            }
            return View(equipe);
            /*
            // Se você quiser usar uma ViewModel para detalhes da equipe:
            var viewModel = new DetalhesEquipeViewModel
            {
                Equipe = equipe,
                ColaboradoresDaEquipe = equipe.Colaboradores.ToList() // Ou use o novo método do repo
            };
            return View(viewModel);
            */
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao buscar detalhes da equipe com ID: {id}");
            TempData["MensagemErro"] = $"Erro ao carregar detalhes da equipe: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
    //---------------------------------------------------------------------------



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}