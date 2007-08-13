// /home/alan/Projects/LunarEclipse/LunarEclipse/Model/Draw/CircleDraw.cs created with MonoDevelop
// User: alan at 3:36 PM 6/19/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace LunarEclipse.Model
{
    public class CircleDraw : DrawBase
    {
        public CircleDraw()
            : base(new System.Windows.Shapes.Ellipse())
        {
        }
        
        internal override void MouseMove (MouseEventArgs e)
        {
            Point end = e.GetPosition(Panel);
            double width = end.X - (double)Element.GetValue(Canvas.LeftProperty);
            double height = end.Y - (double)Element.GetValue(Canvas.TopProperty);

            if(Math.Abs(width) > Math.Abs(height))
            {
                Width = width;
                if(Math.Sign(width) != Math.Sign(height))
                    width = -width;
                Height = width;
            }
            else
            {
                Height = height;
                if(Math.Sign(width) != Math.Sign(height))
                    height = -height;
                Width = height;
            }
        }
    }
}
