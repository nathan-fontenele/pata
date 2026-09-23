using Pata.Application.Comum.Mensagens;
using Pata.Domain.Excecoes;
using Pata.Domain.Repositorios;

namespace Pata.Application.Funcionalidades.Veterinario.Comandos.RecuperarVeterinario;

public sealed class RecuperarVeterinarioManipulador(IRepositorioVeterinario repositorioVeterinario)
    : IManipuladorComando<RecuperarVeterinarioComando>
{
    public async Task Handle(
        RecuperarVeterinarioComando comando,
        CancellationToken tokenCancelamento)
    {
        var veterinario = await repositorioVeterinario.ObterPorIdIncluindoExcluidosAsync(
                              comando.Id,
                              tokenCancelamento)
                          ?? throw new RecursoNaoEncontradoException("Veterinario nao encontrado.");

        veterinario.Recuperar();

        await repositorioVeterinario.AtualizarAsync(veterinario, tokenCancelamento);
    }
}
