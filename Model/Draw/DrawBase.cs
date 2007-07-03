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
        
        private double Left
        {
            get { return (double)Element.GetValue(Canvas.LeftProperty); }
            set { Element.SetValue<double>(Canvas.LeftProperty, value); }
        }
        
        private double Top
        {
            get { return (double)Element.GetValue(Canvas.TopProperty); }
            set { Element.SetValue<double>(Canvas.TopProperty, value); }
        }
#endregion Properties
        
        internal DrawBase(Shape element)
        {
            this.element = element;
        }
        
        internal virtual void Cleanup()
        {
            
        }
        
        internal virtual void DrawStart(Panel panel, MouseEventArgs point)
        {
            this.panel = panel;
            
            Prepare();
            position = point.GetPosition(panel);
            Left = position.X;
            Top = position.Y;
        }
        
        internal virtual void Prepare()
        {
            element = (Shape)Activator.CreateInstance(Element.GetType());
            element.Stroke = new SolidColorBrush(Colors.Red);
            element.Fill = new SolidColorBrush(Colors.Cyan);
            Console.WriteLine("Setting cyan on new drawbase");
            panel.Children.Add(Element);
            Console.WriteLine("Added child, count: " + panel.Children.Count.ToString());
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
            GetNormalisedBounds(out top, out left, out width, out height);
            Element.Width = width;
            Element.Height = height;
            Left = left;
            Top = top;
        }
        
        protected void GetNormalisedBounds(out double top, out double left, out double width, out double height)
        {
            // We ensure that the width and height are positive quantities and the top
            // left corner is actually in the top left. For example: 
            // top: 10, left: 10, width: -10, height: -10 
            // is the same as top:0, left:0, width: 10, height: 10
            width = Element.Width;
            left = Left;
            if(width < 0)
            {
                left = left + width;
                width = -width;
            }
            
            height = Element.Height;
            top = Top;
            if(height < 0)
            {
                top = top + height;
                height = -height;
            }
        }
        
        protected virtual void MoveBy(Point p)
        {
            Left += p.X;
            Top += p.Y;
        }
    }
}
