using Pata.Domain.ObjetosValor;
using Pata.Domain.Excecoes;

namespace Pata.Domain.Tests.ObjetosValor;

public class TelefoneTests
{
    [Theory]
    [InlineData("(11) 3456-7890")]
    [InlineData("1134567890")]
    public void DeveAceitarTelefoneFixo(string entrada)
    {
        var telefone = new Telefone(entrada);

        Assert.Equal("1134567890", telefone.Valor);
        Assert.Equal(TipoTelefone.Fixo, telefone.Tipo);
        Assert.Equal("(11) 3456-7890", telefone.Formatado);
    }

    [Theory]
    [InlineData("(11) 91234-5678")]
    [InlineData("11912345678")]
    [InlineData("+55 (11) 91234-5678")]
    public void DeveAceitarTelefoneCelular(string entrada)
    {
        var telefone = new Telefone(entrada);

        Assert.Equal("11912345678", telefone.Valor);
        Assert.Equal(TipoTelefone.Celular, telefone.Tipo);
        Assert.Equal("(11) 91234-5678", telefone.Formatado);
    }

    [Theory]
    [InlineData("(11) 61234-5678")]
    [InlineData("(11) 8123-4567")]
    [InlineData("12345")]
    [InlineData("telefone")]
    [InlineData("")]
    public void DeveRejeitarTelefoneInvalido(string entrada)
    {
        Assert.Throws<ErroDeValidacao>(() => new Telefone(entrada));
    }
}
