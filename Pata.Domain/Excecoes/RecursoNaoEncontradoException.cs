namespace Pata.Domain.Excecoes;

public sealed class RecursoNaoEncontradoException(string mensagem) : KeyNotFoundException(mensagem);
