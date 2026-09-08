using Pata.Domain.ObjetosValor;

namespace Pata.Domain.Tests.ObjetosValor;

public class CpfTests
{
    [Theory]
    [InlineData("529.982.247-25", "52998224725")]
    [InlineData("52998224725", "52998224725")]
    public void DeveAceitarCpfValido(string entrada, string esperado)
    {
        var cpf = new Cpf(entrada);

        Assert.Equal(esperado, cpf.Valor);
    }

    [Theory]
    [InlineData("529.982.247-24")]
    [InlineData("111.111.111-11")]
    [InlineData("123")]
    [InlineData("")]
    public void DeveRejeitarCpfInvalido(string entrada)
    {
        Assert.Throws<ArgumentException>(() => new Cpf(entrada));
    }
}
