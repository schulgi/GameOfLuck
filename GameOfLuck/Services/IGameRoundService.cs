using GameOfLuck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLuck.Services
{
    public interface IGameRoundService
    {
        BetResultDTO GameRoundBet(BetDTO bet, string username);
    }
}
