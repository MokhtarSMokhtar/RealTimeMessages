using API.Data;
using API.Entities;
using DatingApi.DTOs;
using DatingApi.Entities;
using DatingApi.Extentions;
using DatingApi.Helper;
using DatingApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatingApi.Data
{
    public class LikeRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserLikeId)
        {
            return await _context.UserLikes.FindAsync(sourceUserId, targetUserLikeId);
        }

        public async Task<PagedList<LikeDto>> GetUserLike(LikeParams likeParams)
        {
            var users = _context.AppUser.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.UserLikes.AsQueryable();

            if(likeParams.Predicate =="liked")
            {
                likes = likes.Where(like=> like.SourceUserId == likeParams.UserId);
                users = likes.Select(like => like.TargetUser); 
            }
            if (likeParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserId == likeParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }
            var likedUser =  users.Select(u => new LikeDto
            {
                Id = u.Id,
                UserName = u.UserName,
                City = u.City,
                KnownAs = u.KnownAs,
                PhotoUrl = u.Photos.FirstOrDefault(p => p.IsMain).Url,
                Age = u.DateOfBirth.CalculateAge()

            });

            return await PagedList<LikeDto>.CreateAsync(likedUser,
                likeParams.PageNumber, likeParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLike(int userId)
        {
          return await _context.AppUser.Include(i=> i.LikedUsers)
                .FirstOrDefaultAsync(i=> i.Id == userId);
        }
    }
}
