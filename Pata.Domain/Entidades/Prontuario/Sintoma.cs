namespace Pata.Domain.Entidades.Prontuario;

public sealed record Sintoma
{
    public const int TamanhoMaximoDescricao = 500;

    public Sintoma(string descricao)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(descricao);

        var descricaoNormalizada = descricao.Trim();

        if (descricaoNormalizada.Length > TamanhoMaximoDescricao)
            throw new ArgumentOutOfRangeException(
                nameof(descricao),
                $"A descricao do sintoma deve ter no maximo {TamanhoMaximoDescricao} caracteres.");

        Descricao = descricaoNormalizada;
    }

    public string Descricao { get; }

    public override string ToString() => Descricao;
}
