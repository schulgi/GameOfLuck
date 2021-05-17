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
        private readonly IPersonService _personService;
        public GameRoundController(ApplicationDbContext context, IMapper mapper, IGameRoundService gameRoundService, IPersonService personService)
        {
            this.context = context;
            this.mapper = mapper;
            _gameRoundService = gameRoundService;
            _personService = personService;
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

     
        [HttpGet("CurrentPoints")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult<PersonDTO> CurrentPoints()
        {

            var claims = User.Claims.ToList();

            string username = claims.FirstOrDefault(x => x.Type == "Username").Value;

            PersonDTO person = _personService.GetPersonByEmail(username);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }


    }
}