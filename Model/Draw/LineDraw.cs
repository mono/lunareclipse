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
using System.Windows.Input;


namespace LunarEclipse.Model
{
    public class LineDraw : DrawBase
    {
        public LineDraw()
            : base(new System.Windows.Shapes.Line())
        {
            
        }
        
        internal override void Prepare ()
        {
            base.Prepare();
            Element.SetValue<object>(Shape.FillProperty, new SolidColorBrush(Colors.Red));
        }

        internal override void DrawStart (Panel panel, MouseEventArgs e)
        {
            base.DrawStart(panel, e);
            Line l = Element as Line;
            l.X1 = (l.X2 = Position.X);
            l.Y1 = (l.Y2 = Position.Y);
            l.SetValue<double>(Canvas.TopProperty, 0);
            l.SetValue<double>(Canvas.LeftProperty, 0);
        }
        
        internal override void MouseMove (MouseEventArgs end)
        {
            Line l = Element as Line;
            Point p = end.GetPosition(Panel);
            l.X2 = p.X;
            l.Y2 = p.Y;
        }
    }
}
