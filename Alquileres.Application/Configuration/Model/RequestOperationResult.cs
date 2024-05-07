namespace Alquileres.Application.Configuration.Model
{
    public sealed class RequestOperationResult<T> where T : class
    {
        public int? StatusCode { get; set; }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public T? Data { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public static RequestOperationResult<T> Ok(T data)
        {
            return new RequestOperationResult<T>()
            {
                Data = data,
                StatusCode = 200,
                Success = true,
            };
        }

        public static RequestOperationResult<T> Ok(T data, int totalCount, int pageSize, int pageNumber)
        {
            return new RequestOperationResult<T>()
            {
                Data = data,
                StatusCode = 200,
                Success = true,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public static RequestOperationResult<T> Error(Exception exception)
        {
            return new RequestOperationResult<T>()
            {
                StatusCode = 500,
                Success = false,
                ErrorMessage = exception.Message
            };
        }

        public static RequestOperationResult<T> BadRequest(Exception exception)
        {
            return new RequestOperationResult<T>()
            {
                StatusCode = 400,
                Success = false,
                ErrorMessage = exception.Message
            };
        }
    }
}
