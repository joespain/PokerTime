// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(), //standard openid (subject id)
                new IdentityResources.Profile(), //first name, last name, etc.
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResources.Phone()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            { 
                new ApiScope("api-access")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("PokerTimeApi", "PokerTime API")
                {
                    Scopes = {"api-access"}
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[] 
            { 
                new Client
                {
                    ClientId = "PokerTimeApp",
                    ClientName = "PokerTime Webassembly App",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    RequirePkce = true,
                    AllowedScopes = {"openid", "profile", "offline_access", "api-access"},
                    RequireConsent = false,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    //AllowedCorsOrigins = {"https://pokertimeapp.azurewebsites.net"},
                    //RedirectUris = { "https://pokertimeapp.azurewebsites.net/authentication/login-callback" },
                    //PostLogoutRedirectUris = { "https://pokertimeapp.azurewebsites.net/authentication/logout-callback" }

                    AllowedCorsOrigins = {"https://localhost:5015"},
                    RedirectUris = { "https://localhost:5015/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:5015/authentication/logout-callback" }
                }

            };
    }
}