using Pata.Application.Comum.Mensagens;

namespace Pata.Application.Funcionalidades.Veterinario.Comandos.RecuperarVeterinario;

public sealed record RecuperarVeterinarioComando(Guid Id) : IComando;
