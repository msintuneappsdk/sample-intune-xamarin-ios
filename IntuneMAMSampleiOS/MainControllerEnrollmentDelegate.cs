//-----------------------------------------------------------------------
// <copyright file="MainControllerEnrollmentDelegate.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Intune.MAM;

namespace IntuneMAMSampleiOS
{
    public class MainControllerEnrollmentDelegate : IntuneMAMEnrollmentDelegate
    {
        MainViewController ViewController { get; }

        public MainControllerEnrollmentDelegate(MainViewController controller)
        {
            this.ViewController = controller;
        }

		public override void EnrollmentRequestWithStatus(IntuneMAMEnrollmentStatus status)
		{
            if(status.DidSucceed)
            {
                this.ViewController.HideLogInButton();
            }
            else if (IntuneMAMEnrollmentStatusCode.MAMEnrollmentStatusLoginCanceled != status.StatusCode)
            {
                this.ViewController.ShowAlert("Enrollment Failed", status.ErrorString);
            }
		}

		public override void UnenrollRequestWithStatus(IntuneMAMEnrollmentStatus status)
		{
            if(status.DidSucceed)
            {
                this.ViewController.HideLogOutButton();
            }
            else
            {
                this.ViewController.ShowAlert("Unenroll Failed", status.ErrorString);
            }
		}
	}
}
