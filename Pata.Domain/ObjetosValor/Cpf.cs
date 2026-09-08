namespace Pata.Domain.ObjetosValor;

public sealed record Cpf
{
    private const int QuantidadeDigitos = 11;

    public string Valor { get; }

    public Cpf(string valor)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(valor);

        var cpfNormalizado = Normalizar(valor);

        if (!EhValido(cpfNormalizado))
            throw new ArgumentException("CPF invalido.", nameof(valor));

        Valor = cpfNormalizado;
    }

    public static bool EhValido(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return false;

        string cpf;
        try
        {
            cpf = Normalizar(valor);
        }
        catch (ArgumentException)
        {
            return false;
        }

        if (cpf.Length != QuantidadeDigitos || cpf.Distinct().Count() == 1)
            return false;

        return CalcularDigito(cpf.AsSpan(0, 9), 10) == cpf[9] - '0'
               && CalcularDigito(cpf.AsSpan(0, 10), 11) == cpf[10] - '0';
    }

    public string Formatado => Convert.ToUInt64(Valor).ToString(@"000\.000\.000\-00");

    public override string ToString() => Valor;

    private static string Normalizar(string valor)
    {
        if (valor.Any(caractere => !char.IsDigit(caractere)
                                   && caractere is not '.' and not '-'
                                   && !char.IsWhiteSpace(caractere)))
            throw new ArgumentException("CPF contem caracteres invalidos.", nameof(valor));

        return string.Concat(valor.Where(char.IsDigit));
    }

    private static int CalcularDigito(ReadOnlySpan<char> digitos, int pesoInicial)
    {
        var soma = 0;

        for (var indice = 0; indice < digitos.Length; indice++)
            soma += (digitos[indice] - '0') * (pesoInicial - indice);

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
