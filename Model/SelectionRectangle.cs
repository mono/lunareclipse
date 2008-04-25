// /home/alan/Projects/LunarEclipse/Model/SelectionRectangle.cs created with MonoDevelop
// User: alan at 4:47 PM 6/27/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using Gtk.Moonlight;

namespace LunarEclipse.Model
{
    public class SelectionRectangle : Control
    {
		private Rectangle rect;
		
        public SelectionRectangle()
            :base()
        {
			// FIXME: Find a proper way to do this.
			GtkSilver silver = new GtkSilver(); 
			rect = (Rectangle)silver.InitializeFromXaml("<Rectangle Name=\"Rect\" />", this);
			
            rect.Opacity = 0.33;
            rect.Fill = new SolidColorBrush(Colors.Blue);
            rect.Stroke = new SolidColorBrush(Colors.Green);
            rect.StrokeDashArray = new double[] {5, 5 };
            rect.StrokeThickness = 2;
            rect.SetValue<int>(ZIndexProperty, int.MaxValue);
        }
		
		public override void SetValue<T> (DependencyProperty property, T obj)
		{
			if(property == Shape.WidthProperty || property == Shape.HeightProperty)
				rect.SetValue<T>(property, obj);
			
			base.SetValue<T>(property, obj);
		}

    }
}
