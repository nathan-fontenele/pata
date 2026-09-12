using Pata.Application.Comum.Modelos;
using Pata.Application.Funcionalidades.Veterinario.Consultas;
using Pata.Application.Funcionalidades.Veterinario.Consultas.ListarVeterinarios;
using Pata.Application.Funcionalidades.Veterinario.Consultas.ListarVeterinariosExcluidos;
using Pata.Application.Funcionalidades.Veterinario.Consultas.ObterVeterinarioPorCrmv;
using Pata.Application.Tests.Apoio;

namespace Pata.Application.Tests.Funcionalidades.Veterinario;

public class ConsultasVeterinarioTests
{
    [Fact]
    public async Task DeveNormalizarCrmvAoObterVeterinario()
    {
        var veterinario = CriarResumo();
        var portaConsulta = new ConsultaVeterinariosFalsa
        {
            Veterinario = veterinario
        };
        var manipulador = new ObterVeterinarioPorCrmvManipulador(portaConsulta);

        var resultado = await manipulador.Handle(
            new ObterVeterinarioPorCrmvConsulta("0114212", "sp"),
            CancellationToken.None);

        Assert.Same(veterinario, resultado);
        Assert.NotNull(portaConsulta.CrmvConsultado);
        Assert.Equal("114212/SP", portaConsulta.CrmvConsultado.Valor);
    }

    [Fact]
    public async Task DeveListarVeterinariosComPaginacao()
    {
        var resposta = new RespostaPaginada<VeterinarioResumoDto>(
            [CriarResumo()],
            2,
            10,
            11);
        var portaConsulta = new ConsultaVeterinariosFalsa
        {
            Veterinarios = resposta
        };
        var manipulador = new ListarVeterinariosManipulador(portaConsulta);
        using var origemToken = new CancellationTokenSource();

        var resultado = await manipulador.Handle(
            new ListarVeterinariosConsulta(2, 10),
            origemToken.Token);

        Assert.Same(resposta, resultado);
        Assert.Equal(2, portaConsulta.PaginaConsultada);
        Assert.Equal(10, portaConsulta.TamanhoPaginaConsultado);
        Assert.False(portaConsulta.ConsultouExcluidos);
        Assert.Equal(origemToken.Token, portaConsulta.UltimoTokenCancelamento);
    }

    [Fact]
    public async Task DeveListarSomenteVeterinariosExcluidos()
    {
        var resposta = new RespostaPaginada<VeterinarioResumoDto>([], 1, 20, 0);
        var portaConsulta = new ConsultaVeterinariosFalsa
        {
            VeterinariosExcluidos = resposta
        };
        var manipulador = new ListarVeterinariosExcluidosManipulador(portaConsulta);

        var resultado = await manipulador.Handle(
            new ListarVeterinariosExcluidosConsulta(),
            CancellationToken.None);

        Assert.Same(resposta, resultado);
        Assert.True(portaConsulta.ConsultouExcluidos);
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(-1, 20)]
    public async Task DeveRejeitarPaginacaoInvalida(int pagina, int tamanhoPagina)
    {
        var manipulador = new ListarVeterinariosManipulador(
            new ConsultaVeterinariosFalsa());

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            manipulador.Handle(
                new ListarVeterinariosConsulta(pagina, tamanhoPagina),
                CancellationToken.None));
    }

    private static VeterinarioResumoDto CriarResumo() => new(
        Guid.NewGuid(),
        "Ana Silva",
        "veterinario@pata.com.br",
        "11912345678",
        "114212/SP",
        "Clinica",
        false,
        null);
}
