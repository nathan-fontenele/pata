using Pata.Domain.Entidades.Veterinario;
using Pata.Domain.Excecoes;
using Pata.Domain.ObjetosValor;

namespace Pata.Domain.Tests.Entidades;

public class VeterinarioTests
{
    private static readonly Email EmailValido = new("veterinario@pata.com.br");
    private static readonly Telefone TelefoneValido = new("(11) 91234-5678");
    private static readonly Crmv CrmvValido = new("114212", "SP");

    [Fact]
    public void DeveCadastrarVeterinarioComCrmvValido()
    {
        var veterinario = new Veterinario(
            "  Ana Silva  ",
            EmailValido,
            TelefoneValido,
            CrmvValido,
            "  Clinica de pequenos animais  ");

        Assert.NotEqual(Guid.Empty, veterinario.Id);
        Assert.Equal("Ana Silva", veterinario.Nome);
        Assert.Equal(EmailValido, veterinario.Email);
        Assert.Equal(TelefoneValido, veterinario.Telefone);
        Assert.Equal(CrmvValido, veterinario.Crmv);
        Assert.Equal("Clinica de pequenos animais", veterinario.Especialidade);
    }

    [Fact]
    public void DeveRejeitarCrmvNuloNoCadastro()
    {
        Assert.Throws<ErroDeValidacao>(() => new Veterinario(
            "Ana Silva",
            EmailValido,
            TelefoneValido,
            null!,
            "Clinica de pequenos animais"));
    }

    [Theory]
    [InlineData("", "Clinica")]
    [InlineData("   ", "Clinica")]
    [InlineData("Ana Silva", "")]
    [InlineData("Ana Silva", "   ")]
    public void DeveRejeitarDadosObrigatoriosInvalidos(string nome, string especialidade)
    {
        Assert.Throws<ErroDeValidacao>(() => new Veterinario(
            nome,
            EmailValido,
            TelefoneValido,
            CrmvValido,
            especialidade));
    }

    [Fact]
    public void DeveRejeitarObjetosValorNulos()
    {
        Assert.Throws<ErroDeValidacao>(() => new Veterinario(
            "Ana Silva", null!, TelefoneValido, CrmvValido, "Clinica"));
        Assert.Throws<ErroDeValidacao>(() => new Veterinario(
            "Ana Silva", EmailValido, null!, CrmvValido, "Clinica"));
    }

    [Fact]
    public void DeveAlterarDadosDoVeterinario()
    {
        var veterinario = new Veterinario(
            "Ana Silva",
            EmailValido,
            TelefoneValido,
            CrmvValido,
            "Clinica");
        var novoEmail = new Email("ana.silva@pata.com.br");
        var novoTelefone = new Telefone("(11) 3456-7890");

        veterinario.AlterarNome("  Ana Souza  ");
        veterinario.AlterarEmail(novoEmail);
        veterinario.AlterarTelefone(novoTelefone);
        veterinario.AlterarEspecialidade("  Cirurgia  ");

        Assert.Equal("Ana Souza", veterinario.Nome);
        Assert.Equal(novoEmail, veterinario.Email);
        Assert.Equal(novoTelefone, veterinario.Telefone);
        Assert.Equal("Cirurgia", veterinario.Especialidade);
        Assert.Equal(CrmvValido, veterinario.Crmv);
    }
}
