using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QNTM.API.Helpers;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public class QNTMRepository : IQNTMRepository
    {
        private readonly DataContext _context;            
        public QNTMRepository(DataContext context)
        {
            _context = context;  
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages.Include(u => u.Sender).Include(u => u.Recipient).AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }       

        public async Task<User> GetUser(string username)
        {
            // usernames are unique, so lowercasing both queries removes uncertainty from end user's typing.
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));

            return user;
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages.Include(u => u.Sender)
                .Include(u => u.Recipient)
                .Where(m => m.RecipientId == userId && m.SenderId == recipientId 
                    || m.RecipientId == recipientId && m.SenderId == userId)
                    .OrderByDescending(m => m.MessageSent)
                    .ToListAsync();
            return messages;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();

            return users;
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<IEnumerable<ActiveChat>> GetActiveChats(int userId)
        {
            var chats = await _context.ActiveChats.Where(m => m.UserId == userId).ToListAsync();
            
            return chats;
        }

        public async Task<ActiveChat> GetChat(int id)
        {
            var chat = await _context.ActiveChats.FirstOrDefaultAsync(c => c.Id == id);

            return chat;
        }
    }
}