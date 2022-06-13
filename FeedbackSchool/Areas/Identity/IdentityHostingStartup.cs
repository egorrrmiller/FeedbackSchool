using FeedbackSchool.Areas.Identity;
using FeedbackSchool.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace FeedbackSchool.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddDbContext<FeedbackSchoolContext>(options =>
                options.UseSqlite(
                    context.Configuration.GetConnectionString("Sqlite")));

            services.AddDefaultIdentity<FeedbackSchoolUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<FeedbackSchoolContext>();
        });
    }
}