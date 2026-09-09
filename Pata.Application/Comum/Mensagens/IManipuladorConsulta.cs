using MediatR;

namespace Pata.Application.Comum.Mensagens;

public interface IManipuladorConsulta<in TConsulta, TResposta> : IRequestHandler<TConsulta, TResposta>
    where TConsulta : IConsulta<TResposta>;
