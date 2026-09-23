using Pata.Application.Comum.Mensagens;

namespace Pata.Application.Funcionalidades.Tutor.Comandos.CriarTutor;

public sealed record CriarTutorComando(
    string Nome,
    string Cpf,
    string Email,
    string Telefone) : IComando<Guid>;
