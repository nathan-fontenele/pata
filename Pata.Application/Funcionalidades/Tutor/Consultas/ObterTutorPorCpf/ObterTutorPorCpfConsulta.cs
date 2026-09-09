using Pata.Application.Comum.Mensagens;

namespace Pata.Application.Funcionalidades.Tutor.Consultas.ObterTutorPorCpf;

public sealed record ObterTutorPorCpfConsulta(string Cpf)
    : IConsulta<TutorResumoDto?>;
