// /home/alan/Projects/LunarEclipse/LunarEclipse/Main.cs created with MonoDevelop
// User: alan at 4:48 PM 6/15/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// project created on 6/15/2007 at 4:48 PM
using System;
using Gtk;


using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using LunarEclipse.View;
using System.Diagnostics;

namespace LunarEclipse
{
	class MainClass
	{	
		public static void Main (string[] args)
		{
		    System.Diagnostics.Debug.Listeners.Add(new ConsoleTraceListener());
		    Gtk.Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Gtk.Application.Run ();
		}
	}
}