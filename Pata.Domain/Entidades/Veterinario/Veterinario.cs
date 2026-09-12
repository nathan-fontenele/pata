using Pata.Domain.Comum;
using Pata.Domain.ObjetosValor;

namespace Pata.Domain.Entidades.Veterinario;

public sealed class Veterinario : RaizAgregadaAuditavel<Guid>
{
    public const int TamanhoMaximoNome = 100;
    public const int TamanhoMaximoEspecialidade = 100;

    public string Nome { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Telefone Telefone { get; private set; } = null!;
    public Crmv Crmv { get; private set; } = null!;
    public string Especialidade { get; private set; } = null!;

    public Veterinario(
        string nome,
        Email email,
        Telefone telefone,
        Crmv crmv,
        string especialidade) : base(Guid.NewGuid())
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(telefone);
        ArgumentNullException.ThrowIfNull(crmv);

        Nome = ValidarTexto(nome, TamanhoMaximoNome, nameof(nome));
        Email = email;
        Telefone = telefone;
        Crmv = crmv;
        Especialidade = ValidarTexto(
            especialidade,
            TamanhoMaximoEspecialidade,
            nameof(especialidade));
    }

    public void AlterarNome(string nome) =>
        Nome = ValidarTexto(nome, TamanhoMaximoNome, nameof(nome));

    public void AlterarEmail(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);
        Email = email;
    }

    public void AlterarTelefone(Telefone telefone)
    {
        ArgumentNullException.ThrowIfNull(telefone);
        Telefone = telefone;
    }

    public void AlterarEspecialidade(string especialidade) =>
        Especialidade = ValidarTexto(
            especialidade,
            TamanhoMaximoEspecialidade,
            nameof(especialidade));

    private static string ValidarTexto(string valor, int tamanhoMaximo, string nomeParametro)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(valor, nomeParametro);

        var valorNormalizado = valor.Trim();

        if (valorNormalizado.Length > tamanhoMaximo)
            throw new ArgumentOutOfRangeException(
                nomeParametro,
                $"O campo deve ter no maximo {tamanhoMaximo} caracteres.");

        return valorNormalizado;
    }
}
