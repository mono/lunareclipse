// Marker.cs created with MonoDevelop
// User: alan at 5:20 PM 7/20/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace LunarEclipse.Controls
{
	public class TimelineMarker : Control, IMarker
	{
		public const double MarkerWidth = 2;
		
		private Rectangle rectangle;
		private TimeSpan time;
		
		public TimeSpan Time
		{
			get { return time; }
			set { time = value; }
		}
		
		public double Left
		{
			get { return (double)GetValue(Canvas.LeftProperty); }
			set { SetValue<double>(Canvas.LeftProperty, value);}
		}
		
		public TimelineMarker(double height, TimeSpan time) : base()
		{
			rectangle = (Rectangle)InitializeFromXaml("<Rectangle Name=\"Rect\" />");
			this.time = time;
			Height = height;
			Width = TimelineMarker.MarkerWidth;
			
			rectangle.SetValue<object>(Shape.StrokeProperty, new SolidColorBrush(Colors.White));
			rectangle.SetValue<object>(Shape.FillProperty, new SolidColorBrush(Colors.White));
		}
		
		public override void SetValue<T> (DependencyProperty property, T obj)
		{
			if(property == Shape.WidthProperty || property == Shape.HeightProperty)
				rectangle.SetValue<T>(property, obj);
			
			base.SetValue<T>(property, obj);
		}
	}
}
