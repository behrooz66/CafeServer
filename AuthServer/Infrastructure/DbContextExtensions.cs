using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServer.Infrastructure
{
    public class DbContextExtensions
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.AddRange(
                    new Country
                    {
                        Name = "Canada"
                    },
                    new Country
                    {
                        Name = "United States"
                    }
                );

                context.AddRange(
                new Province { Name = "British Columbia" },
                new Province { Name = "Alberta" },
                new Province { Name = "Ontario" });

                context.AddRange(
                    new City
                    {
                        Name = "Prince George",
                        ProvinceId = 1,
                        Lat = 0,
                        Lon = 0,
                        SELat = 0,
                        SELon = 0,
                        SWLat = 0,
                        SWLon = 0,
                        NELat = 0,
                        NELon = 0,
                        NWLat = 0,
                        NWLon = 0
                    }, new City
                    {
                        Name = "Prince George",
                        ProvinceId = 1,
                        Lat = 0,
                        Lon = 0,
                        SELat = 0,
                        SELon = 0,
                        SWLat = 0,
                        SWLon = 0,
                        NELat = 0,
                        NELon = 0,
                        NWLat = 0,
                        NWLon = 0
                    });

                context.AddRange(
                    new User
                    {
                        Id = "0000-0000-0000-0000",
                        Active = true,
                        Email = "behrooz66@gmail.com",
                        Verified = true,
                        Name = "Behrooz Dalvandi",
                        Password = "bbcliqa",
                        Username = "behrooz66"
                    });

                context.AddRange(
                    new UserClaim
                    {
                        UserId = "0000-0000-0000-0000",
                        ClaimType = "role",
                        ClaimValue = "manager"
                    });
            }
                
        }
    }
}
