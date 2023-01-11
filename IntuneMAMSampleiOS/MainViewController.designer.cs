// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace IntuneMAMSampleiOS
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		UIKit.UIActivityIndicatorView activityIndicator { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton buttonLogIn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton buttonLogOut { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton buttonSave { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton buttonShare { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton buttonUrl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel labelEmail { get; set; }

		[Outlet]
		UIKit.UILabel labelEnrollState { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextField textCopy { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextField textEmail { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextField textUrl { get; set; }

		[Outlet]
		UIKit.UIView viewEnrollState { get; set; }

		[Action ("ButtonLogIn_TouchUpInside:")]
		partial void ButtonLogIn_TouchUpInside (UIKit.UIButton sender);

		[Action ("ButtonLogOut_TouchUpInside:")]
		partial void ButtonLogOut_TouchUpInside (UIKit.UIButton sender);

		[Action ("ButtonSave_TouchUpInside:")]
		partial void ButtonSave_TouchUpInside (UIKit.UIButton sender);

		[Action ("buttonShare_TouchUpInside:")]
		partial void buttonShare_TouchUpInside (UIKit.UIButton sender);

		[Action ("buttonUrl_TouchUpInside:")]
		partial void buttonUrl_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (buttonLogIn != null) {
				buttonLogIn.Dispose ();
				buttonLogIn = null;
			}

			if (buttonLogOut != null) {
				buttonLogOut.Dispose ();
				buttonLogOut = null;
			}

			if (buttonSave != null) {
				buttonSave.Dispose ();
				buttonSave = null;
			}

			if (buttonShare != null) {
				buttonShare.Dispose ();
				buttonShare = null;
			}

			if (buttonUrl != null) {
				buttonUrl.Dispose ();
				buttonUrl = null;
			}

			if (labelEmail != null) {
				labelEmail.Dispose ();
				labelEmail = null;
			}

			if (textCopy != null) {
				textCopy.Dispose ();
				textCopy = null;
			}

			if (textEmail != null) {
				textEmail.Dispose ();
				textEmail = null;
			}

			if (textUrl != null) {
				textUrl.Dispose ();
				textUrl = null;
			}

			if (labelEnrollState != null) {
				labelEnrollState.Dispose ();
				labelEnrollState = null;
			}

			if (viewEnrollState != null) {
				viewEnrollState.Dispose ();
				viewEnrollState = null;
			}

			if (activityIndicator != null) {
				activityIndicator.Dispose ();
				activityIndicator = null;
			}
		}
	}
}
