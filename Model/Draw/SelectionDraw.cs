// /home/alan/Projects/LunarEclipse/Model/Draw/SelectionDraw.cs created with MonoDevelop
// User: alan at 4:45 PM 6/27/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;


namespace LunarEclipse.Model
{
    public class SelectionDraw : DrawBase
    {
        public SelectionDraw()
            :base(new SelectionRectangle())
        {
                
        }
        
        internal override void DrawEnd (Point point)
        {
            panel.Children.Remove(Element);
        }
    }
}
