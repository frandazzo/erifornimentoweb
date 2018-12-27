using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Db
{
    public class IntegrationContext : DbContext
    {
        public IntegrationContext(DbContextOptions<IntegrationContext> options)
            : base(options)
        { }
    }
}
