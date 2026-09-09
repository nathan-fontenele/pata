using Pata.Application.Comum.Mensagens;
using Pata.Application.Comum.Modelos;

namespace Pata.Application.Funcionalidades.Tutor.Consultas.ListarTutoresExcluidos;

public sealed record ListarTutoresExcluidosConsulta(
    int Pagina = 1,
    int TamanhoPagina = 20) : IConsulta<RespostaPaginada<TutorResumoDto>>;
