// /home/alan/Projects/LunarEclipse/LunarEclipse/IDraw.cs created with MonoDevelop
// User: alan at 6:02 PM 6/15/2007
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
    public abstract class DrawBase
    {
        private Shape element;
        protected Panel panel;
        protected Point position;
        
        public Shape Element
        {
            get { return element; }
        }
        
        internal DrawBase(Shape element)
        {
            this.element = element;
            element.Stroke = new SolidColorBrush(Colors.Red);
        }
        
        internal virtual void DrawStart(Panel panel, Point point)
        {
            this.panel = panel;
            panel.Children.Add(Element);
            position = point;
            Element.SetValue<double>(Canvas.LeftProperty, point.X);
            Element.SetValue<double>(Canvas.TopProperty, point.Y);
        }
        
        internal virtual void Resize(Point end)
        {
            double x = end.X - position.X;
            double y = end.Y - position.Y;
            
            position = end;
            Element.Width += x;
            Element.Height += y;
        }
        
        internal virtual void DrawEnd(Point point)
        {
            
        }
        
        internal virtual DrawBase Clone()
        {
            return (DrawBase)Activator.CreateInstance(this.GetType(), null);
        }
    }
}
