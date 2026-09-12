using Pata.Application.Comum.Mensagens;
using Pata.Application.Comum.Modelos;

namespace Pata.Application.Funcionalidades.Veterinario.Consultas.ListarVeterinariosExcluidos;

public sealed record ListarVeterinariosExcluidosConsulta(
    int Pagina = 1,
    int TamanhoPagina = 20) : IConsulta<RespostaPaginada<VeterinarioResumoDto>>;
