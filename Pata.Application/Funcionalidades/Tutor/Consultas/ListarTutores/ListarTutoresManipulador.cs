using Pata.Application.Comum.Mensagens;
using Pata.Application.Comum.Modelos;

namespace Pata.Application.Funcionalidades.Tutor.Consultas.ListarTutores;

public sealed class ListarTutoresManipulador(IConsultaTutores consultaTutores)
    : IManipuladorConsulta<ListarTutoresConsulta, RespostaPaginada<TutorResumoDto>>
{
    public Task<RespostaPaginada<TutorResumoDto>> Handle(
        ListarTutoresConsulta consulta,
        CancellationToken tokenCancelamento)
    {
        Paginacao.Validar(consulta.Pagina, consulta.TamanhoPagina);

        return consultaTutores.ListarAsync(
            consulta.Pagina,
            consulta.TamanhoPagina,
            tokenCancelamento);
    }
}
