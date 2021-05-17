using AutoMapper;
using GameOfLuck.Context;
using GameOfLuck.Entities;
using GameOfLuck.Models;
using GameOfLuck.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameOfLuck.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class GameRoundController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IGameRoundService _gameRoundService;
        public GameRoundController(ApplicationDbContext context, IMapper mapper, IGameRoundService gameRoundService)
        {
            this.context = context;
            this.mapper = mapper;
            _gameRoundService = gameRoundService;
        }

        [HttpGet("")]
        public ActionResult<GameRound> GetPrimer()
        {
            return context.GameRound.FirstOrDefault();
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("MyBet")]
        public ActionResult<BetResultDTO> GuessNumber(BetDTO bet)
        {
            if (ModelState.IsValid)
            {
                if (bet.Number >= 1 && bet.Number <= 9) 
                {
                    var claims = User.Claims.ToList();

                    string username = claims.FirstOrDefault(x => x.Type == "Username").Value;

                    BetResultDTO betresult = _gameRoundService.GameRoundBet(bet, username);

                    if (betresult == null)
                    {
                        return BadRequest("Wrong bet parameters");
                    }

                    return betresult;
                    
                }
                else
                {
                    return BadRequest("Insert a valid number between 1 and 9");
                }
            }
            else
            {
                return BadRequest("Invalid model");
            }
        }

        //[HttpGet("MyGames")]
        //[Authorize]
        //public async Task<ActionResult<List<GameRoundDTO>>> GetGames(int numberOfRecords = 5)
        //{

        //    var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();



        //    Person person = context.Persons.FirstOrDefault(x => x.Email == username);

        //    List<GameRoundDTO> gameRoundDTOList = new List<GameRoundDTO>();

        //    var gameRoundBool = context.GameRound.Select(x => x.PersonId == person.Id).Take(numberOfRecords).ToList();

        //    if (gameRoundBool == null)
        //    {
        //        return NotFound();
        //    }

        //    gameRoundDTOList = mapper.Map<List<GameRoundDTO>>(gameRoundBool);

        //    return gameRoundDTOList;
        //}


        //    [HttpPut("{id}")]
        //    public ActionResult Put(int id, [FromBody] Person value)
        //    {
        //        // Esto no es necesario en asp.net core 2.1
        //        // if (ModelState.IsValid){

        //        // }

        //        if (id != value.Id)
        //        {
        //            return BadRequest();
        //        }

        //        context.Entry(value).State = EntityState.Modified;
        //        context.SaveChanges();
        //        return Ok();
        //    }

        //    [HttpDelete("{id}")]
        //    public ActionResult<Person> Delete(int id)
        //    {
        //        var autor = context.Person.FirstOrDefault(x => x.Id == id);

        //        if (autor == null)
        //        {
        //            return NotFound();
        //        }

        //        context.Person.Remove(autor);
        //        context.SaveChanges();
        //        return autor;
        //    }
        //}
    }
}