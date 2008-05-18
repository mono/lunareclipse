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
#region Events

		public event EventHandler<EventArgs> MouseDown;
		public event EventHandler<EventArgs> MouseUp;
		
#endregion Events
		
		
#region Fields
        
        private Visual element;
        private Panel panel;
        private Point position;
		
		protected double actualTop;
		protected double actualLeft;
		protected double actualWidth;
		protected double actualHeight;
        
#endregion Fields
        
#region Properties
        
        public virtual bool CanUndo
        {
            get { return true; }
        }
        
        public Visual Element
        {
            get { return element; }
        }
        
        protected Panel Panel
        {
            get { return panel; }
            private set { panel = value; }
        }
        
        protected Point Position
        {
            get { return position; }
            set { position = value; }
        }
        
        protected double Left
        {
            get { return (double)Element.GetValue(Canvas.LeftProperty); }
            set { Element.SetValue(Canvas.LeftProperty, value); }
        }
        
        protected double Top
        {
            get { return (double)Element.GetValue(Canvas.TopProperty); }
            set { Element.SetValue(Canvas.TopProperty, value); }
        }

		public double Width
		{
			get { return (double)Element.GetValue(Shape.WidthProperty); }
			set { Element.SetValue(Shape.WidthProperty, value); }
		}
		
		public double Height
		{
			get { return (double)Element.GetValue(Shape.HeightProperty); }
			set { Element.SetValue(Shape.HeightProperty, value); }
		}
		
#endregion Properties
        
		
#region Methods
		
        internal DrawBase(Visual element)
        {
            this.element = element;
        }
        
		
        internal virtual void Cleanup()
        {
            // By default nothing needs to be cleaned up
        }
        
		        
        internal virtual void DrawEnd(MouseEventArgs point)
        {
            double top, left, width, height;
            GetNormalisedBounds(out top, out left, out width, out height);
            Width = width;
            Height = height;
            Left = left;
            Top = top;
			Toolbox.RaiseEvent<EventArgs>(MouseUp, Panel, EventArgs.Empty);
        }

		
        internal virtual void DrawStart(Panel panel, MouseEventArgs point)
        {
			Toolbox.RaiseEvent<EventArgs>(MouseDown, Panel, EventArgs.Empty);
			this.panel = panel;
			
			element = (Visual)Activator.CreateInstance(Element.GetType());
            element.SetValue(Shape.StrokeProperty, new SolidColorBrush(Colors.Black));
			panel.Children.Add(Element);
            position = point.GetPosition(panel);
            Left = position.X;
            Top = position.Y;
			
			actualTop = Top;
			actualLeft = Left;
			actualHeight = Height;
			actualWidth = Width;
        }
        
		
		protected void GetNormalisedBounds(out double top, out double left, out double width, out double height)
        {
            // We ensure that the width and height are positive quantities and the top
            // left corner is actually in the top left. For example: 
            // top: 10, left: 10, width: -10, height: -10 
            // is the same as top:0, left:0, width: 10, height: 10
            width = Width;
            left = Left;
            if(width < 0)
            {
                left = left + width;
                width = -width;
            }
            
            height = Height;
            top = Top;
            if(height < 0)
            {
                top = top + height;
                height = -height;
            }
        }
		
		
		internal static void GetTransformedBounds(Visual visual, out double top, out double left, out double width, out double height)
        {
            top = (double)visual.GetValue(Canvas.TopProperty);
            left = (double)visual.GetValue(Canvas.LeftProperty);
            width = (double)visual.GetValue(Shape.WidthProperty) + left;
            height = (double)visual.GetValue(Shape.HeightProperty) + top;
            
            if(visual is SelectedBorder)
            {
                left -= SelectedBorder.BorderWidth;
                width += SelectedBorder.BorderWidth * 2;
                top -= SelectedBorder.BorderWidth;
                height += SelectedBorder.BorderWidth * 2;
            }
		}
		
		internal virtual void MouseMove(MouseEventArgs e)
        {
            Point end = e.GetPosition(panel);
            double x = end.X - position.X;
            double y = end.Y - position.Y;
            
            position = end;
            actualWidth += x;
            actualHeight += y;
			
			if (actualWidth < 0)
			{
				Left = actualLeft + actualWidth;
				Width = -actualWidth;
			}
			else
			{
				Left = actualLeft;
				Width = actualWidth;
			}
			
			if (actualHeight < 0)
			{
				Top = actualTop + actualHeight;
				Height = -actualHeight;
			}
			else
			{
				Top = actualTop;
				Height = actualHeight;
			}
        }
		
		
		internal virtual void Prepare()
        {
			// By default, nothing has to be 'prepared'
        }
		
		public override string ToString()
		{
			return string.Format("Top: {0} Left: {1} Height: {2} Width: {3}", Top, Left, Height, Width);
		}
#endregion Methods
    }
}
