using Pata.Domain.ObjetosValor;
using VeterinarioAgregado = Pata.Domain.Entidades.Veterinario.Veterinario;

namespace Pata.Domain.Repositorios;

public interface IRepositorioVeterinario
{
    Task<VeterinarioAgregado?> ObterPorIdAsync(
        Guid id,
        CancellationToken tokenCancelamento = default);

    Task<VeterinarioAgregado?> ObterPorCrmvAsync(
        Crmv crmv,
        CancellationToken tokenCancelamento = default);

    Task<bool> ExisteCrmvAsync(
        Crmv crmv,
        CancellationToken tokenCancelamento = default);

    Task AdicionarAsync(
        VeterinarioAgregado veterinario,
        CancellationToken tokenCancelamento = default);

    Task AtualizarAsync(
        VeterinarioAgregado veterinario,
        CancellationToken tokenCancelamento = default);
}
