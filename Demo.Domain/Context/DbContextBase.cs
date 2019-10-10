using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Domain.Context
{
    class DbContextBase: DbContext
    {
        public DbContextBase(DbContextOptions options) : base(options)
        {

        }

    }
}
