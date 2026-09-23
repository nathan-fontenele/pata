namespace Pata.Domain.Excecoes;

public sealed class ConflitoException(string mensagem) : InvalidOperationException(mensagem);
