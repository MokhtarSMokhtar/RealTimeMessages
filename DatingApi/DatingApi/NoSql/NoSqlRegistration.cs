
using Microsoft.EntityFrameworkCore;
using ProductDAL.Context;

namespace DatingApi.NoSql
{
    public static class NoSqlRegistration
    {
        public static IServiceCollection AddNoSqlService(this IServiceCollection services, IConfiguration config)
        {
            string accountEndpoint = config["Cosmos:AccountEndpoint"];
            string accountKey = config["Cosmos:AccountKey"];
            string quoteDatabaseName = config["Cosmos:productDbDatabaseName"];
            services.AddDbContext<ProductContext>(options =>
            {
                options.UseCosmos(accountEndpoint, accountKey, quoteDatabaseName);
            }); 
            return services;
        }
    }
}
