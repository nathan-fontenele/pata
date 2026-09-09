using Pata.Application.Comum.Mensagens;
using Pata.Domain.ObjetosValor;
using Pata.Domain.Repositorios;

namespace Pata.Application.Funcionalidades.Tutor.Comandos.AtualizarTutor;

public sealed class AtualizarTutorManipulador(IRepositorioTutor repositorioTutor)
    : IManipuladorComando<AtualizarTutorComando>
{
    public async Task Handle(
        AtualizarTutorComando comando,
        CancellationToken tokenCancelamento)
    {
        var tutor = await repositorioTutor.ObterPorIdAsync(comando.Id, tokenCancelamento)
                    ?? throw new KeyNotFoundException("Tutor nao encontrado.");

        tutor.AlterarNome(comando.Nome);
        tutor.AlterarEmail(comando.Email is null ? null : new Email(comando.Email));
        tutor.AlterarTelefone(comando.Telefone is null ? null : new Telefone(comando.Telefone));

        await repositorioTutor.AtualizarAsync(tutor, tokenCancelamento);
    }
}
