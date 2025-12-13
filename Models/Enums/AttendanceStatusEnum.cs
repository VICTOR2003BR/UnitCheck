namespace UnitCheck.Models.Enums
{
    // Define os possíveis status de presença de um colaborador em uma lista.
    public enum AttendanceStatus
    {
        // Padrão: Não Marado (Estado inicial antes de qualquer ação)
        Unmarked = 0, 
        
        // Colaborador presente no evento/treinamento
        Presente = 1,
        
        // Colaborador ausente sem justificativa
        Ausente = 2,
        
        // Colaborador ausente com justificativa (Ex: Atestado, Férias)
        Justificado = 3
    }
}