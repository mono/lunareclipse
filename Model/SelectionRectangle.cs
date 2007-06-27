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
            this.Opacity = 0.5;
            this.Fill = new SolidColorBrush(Colors.Blue);
            this.Stroke = new SolidColorBrush(Colors.Green);
            this.StrokeDashArray = new double[] {1, 2 };
        }
    }
}
