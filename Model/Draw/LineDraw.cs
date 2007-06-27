// /home/alan/Projects/LunarEclipse/LunarEclipse/Draw/LineDraw.cs created with MonoDevelop
// User: alan at 6:03 PM 6/15/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace LunarEclipse.Model
{
    public class LineDraw : DrawBase
    {
        public LineDraw(Point startLocation)
            : base(startLocation, new System.Windows.Shapes.Line())
        {
            base.Element.Fill = new SolidColorBrush(Colors.Red);
        }
 
        public override void Resize (Point end)
        {
            Line l = (Line)Element;
            l.X2 = end.X;
            l.Y2 = end.Y;
        }
    }
}
