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

        [HttpGet("{id}", Name="GetChat")]
        public async Task<IActionResult> GetChat(int id)
        {
            var chatFromRepo = await _repo.GetChat(id);
            
            var chat = _mapper.Map<ChatForCreationDto>(chatFromRepo);

            return Ok(chat);
        }

        [HttpGet("getchats")]
        public async Task<IActionResult> GetChats(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var chatFromRepo = await _repo.GetActiveChats(userId);

            var activeChats = _mapper.Map<IEnumerable<ChatForCreationDto>>(chatFromRepo);

            return Ok(activeChats);
            
        }


    }
}