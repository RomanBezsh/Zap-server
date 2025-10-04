using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.DAL.EF;

namespace Zap.BLL.Infrastructure
{
    public static class ZapContextExtensions
    {
        public static void AddZapContext(this IServiceCollection services, string connection)
        {
            services.AddDbContext<ZapContext>(options => options.UseSqlServer(connection));
        }
    }

}
