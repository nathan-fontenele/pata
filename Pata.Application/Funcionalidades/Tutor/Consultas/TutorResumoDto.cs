namespace Pata.Application.Funcionalidades.Tutor.Consultas;

public sealed record TutorResumoDto(
    Guid Id,
    string Nome,
    string Cpf,
    string? Email,
    string? Telefone,
    bool Excluido,
    DateTime? ExcluidoEm);
