using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Demo.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public ulong Id { get; set; }
    }
}
