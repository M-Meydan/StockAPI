using System.Linq;
using System.Threading.Tasks;
using WebAPI.App.Models;

namespace WebAPI.App.Mappings;

public static class MappingExtensions
{
    public static PaginatedList<TDestination> PaginatedList<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
        =>Models.PaginatedList<TDestination>.Create(queryable, pageNumber, pageSize);
}
