//-----------------------------------------------------------------------
// <copyright file="MainControllerEnrollmentDelegate.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace IntuneMAMSampleiOS
{
    public class EnrollmentEventArgs : EventArgs
    {
        public bool Enrolled { get; set; }
    }
}
