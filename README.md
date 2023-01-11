# sample-intune-xamarin-ios
A simple, open source, sample app on Xamarin.iOS.

Added support for email and MSAL login

MSAL specifics:
This application implement the login through MSAL and then will register to Intune with the credentials recevied. More info here: https://learn.microsoft.com/es-es/mem/intune/developer/app-sdk-ios#apps-that-already-use-adal-or-msal
In order to login using MSAL you should set the values of your Azure Active Directoy and MSAL application on MsalClientService. 
Make sure your Bundle Id is the same on the Info.plist and Entitlement.plist keychain-access-groups.
