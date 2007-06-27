// /home/alan/Projects/LunarEclipse/LunarEclipse/Draw/LineDraw.cs created with MonoDevelop
// User: alan at 6:03 PM 6/15/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace LunarEclipse.Model
{
    public class LineDraw : DrawBase
    {
        public LineDraw()
            : base(new System.Windows.Shapes.Line())
        {
            Element.Fill = new SolidColorBrush(Colors.Red);
        }
        
        internal override void DrawStart (Panel panel, Point point)
        {
            base.DrawStart(panel, point);
            Line l = Element as Line;
            l.X1 = (l.X2 = point.X);
            l.Y1 = (l.Y2 = point.Y);
            l.SetValue<double>(Canvas.TopProperty, 0);
            l.SetValue<double>(Canvas.LeftProperty, 0);
        }
        
        internal override void Resize (Point end)
        {
            Line l = Element as Line;
            l.X2 = end.X;
            l.Y2 = end.Y;
        }


    }
}
