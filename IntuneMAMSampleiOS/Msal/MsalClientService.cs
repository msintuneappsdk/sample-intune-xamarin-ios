using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace IntuneMAMSampleiOS.Msal
{
	public class MsalClientService
    {
        const string AAD_CLIENT_ID = "YOUR AAD CLIENT ID HERE";//ACTIVE DIRECTORY GUID;
        const string AAD_AUTHORITY_URI = "https://login.microsoftonline.com/" + AAD_CLIENT_ID;
        const string MSAL_REDIRECT_URI = "YOUR MSAL REDIRECT URI HERE";//msauth.com.xamarin.microsoftintunemamsample://auth
        const string MSAL_CLIENT_ID = "YOUR MSAL CLIENT ID HERE";//MSAL GUID;
        const string MSAL_SCOPE = "https://wip.mam.manage.microsoft.us//DeviceManagementManagedApps.ReadWrite";
        public static bool MSAL_CONFIGURED => (AAD_CLIENT_ID != DEFAULT_AAD_CLIENT_ID &&
                                               MSAL_REDIRECT_URI != DEFAULT_MSAL_REDIRECT_URI &&
                                               MSAL_CLIENT_ID != DEFAULT_MSAL_CLIENT_ID);

        const string DEFAULT_AAD_CLIENT_ID = "YOUR AAD CLIENT ID HERE";
        const string DEFAULT_MSAL_REDIRECT_URI = "YOUR MSAL REDIRECT URI HERE";
        const string DEFAULT_MSAL_CLIENT_ID = "YOUR MSAL CLIENT ID HERE";

        static IPublicClientApplication pca;
        static IPublicClientApplication PCA => pca ?? CreateMsalClient();
		
		static void MsalLogger(LogLevel level, string message, bool containsPii)
        {
            Console.WriteLine($"MSAL: {level} {message}");
		}
		
		static IPublicClientApplication CreateMsalClient()
		{
			var builder = PublicClientApplicationBuilder.Create(MSAL_CLIENT_ID)
				.WithAuthority(AzureCloudInstance.AzurePublic, AAD_CLIENT_ID)
				.WithBroker(true)
				.WithLogging(MsalLogger, LogLevel.Info, true)
                .WithRedirectUri(MSAL_REDIRECT_URI)
                .WithIosKeychainSecurityGroup("com.microsoft.adalcache");

            pca =  builder.Build();
            return pca;
		}
		
		public async Task<string> LoginInteractive(object rootViewController)
		{
			var request = PCA.AcquireTokenInteractive(new[] { MSAL_SCOPE });

			request = request.WithParentActivityOrWindow(rootViewController)
							 .WithPrompt(Prompt.SelectAccount);

            var result = await request.ExecuteAsync();
            var accessToken = result.AccessToken;

            var accounts = await PCA.GetAccountsAsync();

            Console.WriteLine($"Successful MSAL interactive login");

			return accounts.First().Username;
        }

		public async Task<string> LoginSilent()
        {
            var accounts = await PCA.GetAccountsAsync();

            var result = await PCA.AcquireTokenSilent(new[] { MSAL_SCOPE }, accounts.FirstOrDefault()).ExecuteAsync();
			var accessToken = result.AccessToken;
            Console.WriteLine($"Successful MSAL silent login");

            return accounts.First().Username;
        }

		public Task Logout()
        {
            return Task.Run(async () =>
            {
                var loggedAccounts = await PCA.GetAccountsAsync();
                foreach (var account in loggedAccounts)
                    await PCA.RemoveAsync(account);
            });
		}
	}
}