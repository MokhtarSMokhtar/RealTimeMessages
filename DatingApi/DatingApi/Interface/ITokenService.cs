using API.Entities;

namespace DatingApi.Interface
{
    public interface ITokenService
    {
       Task<string> CreateToken(AppUser user);
    }
}
