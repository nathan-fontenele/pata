using Pata.Domain.Excecoes;

namespace Pata.Application.Comum.Modelos;

internal static class Paginacao
{
    public static void Validar(int pagina, int tamanhoPagina)
    {
        if (pagina <= 0)
            throw new ErroDeValidacao("A pagina deve ser maior que zero.", nameof(pagina));

        if (tamanhoPagina <= 0)
            throw new ErroDeValidacao(
                "O tamanho da pagina deve ser maior que zero.",
                nameof(tamanhoPagina));
    }
}
