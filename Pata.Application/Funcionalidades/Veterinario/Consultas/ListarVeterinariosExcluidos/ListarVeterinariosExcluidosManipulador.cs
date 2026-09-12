using Pata.Application.Comum.Mensagens;
using Pata.Application.Comum.Modelos;

namespace Pata.Application.Funcionalidades.Veterinario.Consultas.ListarVeterinariosExcluidos;

public sealed class ListarVeterinariosExcluidosManipulador(IConsultaVeterinarios consultaVeterinarios)
    : IManipuladorConsulta<
        ListarVeterinariosExcluidosConsulta,
        RespostaPaginada<VeterinarioResumoDto>>
{
    public Task<RespostaPaginada<VeterinarioResumoDto>> Handle(
        ListarVeterinariosExcluidosConsulta consulta,
        CancellationToken tokenCancelamento)
    {
        Paginacao.Validar(consulta.Pagina, consulta.TamanhoPagina);

        return consultaVeterinarios.ListarExcluidosAsync(
            consulta.Pagina,
            consulta.TamanhoPagina,
            tokenCancelamento);
    }
}
