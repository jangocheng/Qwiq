﻿using System;

using Microsoft.Qwiq.Credentials;
using Microsoft.Qwiq.Soap;
using Microsoft.Qwiq.Tests.Common;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Should;

namespace Microsoft.Qwiq.Integration.Tests
{
    public abstract class WorkItemStoreFactoryContextSpecification : TimedContextSpecification
    {
        protected IWorkItemStoreFactory Instance { get; private set; }

        protected IWorkItemStore WorkItemStore { get; private set; }

        public override void Given()
        {
            Instance = WorkItemStoreFactory.Default;
            WorkItemStore = TimedAction(Create, "SOAP", "WIS Create");
            base.Given();
        }



        public override void Cleanup()
        {
            TimedAction(()=> WorkItemStore?.Dispose(), "SOAP", "WIS Dispose");

            base.Cleanup();

        }

        public abstract IWorkItemStore Create();
    }

    [TestClass]
    public class Given_a_Uri_and_Credential : WorkItemStoreFactoryContextSpecification
    {
        public override IWorkItemStore Create()
        {
            var uri = new Uri("https://microsoft.visualstudio.com/DefaultCollection");
            var cred = new VssClientCredentials(
                                                new WindowsCredential(true),
                                                CredentialPromptType.PromptIfNeeded)
                           {
                               Storage = new VssClientCredentialStorage()
                           };

#pragma warning disable CS0618 // Type or member is obsolete
            return Instance.Create(
                                   uri,
                                   new TfsCredentials(cred));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        [TestCategory("localOnly")]
        [TestCategory("SOAP")]
        public void Store_is_Created()
        {
            WorkItemStore.ShouldNotBeNull();
        }
    }

    [TestClass]
    public class Given_a_Uri_and_Credentials_from_the_CredentialsFactory : WorkItemStoreFactoryContextSpecification
    {
        public override IWorkItemStore Create()
        {
            var uri = new Uri("https://microsoft.visualstudio.com/DefaultCollection");


#pragma warning disable CS0618 // Type or member is obsolete
            return Instance.Create(
                uri,
                CredentialsFactory.CreateCredentials((string)null));
#pragma warning restore CS0618 // Type or member is obsolete
        }



        [TestMethod]
        [TestCategory("localOnly")]
        [TestCategory("SOAP")]
        public void Store_is_Created()
        {
            WorkItemStore.ShouldNotBeNull();
        }
    }
}
