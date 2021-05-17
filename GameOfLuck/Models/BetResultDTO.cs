using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLuck.Models
{
    public class BetResultDTO
    {
        public string Username{get;set;}
        public string Status { get; set; }
        public int PointsWon { get; set; }
        public int CurrentPoints { get; set; }
        public int Bet { get; set; }
        public int RandomResult { get; set; }
    }
}
