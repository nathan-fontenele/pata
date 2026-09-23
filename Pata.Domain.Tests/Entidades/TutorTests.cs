using Pata.Domain.Entidades.Tutor;
using Pata.Domain.Excecoes;
using Pata.Domain.ObjetosValor;

namespace Pata.Domain.Tests.Entidades;

public class TutorTests
{
    private static readonly Cpf CpfValido = new("529.982.247-25");
    private static readonly Email EmailValido = new("tutor@pata.com.br");
    private static readonly Telefone TelefoneValido = new("(11) 91234-5678");

    [Fact]
    public void DeveCriarTutorComDadosValidos()
    {
        var email = new Email("tutor@pata.com.br");
        var telefone = new Telefone("(11) 91234-5678");

        var tutor = new Tutor("  Maria Silva  ", CpfValido, email, telefone);

        Assert.NotEqual(Guid.Empty, tutor.Id);
        Assert.Equal("Maria Silva", tutor.Nome);
        Assert.Equal(CpfValido, tutor.Cpf);
        Assert.Equal(email, tutor.Email);
        Assert.Equal(telefone, tutor.Telefone);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void DeveRejeitarNomeInvalido(string nome)
    {
        Assert.Throws<ErroDeValidacao>(() =>
            new Tutor(nome, CpfValido, EmailValido, TelefoneValido));
    }

    [Fact]
    public void DeveRejeitarCpfNulo()
    {
        Assert.Throws<ErroDeValidacao>(() =>
            new Tutor("Maria Silva", null!, EmailValido, TelefoneValido));
    }

    [Fact]
    public void DeveRejeitarDadosDeContatoNulos()
    {
        Assert.Throws<ErroDeValidacao>(() =>
            new Tutor("Maria Silva", CpfValido, null!, TelefoneValido));
        Assert.Throws<ErroDeValidacao>(() =>
            new Tutor("Maria Silva", CpfValido, EmailValido, null!));
    }

    [Fact]
    public void DeveAlterarDadosDoTutor()
    {
        var tutor = CriarTutor();
        var email = new Email("novo@pata.com.br");
        var telefone = new Telefone("(11) 3456-7890");

        tutor.AlterarNome("  Maria Souza  ");
        tutor.AlterarEmail(email);
        tutor.AlterarTelefone(telefone);

        Assert.Equal("Maria Souza", tutor.Nome);
        Assert.Equal(email, tutor.Email);
        Assert.Equal(telefone, tutor.Telefone);
    }

    [Fact]
    public void DeveRejeitarRemocaoDosDadosDeContato()
    {
        var tutor = new Tutor(
            "Maria Silva",
            CpfValido,
            new Email("tutor@pata.com.br"),
            new Telefone("(11) 91234-5678"));

        Assert.Throws<ErroDeValidacao>(() => tutor.AlterarEmail(null!));
        Assert.Throws<ErroDeValidacao>(() => tutor.AlterarTelefone(null!));
    }

    [Fact]
    public void DeveExcluirTutorLogicamente()
    {
        var tutor = CriarTutor();
        var excluidoEm = new DateTime(2026, 8, 9, 12, 30, 0, DateTimeKind.Utc);

        tutor.Excluir(excluidoEm, "  usuario@pata.com.br  ");

        Assert.True(tutor.Excluido);
        Assert.Equal(excluidoEm, tutor.ExcluidoEm);
        Assert.Equal("usuario@pata.com.br", tutor.ExcluidoPor);
    }

    [Fact]
    public void DeveRecuperarTutorExcluido()
    {
        var tutor = CriarTutor();
        tutor.Excluir(DateTime.UtcNow, "usuario@pata.com.br");

        tutor.Recuperar();

        Assert.False(tutor.Excluido);
        Assert.Null(tutor.ExcluidoEm);
        Assert.Null(tutor.ExcluidoPor);
    }

    [Fact]
    public void DeveRejeitarDataDeExclusaoForaDeUtc()
    {
        var tutor = CriarTutor();
        var excluidoEm = new DateTime(2026, 8, 9, 12, 30, 0, DateTimeKind.Local);

        Assert.Throws<ErroDeValidacao>(() =>
            tutor.Excluir(excluidoEm, "usuario@pata.com.br"));
    }

    private static Tutor CriarTutor() =>
        new("Maria Silva", CpfValido, EmailValido, TelefoneValido);
}
