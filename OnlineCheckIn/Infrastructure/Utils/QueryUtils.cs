using Microsoft.EntityFrameworkCore;

namespace OnlineCheckIn.Infrastructure.Utils;

public static class QueryUtils
{
    public static Func<IQueryable<T>, IOrderedQueryable<T>>? CreateOrderByFunc<T>(
        string? orderBy, bool ascending)
    {
        return !string.IsNullOrWhiteSpace(orderBy)
            ? q => ascending
                ? q.OrderBy(e => EF.Property<object>(e, orderBy))
                : q.OrderByDescending(e => EF.Property<object>(e, orderBy))
            : null;
    }
}