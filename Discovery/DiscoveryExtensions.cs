﻿using Digioma.Office365.Client.Adal;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Office365.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Digioma.Office365.Client.Discovery
{
    public static class DiscoveryExtensions
    {
        public static DiscoveryClient CreateDiscoveryClient(this AuthenticationContext authContext)
        {
            return new DiscoveryClient(AppSettings.DiscoveryServiceEndpointUri,
                async () =>
                {
                    var authResult = await authContext.AcquireDiscoveryServiceTokenSilentAsync();
                    return authResult.AccessToken;
                });

        }

        public static DiscoveryClient CreateDiscoveryClient(this IIdentity identity)
        {
            if(null != identity)
            {
                return identity
                    .CreateAuthenticationContext()
                    .CreateDiscoveryClient();
            }
            return null;
        }

        public static DiscoveryClient CreateDiscoveryClient(this AuthenticationResult token)
        {
            return new DiscoveryClient(AppSettings.DiscoveryServiceEndpointUri,
                () =>
                {
                    return token.AccessToken;
                });
        }

        public static DiscoveryClient CreateDiscoveryClient(this IPrincipal user)
        {
            if(null != user && null != user.Identity)
            {
                return user.Identity.CreateDiscoveryClient();
            }
            return null;
        }

        public static CapabilityDiscoveryResult DiscoverCapability(this DiscoveryClient discoverClient, Capability capability)
        {
            return AsyncHelper.RunSync<CapabilityDiscoveryResult>(async () =>
                {
                    return await discoverClient.DiscoverCapabilityAsync(capability);
                });
        }

        public static async Task<CapabilityDiscoveryResult> DiscoverCapabilityAsync(this DiscoveryClient discoveryClient, Capability capability)
        {
            return await discoveryClient.DiscoverCapabilityAsync(capability.ToString());
        }

        public static CapabilityDiscoveryResult DiscoverContactsCapability(this DiscoveryClient discoveryClient)
        {
            return discoveryClient.DiscoverCapability(Capability.Contacts);
        }

        public static async Task<CapabilityDiscoveryResult> DiscoverContactsCapabilityAsync(this DiscoveryClient discoveryClient)
        {
            return await discoveryClient.DiscoverCapabilityAsync(Capability.Contacts);
        }

        public static CapabilityDiscoveryResult DiscoverDirectoryCapability(this DiscoveryClient discoveryClient)
        {
            return discoveryClient.DiscoverCapability(Capability.Directory);
        }

        public static async Task<CapabilityDiscoveryResult> DiscoverDirectoryCapabilityAsync(this DiscoveryClient discoveryClient)
        {
            return await discoveryClient.DiscoverCapabilityAsync(Capability.Directory);
        }
    }
}
