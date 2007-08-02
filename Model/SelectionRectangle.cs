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

namespace LunarEclipse.Model
{
#warning Fix me
    public class SelectionRectangle : Control
    {
		private Rectangle rect;
		
        public SelectionRectangle()
            :base()
        {
			InitializeFromXaml("<Canvas><Rectangle Name=\"Rect\" /></Canvas>");
			rect = (Rectangle)FindName("Rect");
            rect.Opacity = 0.33;
            rect.Fill = new SolidColorBrush(Colors.Blue);
            rect.Stroke = new SolidColorBrush(Colors.Green);
            rect.StrokeDashArray = new double[] {5, 5 };
            rect.StrokeThickness = 2;
            SetValue<int>(ZIndexProperty, int.MaxValue);
        }
		
		public override object GetValue (DependencyProperty property)
		{
			return rect.GetValue(property);
		}
		
		public override void SetValue<T> (DependencyProperty property, T obj)
		{
			rect.SetValue<T>(property, obj);
		}

    }
}
