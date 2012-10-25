using System;
#if DEBUG
using System.Diagnostics;
#endif // DEBUG

namespace GtkEdit
{
	#region class Program

	public static class Program
    {
		#region Static Methods

		[STAThread]
		public static void Main()
		{
#			if DEBUG
			InitDebug();
#			endif // DEBUG

			ShowSplash();
			StartGui();
		}

#		if DEBUG
		private static void InitDebug()
		{
			Debug.Listeners.Add(new TextWriterTraceListener("debug.log", "log"));
		}
#		endif // DEBUG

		private static void ShowSplash()
        {
        }

        private static void StartGui()
        {
            Gtk.Application.Init();
            using (var windowMain = new WindowMain())
            {
				windowMain.Hidden += WindowMain_Hidden;
                windowMain.Show();
                Gtk.Application.Run();
                windowMain.Destroy();
            }
		}

		#region Static Event Handlers

		private static void WindowMain_Hidden(object sender, EventArgs e)
		{
			Gtk.Application.Quit();
		}

		#endregion Static Event Handlers

		#endregion Static Methods
	}

	#endregion class Program
}