using Microsoft.EntityFrameworkCore;

namespace Alquileres.Application.Extensions
{
    public static class PagedListExtension
    {
        public static List<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public static List<T> ToPagedList<T>(this IOrderedQueryable<T> source, int pageNumber, int pageSize)
        {
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public static async Task<List<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public static async Task<List<T>> ToPagedListAsync<T>(this IOrderedQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }
    }
}
