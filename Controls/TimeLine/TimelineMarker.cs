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
using Gtk.Moonlight;

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
			set { SetValue(Canvas.LeftProperty, value);}
		}
		
		public TimelineMarker(GtkSilver parent, double height, TimeSpan time) : base()
		{
			rectangle = (Rectangle) parent.InitializeFromXaml("<Rectangle Name=\"Rect\" />", this);
			this.time = time;
			Height = height;
			Width = TimelineMarker.MarkerWidth;
			
			rectangle.SetValue(Shape.StrokeProperty, new SolidColorBrush(Colors.White));
			rectangle.SetValue(Shape.FillProperty, new SolidColorBrush(Colors.White));
		}
		
		public override void SetValue (DependencyProperty property, object obj)
		{
			if(property == Shape.WidthProperty || property == Shape.HeightProperty)
				rectangle.SetValue(property, obj);
			
			base.SetValue(property, obj);
		}
	}
}
