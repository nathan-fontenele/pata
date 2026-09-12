using Pata.Application.Comum.Mensagens;
using Pata.Application.Comum.Modelos;

namespace Pata.Application.Funcionalidades.Veterinario.Consultas.ListarVeterinarios;

public sealed class ListarVeterinariosManipulador(IConsultaVeterinarios consultaVeterinarios)
    : IManipuladorConsulta<ListarVeterinariosConsulta, RespostaPaginada<VeterinarioResumoDto>>
{
    public Task<RespostaPaginada<VeterinarioResumoDto>> Handle(
        ListarVeterinariosConsulta consulta,
        CancellationToken tokenCancelamento)
    {
        Paginacao.Validar(consulta.Pagina, consulta.TamanhoPagina);

        return consultaVeterinarios.ListarAsync(
            consulta.Pagina,
            consulta.TamanhoPagina,
            tokenCancelamento);
    }
}
