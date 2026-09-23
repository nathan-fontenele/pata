using Pata.Domain.Comum;
using Pata.Domain.Excecoes;

namespace Pata.Domain.Tests.Entidades;

public class EntidadeTests
{
    [Fact]
    public void EntidadesDoMesmoTipoEIdentificadorDevemSerIguais()
    {
        var id = Guid.NewGuid();

        var primeira = new EntidadeTeste(id);
        var segunda = new EntidadeTeste(id);

        Assert.Equal(primeira, segunda);
        Assert.Equal(primeira.GetHashCode(), segunda.GetHashCode());
    }

    [Fact]
    public void EntidadesComIdentificadoresDiferentesNaoDevemSerIguais()
    {
        var primeira = new EntidadeTeste(Guid.NewGuid());
        var segunda = new EntidadeTeste(Guid.NewGuid());

        Assert.NotEqual(primeira, segunda);
    }

    [Fact]
    public void DeveRejeitarIdentificadorVazio()
    {
        Assert.Throws<ErroDeValidacao>(() => new EntidadeTeste(Guid.Empty));
    }

    private sealed class EntidadeTeste(Guid id) : Entidade<Guid>(id);
}
