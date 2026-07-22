using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Specs
{
    public class Pagination<T> where T : class
    {
        public Pagination()
        {
            
        }

        public Pagination(int _pageindex , int _pagesize , int _count , IReadOnlyList<T>_data)
        {
            PageIndex = _pageindex;
            PageSize = _pagesize;
            Count = _count;
            Data = _data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int Count { get; set; }

        public IReadOnlyList<T> Data { get; set; }
    }
}
