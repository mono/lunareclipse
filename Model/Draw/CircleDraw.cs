// /home/alan/Projects/LunarEclipse/LunarEclipse/Model/Draw/CircleDraw.cs created with MonoDevelop
// User: alan at 3:36 PM 6/19/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;

namespace LunarEclipse.Model
{
    public class CircleDraw : DrawBase
    {
        public CircleDraw(Point startLocation)
            : base(startLocation, new System.Windows.Shapes.Ellipse())
        {
        }
        
        public override void Resize (Point end)
        {
            double size = 0;
            if(Math.Abs(end.X - Start.X) > Math.Abs(end.Y - Start.Y))
                size = end.X - Start.X;
            else
                size = end.Y - Start.Y;
                
            Element.Width = size;
            Element.Height = size;
        }
    }
}
