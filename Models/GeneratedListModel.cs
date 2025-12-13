using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnitCheck.Models.Enums;

// Representa uma lista de presença específica gerada (instância do template)
public class AttendanceList
{
    public int Id { get; set; }

    // Chave estrangeira para o template de lista usado
    [Display(Name = "Template de Origem")]
    public int ListTemplateId { get; set; }

    // Propriedade de navegação
    public ListTemplate ListTemplate { get; set; }

    // Data e hora em que esta lista foi gerada
    public DateTime GenerationDate { get; set; } = DateTime.Now;

    // Campo para registrar quem aplicou o treinamento/evento (Copiado do template no momento da geração ou preenchido pelo usuário)
    [StringLength(100)]
    [Display(Name = "Responsável / Instrutor")]
    public string Responsible { get; set; }
    
    // Lista de IDs das equipes incluídas nesta lista de presença (Armazenada como JSON string ou em uma tabela de junção se for EF Core)
    // Para simplificar, assumimos que será armazenado como uma string (ex: "1,2,5") ou JSON se o banco suportar.
    // Em EF Core, uma tabela de junção seria a abordagem ideal (AttendanceListEquipe).
    [Display(Name = "Equipes Incluídas (IDs)")]
    public string IncludedTeamIds { get; set; }

    // Indica o tipo de uso final da lista no momento da geração (baseado na escolha do usuário, que pode sobrescrever o template)
    [Display(Name = "Tipo de Geração")]
    public ListType GeneratedType { get; set; }
    
    // Coleção dos registros de presença individuais vinculados a esta lista
    public ICollection<AttendanceRecord> Records { get; set; } = new List<AttendanceRecord>();
}

// Enum simples para o tipo de uso da lista
public enum ListType 
{
    Digital = 0, // Para marcação de presença no sistema
    Blank = 1    // Para impressão e assinatura manual
}