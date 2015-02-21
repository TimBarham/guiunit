using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using GuiUnit;

namespace GuiUnit.Android
{
	[Activity (Label = "GuiUnit_Android")]
	public class TestActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RunTests ();
		}

		protected virtual void RunTests ()
		{
			new TestRunner ().Execute (System.Environment.GetCommandLineArgs ());
		}
	}
}


