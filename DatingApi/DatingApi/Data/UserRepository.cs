using API.Data;
using API.Entities;
using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.QueryableExtensions;
using DatingApi.DTOs;
using DatingApi.Helper;
using DatingApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatingApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<MemberDto> GetUserByIdAsync(int id)
        {
            var user = await _context.AppUser.FindAsync(id);
            return _mapper.Map<MemberDto>(user);
        }


        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.AppUser
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }


        public async Task<PagedList<MemberDto>> GetUsersAsync(UserPramas userPramas)
        {
            var query = _context.AppUser.AsQueryable();
            query = query.Where(u => u.UserName != userPramas.CurrentUsername);
            query = query.Where(u => u.Gender != userPramas.Gender);
            var minDob = DateTime.Today.AddYears(-userPramas.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userPramas.MinAge);
            query = userPramas.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };
            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            return await PagedList<MemberDto>
                .CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), userPramas.PageNumber,userPramas.PageSize);

        }
     
        public  void Update(AppUser user)
        {
             _context.Entry(user).State= EntityState.Modified;
        }
        public async Task<bool>SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.AppUser
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
    }
}
