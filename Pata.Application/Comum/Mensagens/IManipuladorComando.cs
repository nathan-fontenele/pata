using MediatR;

namespace Pata.Application.Comum.Mensagens;

public interface IManipuladorComando<in TComando> : IRequestHandler<TComando>
    where TComando : IComando;

public interface IManipuladorComando<in TComando, TResposta> : IRequestHandler<TComando, TResposta>
    where TComando : IComando<TResposta>;