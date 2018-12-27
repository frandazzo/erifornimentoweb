using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erifornimento.Utils
{
    public class ApiException: Exception
    {
        public int StatusCode { get; set; }

        public string Content { get; set; }
    }
}
