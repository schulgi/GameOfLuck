using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLuck.Entities
{
    public class GameRound
    {
        [Key]
        public int GameId { get; set; }

        [ForeignKey("PersonId")]
        [Required]
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int Bet { get; set; }
        public int RandomResult { get; set; }
        public int Reward { get; set; }
        public DateTime GameDate { get; set; }
        public bool Result { get; set; }
    }
}
