using Pata.Domain.Entidades.Tutor;
using Pata.Domain.ObjetosValor;

namespace Pata.Domain.Tests.Entidades;

public class TutorTests
{
    private static readonly Cpf CpfValido = new("529.982.247-25");

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
        Assert.Throws<ArgumentException>(() => new Tutor(nome, CpfValido, null, null));
    }

    [Fact]
    public void DeveRejeitarCpfNulo()
    {
        Assert.Throws<ArgumentNullException>(() => new Tutor("Maria Silva", null!, null, null));
    }

    [Fact]
    public void DeveAlterarDadosDoTutor()
    {
        var tutor = new Tutor("Maria Silva", CpfValido, null, null);
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
    public void DevePermitirRemoverDadosDeContato()
    {
        var tutor = new Tutor(
            "Maria Silva",
            CpfValido,
            new Email("tutor@pata.com.br"),
            new Telefone("(11) 91234-5678"));

        tutor.AlterarEmail(null);
        tutor.AlterarTelefone(null);

        Assert.Null(tutor.Email);
        Assert.Null(tutor.Telefone);
    }
}
