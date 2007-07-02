// /home/alan/Projects/LunarEclipse/Model/SelectedBorder.cs created with MonoDevelop
// User: alan at 11:00 AM 7/2/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace LunarEclipse
{
    public class SelectedBorder : System.Windows.Controls.Canvas
    {
        public const int BorderWidth = 5;
        public SelectedBorder()
        {
            this.Background = new SolidColorBrush(Colors.Blue);
            this.IsHitTestVisible = false;
            this.Opacity = 0.3;
        }
    }
}
