﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.IE.Qwiq.Exceptions;
using Tfs = Microsoft.TeamFoundation.Framework;

namespace Microsoft.IE.Qwiq.Proxies
{
    public class IdentityManagementServiceProxy : IIdentityManagementService
    {
        private readonly Tfs.Client.IIdentityManagementService2 _identityManagementService2;

        internal IdentityManagementServiceProxy(Tfs.Client.IIdentityManagementService2 identityManagementService2)
        {
            _identityManagementService2 = identityManagementService2;
        }

        public IEnumerable<ITeamFoundationIdentity> ReadIdentities(IEnumerable<IIdentityDescriptor> descriptors, MembershipQuery membershipQuery)
        {
            var rawDescriptors = descriptors.Select(descriptor =>
                new Tfs.Client.IdentityDescriptor(descriptor.IdentityType, descriptor.Identifier)).ToArray();
            var membership = (Tfs.Common.MembershipQuery) membershipQuery;

            var identities = _identityManagementService2.ReadIdentities(rawDescriptors, membership,
                Tfs.Common.ReadIdentityOptions.None);

            return identities.Select(identity => identity == null ? null : ExceptionHandlingDynamicProxyFactory.Create<ITeamFoundationIdentity>(new TeamFoundationIdentityProxy(identity)));
        }

        public IEnumerable<ITeamFoundationIdentity> ReadIdentities(IdentitySearchFactor searchFactor, string[] searchFactorValues, MembershipQuery membershipQuery)
        {
            var factor = (Tfs.Common.IdentitySearchFactor) searchFactor;
            var membership = (Tfs.Common.MembershipQuery) membershipQuery;

            var identities = _identityManagementService2.ReadIdentities(factor, searchFactorValues,
                membership, Tfs.Common.ReadIdentityOptions.None)[0];

            return identities.Select(identity => identity == null ? null : ExceptionHandlingDynamicProxyFactory.Create<ITeamFoundationIdentity>(new TeamFoundationIdentityProxy(identity)));
        }

        public IIdentityDescriptor CreateIdentityDescriptor(string identityType, string identifier)
        {
            return ExceptionHandlingDynamicProxyFactory.Create<IIdentityDescriptor>(new IdentityDescriptorProxy(new Tfs.Client.IdentityDescriptor(identityType, identifier)));
        }
    }
}