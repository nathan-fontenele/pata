namespace Pata.Application.Comum.Modelos;

internal static class Paginacao
{
    public static void Validar(int pagina, int tamanhoPagina)
    {
        if (pagina <= 0)
            throw new ArgumentOutOfRangeException(nameof(pagina), "A pagina deve ser maior que zero.");

        if (tamanhoPagina <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(tamanhoPagina),
                "O tamanho da pagina deve ser maior que zero.");
    }
}
