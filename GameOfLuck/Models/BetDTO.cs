using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLuck.Models
{
    public class BetDTO
    {
        [Required]
        public int Bet { get; set; }
        [Required]
        public int Number { get; set; }
    }
}
