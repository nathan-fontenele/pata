namespace Pata.Domain.Comum;

public interface IAuditavel
{
    DateTime CriadoEm { get; }
    string CriadoPor { get; }
    DateTime? AtualizadoEm { get; }
    string? AtualizadoPor { get; }
}