using Pata.Application.Comum.Abstracoes;
using Pata.Application.Comum.Modelos;
using Pata.Application.Funcionalidades.Veterinario.Consultas;
using Pata.Domain.ObjetosValor;
using Pata.Domain.Repositorios;
using VeterinarioAgregado = Pata.Domain.Entidades.Veterinario.Veterinario;

namespace Pata.Application.Tests.Apoio;

internal sealed class RepositorioVeterinarioFalso : IRepositorioVeterinario
{
    public VeterinarioAgregado? VeterinarioAtivo { get; set; }
    public VeterinarioAgregado? VeterinarioIncluindoExcluidos { get; set; }
    public bool CrmvExisteIncluindoExcluidos { get; set; }
    public VeterinarioAgregado? VeterinarioAdicionado { get; private set; }
    public VeterinarioAgregado? VeterinarioAtualizado { get; private set; }
    public CancellationToken UltimoTokenCancelamento { get; private set; }

    public Task<VeterinarioAgregado?> ObterPorIdAsync(
        Guid id,
        CancellationToken tokenCancelamento = default)
    {
        UltimoTokenCancelamento = tokenCancelamento;
        return Task.FromResult(VeterinarioAtivo);
    }

    public Task<VeterinarioAgregado?> ObterPorIdIncluindoExcluidosAsync(
        Guid id,
        CancellationToken tokenCancelamento = default)
    {
        UltimoTokenCancelamento = tokenCancelamento;
        return Task.FromResult(VeterinarioIncluindoExcluidos);
    }

    public Task<VeterinarioAgregado?> ObterPorCrmvAsync(
        Crmv crmv,
        CancellationToken tokenCancelamento = default) =>
        Task.FromResult(VeterinarioAtivo);

    public Task<VeterinarioAgregado?> ObterPorCrmvIncluindoExcluidosAsync(
        Crmv crmv,
        CancellationToken tokenCancelamento = default) =>
        Task.FromResult(VeterinarioIncluindoExcluidos);

    public Task<bool> ExisteCrmvAsync(
        Crmv crmv,
        CancellationToken tokenCancelamento = default) =>
        Task.FromResult(VeterinarioAtivo is not null);

    public Task<bool> ExisteCrmvIncluindoExcluidosAsync(
        Crmv crmv,
        CancellationToken tokenCancelamento = default)
    {
        UltimoTokenCancelamento = tokenCancelamento;
        return Task.FromResult(CrmvExisteIncluindoExcluidos);
    }

    public Task AdicionarAsync(
        VeterinarioAgregado veterinario,
        CancellationToken tokenCancelamento = default)
    {
        VeterinarioAdicionado = veterinario;
        UltimoTokenCancelamento = tokenCancelamento;
        return Task.CompletedTask;
    }

    public Task AtualizarAsync(
        VeterinarioAgregado veterinario,
        CancellationToken tokenCancelamento = default)
    {
        VeterinarioAtualizado = veterinario;
        UltimoTokenCancelamento = tokenCancelamento;
        return Task.CompletedTask;
    }
}

internal sealed class ConsultaVeterinariosFalsa : IConsultaVeterinarios
{
    public VeterinarioResumoDto? Veterinario { get; set; }
    public RespostaPaginada<VeterinarioResumoDto> Veterinarios { get; set; } =
        new([], 1, 20, 0);
    public RespostaPaginada<VeterinarioResumoDto> VeterinariosExcluidos { get; set; } =
        new([], 1, 20, 0);
    public Crmv? CrmvConsultado { get; private set; }
    public int PaginaConsultada { get; private set; }
    public int TamanhoPaginaConsultado { get; private set; }
    public bool ConsultouExcluidos { get; private set; }
    public CancellationToken UltimoTokenCancelamento { get; private set; }

    public Task<VeterinarioResumoDto?> ObterPorCrmvAsync(
        Crmv crmv,
        CancellationToken tokenCancelamento = default)
    {
        CrmvConsultado = crmv;
        UltimoTokenCancelamento = tokenCancelamento;
        return Task.FromResult(Veterinario);
    }

    public Task<RespostaPaginada<VeterinarioResumoDto>> ListarAsync(
        int pagina,
        int tamanhoPagina,
        CancellationToken tokenCancelamento = default)
    {
        RegistrarPaginacao(pagina, tamanhoPagina, tokenCancelamento, false);
        return Task.FromResult(Veterinarios);
    }

    public Task<RespostaPaginada<VeterinarioResumoDto>> ListarExcluidosAsync(
        int pagina,
        int tamanhoPagina,
        CancellationToken tokenCancelamento = default)
    {
        RegistrarPaginacao(pagina, tamanhoPagina, tokenCancelamento, true);
        return Task.FromResult(VeterinariosExcluidos);
    }

    private void RegistrarPaginacao(
        int pagina,
        int tamanhoPagina,
        CancellationToken tokenCancelamento,
        bool consultouExcluidos)
    {
        PaginaConsultada = pagina;
        TamanhoPaginaConsultado = tamanhoPagina;
        UltimoTokenCancelamento = tokenCancelamento;
        ConsultouExcluidos = consultouExcluidos;
    }
}

internal sealed class RelogioFalso(DateTime utcAgora) : IRelogio
{
    public DateTime UtcAgora { get; } = utcAgora;
}

internal sealed class UsuarioAtualFalso(
    string identificador,
    string perfil = "Teste",
    IReadOnlyCollection<string>? papeis = null) : IUsuarioAtual
{
    public string Identificador { get; } = identificador;
    public string Perfil { get; } = perfil;
    public IReadOnlyCollection<string> Papeis { get; } = papeis ?? [];
}
