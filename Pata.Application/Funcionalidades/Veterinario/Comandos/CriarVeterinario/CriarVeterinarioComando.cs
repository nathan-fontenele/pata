using Pata.Application.Comum.Mensagens;

namespace Pata.Application.Funcionalidades.Veterinario.Comandos.CriarVeterinario;

public sealed record CriarVeterinarioComando(
    string Nome,
    string Email,
    string Telefone,
    string NumeroCrmv,
    string UfCrmv,
    string Especialidade) : IComando<Guid>;
