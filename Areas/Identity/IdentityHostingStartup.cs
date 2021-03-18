using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Valgapplikasjon.Areas.Identity;
using Valgapplikasjon.Data;

[assembly: HostingStartup(typeof(Valgapplikasjon.Areas.Identity.IdentityHostingStartup))]
namespace Valgapplikasjon.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.AddDbContext<AuthDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthDbContextConnection")));

                // Bekreftelse av ny bruker via email. Foreløpig satt til false. Kan implementeres. 

                services.AddIdentity<ValgapplikasjonUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();
            });
        }
    }
}