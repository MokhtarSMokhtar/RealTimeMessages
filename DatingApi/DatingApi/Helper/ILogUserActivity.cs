using DatingApi.Extentions;
using DatingApi.Interface;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApi.Helper
{
    public class ILogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }
            var id = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await repo.GetUserByIdAsync(int.Parse(id));
            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}
