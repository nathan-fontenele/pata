using Pata.Application.Comum.Mensagens;

namespace Pata.Application.Funcionalidades.Veterinario.Comandos.ExcluirVeterinario;

public sealed record ExcluirVeterinarioComando(Guid Id) : IComando;
