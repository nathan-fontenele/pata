namespace Pata.Domain.Comum;

public abstract class Entidade<TId>
    where TId : notnull
{
    protected Entidade()
    {
    }

    protected Entidade(TId id)
    {
        if (EqualityComparer<TId>.Default.Equals(id, default!))
            throw new ArgumentException("O identificador e obrigatorio.", nameof(id));

        Id = id;
    }

    public TId Id { get; private set; } = default!;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is not Entidade<TId> outra || GetType() != outra.GetType())
            return false;

        if (EqualityComparer<TId>.Default.Equals(Id, default!)
            || EqualityComparer<TId>.Default.Equals(outra.Id, default!))
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, outra.Id);
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
}
