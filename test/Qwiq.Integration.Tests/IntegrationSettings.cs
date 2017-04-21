using System;

using Microsoft.Qwiq.Credentials;
using Microsoft.Qwiq.Soap;

namespace Microsoft.Qwiq.Integration.Tests
{
    public static class IntegrationSettings
    {
        private static readonly Uri Uri = new Uri("https://microsoft.visualstudio.com/defaultcollection");

        public static Func<IWorkItemStore> CreateRestStore { get; } = () =>
                                                                          {
                                                                              var options = RestOptions;
                                                                              return WorkItemStoreFactory.Default.Create(options);
                                                                          };

        public static Func<IWorkItemStore> CreateSoapStore { get; } = () =>
                                                                          {
                                                                              var options = SoapOptions;
                                                                              return WorkItemStoreFactory.Default.Create(options);
                                                                          };

        public static AuthenticationOptions RestOptions { get; } =
            new AuthenticationOptions(Uri, AuthenticationTypes.Windows, ClientType.Rest);

        public static AuthenticationOptions SoapOptions { get; } =
            new AuthenticationOptions(Uri, AuthenticationTypes.Windows, ClientType.Soap);
    }
}