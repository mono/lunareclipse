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
    public enum MoveType
    {
        Standard,
        StretchWidth,
        StretchHeight,
        StretchTop,
        StretchLeft,
        Rotate
    }
    
    public class SelectedBorder : System.Windows.Controls.Canvas
    {
        private MoveType movetype;
        private bool updating;
              
        public const double BorderWidth = 13;
        public const double HandleRadius = 6.5;
        
        private Visual child;

        public SelectedBorder()
        {
            this.movetype = MoveType.Standard;
            base.SetValue<bool>(IsHitTestVisibleProperty, false);
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
                base.Children.Add(value);
                
                if(!updating)
                {
                    updating = true;
                    ResizeBorder();
                    DrawHandles();
                    updating = false;
                }
            }
        }
            
        
        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue<double>(LeftProperty, value); }
        }
        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue<double>(TopProperty, value); }
        }
        
        
        public MoveType MoveType
        {
            get { return this.movetype; }
            internal set { this.movetype = value; }
        }
        
        internal void ResizeBorder()
        {         
            Children.Clear();
            Children.Add(child);
            
            double childTop = (double)child.GetValue(TopProperty);
            double childLeft = (double)child.GetValue(LeftProperty);
            double childWidth = (double)child.GetValue(WidthProperty);
            double childHeight = (double)child.GetValue(HeightProperty);
            
            // First set the position of the selection canvas
            Top = Top + (childTop - BorderWidth);
            Left = Left + (childLeft - BorderWidth);
            Width = childWidth + BorderWidth * 2;
            Height = childHeight + BorderWidth * 2;
            
            child.SetValue<double>(TopProperty, BorderWidth);
            child.SetValue<double>(LeftProperty, BorderWidth);

            DrawHandles();
        }
        
        private void DrawHandles()
        {
            Ellipse circle;
            Line line;
            
            double midX = Width / 2.0;
            double midY = Height / 2.0;
            
            // Top side
            circle = CreateCircle(new Point(0, 0));
            Children.Add(circle);
            circle.MouseLeftButtonDown+= delegate {
                this.movetype = MoveType.Rotate;
                Console.WriteLine(movetype.ToString());
            };
            
            Children.Add(CreateLine(new Point(HandleRadius * 2, HandleRadius),
                                    new Point(midX - HandleRadius, HandleRadius)));
            
            circle = CreateCircle(new Point(midX - HandleRadius, 0));
            Children.Add(circle);
            circle.MouseLeftButtonDown+= delegate {
                movetype = MoveType.StretchTop;
                Console.WriteLine(movetype.ToString());
            };

            Children.Add(CreateLine(new Point(midX + HandleRadius, HandleRadius),
                                    new Point(Width - HandleRadius * 2, HandleRadius)));
            
            // Right side            
            circle = CreateCircle(new Point(Width - HandleRadius * 2, 0));
            Children.Add(circle);
            circle.MouseLeftButtonDown+= delegate {
                movetype = MoveType.Rotate;
                Console.WriteLine(movetype.ToString());
            };
            
            Children.Add(CreateLine(new Point(Width - HandleRadius, HandleRadius * 2),
                                    new Point( Width - HandleRadius, midY - HandleRadius)));
            
            circle = CreateCircle(new Point(Width - HandleRadius * 2, midY - HandleRadius));
            Children.Add(circle);
            circle.MouseLeftButtonDown+= delegate {
                movetype = MoveType.StretchWidth;
                Console.WriteLine(movetype.ToString());
            };
            
            Children.Add(CreateLine(new Point(Width - HandleRadius, midY + HandleRadius),
                                    new Point(Width - HandleRadius, Height - HandleRadius * 2)));
            
            // Bottom side
            circle = CreateCircle(new Point(Width - HandleRadius * 2, Height - HandleRadius * 2));
            Children.Add(circle);
            circle.MouseLeftButtonDown+= delegate {
                movetype = MoveType.Rotate;
                Console.WriteLine(movetype.ToString());
            };
            
            Children.Add(CreateLine(new Point(Width - HandleRadius * 2, Height - HandleRadius),
                                    new Point(midX + HandleRadius, Height - HandleRadius)));
            
            circle = CreateCircle(new Point(midX - HandleRadius, Height - HandleRadius * 2));
            Children.Add(circle);
            circle.MouseLeftButtonDown+= delegate {
                movetype = MoveType.StretchHeight;
                Console.WriteLine(movetype.ToString());
            };
            Children.Add(CreateLine(new Point(midX - HandleRadius, Height - HandleRadius),
                                    new Point(HandleRadius * 2, Height - HandleRadius)));
            
            // Left side
            circle = CreateCircle(new Point(0, Height - HandleRadius * 2));
            Children.Add(circle);
            circle.MouseLeftButtonDown+= delegate {
                movetype = MoveType.Rotate;
                Console.WriteLine(movetype.ToString());
            };
            
            Children.Add(CreateLine(new Point(HandleRadius, Height - HandleRadius * 2),
                                    new Point(HandleRadius, midY + HandleRadius)));
            
            circle = CreateCircle(new Point(0, midY - HandleRadius));
            Children.Add(circle);
            circle.MouseLeftButtonDown+= delegate {
                movetype = MoveType.StretchLeft;
                Console.WriteLine(movetype.ToString());
            };
            
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
            circle.Stroke = new SolidColorBrush(Colors.Blue);
            circle.Cursor = System.Windows.Input.Cursors.Hand;
            return circle;
        }
    }
}
