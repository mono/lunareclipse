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
using System.Windows.Input;


namespace LunarEclipse.Model
{
    public abstract class DrawBase
    {
#region Fields
        
        private Shape element;
        private Panel panel;
        private Point position;
        
#endregion Fields
        
#region Properties
        
        public virtual bool CanUndo
        {
            get { return true; }
        }
        
        public Shape Element
        {
            get { return element; }
        }
        
        protected Panel Panel
        {
            get { return panel; }
            set { panel = value; }
        }
        
        protected Point Position
        {
            get { return position; }
            set { position = value; }
        }
        
#endregion Properties
        
        internal DrawBase(Shape element)
        {
            this.element = element;
            element.Stroke = new SolidColorBrush(Colors.Red);
        }
        
        internal virtual void Cleanup()
        {
            
        }
        
        internal virtual void DrawStart(Panel panel, MouseEventArgs point)
        {
            this.panel = panel;
            
            Prepare();
            position = point.GetPosition(panel);
            Element.SetValue<double>(Canvas.LeftProperty, position.X);
            Element.SetValue<double>(Canvas.TopProperty, position.Y);
        }
        
        internal virtual void Prepare()
        {
            element = (Shape)Activator.CreateInstance(Element.GetType());
            element.Stroke = new SolidColorBrush(Colors.Red);
            panel.Children.Add(Element);
        }
        
        internal virtual void Resize(MouseEventArgs e)
        {
            Point end = e.GetPosition(panel);
            double x = end.X - position.X;
            double y = end.Y - position.Y;
            
            position = end;
            Element.Width += x;
            Element.Height += y;
        }
        
        internal virtual void DrawEnd(MouseEventArgs point)
        {
            double top, left, width, height;
            GetNormalisedBounds(Element, out top, out left, out width, out height);
            Element.Width = width;
            Element.Height = height;
            Element.SetValue<double>(Canvas.LeftProperty, left);
            Element.SetValue<double>(Canvas.TopProperty, top);
        }
        
        protected static void GetNormalisedBounds(Shape shape, out double top, out double left, out double width, out double height)
        {
            width = shape.Width;
            left = (double)shape.GetValue(Canvas.LeftProperty);
            if(width < 0)
            {
                left = left + width;
                width = -width;
            }
            
            height = shape.Height;
            top = (double)shape.GetValue(Canvas.TopProperty);
            if(height < 0)
            {
                top = top + height;
                height = -height;
            }
        }
    }
}
