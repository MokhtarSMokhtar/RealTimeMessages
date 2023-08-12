using API.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApi.DTOs;
using DatingApi.Entities;
using DatingApi.Helper;
using DatingApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatingApi.Data
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

        public void AddGroup(Group.Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
          _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessageAsync(int id)
        {
           return await _context.Messages.FindAsync(id);
        }

        public Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                  .OrderByDescending(m => m.MessageSent)
                  
                  .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username
                    && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username
                    && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername ==
                    messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)
            };
          var messageDtos = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return  PagedList<MessageDto>.CreateAsync(messageDtos, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipintUserName)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender).ThenInclude(i => i.Photos)
                .Include(u => u.Recipient).ThenInclude(i => i.Photos)
                .Where(
                    m => m.RecipientUsername == currentUserName && m.RecipientDeleted == false
                    && m.SenderUsername == recipintUserName ||
                    m.RecipientUsername == recipintUserName && m.SenderDeleted == false
                    && m.SenderUsername == currentUserName
                ).OrderBy(m => m.MessageSent).ToListAsync();
            var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUserName)
                .ToList();
            if(unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);

        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);

        }

        public async Task<bool> SaveAllAsync()
        {
          return await _context.SaveChangesAsync() > 0;
        }



        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Data.Group.Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                .Include(c => c.Connections)
                .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }
        public async Task<Data.Group.Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }
    }
}
