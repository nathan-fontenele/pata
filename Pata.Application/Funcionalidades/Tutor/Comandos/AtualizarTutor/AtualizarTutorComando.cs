using Pata.Application.Comum.Mensagens;

namespace Pata.Application.Funcionalidades.Tutor.Comandos.AtualizarTutor;

public sealed record AtualizarTutorComando(
    Guid Id,
    string Nome,
    string? Email,
    string? Telefone) : IComando;
