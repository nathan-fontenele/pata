using System.Net.Mail;

namespace Pata.Domain.ObjetosValor;

public sealed record Email
{
    private const int TamanhoMaximo = 254;

    public string Valor { get; }

    public Email(string valor)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(valor);

        var emailNormalizado = valor.Trim();

        if (!EhValido(emailNormalizado))
            throw new ArgumentException("E-mail invalido.", nameof(valor));

        Valor = emailNormalizado;
    }

    public static bool EhValido(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return false;

        var email = valor.Trim();
        if (email.Length > TamanhoMaximo || email.Any(char.IsWhiteSpace))
            return false;

        var separador = email.LastIndexOf('@');
        if (separador <= 0 || separador == email.Length - 1)
            return false;

        var parteLocal = email[..separador];
        var dominio = email[(separador + 1)..];

        if (parteLocal.Length > 64
            || parteLocal.StartsWith('.')
            || parteLocal.EndsWith('.')
            || parteLocal.Contains("..", StringComparison.Ordinal)
            || dominio.StartsWith('.')
            || dominio.EndsWith('.')
            || dominio.Contains("..", StringComparison.Ordinal)
            || !dominio.Contains('.'))
            return false;

        try
        {
            var endereco = new MailAddress(email);
            return endereco.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public override string ToString() => Valor;
}
