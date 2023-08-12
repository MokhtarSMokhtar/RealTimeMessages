using System.Text.RegularExpressions;
using DatingApi.Data;
using DatingApi.DTOs;
using DatingApi.Entities;
using DatingApi.Helper;
using Newtonsoft.Json.Linq;
using DatingApi.Data.Group;

namespace DatingApi.Interface
{
    public interface IMessageRepository
    {
           
        void AddGroup(Data.Group.Group group);
        void RemoveConnection(Connection connection);
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<Message> GetMessageAsync(int id);
        Task<Data.Group.Group> GetMessageGroup(string groupName);
        Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipintUserName);
        Task<bool> SaveAllAsync();
        Task<Data.Group.Group> GetGroupForConnection(string connectionId);
        Task<Connection> GetConnection(string connectionId);
    }
}
