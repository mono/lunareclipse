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

namespace LunarEclipse.View
{
	public class PositionMarker : Control, IMarker
	{
		private Ellipse ellipse;
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
			get { return (double)GetValue(Canvas.LeftProperty); }
			set { SetValue<double>(Canvas.LeftProperty, value);}
		}
		
		public PositionMarker(TimeSpan time, double width, double height)
			: base()
		{
			ellipse = (Ellipse)InitializeFromXaml("<Ellipse Name=\"Ellipse\" />");
			this.time = time;
			Height = height;
			Width = width;
			ellipse.SetValue<object>(Shape.FillProperty, new SolidColorBrush(Colors.Yellow));
		}
		
		public override void SetValue<T> (DependencyProperty property, T obj)
		{
			if(property == Shape.WidthProperty || property == Shape.HeightProperty)
				ellipse.SetValue<T>(property, obj);
			
			base.SetValue<T>(property, obj);
		}

	}
}
