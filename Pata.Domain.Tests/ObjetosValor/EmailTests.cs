using Pata.Domain.ObjetosValor;
using Pata.Domain.Excecoes;

namespace Pata.Domain.Tests.ObjetosValor;

public class EmailTests
{
    [Theory]
    [InlineData("tutor@pata.com.br")]
    [InlineData("contato+pet@pata.org")]
    public void DeveAceitarEmailValido(string entrada)
    {
        var email = new Email(entrada);

        Assert.Equal(entrada, email.Valor);
    }

    [Theory]
    [InlineData("tutor")]
    [InlineData("@pata.com.br")]
    [InlineData("tutor@pata")]
    [InlineData("tutor..silva@pata.com.br")]
    [InlineData("")]
    public void DeveRejeitarEmailInvalido(string entrada)
    {
        Assert.Throws<ErroDeValidacao>(() => new Email(entrada));
    }
}
