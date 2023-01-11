//-----------------------------------------------------------------------
// <copyright file="MainViewController.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using CoreGraphics;
using Foundation;
using System;
using System.IO;
using UIKit;
using Microsoft.Intune.MAM;
using CoreFoundation;
using IntuneMAMSampleiOS.Msal;
using System.Threading.Tasks;

namespace IntuneMAMSampleiOS
{
    public partial class MainViewController: UIViewController
    {
        Task msalLoginTask;

        public MainViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            var intuneMAMEnrollmentDelegate = new MainControllerEnrollmentDelegate(this);
            intuneMAMEnrollmentDelegate.EnrollmentStateChanged += IntuneMAMEnrollmentDelegate_EnrollmentStateChanged;
            IntuneMAMEnrollmentManager.Instance.Delegate = intuneMAMEnrollmentDelegate;

            this.textCopy.ShouldReturn += this.DismissKeyboard;
            this.textUrl.ShouldReturn += this.DismissKeyboard;

            RefreshIntuneEnrollState();
        }

        partial void buttonUrl_TouchUpInside (UIButton sender)
        {
            string url = textUrl.Text;

            if (string.IsNullOrWhiteSpace(url))
            {
                url = "http://www.microsoft.com/en-us/server-cloud/products/microsoft-intune/";
            }
            
            UIApplication.SharedApplication.OpenUrl (NSUrl.FromString (url));
        }

        partial void buttonShare_TouchUpInside (UIButton sender)
        {
            NSString text = new NSString ("Test Content Sharing from Intune Sample App");
            UIActivityViewController avc = new UIActivityViewController (new NSObject[] { text }, null);
           
            if (avc.PopoverPresentationController != null)
            {
                UIViewController topController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                CGRect frame = UIScreen.MainScreen.Bounds;
                frame.Height /= 2;
                avc.PopoverPresentationController.SourceView = topController.View;
                avc.PopoverPresentationController.SourceRect = frame;
                avc.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Unknown;
            }

            this.PresentViewController(avc, true, null);
        }

        partial void ButtonSave_TouchUpInside(UIButton sender)
        {
            // Apps are responsible for enforcing Save-As policy
            if (!IntuneMAMPolicyManager.Instance.Policy.IsSaveToAllowedForLocation(IntuneMAMSaveLocation.LocalDrive, IntuneMAMEnrollmentManager.Instance.EnrolledAccount))
            {
                this.ShowAlert("Blocked", "Blocked from writing to local location");
                return;
            }

            string fileName = "intune-test.txt";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);

            try
            {
                File.WriteAllText(path, "Test Save to Personal");
                this.ShowAlert("Succeeded", "Wrote to " + fileName);
            }
            catch (Exception)
            {
                this.ShowAlert("Failed", "Failed to write to " + fileName);
            }
        }

        partial void ButtonLogIn_TouchUpInside(UIButton sender)
        {
            if (MsalClientService.MSAL_CONFIGURED)
            {
                Console.WriteLine("MSAL configured the login is delegated to the MSAL library");
                if (this.msalLoginTask is null || this.msalLoginTask.Status != TaskStatus.Running)
                {
                    SetLoginInProgressState();
                    msalLoginTask = LoginAndEnrollWithMSAL();
                }
            }
            else
            {
                Console.WriteLine("MSAL not configured Intune will handle the login");
                SetLoginInProgressState();
                IntuneMAMEnrollmentManager.Instance.LoginAndEnrollAccount(this.textEmail.Text);
            }
        }

        partial void ButtonLogOut_TouchUpInside(UIButton sender)
        {
            IntuneMAMEnrollmentManager.Instance.DeRegisterAndUnenrollAccount(IntuneMAMEnrollmentManager.Instance.EnrolledAccount, true);
            if (MsalClientService.MSAL_CONFIGURED)
            {
                SetLoginInProgressState();
                (new MsalClientService()).Logout()
                    .ContinueWith((t) => {
                        if(t.IsCompletedSuccessfully)
                            BeginInvokeOnMainThread(() => {
                                SetLoggedOutState();
                            });
                });
            }
        }

        public void ShowAlert (string title, string message)
        {
            BeginInvokeOnMainThread (() => {
                UIAlertController alertController = new UIAlertController
                {
                    Title = title,
                    Message = message
                };

                UIAlertAction alertAction = UIAlertAction.Create("OK", UIAlertActionStyle.Default, null);
                alertController.AddAction(alertAction);

                UIPopoverPresentationController popoverPresenter = alertController.PopoverPresentationController;
                if(null != popoverPresenter)
                {
                    CGRect frame = UIScreen.MainScreen.Bounds;
                    frame.Height /= 2;
                    popoverPresenter.SourceView = this.View;
                    popoverPresenter.SourceRect = frame;
                }

                this.PresentViewController(alertController, false, null);
            });
        }

		bool DismissKeyboard (UITextField textField)
		{
			textField.ResignFirstResponder ();
			return true;
		}

        public void SetLoginInProgressState()
        {
            activityIndicator.StartAnimating();
            this.buttonLogIn.Hidden = true;
            this.buttonLogOut.Hidden = true;
            this.textEmail.Hidden = true;
            this.labelEmail.Hidden = true;
            this.buttonLogIn.Hidden = true;
        }

        public void SetLoggedInState()
        {
            activityIndicator.StopAnimating();
            this.buttonLogOut.Hidden = false;
            this.buttonLogIn.Hidden = true;
            this.textEmail.Hidden = true;
            this.labelEmail.Hidden = true;
        }

        public void SetLoggedOutState()
        {
            activityIndicator.StopAnimating();
            this.buttonLogOut.Hidden = true;
            this.buttonLogIn.Hidden = false;

            if (!MsalClientService.MSAL_CONFIGURED)
            {
                this.textEmail.Hidden = false;
                this.labelEmail.Hidden = false;
                this.buttonLogIn.Hidden = false;
            }
        }

        async Task LoginAndEnrollWithMSAL()
        {
            var msalClient = new MsalClientService();
            string upn = null;
            try
            {
                upn = await msalClient.LoginSilent();
                Console.WriteLine("MSAL silent Login successful");
            }
            catch (Exception e)
            {
                Console.WriteLine("MSAL silent Login failed", e.Message);
            }

            if (upn is null)
            {
                try
                {
                    upn = await msalClient.LoginInteractive(this);
                    Console.WriteLine("MSAL interactive Login successful");
                }
                catch (Exception e)
                {
                    this.ShowAlert("MSAL interactive Login failed", e.Message);
                    return;
                }
            }

            Console.WriteLine($"Trying to enroll with Intune {upn}");
            IntuneMAMEnrollmentManager.Instance.RegisterAndEnrollAccount(upn);
        }

        void RefreshIntuneEnrollState()
        {
            var enrolled = IntuneMAMEnrollmentManager.Instance.EnrolledAccount != null;
            BeginInvokeOnMainThread(() => {
                labelEnrollState.Text = enrolled ? $"Enrolled with Intune on account: {IntuneMAMEnrollmentManager.Instance.EnrolledAccount}" : "Not enrolled with Intune";
                viewEnrollState.BackgroundColor = enrolled ? UIColor.Green : UIColor.Red;

                if (MsalClientService.MSAL_CONFIGURED)
                {
                    buttonLogIn.SetTitle("MSAL login", UIControlState.Normal);
                    buttonLogIn.SetTitle("MSAL login", UIControlState.Focused);
                    buttonLogIn.SetTitle("MSAL login", UIControlState.Selected);
                }

                if (enrolled)
                    SetLoggedInState();
                else
                    SetLoggedOutState();
            });
        }

        void IntuneMAMEnrollmentDelegate_EnrollmentStateChanged(object sender, EnrollmentEventArgs e)
        {
            RefreshIntuneEnrollState();
        }
    }
}
