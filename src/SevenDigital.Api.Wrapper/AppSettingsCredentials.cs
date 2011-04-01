using System.Configuration;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;

namespace SevenDigital.Api.Wrapper
{
    public class AppSettingsCredentials : OAuthCredentials
    {
        public AppSettingsCredentials()
        {
            ConsumerKey = ConfigurationManager.AppSettings["Wrapper.ConsumerKey"];
            ConsumerSecret = ConfigurationManager.AppSettings["Wrapper.ConsumerSecret"];
        }
    }
}