// Marker.cs created with MonoDevelop
// User: alan at 5:20 PM 7/20/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace LunarEclipse.View
{
#warning Fix me
	public class TimelineMarker : Canvas, IMarker
	{
		public const double MarkerWidth = 2;
		private TimeSpan time;
		
		public TimeSpan Time
		{
			get { return time; }
			set { time = value; }
		}
		
		public double Left
		{
			get { return (double)GetValue(System.Windows.Controls.Canvas.LeftProperty); }
			set { SetValue<double>(System.Windows.Controls.Canvas.LeftProperty, value);}
		}
		
		public TimelineMarker(double height, TimeSpan time)
		{
			this.time = time;
			base.Height = height;
			base.Width = TimelineMarker.MarkerWidth;
			
#warning fix this
			//Stroke = new SolidColorBrush(Colors.White);
			//Fill = new SolidColorBrush(Colors.White);
		}
	}
}
