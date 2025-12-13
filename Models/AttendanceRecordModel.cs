using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnitCheck.Models.Enums;

// Representa a linha de um colaborador em uma lista de presença gerada.
public class AttendanceRecord
{
    public int Id { get; set; }

    // Chave estrangeira para a lista de presença à qual este registro pertence
    [Display(Name = "Lista de Presença")]
    public int AttendanceListId { get; set; }
    public AttendanceList AttendanceList { get; set; }

    // Chave estrangeira para o colaborador
    // (Ainda precisamos do modelo 'Collaborator' ou 'Employee', mas assumimos 'CollaboratorId' por enquanto)
    [Display(Name = "Colaborador")]
    public int CollaboratorId { get; set; }
    // public Collaborator Collaborator { get; set; } // Propriedade de navegação para o modelo de Colaborador

    // Status de presença atual (Presente, Ausente, Justificado)
    [Display(Name = "Status de Presença")]
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Unmarked;
    
    // Campo opcional para registrar a justificativa de ausência.
    [StringLength(255)]
    [Display(Name = "Justificativa")]
    public string Justification { get; set; }

    // Campo opcional que pode ser usado para armazenar a imagem/string da assinatura digital, se aplicável.
    [DataType(DataType.Text)]
    [Display(Name = "Assinatura")]
    public string SignatureData { get; set; } 
    
    // Data da última atualização do status
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}