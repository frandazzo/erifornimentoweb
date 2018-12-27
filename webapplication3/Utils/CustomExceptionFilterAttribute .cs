using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Utils
{
    public class CustomExceptionFilterAttribute : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public CustomExceptionFilterAttribute(
            IHostingEnvironment hostingEnvironment)

        {
            _hostingEnvironment = hostingEnvironment;

        }

        public void OnException(ExceptionContext context)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                // do nothing

            }
        }
    }

}