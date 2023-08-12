using API.Entities;
using DatingApi.DTOs;
using DatingApi.Helper;

namespace DatingApi.Interface
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<PagedList<MemberDto>> GetUsersAsync(UserPramas userPramas);
        Task<MemberDto> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<MemberDto> GetMemberAsync(string username);
        Task<bool> SaveAllAsync();

    }
}
