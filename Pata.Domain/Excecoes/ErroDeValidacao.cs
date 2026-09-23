namespace Pata.Domain.Excecoes;

public sealed class ErroDeValidacao : ArgumentException
{
    public ErroDeValidacao(string mensagem, string? nomeParametro = null)
        : base(mensagem, nomeParametro)
    {
    }
}
