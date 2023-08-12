using API.Data;
using CloudinaryDotNet;
using DatingApi.Data;
using DatingApi.Helper;
using DatingApi.Interface;
using DatingApi.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DatingApi.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(option =>
            {
                option.UseSqlServer((config.GetConnectionString("DefaultConnection")));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySeittings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ILogUserActivity>();
            services.AddScoped<ILikesRepository, LikeRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddSingleton<PresenceTracker>();

            return services;    
        }
    }
}
