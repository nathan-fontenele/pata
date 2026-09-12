using Pata.Application.Comum.Mensagens;
using Pata.Domain.ObjetosValor;

namespace Pata.Application.Funcionalidades.Veterinario.Consultas.ObterVeterinarioPorCrmv;

public sealed class ObterVeterinarioPorCrmvManipulador(IConsultaVeterinarios consultaVeterinarios)
    : IManipuladorConsulta<ObterVeterinarioPorCrmvConsulta, VeterinarioResumoDto?>
{
    public Task<VeterinarioResumoDto?> Handle(
        ObterVeterinarioPorCrmvConsulta consulta,
        CancellationToken tokenCancelamento) =>
        consultaVeterinarios.ObterPorCrmvAsync(
            new Crmv(consulta.NumeroCrmv, consulta.UfCrmv),
            tokenCancelamento);
}
