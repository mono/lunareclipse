// /home/alan/Projects/LunarEclipse/Model/Draw/SelectionDraw.cs created with MonoDevelop
// User: alan at 4:45 PM 6/27/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;
using LunarEclipse.Controller;
using LunarEclipse;

namespace LunarEclipse.Model
{
	public class Selector : DrawBase
	{
		
#region Events
		
		public event EventHandler<PropertyChangedEventArgs> ChangedWidth;
		public event EventHandler<PropertyChangedEventArgs> ChangedHeight;
		public event EventHandler<PropertyChangedEventArgs> ChangedTop;
		public event EventHandler<PropertyChangedEventArgs> ChangedLeft;
		public event EventHandler<PropertyChangedEventArgs> ChangedRotation;
		public event EventHandler<SelectionChangedEventArgs> ItemSelected;
		public event EventHandler<SelectionChangedEventArgs> ItemDeselected;
		
#endregion
		
		
        private MoonlightController controller;
        private Point mouseStart;
        private Visual clickedOnShape;
        private bool prepared;
        private Dictionary<Visual, SelectedBorder> selectedObjects;
        private bool shapeAdded;
		
		protected MoonlightController Controller
		{
			get { return controller; }
		}
        
        public Dictionary<Visual, SelectedBorder> SelectedObjects
        {
            get { return this.selectedObjects; }
        }

        public Selector(MoonlightController controller)
            :base(new SelectionRectangle())
        {
            this.controller = controller;
            this.selectedObjects = new Dictionary<Visual, SelectedBorder>();
        }
        
        internal override void Cleanup ()
        {
            Visual[] shapes = new Visual[selectedObjects.Count];
            selectedObjects.Keys.CopyTo(shapes, 0);
            foreach(Visual s in shapes)
                Deselect(s);
            
            foreach(Visual v in this.controller.Canvas.Children)
            {
                UIElement e = v as UIElement;
                if(e != null)
                    e.MouseLeftButtonDown -= new MouseEventHandler(ClickedOnVisual);
            }
        }

        internal override void DrawStart (Panel panel, MouseEventArgs e)
        {
            base.DrawStart(panel, e);
            mouseStart = Position;
            if(clickedOnShape == null)
                return;
        }
        
        private List<Visual> GetSelectedObjects(MouseEventArgs e)
        {
            // This will contain the list of shapes that are selected
            List<Visual> shapes = new List<Visual>();
            
            double rectTop, rectLeft, rectWidth, rectHeight;
            GetNormalisedBounds(out rectTop, out rectLeft, out rectWidth, out rectHeight);
            
            foreach(Visual v in Panel.Children)
            {
                Visual visual = v;
                if(visual == Element)
                    continue;
                
                double top;
                double left;
                double right;
                double bottom;
                
                DrawBase.GetTransformedBounds(visual, out top, out left, out right, out bottom);
                
                if(visual is SelectedBorder)
                    visual = ((SelectedBorder)visual).Child;
                
                if(visual is SelectedBorder)
                    throw new Exception("I've done the impossible");
                
                if(((rectLeft < (right)) && (rectLeft + rectWidth) > left)
                   && (rectTop < (bottom)) && ((rectTop + rectHeight) > top))
                {
                    // Due to the special handling for borders, we need to
                    // make sure we don't add the same shape twice
                    if(!shapes.Contains(visual))
                        shapes.Add(visual);
                }
            }
            
            
            shapes.Sort(new ZIndexComparer());
            return shapes;
        }
		
		internal override void Prepare ()
		{
			base.Prepare();
			
			if(!prepared)
			{
				prepared = true;
				foreach(Visual v in this.controller.Canvas.Children)
				{
					UIElement e = v as UIElement;
					
					if(e == null)
						continue;
					
					e.MouseLeftButtonDown += new MouseEventHandler(ClickedOnVisual);
				}
			}
		}
		
       private void ClickedOnVisual(object sender, MouseEventArgs e)
       {
            if(sender is SelectedBorder)
                this.clickedOnShape = ((SelectedBorder)sender).Child;
            else
                this.clickedOnShape = (Visual)sender;
       }

        internal override void Resize (MouseEventArgs e)
        {
            List<Visual> clickedShapes = GetSelectedObjects(e);
            Point mousePoint = e.GetPosition(Panel);
            mousePoint.Offset(-Position.X, -Position.Y);
            
            base.Resize(e);

            if(clickedOnShape != null)
            {
                if(!shapeAdded)
                {
                    shapeAdded = true;
                    if(!selectedObjects.ContainsKey(clickedOnShape))
                    {
                        DeselectAll();
                        Select(clickedOnShape);
                    }
                }
                
                foreach(KeyValuePair<Visual, SelectedBorder> keypair in selectedObjects)
                    MoveShape(keypair.Key, mousePoint, e);
                
                Width = 0;
                Height = 0;
            }
            else
            {
                // Make sure that everything is selected that is currently
                // highlighted by the selection rectangle
                foreach(Visual s in clickedShapes)
                    if(!selectedObjects.ContainsKey(s))
                        Select(s);
                
                // If the user is not holding ctrl or shift, deselect everything
                // that was previously selected but is *not* currently in the list
                // of selected objects
                List<Visual> deselectList = new List<Visual>();
                foreach(Visual v in this.selectedObjects.Keys)   
                    if(!clickedShapes.Contains(v))
                        deselectList.Add(v);

                foreach(Visual v in deselectList)
                    Deselect(v);
            }
        }

        internal override void DrawEnd (MouseEventArgs e)
        {
            Panel.Children.Remove(Element);
            
            bool mouseMoved = !mouseStart.Equals(Position);
            shapeAdded = false;

            // If we clicked and released on a visual, we select it, unless
            // we are using one of the modifiers (ctrl/shift)
            // If we clicked and released on an empty area of canvas, we deselect
            // everything
            if(!mouseMoved)
            {
                if(this.clickedOnShape != null)
                {
                    if(!e.Ctrl && !e.Shift)
                        this.DeselectAll();
                    
                    if(!this.selectedObjects.ContainsKey(this.clickedOnShape))
                        Select(this.clickedOnShape);
                    else
                        Deselect(this.clickedOnShape);
                }
                else
                {
                    DeselectAll();
                }
            }
            
            // Reset all the 'move types' to be a standard move. This is needed so
            // that on the nextMouseDown, a standard move will be performed unless
            // the user clicks on one of the handles for stretching/skewing/rotating
            foreach(KeyValuePair<Visual, SelectedBorder> keypair in selectedObjects)
                keypair.Value.Handle = null;
            
            // Reset the clicked on shape = null
            clickedOnShape = null;
        }
        
        private void DeselectAll()
        {
            Visual[] shapes = new Visual[selectedObjects.Count];
            selectedObjects.Keys.CopyTo(shapes, 0);
            foreach(Visual s in shapes)
                Deselect(s);
        }
        
        private void Deselect(Visual s)
        {
            Console.WriteLine("Deselect method: {0}", s.ToString());
            SelectedBorder b = this.selectedObjects[s];
            b.MouseLeftButtonDown -= new MouseEventHandler(ClickedOnVisual);   
            
            // Remove the border from the canvas
            controller.Canvas.Children.Remove(b);
            // Remove the selection from the list of currently selected items
            this.selectedObjects.Remove(s);
        }
        
        private void Select(Visual s)
        {
			// When selecting an item, we make sure it has a valid name. This means that
			// when selecting an item for animations or selecting it to change it's properties
			// in the property pane, it always has a valid name
			if(string.IsNullOrEmpty(s.Name))
				s.SetValue<string>(Visual.NameProperty, NameGenerator.GetName(Panel, s));
			
            SelectedBorder border = new SelectedBorder(s);
            controller.Canvas.Children.Add(border);
            selectedObjects.Add(s, border);
            border.MouseLeftButtonDown += new MouseEventHandler(ClickedOnVisual);
			RaiseEvent<SelectionChangedEventArgs>(ItemSelected, new SelectionChangedEventArgs(s, border));
        }
        
        private void MoveShape(Visual s, Point offset, MouseEventArgs e)
        {
            SelectedBorder border = selectedObjects[s];
            double oldLeft = (double)border.Child.GetValue(Canvas.LeftProperty);
            double oldTop = (double)border.Child.GetValue(Canvas.TopProperty);

            // Do a standard move of the selected shapes
            if(border.Handle == null)
			{
				border.Child.SetValue<double>(Canvas.LeftProperty, oldLeft + offset.X);
				RaiseEvent(ChangedLeft, border.Child, Canvas.LeftProperty, oldLeft, oldLeft + offset.X);
				
                border.Child.SetValue<double>(Canvas.TopProperty, oldTop + offset.Y);
				RaiseEvent(ChangedTop, border.Child, Canvas.TopProperty, oldTop, oldTop + offset.Y);
            }
            
            // Do a rotationPropertyChangedEventArgs of the selected shape
            else if(border.Handle == border.RotateHandle1 || 
			        border.Handle == border.RotateHandle2 ||
			        border.Handle == border.RotateHandle3 || 
			        border.Handle == border.RotateHandle4)
            {
                Point center;
                center = new Point((double)border.Child.GetValue(Canvas.WidthProperty) * 0.5 + 
				                   (double)border.Child.GetValue(Canvas.LeftProperty),
                                   (double)border.Child.GetValue(Canvas.HeightProperty) * 0.5 + 
				                   (double)border.Child.GetValue(Canvas.TopProperty));
               
                Point mouseStart = new Point(Position.X - offset.X, Position.Y - offset.Y);
                
                // Formula: angle = atan( (Slope2 - Slope1 ) / (1 + slope2 * slope1) )
                double slope1 = (mouseStart.Y - center.Y) / (mouseStart.X - center.X);
                double slope2 = (Position.Y - center.Y) / (Position.X - center.X);
                double difference = Math.Atan((slope2 - slope1) / ( 1 + slope1 * slope2));
                
                // Sometimes the angle will hit infinity, so in those cases
                // don't apply the change and wait for the next mouse move which
                // will give a proper angle
                if(!double.IsNaN(difference))
				{
					border.Rotate.Angle += Toolbox.RadiansToDegrees(difference);
					RaiseEvent(ChangedRotation, border.Rotate,
					           RotateTransform.AngleProperty, 
					           border.Rotate.Angle, 
					           border.Rotate.Angle + Toolbox.RadiansToDegrees(difference));
				}
			}
            
			else
			{
				ResizeHeightOrWidth(border, offset, e);
			}
        }
        
        private void ResizeHeightOrWidth(SelectedBorder b, Point offset, MouseEventArgs e)
        {
			// Get the absolute value of the angle the shape is rotated by
			double angle = (b.Rotate != null) ? b.Rotate.Angle % 360 : 0;
			if(angle < 0)
				angle += 360;
			
			// Get the initial values for it's dimensions
			double oldLeft = (double)b.Child.GetValue(Canvas.LeftProperty);
            double oldWidth = (double)b.Child.GetValue(Canvas.WidthProperty);
            double oldTop = (double)b.Child.GetValue(Canvas.TopProperty);
			double oldHeight = (double)b.Child.GetValue(Canvas.HeightProperty);
			double newLeft = oldLeft;
			double newWidth = oldWidth;
			double newTop = oldTop;
			double newHeight = oldHeight;

			// Calculate the cos and sin of the angle of the rotated shape
			double cosAngle = Math.Cos(Toolbox.DegreesToRadians(angle));
			double sinAngle = Math.Sin(Toolbox.DegreesToRadians(angle));
			
			// When a shape is rotated, we need to convert the mouse X, Y coordinates from
			// canvas coordinates into 'shape' coordinates. In 'shape' coordinates, the
			// X coord corresponds to moving perpendicularly to the width of the rotated
			// shape and the Y coordinate corresponds to moving perpendicularly to the Height
			// of the rotated shape.
			double mouseDistanceTravelled = Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);
			double mouseAngle = Math.Atan2(offset.Y, offset.X);
			offset = new Point(mouseDistanceTravelled * Math.Cos(mouseAngle - Toolbox.DegreesToRadians(angle)),
			                   mouseDistanceTravelled * Math.Sin(mouseAngle - Toolbox.DegreesToRadians(angle)));

			// When resizing shapes, we need to do some crazy maths to make sure
			// that when we resize, the shape doesn't 'float' around the canvas.
			if(b.Handle == b.WidthHandle1 && (oldWidth + offset.X) >= 0)
            {
				newWidth = oldWidth + offset.X;
				newLeft = oldLeft  - offset.X * (1 - cosAngle) / 2;
				newTop =  oldTop + offset.X * sinAngle / 2.0;
            }
			else if(b.Handle == b.WidthHandle2 && (oldWidth - offset.X) >= 0)
			{
				newWidth =  oldWidth - offset.X;
				newLeft = oldLeft + offset.X * cosAngle + offset.X * (1 - cosAngle) / 2;
				newTop =  oldTop + offset.X / 2.0 * sinAngle;
            }

			// Change the height
			else if(b.Handle == b.HeightHandle1 && (oldHeight - offset.Y) >= 0)
            {
				newHeight =  oldHeight - offset.Y;
				newLeft = oldLeft - offset.Y * sinAngle / 2.0;
				newTop =  oldTop + offset.Y * cosAngle + offset.Y * (1 - cosAngle) / 2.0;
            }
            else if(b.Handle == b.HeightHandle2 && (oldHeight + offset.Y) >= 0)
            {
				newHeight =  oldHeight + offset.Y;
				newLeft = oldLeft - offset.Y * sinAngle / 2.0;
				newTop = oldTop -  offset.Y * (1 - cosAngle) / 2.0;
			}
			
			if(oldLeft != newLeft)
			{
				b.Child.SetValue<object>(Canvas.LeftProperty, newLeft);
				RaiseEvent(ChangedLeft, b.Child, Canvas.LeftProperty, oldLeft, newLeft);
			}
			if(oldTop != newTop)
			{
				b.Child.SetValue<object>(Canvas.TopProperty, newTop);
				RaiseEvent(ChangedTop, b.Child, Canvas.TopProperty, oldTop, newTop);
			}
			if(oldWidth != newWidth)
			{
				b.Child.SetValue<object>(Canvas.WidthProperty, newWidth);
				RaiseEvent(ChangedWidth, b.Child, Canvas.WidthProperty, oldWidth, newWidth);
			}
			if(oldHeight != newHeight)
			{
				b.Child.SetValue<object>(Canvas.HeightProperty, newHeight);
				RaiseEvent(ChangedHeight, b.Child, Canvas.HeightProperty, oldHeight, newHeight);
			}
		}
		
		private void RaiseEvent(EventHandler<PropertyChangedEventArgs> e, DependencyObject target, 
		                        DependencyProperty property, object oldValue, object newValue)
		{
			RaiseEvent<PropertyChangedEventArgs>(e, new PropertyChangedEventArgs(target, property, oldValue, newValue));
			
			SelectedBorder border;
			Visual visual = target as Visual;
			if(visual == null || !selectedObjects.TryGetValue(visual, out border))
				return;
			
			border.ResizeBorder();
		}
		
		
		private void RaiseEvent<T>(EventHandler<T> e, T args) where T : EventArgs
		{
			if(e != null)
				e(controller, args);
		}
	}
}
