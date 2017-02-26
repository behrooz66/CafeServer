using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using AuthServer.RepositoryInterfaces;

namespace AuthServer.Infrastructure
{
    public class ProfileService : IProfileService
    {
        private IAuthRepository _rep;
        public ProfileService(IAuthRepository rep)
        {
            this._rep = rep;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var subjectId = context.Subject.GetSubjectId();
                var user = this._rep.GetUserById(subjectId);
                var locIds = this._rep.GetDefaultLocationIds(subjectId);
                var locNames = this._rep.GetDefaultLocationNames(subjectId);
                var Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                    new Claim(JwtClaimTypes.PreferredUserName, user.Username),
                    new Claim("cityId", locIds[0].ToString()),
                    //new Claim("provinceId", locIds[1].ToString()),
                    //new Claim("countryId", locIds[2].ToString()),
                    new Claim("city", locNames[0].ToString()),
                    new Claim("province", locNames[1].ToString()),
                    new Claim("restaurantId", locIds[3].ToString())
                };
                var uc = this._rep.GetClaims(subjectId);
                foreach(var c in uc)
                {
                    Claims.Add(new Claim(c.ClaimType, c.ClaimValue));
                }
                
                context.IssuedClaims = Claims;
                return Task.FromResult(0);
            }
            catch(Exception x)
            {
                return Task.FromResult(0);
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = this._rep.GetUserById(context.Subject.GetSubjectId());
            context.IsActive = user != null && user.Active && user.Verified;
            return Task.FromResult(0);
        }
    }
}
