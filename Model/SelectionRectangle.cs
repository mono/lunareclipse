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

namespace LunarEclipse
{
    public class SelectionRectangle : Rectangle
    {
        public SelectionRectangle()
            :base()
        {
            this.Opacity = 0.33;
            this.Fill = new SolidColorBrush(Colors.Blue);
            Console.WriteLine("Setting blue on selection rectangle");
            this.Stroke = new SolidColorBrush(Colors.Green);
            this.StrokeDashArray = new double[] {5, 5 };
            this.StrokeThickness = 2;
        }
    }
}
