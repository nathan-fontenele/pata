namespace Pata.Application.Funcionalidades.Veterinario.Consultas;

public sealed record VeterinarioResumoDto(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string Crmv,
    string Especialidade,
    bool Excluido,
    DateTime? ExcluidoEm);
