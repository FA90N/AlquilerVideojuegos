using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Alquileres.Application.Models.Parameters
{
    public class QueryStringPagingParameters
    {
        const int maxPageSize = 50;

        [BindRequired]
        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;

        [BindRequired]
        public int PageSize { get => pageSize; set { pageSize = (value > maxPageSize) ? maxPageSize : value; } }
    }
}
