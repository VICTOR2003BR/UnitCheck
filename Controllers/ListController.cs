// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using UnitCheck.Models;
// using UnitCheck.Models.Enums;
// using System.Text.Json;
// using System.Linq; // Garante que a extensão Linq esteja disponível

// namespace UnitCheck.Controllers
// {
//     // O Controller que gerencia as ações relacionadas às Listas de Presença
//     public class ListController : Controller
//     {
//         private readonly ILogger<ListController> _logger;
//         // private readonly ApplicationDbContext _context; // Assumindo seu contexto de banco

//         // Construtor
//         public ListController(ILogger<ListController> logger /*, ApplicationDbContext context */)
//         {
//             _logger = logger;
//             // _context = context;
//         }

//         // ----------------------------------------------------------------------------------
//         // [GET] /List/Generate
//         // Mostra o formulário de geração de lista
//         // ----------------------------------------------------------------------------------
//         [HttpGet]
//         public async Task<IActionResult> Generate()
//         {
//             var viewModel = new ListGenerationViewModel
//             {
//                 // Simulação: Carrega templates e equipes
//                 AvailableTemplates = GetSimulatedTemplates(), 
//                 AvailableTeams = GetSimulatedTeams(),
//                 // Define um tipo padrão
//                 GeneratedType = ListType.Digital
//             };

//             return View(viewModel);
//         }

//         // ----------------------------------------------------------------------------------
//         // [POST] /List/Generate
//         // Processa a requisição para criar a AttendanceList e os AttendanceRecords
//         // ----------------------------------------------------------------------------------
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Generate(ListGenerationViewModel viewModel)
//         {
//             // O primeiro erro de compilação que você viu em 'Responsible' e 'Type' foi corrigido
//             // com a adição do modelo ListTemplate. Este novo erro vem de uma simulação de dados
//             // que não corresponde ao seu EquipeModel.

//             // 1. Recarregar dados de suporte em caso de falha de validação
//             if (!ModelState.IsValid)
//             {
//                 // Se falhar, recarrega os dados de suporte e retorna à View com os erros.
//                 viewModel.AvailableTemplates = GetSimulatedTemplates();
//                 viewModel.AvailableTeams = GetSimulatedTeams();
//                 return View(viewModel);
//             }

//             try
//             {
//                 // 2. Busca e Validação
//                 var templates = GetSimulatedTemplates();
//                 var template = templates.FirstOrDefault(t => t.Id == viewModel.ListTemplateId);
                
//                 if (template == null)
//                 {
//                     // Recarregar dados de suporte em caso de erro
//                     viewModel.AvailableTemplates = templates;
//                     viewModel.AvailableTeams = GetSimulatedTeams();
//                     ModelState.AddModelError("", "Modelo de Lista não encontrado.");
//                     return View(viewModel);
//                 }

//                 // Simulação: Busca Colaboradores filtrando pelo EquipeId
//                 var collaborators = GetSimulatedCollaborators()
//                                         .Where(c => viewModel.SelectedEquipeIds.Contains(c.EquipeId))
//                                         .ToList();

//                 if (!collaborators.Any())
//                 {
//                     // Recarregar dados de suporte em caso de erro
//                     viewModel.AvailableTemplates = templates;
//                     viewModel.AvailableTeams = GetSimulatedTeams();
//                     ModelState.AddModelError("", "Nenhum colaborador encontrado nas equipes selecionadas.");
//                     return View(viewModel);
//                 }

//                 // 3. Criação da AttendanceList (A Lista Mestra)
//                 var attendanceList = new AttendanceList
//                 {
//                     ListTemplateId = viewModel.ListTemplateId,
//                     GenerationDate = DateTime.Now,
//                     Responsible = viewModel.Responsible ?? template.Responsible, // Usa o responsável do form ou do template
//                     GeneratedType = viewModel.GeneratedType,
//                     // Serializa os IDs das equipes para armazenamento
//                     IncludedTeamIds = JsonSerializer.Serialize(viewModel.SelectedEquipeIds), 
//                     Records = new List<AttendanceRecord>()
//                 };

//                 // 4. Criação dos AttendanceRecords (Registros de Presença)
//                 foreach (var collab in collaborators)
//                 {
//                     attendanceList.Records.Add(new AttendanceRecord
//                     {
//                         CollaboratorId = collab.Id, 
//                         Status = AttendanceStatus.Unmarked, // Status inicial
//                         LastUpdated = DateTime.Now,
//                     });
//                 }

//                 // 5. Simulação: Salvar no Banco de Dados (DbContext)
//                 // await _context.AttendanceLists.AddAsync(attendanceList);
//                 // await _context.SaveChangesAsync();
                
//                 _logger.LogInformation($"Lista de Presença '{template.Header}' gerada com sucesso para {collaborators.Count} colaboradores.");
                
//                 return RedirectToAction("Details", new { id = 1 }); 
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Erro ao gerar lista de presença.");
//                 ModelState.AddModelError("", "Ocorreu um erro inesperado ao gerar a lista. Tente novamente.");
                
//                 // Recarrega dados de suporte em caso de erro
//                 viewModel.AvailableTemplates = GetSimulatedTemplates();
//                 viewModel.AvailableTeams = GetSimulatedTeams();
//                 return View(viewModel);
//             }
//         }
        
//         // ----------------------------------------------------------------------------------
//         // SIMULAÇÃO DE DADOS (AJUSTADA PARA SEU EquipeModel)
//         // ----------------------------------------------------------------------------------

//         private List<ListTemplate> GetSimulatedTemplates()
//         {
//             return new List<ListTemplate>
//             {
//                 new ListTemplate { Id = 1, Name = "Treinamento de Integração", Header = "LISTA DE PRESENÇA - TREINAMENTO DE INTEGRAÇÃO", Responsible = "RH-Geral", Type = ListType.Digital, CreatedAt = DateTime.Now },
//                 new ListTemplate { Id = 2, Name = "Reunião de Equipe Mensal", Header = "REGISTRO DE PRESENÇA - REUNIÃO DE RESULTADOS", Responsible = "Gerente", Type = ListType.Blank, CreatedAt = DateTime.Now.AddDays(-5) }
//             };
//         }
        
//         private List<EquipeModel> GetSimulatedTeams()
//         {
//             // CORREÇÃO: Usando a propriedade 'Nome' do seu EquipeModel
//             return new List<EquipeModel>
//             {
//                 new EquipeModel { Id = 1, Nome = "Desenvolvimento Backend" },
//                 new EquipeModel { Id = 2, Nome = "Design UX/UI" },
//                 new EquipeModel { Id = 3, Nome = "Vendas e Suporte" }
//             };
//         }
        
//         private List<CollaboratorModel> GetSimulatedCollaborators()
//         {
//             // Modelo de Colaborador Simulado interno
//             return new List<CollaboratorModel>
//             {
//                 // Equipe 1
//                 new CollaboratorModel { Id = 101, Name = "Alice Rocha", EquipeId = 1 },
//                 new CollaboratorModel { Id = 102, Name = "Bruno Costa", EquipeId = 1 },
//                 // Equipe 2
//                 new CollaboratorModel { Id = 201, Name = "Carla Dias", EquipeId = 2 },
//                 new CollaboratorModel { Id = 202, Name = "Daniela Lima", EquipeId = 2 },
//                 // Equipe 3
//                 new CollaboratorModel { Id = 301, Name = "Eduardo Neves", EquipeId = 3 },
//             };
//         }
        
//         // Modelo de suporte (simulação de colaborador)
//         // Mantive o nome 'CollaboratorModel' e a propriedade 'EquipeId' para a lógica de filtragem
//         private class CollaboratorModel
//         {
//             public int Id { get; set; }
//             public string Name { get; set; }
//             // Propriedade usada para linkar com o Id do EquipeModel na simulação
//             public int EquipeId { get; set; } 
//         }
//     }
// }