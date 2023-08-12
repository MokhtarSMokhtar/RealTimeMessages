using API.Entities;
using DatingApi.DTOs;
using DatingApi.Entities;
using DatingApi.Helper;

namespace DatingApi.Interface
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId,int targetUserLikeId);
        Task<AppUser> GetUserWithLike(int userId);
        Task<PagedList<LikeDto>> GetUserLike(LikeParams likeParams);
    }
}
