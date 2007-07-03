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
        public const double BorderWidth = 5;
        public const double HandleRadius = 3;
        
        private Visual child;

        public SelectedBorder()
        {
            this.IsHitTestVisible = false;
        }
        
                
        public Visual Child
        {
            get { return child; }
            set 
            {
                if(value == null)
                    throw new ArgumentNullException();
                
                if(child != null)
                    throw new InvalidOperationException("Cannot have more than one child");
                
                
                child = value;
                Children.Add(value);
                
                SetValue<double>(TopProperty,
                        (double)value.GetValue(TopProperty) - BorderWidth);
                SetValue<double>(LeftProperty,
                        (double)value.GetValue(LeftProperty) - BorderWidth);
                
                value.SetValue<double>(Canvas.TopProperty, BorderWidth);
                value.SetValue<double>(Canvas.LeftProperty, BorderWidth);
                
                Width = (double)value.GetValue(WidthProperty) + 2 * BorderWidth;
                Height = (double)value.GetValue(HeightProperty) + 2 * BorderWidth;
                
                DrawHandles();
            }
        }
        
        private void DrawHandles()
        {
            Ellipse circle = new Ellipse();
            Line line = new Line();
            
            double midX = Width / 2.0;
            double midY = Height / 2.0;
            
            // Top side
            Children.Add(CreateCircle(new Point(0, 0)));
            
            Children.Add(CreateLine(new Point(HandleRadius * 2, HandleRadius),
                                    new Point(midX - HandleRadius, HandleRadius)));
            
            Children.Add(CreateCircle(new Point(midX - HandleRadius, 0)));


            Children.Add(CreateLine(new Point(midX + HandleRadius, HandleRadius),
                                    new Point(Width - HandleRadius * 2, HandleRadius)));
            
            // Right side            
            Children.Add(CreateCircle(new Point(Width - HandleRadius * 2, 0)));
            
            Children.Add(CreateLine(new Point(Width - HandleRadius, HandleRadius * 2),
                                    new Point( Width - HandleRadius, midY - HandleRadius)));
            
            Children.Add(CreateCircle(new Point(Width - HandleRadius * 2, midY - HandleRadius)));

            Children.Add(CreateLine(new Point(Width - HandleRadius, midY + HandleRadius),
                                    new Point(Width - HandleRadius, Height - HandleRadius * 2)));
            
            // Bottom side
            Children.Add(CreateCircle(new Point(Width - HandleRadius * 2, Height - HandleRadius * 2)));
            
            Children.Add(CreateLine(new Point(Width - HandleRadius * 2, Height - HandleRadius),
                                    new Point(midX + HandleRadius, Height - HandleRadius)));
            
            Children.Add(CreateCircle(new Point(midX - HandleRadius, Height - HandleRadius * 2)));
            
            Children.Add(CreateLine(new Point(midX - HandleRadius, Height - HandleRadius),
                                    new Point(HandleRadius * 2, Height - HandleRadius)));
            
            // Left side
            Children.Add(CreateCircle(new Point(0, Height - HandleRadius * 2)));
            
            Children.Add(CreateLine(new Point(HandleRadius, Height - HandleRadius * 2),
                                    new Point(HandleRadius, midY + HandleRadius)));
            
            Children.Add(CreateCircle(new Point(0, midY - HandleRadius)));
            
            Children.Add(CreateLine(new Point(HandleRadius, midY - HandleRadius),
                                    new Point(HandleRadius, HandleRadius * 2)));
        }
        
        private Line CreateLine(Point start, Point end)
        {
            Line line = new Line();
            line.X1 = start.X;  line.Y1 = start.Y;
            line.X2 = end.X;    line.Y2 = end.Y;
            line.StrokeThickness = 2;
            line.Stroke = new SolidColorBrush(Colors.Black);
                
            return line;
        }
        
        private Ellipse CreateCircle(Point start)
        {
            Ellipse circle = new Ellipse();
            circle.SetValue<double>(TopProperty, start.Y);
            circle.SetValue<double>(LeftProperty, start.X);
            circle.Width = HandleRadius * 2;
            circle.Height = HandleRadius * 2;
            circle.Fill = new SolidColorBrush(Colors.White);
            //Console.WriteLine("Setting white - Circle Handle");
            circle.Stroke = new SolidColorBrush(Colors.Blue);
            circle.Cursor = System.Windows.Input.Cursors.Hand;
            return circle;
        }
    }
}
