using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthServer
{
    public class Config
    {
        /*public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                new Scope()
                {
                    Name = "api",
                    Description = "General API of the application",
                    ShowInDiscoveryDocument = true,
                    DisplayName = "api"
                }
            };
        }*/

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "API 1 display name"),
               // new ApiResource("offline_access", "Kiram dahanesh")
            };
        }

        public static IEnumerable<IdentityServer4.Models.Client> GetClients()
        {
            return new List<IdentityServer4.Models.Client>
            {
                new IdentityServer4.Models.Client
                {
                    ClientId = "resourceOwner",
                    ClientSecrets = new List<IdentityServer4.Models.Secret>
                    {
                        new IdentityServer4.Models.Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime = 10,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    SlidingRefreshTokenLifetime = 3600,
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        "api",
                        //"offline_access"
                    }
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "01",
                    Username = "behrooz66@gmail.com",
                    Password = "bbcliqa"
                }
            };
        }
    }
}
