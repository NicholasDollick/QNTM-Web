using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using QNTM.API.Dtos;
using QNTM.API.Helpers;
using QNTM.API.Interfaces;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void AddGroup(Group group)
        {
           throw new System.NotImplementedException();
        }

        public void AddMessage(Message message)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteMessage(Message message)
        {
            throw new System.NotImplementedException();
        }

        public Task<Connection> GetConnection(string connectionId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Group> GetGroupForConnection(string connectionId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Message> GetMessage(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Group> GetMessageGroup(string groupName)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveConnection(Connection connection)
        {
            throw new System.NotImplementedException();
        }
    }
}