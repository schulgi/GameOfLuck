using AutoMapper;
using GameOfLuck.Context;
using GameOfLuck.Entities;
using GameOfLuck.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameOfLuck.Services
{
    public class GameRoundService : IGameRoundService
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IPersonService _personService;

        public GameRoundService(ApplicationDbContext context, IMapper mapper,IPersonService personService)
        {
            this.context = context;
            this.mapper = mapper;
            _personService = personService;
        }

        public BetResultDTO GameRoundBet (BetDTO bet,string username)
        {
            BetResultDTO result = new BetResultDTO();

            PersonDTO person = _personService.GetPersonByEmail(username);
            if (person != null && bet.Bet <= person.Points)
            {
                Random rd = new Random();
                int randomNumber = rd.Next(0, 9);

                if (randomNumber == bet.Number)
                {
                    int winnerPoints = bet.Bet * 9;
                    person.Points += winnerPoints;
                    result.PointsWon = winnerPoints;
                    result.Status = "Won";
                }
                else
                {
                    person.Points -= bet.Bet;
                    result.PointsWon = -bet.Bet;
                    result.Status = "Lost";
                }

                result.CurrentPoints = person.Points;
                result.Username = username;
                result.Bet = bet.Bet;
                result.RandomResult = randomNumber;
                createGameRound(person, result);
                _personService.Update(person);
            }

            return result;
        }


        private void createGameRound(PersonDTO person,BetResultDTO betResultDTO)
        {
            GameRound game = new GameRound();

            game.Bet = betResultDTO.Bet;
            game.GameDate = DateTime.Now;
            game.PersonId = person.Id;
            game.RandomResult = betResultDTO.RandomResult;
            game.Result = betResultDTO.Status == "Won" ? true : false;
            //game.isResult = betResultDTO.Status == "Won" ? 1 : 0;
            game.Reward = betResultDTO.PointsWon;

            context.GameRound.Add(game);
            context.SaveChanges();
        }


        public List<GameRoundDTO> GetRoundByPersonId(string email, int numberOfRecords)
        {
            List<GameRoundDTO> ret = new List<GameRoundDTO>();
            Person person = context.Persons.FirstOrDefault(x => x.Email == email);

            context.Entry(person).State = EntityState.Detached;

            var gameRoundBool = context.GameRound.Where(x => x.PersonId == person.Id).FirstOrDefault();

            if (gameRoundBool != null)
            {
                //foreach (var game in gameRoundBool)
                //{
                //    ret.Add(mapper.Map<GameRoundDTO>(game));
                //}
            }

            return ret;
        }
    }
}
