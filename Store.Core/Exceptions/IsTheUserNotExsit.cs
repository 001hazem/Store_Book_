using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Exceptions
{
    public class IsTheUserNotExsit :Exception
    {
        public IsTheUserNotExsit() :base("The User Not Exsit")
        { 

        }

    }
}
