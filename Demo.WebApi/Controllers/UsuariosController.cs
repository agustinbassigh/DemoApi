using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Domain.Context;
using Demo.Domain.Contracts;
using Demo.Domain.Entities;
using Demo.Domain.Implementations;
using Demo.Service.Implmentation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private DbContextBase _db;
        private IUnitOfWork _unitOfWork;
        private IRepositorio<Usuario> _repositorio;
        public UsuariosController(DbContextBase dbContextBase, IUnitOfWork unitOfWork, IRepositorio<Usuario> repositorio)
        {
            _db = dbContextBase;
            _unitOfWork = unitOfWork;
            _repositorio = repositorio;
        }

        // GET: api/Usuarios
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            //_db.Add(new Usuario { Nombre = "Juan", Apellido = "Perez" });
            //_db.Add(new Usuario { Nombre = "Pedro", Apellido = "Gonzalez" });
            //_db.SaveChanges();

            //return _db.Usuario.ToList();


            


            Servicio<Usuario> usuarios = new Servicio<Usuario>(_repositorio, _unitOfWork, null, null);

            usuarios.Create(new Usuario { Nombre = "Juan", Apellido = "Perez" });
            usuarios.Create(new Usuario { Nombre = "Pedro", Apellido = "Gonzalez" });

          


            return usuarios.GetAll();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Usuarios
        [HttpPost]
        public void Post([FromBody] Usuario value)
        {
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Usuario value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
