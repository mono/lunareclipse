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
        private Visual handle;
        private bool updating;
              
        public const double BorderWidth = 20;
        public const double HandleRadius = 5;
        
        private Visual child;
        private Shape height1;
        private Shape height2;
        private Shape width1;
        private Shape width2;
        private Shape rotate1;
        private Shape rotate2;
        private Shape rotate3;
        private Shape rotate4;
        private Shape[] lines;
        
        public Visual Handle
        {
            get { return this.handle; }
            internal set { this.handle = value; }
        }
        
        public Shape HeightHandle1
        {
            get { return this.height1; }
        }
        public Shape HeightHandle2
        {
            get { return this.height2; }
        }
        public Shape WidthHandle1
        {
            get { return this.width1; }
        }
        public Shape WidthHandle2
        {
             get { return this.width2; }
        }
        public Shape RotateHandle1
        {
            get { return this.rotate1; }
        }
        public Shape RotateHandle2
        {
            get { return this.rotate2; }
        }
        public Shape RotateHandle3
        {
            get { return this.rotate3; }
        }
        public Shape RotateHandle4
        {
            get { return this.rotate4; }
        }
        
        public SelectedBorder()
        {
            
        }
        
        public RotateTransform RotateTransform
        {
            get { return (RotateTransform)this.child.GetValue(RenderTransformProperty); }
            set 
            {
                this.child.SetValue<RotateTransform>(RenderTransformProperty, value); 
                base.SetValue<RotateTransform>(RenderTransformProperty, value);
            }
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
                
                base.SetValue<int>(ZIndexProperty, int.MaxValue);
                if(!updating)
                {
                    updating = true;
                    ResizeBorder();
                    updating = false;
                }

                child.SetValue<Point>(RenderTransformOriginProperty, new Point(0.5, 0.5));
                base.SetValue<Point>(RenderTransformOriginProperty, new Point(0.5, 0.5));
                
                if(RotateTransform == null)
                    RotateTransform = new RotateTransform();
                base.SetValue<RotateTransform>(RenderTransformProperty, RotateTransform);
            }
        }
            
        
        public double Left
        {
            get { return (double)base.GetValue(LeftProperty); }
            set { base.SetValue<double>(LeftProperty, value); }
        }
        public double Top
        {
            get { return (double)base.GetValue(TopProperty); }
            set { base.SetValue<double>(TopProperty, value); }
        }

        internal void ResizeBorder()
        {         
            double childTop = (double)child.GetValue(TopProperty);
            double childLeft = (double)child.GetValue(LeftProperty);
            double childWidth = (double)child.GetValue(WidthProperty);
            double childHeight = (double)child.GetValue(HeightProperty);
            
            // First set the position of the selection canvas
            Top = (childTop - BorderWidth);
            Left = (childLeft - BorderWidth);
            base.SetValue<double>(WidthProperty, childWidth + BorderWidth * 2);
            base.SetValue<double>(HeightProperty, childHeight + BorderWidth * 2);

            DrawHandles(((VisualCollection)base.GetValue(ChildrenProperty)).Count == 0);
            //DrawTransform();
        }
        
        private void DrawHandles(bool firstDraw)
        {
            VisualCollection Children = (VisualCollection)base.GetValue(ChildrenProperty);
            Ellipse circle;
            Line line;
            
            double left = 0;
            double top = 0;
            double right = (double)GetValue(WidthProperty) - left;
            double bottom = (double)GetValue(HeightProperty) - top;
            double midX = (right + left) / 2.0;
            double midY = (top + bottom) / 2.0;
            
            // Top left rotate handle
            if(firstDraw)
            {
                Console.WriteLine("DRAWING NEW BORDER SHAPES");
                circle = new Ellipse();
                Children.Add(circle);
                rotate1 = circle;
                circle.MouseLeftButtonDown += delegate { Handle = rotate1; Console.WriteLine("Rotate1 {0}", rotate1); };
                
                
                circle = new Ellipse();
                Children.Add(circle);
                height1= circle;
                circle.MouseLeftButtonDown+= delegate { Handle = height1; Console.WriteLine("height1 {0}", height1);};
                
                
                circle = new Ellipse();
                Children.Add(circle);
                rotate2 = circle;
                circle.MouseLeftButtonDown+= delegate { Handle = rotate2; };
                
                
                circle = new Ellipse();
                Children.Add(circle);
                width1 = circle;
                circle.MouseLeftButtonDown += delegate { Handle = width1; };
                
                
                circle = new Ellipse();
                Children.Add(circle);
                rotate3 = circle;
                circle.MouseLeftButtonDown += delegate { Handle = rotate3; };
                
                
                circle = new Ellipse();
                Children.Add(circle);
                height2 = circle;
                circle.MouseLeftButtonDown += delegate { Handle = height2; };
                
                
                circle = new Ellipse();
                Children.Add(circle);
                rotate4 = circle;
                circle.MouseLeftButtonDown+= delegate { Handle = rotate4; };
                
                
                circle = new Ellipse();
                Children.Add(circle);
                width2 = circle;
                circle.MouseLeftButtonDown+= delegate { Handle = width2; };
                
                lines = new Shape[8];
                for(int i=0; i < 8; i++)
                {
                    lines[i] = new Line();
                    Children.Add(lines[i]);
                }
            }
            
            // Top left rotate handle
            SetCircle(rotate1, new Point(left, top));
            // Top centre stretch handle
            SetCircle(height1, new Point(midX - HandleRadius, top));
            // Top right rotate handle
            SetCircle(rotate2, new Point(right - HandleRadius * 2, top));
            // Middle right stretch handle
            SetCircle(width1, new Point(right - HandleRadius * 2, midY - HandleRadius));
            // Bottom right rotate handle
            SetCircle(rotate3, new Point(right - HandleRadius * 2, bottom - HandleRadius * 2));
            // Bottom centre stretch handle
            SetCircle(height2, new Point(midX - HandleRadius, bottom - HandleRadius * 2));
            // Bottom left rotate handle
            SetCircle(rotate4, new Point(left, bottom - HandleRadius * 2));
            // Middle left stretch handle
            SetCircle(width2, new Point(left, midY - HandleRadius));
            
            // Top left line
            SetLine(lines[0], new Point(left + HandleRadius * 2, top + HandleRadius),
                              new Point(midX - HandleRadius, top + HandleRadius));
           
           
            // Top right line
            SetLine(lines[1], new Point(midX + HandleRadius, top + HandleRadius),
                              new Point(right - HandleRadius * 2, top + HandleRadius));
           
            
            // Right top line
            SetLine(lines[2], new Point(right - HandleRadius, top + HandleRadius * 2),
                              new Point(right - HandleRadius, midY - HandleRadius));
           
            // Right bottom line
            SetLine(lines[3], new Point(right - HandleRadius, midY + HandleRadius),
                              new Point(right - HandleRadius, bottom - HandleRadius * 2));
           
            
            // Bottom right line
            SetLine(lines[4], new Point(right - HandleRadius * 2, bottom - HandleRadius),
                              new Point(midX + HandleRadius, bottom - HandleRadius));
           
           
            // Bottom left line
            SetLine(lines[5], new Point(midX - HandleRadius, bottom - HandleRadius),
                              new Point(left + HandleRadius * 2, bottom - HandleRadius));
           
           
            // Left bottom line
            SetLine(lines[6], new Point(left + HandleRadius, bottom - HandleRadius * 2),
                              new Point(left + HandleRadius, midY + HandleRadius));
           
            // Left top line
            SetLine(lines[7], new Point(left + HandleRadius, midY - HandleRadius),
                              new Point(left + HandleRadius, top + HandleRadius * 2));
        }
        
        private void SetLine(Shape shape, Point start, Point end)
        {
            Line line = (Line)shape;
            line.X1 = start.X;  line.Y1 = start.Y;
            line.X2 = end.X;    line.Y2 = end.Y;
            line.StrokeThickness = 2;
            line.Stroke = new SolidColorBrush(Colors.Black);
        }
        
        private void SetCircle(Shape shape, Point start)
        {
            Ellipse circle = (Ellipse)shape;
            circle.SetValue<double>(TopProperty, start.Y);
            circle.SetValue<double>(LeftProperty, start.X);
            circle.Width = HandleRadius * 2;
            circle.Height = HandleRadius * 2;
            circle.Fill = new SolidColorBrush(Colors.White);
            circle.Stroke = new SolidColorBrush(Colors.Blue);
            circle.Cursor = System.Windows.Input.Cursors.Hand;
        }
    }
}
