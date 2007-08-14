// Toolbox.cs created with MonoDevelop
// User: alan at 12:57 PM 7/30/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using LunarEclipse.Model;


namespace LunarEclipse
{
	public static class Toolbox
	{
		public static event EventHandler<PropertyChangedEventArgs> PropertyChanged;

		public static void ChangeProperty(DependencyObject target, DependencyProperty property, object value)
		{
			PropertyChangedEventArgs e = new PropertyChangedEventArgs(target, property, target.GetValue(property), value);
			target.SetValue<object>(property, value);
			RaiseEvent<PropertyChangedEventArgs>(PropertyChanged, null, e);
		}
		
		public static double DegreesToRadians(double angle)
		{
			return (angle * Math.PI) / 180.0 ;
		}
		
		public static double RadiansToDegrees(double angle)
		{
			return (angle / Math.PI) * 180.0;
		}
		
		internal static void RaiseEvent<T>(EventHandler<T> e, object sender, T args) where T : EventArgs
		{
			if(e != null)
				e(sender, args);
		}
	}
}
