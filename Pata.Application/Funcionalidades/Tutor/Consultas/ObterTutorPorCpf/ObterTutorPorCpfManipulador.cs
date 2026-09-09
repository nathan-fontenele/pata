using Pata.Application.Comum.Mensagens;
using Pata.Domain.ObjetosValor;

namespace Pata.Application.Funcionalidades.Tutor.Consultas.ObterTutorPorCpf;

public sealed class ObterTutorPorCpfManipulador(IConsultaTutores consultaTutores)
    : IManipuladorConsulta<ObterTutorPorCpfConsulta, TutorResumoDto?>
{
    public Task<TutorResumoDto?> Handle(
        ObterTutorPorCpfConsulta consulta,
        CancellationToken tokenCancelamento) =>
        consultaTutores.ObterPorCpfAsync(
            new Cpf(consulta.Cpf),
            tokenCancelamento);
}
