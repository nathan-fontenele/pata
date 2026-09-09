using Pata.Application.Comum.Modelos;
using Pata.Domain.ObjetosValor;

namespace Pata.Application.Funcionalidades.Tutor.Consultas;

public interface IConsultaTutores
{
    Task<TutorResumoDto?> ObterPorCpfAsync(
        Cpf cpf,
        CancellationToken tokenCancelamento = default);

    Task<RespostaPaginada<TutorResumoDto>> ListarAsync(
        int pagina,
        int tamanhoPagina,
        CancellationToken tokenCancelamento = default);

    Task<RespostaPaginada<TutorResumoDto>> ListarExcluidosAsync(
        int pagina,
        int tamanhoPagina,
        CancellationToken tokenCancelamento = default);
}
