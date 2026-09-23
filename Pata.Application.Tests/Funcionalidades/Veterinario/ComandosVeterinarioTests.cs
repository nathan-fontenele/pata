using Pata.Application.Funcionalidades.Veterinario.Comandos.AtualizarVeterinario;
using Pata.Application.Funcionalidades.Veterinario.Comandos.CriarVeterinario;
using Pata.Application.Funcionalidades.Veterinario.Comandos.ExcluirVeterinario;
using Pata.Application.Funcionalidades.Veterinario.Comandos.RecuperarVeterinario;
using Pata.Application.Tests.Apoio;
using Pata.Domain.Excecoes;
using Pata.Domain.ObjetosValor;
using VeterinarioAgregado = Pata.Domain.Entidades.Veterinario.Veterinario;

namespace Pata.Application.Tests.Funcionalidades.Veterinario;

public class ComandosVeterinarioTests
{
    [Fact]
    public async Task DeveCriarVeterinarioEPropagarTokenCancelamento()
    {
        var repositorio = new RepositorioVeterinarioFalso();
        var manipulador = new CriarVeterinarioManipulador(repositorio);
        using var origemToken = new CancellationTokenSource();
        var comando = new CriarVeterinarioComando(
            "  Ana Silva  ",
            "veterinario@pata.com.br",
            "(11) 91234-5678",
            "0114212",
            "sp",
            "  Clinica  ");

        var id = await manipulador.Handle(comando, origemToken.Token);

        Assert.NotEqual(Guid.Empty, id);
        Assert.NotNull(repositorio.VeterinarioAdicionado);
        Assert.Equal("Ana Silva", repositorio.VeterinarioAdicionado.Nome);
        Assert.Equal("114212/SP", repositorio.VeterinarioAdicionado.Crmv.Valor);
        Assert.Equal("Clinica", repositorio.VeterinarioAdicionado.Especialidade);
        Assert.Equal(origemToken.Token, repositorio.UltimoTokenCancelamento);
    }

    [Fact]
    public async Task DeveRejeitarCrmvExistenteIncluindoVeterinariosExcluidos()
    {
        var repositorio = new RepositorioVeterinarioFalso
        {
            CrmvExisteIncluindoExcluidos = true
        };
        var manipulador = new CriarVeterinarioManipulador(repositorio);
        var comando = new CriarVeterinarioComando(
            "Ana Silva",
            "veterinario@pata.com.br",
            "(11) 91234-5678",
            "114212",
            "SP",
            "Clinica");

        await Assert.ThrowsAsync<ConflitoException>(() =>
            manipulador.Handle(comando, CancellationToken.None));

        Assert.Null(repositorio.VeterinarioAdicionado);
    }

    [Fact]
    public async Task DeveAtualizarVeterinarioSemAlterarCrmv()
    {
        var veterinario = CriarVeterinario();
        var crmvOriginal = veterinario.Crmv;
        var repositorio = new RepositorioVeterinarioFalso
        {
            VeterinarioAtivo = veterinario
        };
        var manipulador = new AtualizarVeterinarioManipulador(repositorio);
        var comando = new AtualizarVeterinarioComando(
            veterinario.Id,
            "  Ana Souza  ",
            "ana.souza@pata.com.br",
            "(11) 3456-7890",
            "  Cirurgia  ");

        await manipulador.Handle(comando, CancellationToken.None);

        Assert.Same(veterinario, repositorio.VeterinarioAtualizado);
        Assert.Equal("Ana Souza", veterinario.Nome);
        Assert.Equal("ana.souza@pata.com.br", veterinario.Email.Valor);
        Assert.Equal("1134567890", veterinario.Telefone.Valor);
        Assert.Equal("Cirurgia", veterinario.Especialidade);
        Assert.Equal(crmvOriginal, veterinario.Crmv);
    }

    [Fact]
    public async Task DeveExcluirVeterinarioLogicamente()
    {
        var veterinario = CriarVeterinario();
        var repositorio = new RepositorioVeterinarioFalso
        {
            VeterinarioAtivo = veterinario
        };
        var excluidoEm = new DateTime(2026, 9, 12, 15, 0, 0, DateTimeKind.Utc);
        var manipulador = new ExcluirVeterinarioManipulador(
            repositorio,
            new RelogioFalso(excluidoEm),
            new UsuarioAtualFalso("usuario@pata.com.br"));

        await manipulador.Handle(
            new ExcluirVeterinarioComando(veterinario.Id),
            CancellationToken.None);

        Assert.Same(veterinario, repositorio.VeterinarioAtualizado);
        Assert.True(veterinario.Excluido);
        Assert.Equal(excluidoEm, veterinario.ExcluidoEm);
        Assert.Equal("usuario@pata.com.br", veterinario.ExcluidoPor);
    }

    [Fact]
    public async Task DeveRecuperarVeterinarioExcluido()
    {
        var veterinario = CriarVeterinario();
        veterinario.Excluir(DateTime.UtcNow, "usuario@pata.com.br");
        var repositorio = new RepositorioVeterinarioFalso
        {
            VeterinarioIncluindoExcluidos = veterinario
        };
        var manipulador = new RecuperarVeterinarioManipulador(repositorio);

        await manipulador.Handle(
            new RecuperarVeterinarioComando(veterinario.Id),
            CancellationToken.None);

        Assert.Same(veterinario, repositorio.VeterinarioAtualizado);
        Assert.False(veterinario.Excluido);
        Assert.Null(veterinario.ExcluidoEm);
        Assert.Null(veterinario.ExcluidoPor);
    }

    [Fact]
    public async Task DeveInformarQuandoVeterinarioNaoForEncontradoParaAtualizacao()
    {
        var manipulador = new AtualizarVeterinarioManipulador(
            new RepositorioVeterinarioFalso());
        var comando = new AtualizarVeterinarioComando(
            Guid.NewGuid(),
            "Ana Silva",
            "veterinario@pata.com.br",
            "(11) 91234-5678",
            "Clinica");

        await Assert.ThrowsAsync<RecursoNaoEncontradoException>(() =>
            manipulador.Handle(comando, CancellationToken.None));
    }

    private static VeterinarioAgregado CriarVeterinario() => new(
        "Ana Silva",
        new Email("veterinario@pata.com.br"),
        new Telefone("(11) 91234-5678"),
        new Crmv("114212", "SP"),
        "Clinica");
}
