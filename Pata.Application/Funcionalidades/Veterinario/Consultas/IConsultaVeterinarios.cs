using Pata.Application.Comum.Modelos;
using Pata.Domain.ObjetosValor;

namespace Pata.Application.Funcionalidades.Veterinario.Consultas;

public interface IConsultaVeterinarios
{
    Task<VeterinarioResumoDto?> ObterPorCrmvAsync(
        Crmv crmv,
        CancellationToken tokenCancelamento = default);

    Task<RespostaPaginada<VeterinarioResumoDto>> ListarAsync(
        int pagina,
        int tamanhoPagina,
        CancellationToken tokenCancelamento = default);

    Task<RespostaPaginada<VeterinarioResumoDto>> ListarExcluidosAsync(
        int pagina,
        int tamanhoPagina,
        CancellationToken tokenCancelamento = default);
}
