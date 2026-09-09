using MediatR;

namespace Pata.Application.Comum.Mensagens;

public interface IConsulta<out TResposta> : IRequest<TResposta>;