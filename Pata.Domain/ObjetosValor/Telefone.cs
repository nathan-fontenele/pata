namespace Pata.Domain.ObjetosValor;

public sealed record Telefone
{
    private const string CodigoPaisBrasil = "55";

    public string Valor { get; }
    public string Ddd => Valor[..2];
    public string Numero => Valor[2..];
    public TipoTelefone Tipo => Numero.Length == 9 ? TipoTelefone.Celular : TipoTelefone.Fixo;

    public Telefone(string valor)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(valor);

        var telefoneNormalizado = Normalizar(valor);

        if (!EhValidoNormalizado(telefoneNormalizado))
            throw new ArgumentException("Telefone invalido.", nameof(valor));

        Valor = telefoneNormalizado;
    }

    public static bool EhValido(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return false;

        try
        {
            return EhValidoNormalizado(Normalizar(valor));
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public string Formatado => Tipo == TipoTelefone.Celular
        ? $"({Ddd}) {Numero[..5]}-{Numero[5..]}"
        : $"({Ddd}) {Numero[..4]}-{Numero[4..]}";

    public override string ToString() => Valor;

    private static string Normalizar(string valor)
    {
        if (valor.Any(caractere => !char.IsDigit(caractere)
                                   && caractere is not '(' and not ')' and not '-' and not '+' and not '.'
                                   && !char.IsWhiteSpace(caractere)))
            throw new ArgumentException("Telefone contem caracteres invalidos.", nameof(valor));

        var digitos = string.Concat(valor.Where(char.IsDigit));

        // Aceita tambem o formato internacional brasileiro: +55 (DD) numero.
        if (digitos.Length is 12 or 13 && digitos.StartsWith(CodigoPaisBrasil, StringComparison.Ordinal))
            digitos = digitos[CodigoPaisBrasil.Length..];

        return digitos;
    }

    private static bool EhValidoNormalizado(string telefone)
    {
        if (telefone.Length is not (10 or 11))
            return false;

        var dddValido = telefone[0] is >= '1' and <= '9';
        if (!dddValido)
            return false;

        var numero = telefone.AsSpan(2);

        return numero.Length switch
        {
            8 => numero[0] is >= '2' and <= '5',
            9 => numero[0] == '9',
            _ => false
        };
    }
}

public enum TipoTelefone
{
    Fixo,
    Celular
}
