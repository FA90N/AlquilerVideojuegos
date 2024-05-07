using MediatR;

namespace Alquileres.Application.Configuration.CQRS
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}