using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servixa.Shared.Commen.Pagination
{
    public class PaginationResponse<TData>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 0;
        public int TotalItems { get; set; }  // total items in the database get from the method that return the count of items in the database
        public IReadOnlyList<TData> Data { get; set; }

        public PaginationResponse(int pageIndex, int pageSize, int totalItems, IReadOnlyList<TData> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = totalItems;
            Data = data;
        }
    }
}
