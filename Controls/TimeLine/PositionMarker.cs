// PositionMarker.cs created with MonoDevelop
// User: alan at 12:10 PM 7/25/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Shapes;
using System.Windows.Media;

namespace LunarEclipse.View
{
	public class PositionMarker : Ellipse, IMarker
	{
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
		
		public PositionMarker(TimeSpan time, double width, double height)
		{
			this.time = time;
			Width = width;
			Height = height;
			Fill = new SolidColorBrush(Colors.Yellow);
//			RadialGradientBrush b = new RadialGradientBrush();
//			GradientStopCollection stops = new GradientStopCollection();
//			
//			GradientStop stop = new GradientStop();
//			stop.Color = Colors.Yellow;
//			stops.Add(stop);
//			
//			stop = new GradientStop();
//			stop.Color = Colors.Transparent;
//			stops.Add(stop);
//			
//			b.GradientStops = stops;
//			Fill = b;
		}
	}
}
