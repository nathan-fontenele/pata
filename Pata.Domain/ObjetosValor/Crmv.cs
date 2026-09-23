using Pata.Domain.Excecoes;

namespace Pata.Domain.ObjetosValor;

public sealed record Crmv
{
    private const int TamanhoMaximoNumero = 6;

    private static readonly HashSet<string> UfsValidas = new(StringComparer.Ordinal)
    {
        "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
        "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
        "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
    };

    public string Numero { get; }
    public string Uf { get; }
    public string Valor => $"{Numero}/{Uf}";
    public string Formatado => $"CRMV-{Uf} nº {Numero.PadLeft(5, '0')}";

    public Crmv(string numero, string uf)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ErroDeValidacao("Numero do CRMV e obrigatorio.", nameof(numero));
        if (string.IsNullOrWhiteSpace(uf))
            throw new ErroDeValidacao("UF do CRMV e obrigatoria.", nameof(uf));

        var numeroNormalizado = NormalizarNumero(numero);
        var ufNormalizada = NormalizarUf(uf);

        if (!EhValidoNormalizado(numeroNormalizado, ufNormalizada))
            throw new ErroDeValidacao("CRMV invalido.");

        Numero = numeroNormalizado;
        Uf = ufNormalizada;
    }

    public static bool EhValido(string? numero, string? uf)
    {
        if (string.IsNullOrWhiteSpace(numero) || string.IsNullOrWhiteSpace(uf))
            return false;

        try
        {
            return EhValidoNormalizado(NormalizarNumero(numero), NormalizarUf(uf));
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public override string ToString() => Valor;

    private static string NormalizarNumero(string numero)
    {
        var numeroSemEspacos = numero.Trim();

        if (numeroSemEspacos.Any(caractere => caractere is < '0' or > '9'))
            throw new ErroDeValidacao(
                "O numero do CRMV deve conter apenas digitos.",
                nameof(numero));

        return numeroSemEspacos.TrimStart('0') switch
        {
            "" => "0",
            var valor => valor
        };
    }

    private static string NormalizarUf(string uf) => uf.Trim().ToUpperInvariant();

    private static bool EhValidoNormalizado(string numero, string uf) =>
        numero.Length <= TamanhoMaximoNumero
        && numero != "0"
        && UfsValidas.Contains(uf);
}
