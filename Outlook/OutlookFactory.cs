﻿using Digioma.Office365.Client.Adal;
using Digioma.Office365.Client.Claims;
using Digioma.Office365.Client.Discovery;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Office365.Discovery;
using Microsoft.Office365.OutlookServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Digioma.Office365.Client.Outlook
{
    public static class OutlookFactory
    {

        public static OutlookServicesClient CreateOutlookServicesClient(AuthenticationContext authContext, CapabilityDiscoveryResult discoveryResult)
        {
            return new OutlookServicesClient(discoveryResult.ServiceEndpointUri,
                async () =>
                {
                    var authResult = await authContext.AcquireTokenSilentAsync(discoveryResult.ServiceResourceId);
                    return authResult.AccessToken;
                });
        }

        public static async Task<IContactCollection> MyContactsAsync()
        {
            return await ClaimsPrincipal.Current.MyContactsAsync();
        }

        public static async Task<IContactCollection> MyContactsAsync(this IPrincipal user)
        {
            AuthenticationContext authContext = user.CreateAuthenticationContext();

            var discoClient = authContext.CreateDiscoveryClient();
            var dcr = await discoClient.DiscoverContactsCapabilityAsync();
            var outlookClient = CreateOutlookServicesClient(authContext, dcr);

            return outlookClient.Me.Contacts;
        }

    }
}
