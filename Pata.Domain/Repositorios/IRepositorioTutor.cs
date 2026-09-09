using Pata.Domain.ObjetosValor;
using TutorAgregado = Pata.Domain.Entidades.Tutor.Tutor;

namespace Pata.Domain.Repositorios;

public interface IRepositorioTutor
{
    Task<TutorAgregado?> ObterPorIdAsync(
        Guid id,
        CancellationToken tokenCancelamento = default);

    Task<TutorAgregado?> ObterPorCpfAsync(
        Cpf cpf,
        CancellationToken tokenCancelamento = default);

    Task<bool> ExisteCpfAsync(
        Cpf cpf,
        CancellationToken tokenCancelamento = default);

    Task AdicionarAsync(
        TutorAgregado tutor,
        CancellationToken tokenCancelamento = default);

    Task AtualizarAsync(
        TutorAgregado tutor,
        CancellationToken tokenCancelamento = default);
}
