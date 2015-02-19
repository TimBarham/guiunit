using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GuiUnit
{
	public class iOSMainLoopIntegration : IMainLoopIntegration
	{
		Type Application {
			get; set;
		}

		object sharedApplication;
		object SharedApplication {
			get {
				if (sharedApplication == null) {
					var sharedAppProperty = Application.GetProperty ("SharedApplication", BindingFlags.Public | BindingFlags.Static);
					sharedApplication = sharedAppProperty.GetValue (null, null);
				}
				return sharedApplication;
			}
		}

		public iOSMainLoopIntegration ()
		{
			Application =
				Type.GetType ("UIKit.UIApplication, Xamarin.iOS") ??
				Type.GetType ("MonoTouch.UIKit.UIApplication, monotouch");
			if (Application == null)
				throw new NotSupportedException ();
		}

		public void InitializeToolkit ()
		{
			// Initialization happens in RunMainLoop
		}

		public void InvokeOnMainLoop (InvokerHelper helper)
		{
			var potentialMethods = Application.GetMethods (System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
			var invokeOnMainThreadMethod = potentialMethods.First (m => m.Name == "BeginInvokeOnMainThread" && m.GetParameters ().Length == 1);
			var invoker = Delegate.CreateDelegate (invokeOnMainThreadMethod.GetParameters () [0].ParameterType, helper, "Invoke");
			invokeOnMainThreadMethod.Invoke (SharedApplication, new [] { invoker });
		}

		public void RunMainLoop ()
		{
			var mainMethod = Application.GetMethod ("Main", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof (string[]) }, null);
			mainMethod.Invoke (null, new object[] { new[] { "monotouch" } });
		}

		public void Shutdown ()
		{
			throw new NotSupportedException ();
		}
	}
}

