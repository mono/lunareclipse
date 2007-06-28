// /home/alan/Projects/LunarEclipse/LunarEclipse/Model/Draw/SquareDraw.cs created with MonoDevelop
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
    public class SquareDraw : DrawBase
    {
        public SquareDraw()
            : base(new System.Windows.Shapes.Rectangle())
        {
        }

        internal override void Resize (MouseEventArgs e)
        {
            Point end = e.GetPosition(Panel);
            double width =  end.X - (double)Element.GetValue(Canvas.LeftProperty);
            double height = end.Y - (double)Element.GetValue(Canvas.TopProperty);
            
            width = Math.Max(width, height);
            Element.Width = width;
            Element.Height = width;
        }
    }
}
