using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Helpers.Quiz
{
    public static class PagedListExtensions
    {
        public static Dictionary<string, object?> GeneratePaginationMetadata<T>(this PagedList<T> pagedList,object? prevPageLink, object? nextPageLink )
        {
            return new Dictionary<string, object?>
                {
                    { "totalCount", pagedList.TotalCount },
                    { "pageSize", pagedList.PageSize },
                    { "currentPage", pagedList.CurrentPage },
                    { "totalPages", pagedList.TotalPages },
                    { "previousPageLink",prevPageLink },
                    { "nextPageLink", nextPageLink }
                };

            
        }

    }
}
