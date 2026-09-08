namespace Pata.Domain.Comum;

public abstract class RaizAgregadaAuditavel<TId> : Entidade<TId>, IAuditavel, IExcluivel
    where TId : notnull
{
    protected RaizAgregadaAuditavel()
    {
    }

    protected RaizAgregadaAuditavel(TId id) : base(id)
    {
    }

    public DateTime CriadoEm { get; private set; }
    public string CriadoPor { get; private set; } = default!;
    public DateTime? AtualizadoEm { get; private set; }
    public string? AtualizadoPor { get; private set; }
    public bool Excluido { get; private set; }
    public DateTime? ExcluidoEm { get; private set; }
    public string? ExcluidoPor { get; private set; }

    public void Recuperar()
    {
        Excluido = false;
        ExcluidoEm = null;
        ExcluidoPor = null;
    }
}
