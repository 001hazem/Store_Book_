using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.ViewModel
{
    public class PaginationViewModel
    {
        public int NumberOfPage { get; set; }
        public int CurrentPage { get; set; }
        public object data { get; set; }
    }
}
