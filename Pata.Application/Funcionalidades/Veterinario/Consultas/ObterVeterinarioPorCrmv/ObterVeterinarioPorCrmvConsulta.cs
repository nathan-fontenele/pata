using Pata.Application.Comum.Mensagens;

namespace Pata.Application.Funcionalidades.Veterinario.Consultas.ObterVeterinarioPorCrmv;

public sealed record ObterVeterinarioPorCrmvConsulta(
    string NumeroCrmv,
    string UfCrmv) : IConsulta<VeterinarioResumoDto?>;
