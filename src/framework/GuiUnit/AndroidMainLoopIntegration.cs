using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GuiUnit
{
	public class AndroidMainLoopIntegration : IMainLoopIntegration
	{
		Type HandlerType {
			get; set;
		}

		MethodInfo Post {
			get; set;
		}

		object Handler {
			get; set;
		}

		public AndroidMainLoopIntegration ()
		{
			HandlerType = Type.GetType ("Android.OS.Handler, Mono.Android");
			if (HandlerType == null)
				throw new NotSupportedException ();
			Post = HandlerType.GetMethod ("Post", new[] { typeof (Action) });
			if (Post == null)
				throw new MissingMethodException ("Post(Action)");
		}

		public void InitializeToolkit ()
		{
			Handler = Activator.CreateInstance (HandlerType);
		}

		public void InvokeOnMainLoop (InvokerHelper helper)
		{
			Post.Invoke (Handler, new object[] { new Action (helper.Invoke) });
		}

		public void RunMainLoop ()
		{
			// Should already be running
		}

		public void Shutdown ()
		{
		}
	}
}

