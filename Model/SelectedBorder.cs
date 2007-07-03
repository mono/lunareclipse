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

namespace LunarEclipse.Model
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
        
                
        public Visual Child
        {
            get { return (Children.Count == 1) ? this.Children[0] : null; }
            set 
            {
                if(value == null)
                    throw new ArgumentNullException();
                
                if(Children.Count != 0)
                    throw new InvalidOperationException("Cannot have more than one child");
                
                Children.Add(value);
                
                SetValue<double>(TopProperty,
                        (double)value.GetValue(TopProperty) - BorderWidth);
                SetValue<double>(LeftProperty,
                        (double)value.GetValue(LeftProperty) - BorderWidth);
                
                value.SetValue<double>(Canvas.TopProperty, BorderWidth);
                value.SetValue<double>(Canvas.LeftProperty, BorderWidth);
                
                Width = (double)value.GetValue(WidthProperty) + 2 * BorderWidth;
                Height = (double)value.GetValue(HeightProperty) + 2 * BorderWidth;
            }
        }
    }
}
