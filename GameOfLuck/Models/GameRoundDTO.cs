using GameOfLuck.Entities;
using GameOfLuck.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLuck.Models
{
    public class GameRoundDTO
    {
        public int GameId { get; set; }
        public int PersonId { get; set; }
        public int Bet { get; set; }
        public int RandomResult { get; set; }
        public int Reward { get; set; }
        public DateTime GameDate { get; set; }
        public bool Result { get; set; }
    }
}
