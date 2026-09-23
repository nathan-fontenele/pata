using Pata.Application.Comum.Mensagens;
using Pata.Domain.Excecoes;
using Pata.Domain.ObjetosValor;
using Pata.Domain.Repositorios;

namespace Pata.Application.Funcionalidades.Veterinario.Comandos.AtualizarVeterinario;

public sealed class AtualizarVeterinarioManipulador(IRepositorioVeterinario repositorioVeterinario)
    : IManipuladorComando<AtualizarVeterinarioComando>
{
    public async Task Handle(
        AtualizarVeterinarioComando comando,
        CancellationToken tokenCancelamento)
    {
        var veterinario = await repositorioVeterinario.ObterPorIdAsync(
                              comando.Id,
                              tokenCancelamento)
                          ?? throw new RecursoNaoEncontradoException("Veterinario nao encontrado.");

        veterinario.AlterarNome(comando.Nome);
        veterinario.AlterarEmail(new Email(comando.Email));
        veterinario.AlterarTelefone(new Telefone(comando.Telefone));
        veterinario.AlterarEspecialidade(comando.Especialidade);

        await repositorioVeterinario.AtualizarAsync(veterinario, tokenCancelamento);
    }
}
