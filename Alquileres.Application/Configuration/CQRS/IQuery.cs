using MediatR;

namespace Alquileres.Application.Configuration.CQRS
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}