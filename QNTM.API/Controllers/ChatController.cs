using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QNTM.API.Data;
using QNTM.API.Dtos;
using QNTM.API.Models;

namespace QNTM.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQNTMRepository _repo;

        public ChatController(IQNTMRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        /// <summary>
        /// Returns contact information of a singular active chat
        /// </summary>
        [HttpGet("{id}", Name="GetChat")]
        public async Task<IActionResult> GetChat(int id)
        {
            var chatFromRepo = await _repo.GetChat(id);
            
            var chat = _mapper.Map<ChatForCreationDto>(chatFromRepo);

            return Ok(chat);
        }

        /// <summary>
        /// Returns a list of all active chats the logged in user has.
        /// </summary>
        [HttpGet("getchats")]
        public async Task<IActionResult> GetChats(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var chatFromRepo = await _repo.GetActiveChats(userId);

            var activeChats = _mapper.Map<IEnumerable<ChatForReturnDto>>(chatFromRepo);

            return Ok(activeChats);
        }

        /// <summary>
        /// Creates an instance of an active chat and adds it to the authorized user's chat history
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> addActiveChat(int userId, [FromBody]ChatForCreationDto chatForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userFromRepo = await _repo.GetUser(userId);

            var chat = _mapper.Map<ActiveChat>(chatForCreationDto);

            userFromRepo.ActiveChats.Add(chat);

            if (await _repo.SaveAll())
            {
                var chatToReturn = _mapper.Map<ChatForCreationDto>(chat);
                return CreatedAtRoute("GetChat", new { id = chat.Id }, chatToReturn);
            }

            return BadRequest("Could not save chat");
        }


    }
}