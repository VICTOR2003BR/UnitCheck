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



    public HomeController(ILogger<HomeController> logger, IEquipeRepository equipeRepository, IColaboradorRepository colaboradorRepository)
    {
        _logger = logger;
        _equipeRepository = equipeRepository;
        _colaboradorRepository = colaboradorRepository;

    }

    public IActionResult Index()
    {
        ViewBag.TotalEquipes = _equipeRepository.ListarEquipes().Count;
        ViewBag.TotalColaboradores = _colaboradorRepository.ListarColaboradores().Count;

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
