using Pata.Application.Comum.Abstracoes;
using Pata.Application.Comum.Mensagens;
using Pata.Domain.Repositorios;

namespace Pata.Application.Funcionalidades.Tutor.Comandos.ExcluirTutor;

public sealed class ExcluirTutorManipulador(
    IRepositorioTutor repositorioTutor,
    IRelogio relogio,
    IUsuarioAtual usuarioAtual) : IManipuladorComando<ExcluirTutorComando>
{
    public async Task Handle(
        ExcluirTutorComando comando,
        CancellationToken tokenCancelamento)
    {
        var tutor = await repositorioTutor.ObterPorIdAsync(comando.Id, tokenCancelamento)
                    ?? throw new KeyNotFoundException("Tutor nao encontrado.");

        tutor.Excluir(relogio.UtcAgora, usuarioAtual.Identificador);

        await repositorioTutor.AtualizarAsync(tutor, tokenCancelamento);
    }
}
