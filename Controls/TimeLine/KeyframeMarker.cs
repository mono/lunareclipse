// KeyframeMarker.cs created with MonoDevelop
// User: alan at 1:32 PM 7/24/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Shapes;
using System.Windows.Controls;
namespace LunarEclipse.View
{
	public class KeyframeMarker : Ellipse, IMarker
	{
		private TimeSpan time;
		
		public double Left
		{
			get { return (double)GetValue(Canvas.LeftProperty); }
			set { SetValue<double>(Canvas.LeftProperty, value); }
		}
		
		public TimeSpan Time
		{
			get { return time; }
			set { time = value; }
		}
		
		public KeyframeMarker(TimeSpan time)
		{
			this.time = time;
		}
	}
}
