using Pata.Domain.ObjetosValor;

namespace Pata.Domain.Tests.ObjetosValor;

public class CrmvTests
{
    [Theory]
    [InlineData("1012", "SP", "1012/SP")]
    [InlineData(" 07715 ", " pr ", "7715/PR")]
    [InlineData("114212", "SP", "114212/SP")]
    [InlineData("0388", "to", "388/TO")]
    public void DeveAceitarCrmvValidoEmTerritorioNacional(
        string numero,
        string uf,
        string valorEsperado)
    {
        var crmv = new Crmv(numero, uf);

        Assert.Equal(valorEsperado, crmv.Valor);
    }

    [Theory]
    [InlineData("1234", "AC")]
    [InlineData("1234", "AL")]
    [InlineData("1234", "AP")]
    [InlineData("1234", "AM")]
    [InlineData("1234", "BA")]
    [InlineData("1234", "CE")]
    [InlineData("1234", "DF")]
    [InlineData("1234", "ES")]
    [InlineData("1234", "GO")]
    [InlineData("1234", "MA")]
    [InlineData("1234", "MT")]
    [InlineData("1234", "MS")]
    [InlineData("1234", "MG")]
    [InlineData("1234", "PA")]
    [InlineData("1234", "PB")]
    [InlineData("1234", "PR")]
    [InlineData("1234", "PE")]
    [InlineData("1234", "PI")]
    [InlineData("1234", "RJ")]
    [InlineData("1234", "RN")]
    [InlineData("1234", "RS")]
    [InlineData("1234", "RO")]
    [InlineData("1234", "RR")]
    [InlineData("1234", "SC")]
    [InlineData("1234", "SP")]
    [InlineData("1234", "SE")]
    [InlineData("1234", "TO")]
    public void DeveAceitarTodasAsUnidadesFederativas(string numero, string uf)
    {
        Assert.True(Crmv.EhValido(numero, uf));
    }

    [Theory]
    [InlineData("", "SP")]
    [InlineData("0", "SP")]
    [InlineData("000000", "SP")]
    [InlineData("123A", "SP")]
    [InlineData("١٢٣٤", "SP")]
    [InlineData("1234567", "SP")]
    [InlineData("1234", "XX")]
    [InlineData("1234", "")]
    public void DeveRejeitarCrmvInvalido(string numero, string uf)
    {
        Assert.False(Crmv.EhValido(numero, uf));
        Assert.Throws<ArgumentException>(() => new Crmv(numero, uf));
    }

    [Fact]
    public void DeveFormatarCrmv()
    {
        var crmv = new Crmv("07715", "pr");

        Assert.Equal("CRMV-PR nº 07715", crmv.Formatado);
        Assert.Equal("7715/PR", crmv.ToString());
    }
}
