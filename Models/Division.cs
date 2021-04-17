using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Division
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
