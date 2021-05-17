
using AutoMapper;
using GameOfLuck.Context;
using GameOfLuck.Entities;
using GameOfLuck.Model;
using GameOfLuck.Models;
using GameOfLuck.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLuck.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IGameRoundService _gameRoundService;
        private readonly IPersonService _personService;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AccountController(
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IGameRoundService gameRound, IPersonService personService, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _gameRoundService = gameRound;
            _personService = personService;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] PersonDTO model)
        {
            if (ModelState.IsValid)
            {
                string validPerson = _personService.validateNewUser(model);

                if (!string.IsNullOrEmpty(validPerson))
                {
                    return BadRequest(validPerson);
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    model.Points = _personService.AssignInitialPoints();

                    _personService.Create(model);

                    return BuildToken(model,new List<string>());
                }
                else
                {
                    return BadRequest("Username or password invalid");
                }
            }
            else {
                return BadRequest("Invalid model");
            }

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                    var usuario = await _userManager.FindByEmailAsync(userInfo.Email);

                    var roles = await _userManager.GetRolesAsync(usuario);

                    PersonDTO person = _personService.GetPersonByEmail(userInfo.Email);

                    return BuildToken(person,roles);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest("Invalid model");
            }
        }

        private UserToken BuildToken(PersonDTO userInfo, IList<string> roles)
        {
            var claims = new List<Claim>
            {
        new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim ("Id", userInfo.Id.ToString()),
        new Claim ("Username", userInfo.Email)
    };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }

}