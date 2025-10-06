using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zap.DAL.EF;

namespace Zap.DAL.Infrastructure
{
    public static class ZapContextExtensions
    {
        public static void AddZapContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ZapContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
