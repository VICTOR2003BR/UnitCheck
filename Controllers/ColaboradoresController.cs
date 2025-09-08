using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UnitCheck.Models;
using UnitCheck.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitCheck.Controllers;

public class ColaboradoresController : Controller
{
    private readonly IColaboradorRepository colaborador_Repository;
    public ColaboradoresController(IColaboradorRepository colaboradorRepository)
    {
        colaborador_Repository = colaboradorRepository;
    }

    //---------------------------------------------------------------------------

    // public IActionResult Index()
    // {
    //     List<ColaboradorModel> colaborador = colaborador_Repository.ListarColaboradores();
    //     return View(colaborador);
    // }

    public IActionResult Index(string busca) // Adicione o parâmetro 'busca'
    {
        List<ColaboradorModel> colaboradores = colaborador_Repository.ListarColaboradores();
        if (!string.IsNullOrEmpty(busca))
        {
            colaboradores = colaboradores.Where(c => c.Nome.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                                                     c.funcao.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                                                     c.matricula.Contains(busca, StringComparison.OrdinalIgnoreCase))
                                         .ToList();
        }
        ViewBag.BuscaAtual = busca; // Passa o termo de busca para a View
        return View(colaboradores);
    }


    //---------------------------------------------------------------------------

    [HttpPost]
    public IActionResult Criar(ColaboradorModel colaborador)
    {
        colaborador_Repository.adicionar(colaborador);
        return RedirectToAction("Index");
    }
    public IActionResult Criar()
    {
        return View();
    }

    //---------------------------------------------------------------------------

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var colaborador = colaborador_Repository.buscarId(id);
        // if (colaborador == null)
        // {
        //     return NotFound(); // Retorna 404 se o colaborador não for encontrado
        // }
        return View(colaborador);
    }

    [HttpPost]
    public IActionResult atualizar(ColaboradorModel colaborador)
    {
        colaborador_Repository.atualizar(colaborador);
        return RedirectToAction("Index");
        // if (ModelState.IsValid)
        // {
        //     colaborador_Repository.atualizar(colaborador);
        //     return RedirectToAction("Index");
        // }
        // return View("Editar", colaborador); 
    }

    //---------------------------------------------------------------------------

    [HttpGet]
    public IActionResult Deletar(int id)
    {
        var colaborador = colaborador_Repository.buscarId(id);
        if (colaborador == null)
        {
            return NotFound(); // Retorna 404 se o colaborador não for encontrado
        }
        return View(colaborador);
    }

    [HttpPost]
    [ActionName("Deletar")]
    public IActionResult ConfirmarDelecao(int id)
    {
        colaborador_Repository.deletar(id);
        return RedirectToAction("Index");
    }

    //---------------------------------------------------------------------------

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
