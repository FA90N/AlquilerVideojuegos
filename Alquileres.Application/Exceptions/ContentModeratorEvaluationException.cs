namespace Alquileres.Application.Exceptions
{
    public class ContentModeratorEvaluationException : ApplicationException
    {
        public ContentModeratorEvaluationException(string message) : base(message)
        {
        }

        public ContentModeratorEvaluationException() : base()
        {
        }

        public ContentModeratorEvaluationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
