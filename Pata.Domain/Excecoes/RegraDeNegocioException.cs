namespace Pata.Domain.Excecoes;

public sealed class RegraDeNegocioException(string mensagem) : InvalidOperationException(mensagem);
