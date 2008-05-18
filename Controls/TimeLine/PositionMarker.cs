// PositionMarker.cs created with MonoDevelop
// User: alan at 12:10 PM 7/25/2007
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
	public class PositionMarker : Control, IMarker
	{
		private Ellipse ellipse;
		private TimeSpan time;

		public int ZIndex
		{
			get { return (int)GetValue(Shape.ZIndexProperty); }
			set { SetValue(Shape.ZIndexProperty, value); }
		}
		
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
		
		public PositionMarker(GtkSilver parent, TimeSpan time, double width, double height)
			: base()
		{
			ellipse = (Ellipse)parent.InitializeFromXaml("<Ellipse Name=\"Ellipse\" />", this);
			this.time = time;
			Height = height;
			Width = width;
			ellipse.SetValue(Shape.FillProperty, new SolidColorBrush(Colors.Yellow));
		}
		
		public override void SetValue (DependencyProperty property, object obj)
		{
			if(property == Shape.WidthProperty || property == Shape.HeightProperty)
				ellipse.SetValue(property, obj);
			
			base.SetValue(property, obj);
		}

	}
}
