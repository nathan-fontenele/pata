using Pata.Application.Comum.Mensagens;
using Pata.Domain.Excecoes;
using Pata.Domain.ObjetosValor;
using Pata.Domain.Repositorios;
using TutorAgregado = Pata.Domain.Entidades.Tutor.Tutor;

namespace Pata.Application.Funcionalidades.Tutor.Comandos.CriarTutor;

public sealed class CriarTutorManipulador(IRepositorioTutor repositorioTutor)
    : IManipuladorComando<CriarTutorComando, Guid>
{
    public async Task<Guid> Handle(
        CriarTutorComando comando,
        CancellationToken tokenCancelamento)
    {
        var cpf = new Cpf(comando.Cpf);

        if (await repositorioTutor.ExisteCpfIncluindoExcluidosAsync(cpf, tokenCancelamento))
            throw new ConflitoException("Ja existe um tutor com o CPF informado.");

        var tutor = new TutorAgregado(
            comando.Nome,
            cpf,
            new Email(comando.Email),
            new Telefone(comando.Telefone));

        await repositorioTutor.AdicionarAsync(tutor, tokenCancelamento);

        return tutor.Id;
    }
}
