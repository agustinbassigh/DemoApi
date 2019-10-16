using Demo.Domain.Contracts;
using Demo.Domain.Implementations;
using Demo.Service.Contracts;
using Demo.Service.Implmentation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Domain.Context;

namespace Demo.WebApi.App_Config
{
    public static class Service
    {
        public static void BindServices(this IServiceCollection services)
        {
            //Servicios
            services.AddScoped(typeof(IServicio<>), typeof(Servicio<>));

            //Repositorios
            services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));

            //Contexto
            services.AddScoped<DbContext, DbContextBase>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

    }
}
