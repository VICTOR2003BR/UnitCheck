using System.ComponentModel.DataAnnotations;

// Modelo que representa um "Tipo de Lista" reutilizável.
// Os usuários criarão instâncias deste modelo para definir cabeçalhos, descrições e o tipo de uso da lista.
public class ListTemplate
{
    // Chave primária do Template de Lista
    public int Id { get; set; }

    // Nome do Template (Ex: "Lista de Presença para Treinamento", "Roster Mensal de Equipe")
    [Required(ErrorMessage = "O nome do template é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    [Display(Name = "Nome do Template")]
    public string Name { get; set; }

    // Campo opcional para texto grande no topo da lista (cabeçalho customizado).
    [Display(Name = "Cabeçalho da Lista (Ex: Título do Treinamento)")]
    [DataType(DataType.MultilineText)]
    public string Header { get; set; }

    // Campo opcional para instruções ou detalhes adicionais.
    [Display(Name = "Descrição/Instruções")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    // Indica se esta lista precisa de um campo "Responsável" ou "Instrutor" (Ex: para treinamentos).
    [Display(Name = "Requer Campo de Responsável/Instrutor?")]
    public bool RequiresResponsible { get; set; } = false;

    // Indica o uso principal: se é para marcação de presença no sistema
    // (Presença, Ausente, Justificado) ou apenas para impressão de um roster em branco.
    [Display(Name = "É uma Lista de Presença para Marcação no Sistema?")]
    public bool IsAttendanceList { get; set; } = true;
    
    // Data de criação do template (para fins de auditoria)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}