using Demo.Domain.Constants;
using Demo.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly ILogger<UnitOfWork> _logger;
        public UnitOfWork(DbContext dbContext, ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public int SaveChanges()
        {
            _logger.LogInformation(LogEvents.Service, "Ejecutando SaveChanges()");
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            _logger.LogInformation(LogEvents.Service, "Ejecutando SaveChanges()");
            return await _dbContext.SaveChangesAsync();
        }
    }

}
