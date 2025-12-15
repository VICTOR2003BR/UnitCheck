using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UnitCheck.Models;
using UnitCheck.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace UnitCheck.Controllers;

[Authorize]
public class EventoController : Controller
{
    private readonly ILogger<EventoController> _logger;
    private readonly IEquipeRepository _equipeRepository;
    private readonly IColaboradorRepository _colaboradorRepository;
    private readonly IEventoRepository _eventoRepository;


    public EventoController(ILogger<EventoController> logger,
                            IEquipeRepository equipeRepository,
                            IColaboradorRepository colaboradorRepository,
                            IEventoRepository eventoRepository)
    {
        _logger = logger;
        _equipeRepository = equipeRepository;
        _colaboradorRepository = colaboradorRepository;
        _eventoRepository = eventoRepository;

    }

    [Authorize(Roles = "Admin, LiderDeEquipe")] // Proteção
    public IActionResult Index()
    {
        // Busca todos os eventos, incluindo a Equipe associada.
        List<EventoModel> eventos = _eventoRepository.BuscarPendentes();
        return View(eventos);
    }

     [HttpGet]
    public IActionResult Detalhes(int id)
    {
        // O repositório precisa carregar o evento e a equipe associada (Equipe)
        EventoModel evento = _eventoRepository.BuscarPorId(id); 

        if (evento == null)
        {
            return NotFound();
        }

        // Se o Equipe não for carregado automaticamente pelo repositório (com Include),
        // você pode carregá-lo explicitamente aqui, embora o ideal seja no repositório.
        if (evento.Equipe == null && evento.EquipeId > 0)
        {
            evento.Equipe = _equipeRepository.buscarId(evento.EquipeId);
        }

        return View(evento);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult Criar()
    {
        List<EquipeModel> equipes = _equipeRepository.ListarEquipes();
        ViewBag.EquipeId = new SelectList(equipes, "Id", "Nome"); // Passa para a View

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Criar(EventoModel evento)
    {
        if (ModelState.IsValid)
        {
            // Define a data de criação e o status inicial
            evento.data_criacao = DateTime.Now;
            evento.Status = false; // Pendente

            // Adiciona o evento ao banco de dados
            _eventoRepository.Adicionar(evento);

            // Exemplo: usar TempData para mostrar uma mensagem de sucesso
            TempData["MensagemSucesso"] = "Evento/Atividade criado com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        // Se a validação falhar, recarrega a lista de equipes
        List<EquipeModel> equipes = _equipeRepository.ListarEquipes();
        ViewBag.EquipeId = new SelectList(equipes, "Id", "Nome");

        return View(evento);
    }



    [HttpGet]
    public IActionResult Editar(int id)
    {
        // 1. Busca o evento no banco de dados
        EventoModel evento = _eventoRepository.BuscarPorId(id);

        if (evento == null)
        {
            return NotFound();
        }

        // 2. Carrega todas as equipes para o Dropdown
        List<EquipeModel> equipes = _equipeRepository.ListarEquipes();

        // 3. Cria o SelectList, garantindo que a EquipeId atual do evento esteja pré-selecionada
        ViewBag.EquipeId = new SelectList(equipes, "Id", "Nome", evento.EquipeId);

        return View(evento);
    }

    // POST: Evento/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(EventoModel evento)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // A data de criação original não deve mudar, apenas o status ou dados.
                // O repositório lidará com a atualização no banco.
                _eventoRepository.Atualizar(evento);

                TempData["MensagemSucesso"] = $"Evento '{evento.Nome}' atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Logar o erro (opcional)
                _logger.LogError(ex, "Erro ao tentar atualizar o evento.");
                ModelState.AddModelError("", "Ocorreu um erro ao atualizar o evento. Tente novamente.");
            }
        }

        // Se houver falha na validação, recarrega o SelectList e retorna à View
        List<EquipeModel> equipes = _equipeRepository.ListarEquipes();
        ViewBag.EquipeId = new SelectList(equipes, "Id", "Nome", evento.EquipeId);

        return View(evento);
    }

    [Authorize(Roles = "Admin, LiderDeEquipe")]
    // GET: Exibe a página de confirmação antes de apagar
    public IActionResult Deletar(int id)
    {
        // 1. Busca o evento (incluindo a Equipe, se o BuscarPorId retornar)
        EventoModel evento = _eventoRepository.BuscarPorId(id);

        if (evento == null)
        {
            return NotFound();
        }

        // Passa o evento para a View de confirmação (Apagar.cshtml)
        return View(evento);
    }

    // POST: Executa a exclusão após a confirmação
    [HttpPost, ActionName("Apagar")]
    [ValidateAntiForgeryToken]
    public IActionResult ApagarConfirmado(int id)
    {
        try
        {
            // Chama o método Apagar do Repositório
            bool apagado = _eventoRepository.Apagar(id);

            if (apagado)
            {
                TempData["MensagemSucesso"] = "Evento/Atividade apagado(a) com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Não foi possível apagar o Evento/Atividade. Evento não encontrado.";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Loga o erro e exibe uma mensagem genérica de falha
            _logger.LogError(ex, "Erro ao apagar o evento.");
            TempData["MensagemErro"] = $"Ocorreu um erro ao apagar o evento. Tente novamente. Detalhes: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}
