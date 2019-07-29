using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QNTM.API.Data;
using QNTM.API.Dtos;
using QNTM.API.Helpers;
using QNTM.API.Models;

namespace QNTM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepositroy _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepositroy repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;            
        }  


        /// <summary>
        /// Creates a new user in database
        /// </summary>
        /// <param name="id"></param>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username Already Exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] string captchaResponse)
        {
            if (string.IsNullOrEmpty(captchaResponse))
            {
                return Ok(new {result = "invalid"});
            }
            
            var secret = "";

            if (string.IsNullOrEmpty(secret))
                return Ok(new {result = "invalid"});
            

            using (var client = new System.Net.WebClient())
            {
                var reply = await client.DownloadStringTaskAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={captchaResponse}");
                var result = await Task.Run( () => JsonConvert.DeserializeObject<RecaptchaResponse>(reply).Success);
                if (result)
                    return Ok(new {result = "valid"});
            }
            return Ok(new {result = "invalid"});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            if(!await _repo.UserExists(userForLoginDto.Username))
                return Unauthorized();

            var userFromRepo = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();
            
            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { username = userFromRepo.Username, token = tokenHandler.WriteToken(token) });
        }

    }
}