using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UnitCheck.Models;
using UnitCheck.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace UnitCheck.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEquipeRepository _equipeRepository;
    private readonly IColaboradorRepository _colaboradorRepository;
    private readonly IEventoRepository _eventoRepository;


    public HomeController(ILogger<HomeController> logger,
                          IEquipeRepository equipeRepository,
                          IColaboradorRepository colaboradorRepository,
                          IEventoRepository eventoRepository)
    {
        _logger = logger;
        _equipeRepository = equipeRepository;
        _colaboradorRepository = colaboradorRepository;
        _eventoRepository = eventoRepository;
    }

    public IActionResult Index()
    {
        // 1. Métricas de Equipe e Colaborador (Assumindo métodos de contagem)
        // Você precisará implementar ContarTotalEquipes() e ContarTotalColaboradores() nos respectivos repositórios.
        ViewBag.TotalEquipes = _equipeRepository.ListarEquipes().Count;
        ViewBag.TotalColaboradores = _colaboradorRepository.ListarColaboradores().Count;

        // 2. Métrica de Eventos Cadastrados (Total Geral)
        List<EventoModel> todosEventos = _eventoRepository.BuscarTodos();
        ViewBag.TotalEventos = todosEventos.Count;

        // 3. NOVO: Métrica de Eventos Pendentes (chamando o novo método do repositório)
        ViewBag.TotalEventosPendentes = _eventoRepository.ContarEventosPendentes();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult EmBreve()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
