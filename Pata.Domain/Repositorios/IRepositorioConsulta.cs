using ConsultaAgregado = Pata.Domain.Entidades.Consulta.Consulta;

namespace Pata.Domain.Repositorios;

public interface IRepositorioConsulta
{
    Task<ConsultaAgregado?> ObterPorIdAsync(
        Guid id,
        CancellationToken tokenCancelamento = default);

    Task AdicionarAsync(
        ConsultaAgregado consulta,
        CancellationToken tokenCancelamento = default);

    Task AtualizarAsync(
        ConsultaAgregado consulta,
        CancellationToken tokenCancelamento = default);
}
