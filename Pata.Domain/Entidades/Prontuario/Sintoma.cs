using Pata.Domain.Excecoes;

namespace Pata.Domain.Entidades.Prontuario;

public sealed record Sintoma
{
    public const int TamanhoMaximoDescricao = 500;

    public Sintoma(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ErroDeValidacao("Descricao e obrigatoria.", nameof(descricao));

        var descricaoNormalizada = descricao.Trim();

        if (descricaoNormalizada.Length > TamanhoMaximoDescricao)
            throw new ErroDeValidacao(
                $"A descricao do sintoma deve ter no maximo {TamanhoMaximoDescricao} caracteres.",
                nameof(descricao));

        Descricao = descricaoNormalizada;
    }

    public string Descricao { get; }

    public override string ToString() => Descricao;
}
