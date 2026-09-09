namespace Pata.Application.Comum.Modelos;

public sealed class RespostaPaginada<T>(
    IEnumerable<T> itens,
    int pagina,
    int tamanhoPagina,
    int totalItens)
{
    public int Pagina { get; } = pagina;

    public int TamanhoPagina { get; } = tamanhoPagina;

    public int TotalItens { get; } = totalItens;

    public int TotalPaginas => (int)Math.Ceiling(TotalItens / (double)TamanhoPagina);

    public IEnumerable<T> Itens { get; } = itens;
}
