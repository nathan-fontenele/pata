using Pata.Application.Comum.Abstracoes;
using Pata.Application.Comum.Mensagens;
using Pata.Domain.Repositorios;

namespace Pata.Application.Funcionalidades.Veterinario.Comandos.ExcluirVeterinario;

public sealed class ExcluirVeterinarioManipulador(
    IRepositorioVeterinario repositorioVeterinario,
    IRelogio relogio,
    IUsuarioAtual usuarioAtual) : IManipuladorComando<ExcluirVeterinarioComando>
{
    public async Task Handle(
        ExcluirVeterinarioComando comando,
        CancellationToken tokenCancelamento)
    {
        var veterinario = await repositorioVeterinario.ObterPorIdAsync(
                              comando.Id,
                              tokenCancelamento)
                          ?? throw new KeyNotFoundException("Veterinario nao encontrado.");

        veterinario.Excluir(relogio.UtcAgora, usuarioAtual.Identificador);

        await repositorioVeterinario.AtualizarAsync(veterinario, tokenCancelamento);
    }
}
