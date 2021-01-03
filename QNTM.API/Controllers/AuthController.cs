using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QNTM.API.Data;
using QNTM.API.Dtos;
using QNTM.API.Helpers;
using QNTM.API.Interfaces;
using QNTM.API.Models;

namespace QNTM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepositroy _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IQNTMRepository _qntmRepo;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthRepositroy repo, IConfiguration config, IMapper mapper, IQNTMRepository qntmRepo)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
            _qntmRepo = qntmRepo;
        }


        /// <summary>
        /// Creates a new user in database
        /// </summary>
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

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password, userForRegisterDto.PublicKey, userForRegisterDto.PrivateKeyHash);

            var userFromRepo = await _qntmRepo.GetUser(createdUser.Id);

            var updatedUser = await _repo.SetDefaultImage(userFromRepo);

            return StatusCode(201);
        }

        /// <summary>
        /// Checks if provided username already exists in database
        /// </summary>
        [HttpPost("exists")]
        public async Task<IActionResult> Exists(UserForCheckingDto userForCheckingDto)
        {
            if (await _repo.UserExists(userForCheckingDto.Username))
                return Ok(new { result = true });

            return Ok(new { result = false });
        }

        /// <summary>
        /// Verifies solved captcha token with google's recaptcha api to confirm validity.
        /// </summary>
        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] string captchaResponse)
        {
            if (string.IsNullOrEmpty(captchaResponse))
            {
                return Ok(new { result = "invalid" });
            }

            var secret = "";

            if (string.IsNullOrEmpty(secret))
                return Ok(new { result = "invalid" });


            using (var client = new System.Net.WebClient())
            {
                var reply = await client.DownloadStringTaskAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={captchaResponse}");
                var result = await Task.Run(() => JsonConvert.DeserializeObject<RecaptchaResponse>(reply).Success);
                if (result)
                    return Ok(new { result = "valid" });
            }
            return Ok(new { result = "invalid" });
        }

        /// <summary>
        /// Verifies supplied credentials and returns authorized token and user's details
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            if (!await _repo.UserExists(userForLoginDto.Username))
                return Unauthorized();

            var userFromRepo = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                // new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                // new Claim(ClaimTypes.Name, userFromRepo.Username),
                new Claim(JwtRegisteredClaimNames.NameId, userFromRepo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userFromRepo.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForChatDto>(userFromRepo);

            return Ok(new { token = tokenHandler.WriteToken(token), user, priv = userFromRepo.PrivateKeyHash });
        }

    }
}