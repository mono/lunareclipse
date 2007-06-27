// /home/alan/Projects/LunarEclipse/LunarEclipse/Model/Draw/EllipseDraw.cs created with MonoDevelop
// User: alan at 3:36 PM 6/19/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;


namespace LunarEclipse.Model
{
    public class EllipseDraw : DrawBase
    {
        public EllipseDraw(Point startLocation)
            : base(startLocation, new Ellipse())
        {
        }
    }
}
