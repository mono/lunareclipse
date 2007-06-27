// /home/alan/Projects/LunarEclipse/LunarEclipse/Model/Draw/CircleDraw.cs created with MonoDevelop
// User: alan at 3:36 PM 6/19/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;

namespace LunarEclipse.Model
{
    public class CircleDraw : DrawBase
    {
        public CircleDraw()
            : base(new System.Windows.Shapes.Ellipse())
        {
        }
        
        internal override void Resize (Point end)
        {
            double width = (double)Element.GetValue(Canvas.LeftProperty) - end.X;
            double height = (double)Element.GetValue(Canvas.TopProperty) - end.Y;
            
            double mouseDelta;
            if(Math.Abs(width) > Math.Abs(height))
                mouseDelta = end.X - position.X;
            else
                mouseDelta = end.Y - position.Y;
                
            base.Resize(new Point(position.X + mouseDelta, position.Y + mouseDelta));
        }
    }
}
