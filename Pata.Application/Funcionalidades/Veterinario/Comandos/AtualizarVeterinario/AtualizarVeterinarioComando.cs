using Pata.Application.Comum.Mensagens;

namespace Pata.Application.Funcionalidades.Veterinario.Comandos.AtualizarVeterinario;

public sealed record AtualizarVeterinarioComando(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string Especialidade) : IComando;
