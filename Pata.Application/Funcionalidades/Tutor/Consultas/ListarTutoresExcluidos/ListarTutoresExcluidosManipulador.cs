using Pata.Application.Comum.Mensagens;
using Pata.Application.Comum.Modelos;

namespace Pata.Application.Funcionalidades.Tutor.Consultas.ListarTutoresExcluidos;

public sealed class ListarTutoresExcluidosManipulador(IConsultaTutores consultaTutores)
    : IManipuladorConsulta<ListarTutoresExcluidosConsulta, RespostaPaginada<TutorResumoDto>>
{
    public Task<RespostaPaginada<TutorResumoDto>> Handle(
        ListarTutoresExcluidosConsulta consulta,
        CancellationToken tokenCancelamento)
    {
        Paginacao.Validar(consulta.Pagina, consulta.TamanhoPagina);

        return consultaTutores.ListarExcluidosAsync(
            consulta.Pagina,
            consulta.TamanhoPagina,
            tokenCancelamento);
    }
}
