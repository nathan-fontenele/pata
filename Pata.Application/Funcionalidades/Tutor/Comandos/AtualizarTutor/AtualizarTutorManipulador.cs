using Pata.Application.Comum.Mensagens;
using Pata.Domain.Excecoes;
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
                    ?? throw new RecursoNaoEncontradoException("Tutor nao encontrado.");

        tutor.AlterarNome(comando.Nome);
        tutor.AlterarEmail(new Email(comando.Email));
        tutor.AlterarTelefone(new Telefone(comando.Telefone));

        await repositorioTutor.AtualizarAsync(tutor, tokenCancelamento);
    }
}
