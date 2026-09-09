using MediatR;

namespace Pata.Application.Comum.Mensagens;

public interface IComando : IRequest;

public interface IComando<out TResposta> : IRequest<TResposta>;
