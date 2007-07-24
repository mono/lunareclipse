// Marker.cs created with MonoDevelop
// User: alan at 5:20 PM 7/20/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Shapes;
using System.Windows.Media;
namespace LunarEclipse
{
	public class TimelineMarker : Rectangle
	{
		public const double Width = 2;
		private TimeSpan time;
		
		public TimeSpan Time
		{
			get { return time; }
			internal set { time = value; }
		}
		
		public TimelineMarker(double height, TimeSpan time)
		{
			this.time = time;
			base.Height = height;
			base.Width = TimelineMarker.Width;
			
			Stroke = new SolidColorBrush(Colors.White);
			Fill = new SolidColorBrush(Colors.White);
		}
	}
}
