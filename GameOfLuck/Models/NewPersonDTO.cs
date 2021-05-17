using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLuck.Models
{
    public class NewPersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
       public string Password { get; set; }
        public int Points { get; set; }
        [Required]
        public string Email { get; set; }
    }

}
