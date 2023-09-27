//-----------------------------------------------------------------------
// <copyright file="MainControllerEnrollmentDelegate.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Microsoft.Intune.MAM;

namespace IntuneMAMSampleiOS
{
    public class MainControllerEnrollmentDelegate : IntuneMAMEnrollmentDelegate
    {
        MainViewController ViewController { get; }

        public event EventHandler<EnrollmentEventArgs> EnrollmentStateChanged;

        public MainControllerEnrollmentDelegate(MainViewController controller)
        {
            this.ViewController = controller;
        }

		public override void EnrollmentRequestWithStatus(IntuneMAMEnrollmentStatus status)
		{
            if(status.DidSucceed)
            {
                EnrollmentStateChanged?.Invoke(this, new EnrollmentEventArgs { Enrolled = true });
            }
            else if (IntuneMAMEnrollmentStatusCode.MAMEnrollmentStatusLoginCanceled != status.StatusCode)
            {
                this.ViewController.ShowAlert("Enrollment Failed", status.ErrorString);
                EnrollmentStateChanged?.Invoke(this, new EnrollmentEventArgs { Enrolled = false });
            }
		}

		public override void UnenrollRequestWithStatus(IntuneMAMEnrollmentStatus status)
		{
            if(status.DidSucceed)
            {
                EnrollmentStateChanged?.Invoke(this, new EnrollmentEventArgs { Enrolled = false });
            }
            else
            {
                this.ViewController.ShowAlert("Unenroll Failed", status.ErrorString);
            }
		}
	}
}
