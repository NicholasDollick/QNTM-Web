using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QNTM.API.Data;
using QNTM.API.Dtos;

namespace QNTM.API.Controllers
{
    [Authorize]
    // api/Users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IQNTMRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IQNTMRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Returns IEnumerable of user data objects, containing the user's unique id and username.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForChatDto>>(users);

            return Ok(usersToReturn);
        }

        /// <summary>
        /// Returns username and id of user with supplied id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailDto>(user);

            return Ok(userToReturn);
        }

        /// <summary>
        /// Updates the data of a user in the database
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if(await _repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Upading user {id} failed on save");
        }
    }
}