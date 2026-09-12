using Pata.Application.Comum.Mensagens;
using Pata.Application.Comum.Modelos;

namespace Pata.Application.Funcionalidades.Veterinario.Consultas.ListarVeterinarios;

public sealed record ListarVeterinariosConsulta(
    int Pagina = 1,
    int TamanhoPagina = 20) : IConsulta<RespostaPaginada<VeterinarioResumoDto>>;
