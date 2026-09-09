using Pata.Application.Comum.Mensagens;

namespace Pata.Application.Funcionalidades.Tutor.Comandos.ExcluirTutor;

public sealed record ExcluirTutorComando(Guid Id) : IComando;
