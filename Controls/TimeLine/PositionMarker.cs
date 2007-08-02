// PositionMarker.cs created with MonoDevelop
// User: alan at 12:10 PM 7/25/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace LunarEclipse.View
{
	public class PositionMarker : Control, IMarker
	{
		private TimeSpan time;

		public int ZIndex
		{
			get { return (int)GetValue(Shape.ZIndexProperty); }
			set { SetValue<int>(Shape.ZIndexProperty, value); }
		}
		
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
			SetValue<object>(Shape.FillProperty, new SolidColorBrush(Colors.Yellow));
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
