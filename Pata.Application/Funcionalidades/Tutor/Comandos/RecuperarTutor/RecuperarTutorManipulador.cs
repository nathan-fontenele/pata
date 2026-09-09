using Pata.Application.Comum.Mensagens;
using Pata.Domain.Repositorios;

namespace Pata.Application.Funcionalidades.Tutor.Comandos.RecuperarTutor;

public sealed class RecuperarTutorManipulador(IRepositorioTutor repositorioTutor)
    : IManipuladorComando<RecuperarTutorComando>
{
    public async Task Handle(
        RecuperarTutorComando comando,
        CancellationToken tokenCancelamento)
    {
        var tutor = await repositorioTutor.ObterPorIdIncluindoExcluidosAsync(
                        comando.Id,
                        tokenCancelamento)
                    ?? throw new KeyNotFoundException("Tutor nao encontrado.");

        tutor.Recuperar();

        await repositorioTutor.AtualizarAsync(tutor, tokenCancelamento);
    }
}
