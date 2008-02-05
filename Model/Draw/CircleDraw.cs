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
			base.MouseMove(e);
            double max = Math.Max(Math.Abs(actualWidth), Math.Abs(actualHeight));
			
			Width = max;
			Height = max;
			
			Left = (actualWidth < 0) ? actualLeft - max : actualLeft;
			Top = (actualHeight < 0) ? actualTop - max : actualTop;
        }
    }
}
