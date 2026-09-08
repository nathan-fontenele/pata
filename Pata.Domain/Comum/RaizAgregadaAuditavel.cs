namespace Pata.Domain.Comum;

public abstract class RaizAgregadaAuditavel
{
    bool Excluido { get; }
    DateTime? ExcluidoEm { get; }
    string? ExcluidoPor { get; }
}