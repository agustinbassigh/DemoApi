using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.InMemory;
using Demo.Domain.Entities;

namespace Demo.Domain.Context
{
   public class DbContextBase: DbContext
    {
        public DbContextBase(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Usuario> Usuario { get; set; }
    }
}
