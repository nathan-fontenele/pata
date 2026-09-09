using Pata.Application.Comum.Mensagens;
using Pata.Application.Comum.Modelos;

namespace Pata.Application.Funcionalidades.Tutor.Consultas.ListarTutores;

public sealed record ListarTutoresConsulta(
    int Pagina = 1,
    int TamanhoPagina = 20) : IConsulta<RespostaPaginada<TutorResumoDto>>;
