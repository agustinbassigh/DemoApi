using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
