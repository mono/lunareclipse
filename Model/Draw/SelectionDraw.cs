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
    public class SelectionDraw : DrawBase
    {
        private MoonlightController controller;
        private Point mouseStart;
        private Visual clickedOnShape;
        private bool prepared;
        private Dictionary<Visual, SelectedBorder> selectedObjects;
        private double initialRotation = 0;
        private bool shapeMoved;
        private bool shapeAdded;
        
        public Dictionary<Visual, SelectedBorder> SelectedObjects
        {
            get { return this.selectedObjects; }
        }
        
        public override bool CanUndo
        {
            get { return false; }
        }

        public SelectionDraw(MoonlightController controller)
            :base(new SelectionRectangle())
        {
            this.controller = controller;
            selectedObjects = new Dictionary<Visual, SelectedBorder>();
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
            shapeMoved = false;
            if(clickedOnShape == null)
                return;
            
            RotateTransform t = this.clickedOnShape.GetValue(Canvas.RenderTransformProperty) as RotateTransform;
            this.initialRotation = (t != null) ? t.Angle : 0;
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
                    Console.WriteLine("oopsie");
                
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
                Console.WriteLine("Preparing");
                foreach(Visual v in this.controller.Canvas.Children)
                {
                    UIElement e = v as UIElement;
                    
                    if(e == null)
                        continue;
                    
                    e.MouseLeftButtonDown += new MouseEventHandler(ClickedOnVisual);
                }
            }
            prepared = true;
        }

       private void ClickedOnVisual(object sender, MouseEventArgs e)
       {
            Console.WriteLine("Clicky");
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
                
                Element.Width = 0;
                Element.Height = 0;
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
            List<Visual> shapes = GetSelectedObjects(e);
            Point mouseLocation = e.GetPosition(Panel);
            bool clickedOnShape = shapes.Count != 0 || this.clickedOnShape != null;
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
            
            // If we did move some shapes using a click+drag, then we need to push
            // an undo action onto the stack for the group of items which were moved
            if(shapeMoved)
            {
                Point start = mouseStart;
                start.Offset(-mouseLocation.X, -mouseLocation.Y);

                Visual[] movedShapes = new Visual[selectedObjects.Keys.Count];
                selectedObjects.Keys.CopyTo(movedShapes, 0);
                controller.UndoEngine.PushUndo(new UndoMoveShape(movedShapes, start));
            }
            
            // If we have clicked on a shape, check to see if it was rotated
            // if it was push an undo action for that rotation
            if(this.clickedOnShape != null)
            {
                RotateTransform t = this.clickedOnShape.GetValue(Canvas.RenderTransformProperty) as RotateTransform;
                if(t != null && t.Angle != this.initialRotation)
                    this.controller.UndoEngine.PushUndo(new UndoRotation(this.clickedOnShape, initialRotation, t.Angle));
            }
            
            // Reset all the 'move types' to be a standard move. This is needed so
            // that on the nextMouseDown, a standard move will be performed unless
            // the user clicks on one of the handles for stretching/skewing/rotating
            foreach(KeyValuePair<Visual, SelectedBorder> keypair in selectedObjects)
            {
                Console.WriteLine("Setting null");
                keypair.Value.Handle = null;
            }
            
            // Reset the clicked on shape = null
            this.clickedOnShape = null;
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
            Console.WriteLine("Selecting: {0}", s.ToString());
            SelectedBorder border = new SelectedBorder();
            border.Child = s;
            controller.Canvas.Children.Add(border);
            selectedObjects.Add(s, border);
            border.MouseLeftButtonDown += new MouseEventHandler(ClickedOnVisual);
        }
        
        private void MoveShape(Visual s, Point offset, MouseEventArgs e)
        {
            SelectedBorder b = selectedObjects[s];
            double oldLeft = (double)b.Child.GetValue(Canvas.LeftProperty);
            double oldTop = (double)b.Child.GetValue(Canvas.TopProperty);
            double oldHeight = (double)b.Child.GetValue(Canvas.HeightProperty);
            double oldWidth = (double)b.Child.GetValue(Canvas.WidthProperty);
            
            // Do a standard move of the selected shapes
            if(b.Handle == null)
            {
                b.Child.SetValue<double>(Canvas.LeftProperty, oldLeft + offset.X);
                b.Child.SetValue<double>(Canvas.TopProperty, oldTop + offset.Y);
            }
            
            // Do a rotation of the selected shape
            if(b.Handle == b.RotateHandle1 || b.Handle == b.RotateHandle2 ||
               b.Handle == b.RotateHandle3 || b.Handle == b.RotateHandle4)
            {
                Point center;
                center = new Point((double)b.Child.GetValue(Canvas.WidthProperty) * 0.5 + (double)b.Child.GetValue(Canvas.LeftProperty),
                                   (double)b.Child.GetValue(Canvas.HeightProperty) * 0.5 + (double)b.Child.GetValue(Canvas.TopProperty));
               
                Point mouseStart = new Point(Position.X - offset.X, Position.Y - offset.Y);
                
                // Formula: angle = atan( (Slope2 - Slope1 ) / (1 + slope2 * slope1) )
                double slope1 = (mouseStart.Y - center.Y) / (mouseStart.X - center.X);
                double slope2 = (Position.Y - center.Y) / (Position.X - center.X);
                double difference = Math.Atan((slope2 - slope1) / ( 1 + slope1 * slope2));
                
                // Sometimes the angle will hit infinity, so in those cases
                // don't apply the change and wait for the next mouse move which
                // will give a proper angle
                if(!double.IsNaN(difference))
                    b.RotateTransform.Angle += difference * 360 / (2 * Math.PI);
            }
            
            // Stretch the height/top
            if(b.Handle == b.HeightHandle1 || b.Handle == b.HeightHandle2)
                ResizeHeight(b, offset, e);
            
            if(b.Handle == b.WidthHandle1 || b.Handle == b.WidthHandle2)
                ResizeWidth(b, offset, e);

            b.ResizeBorder();
            shapeMoved = true;
        }
        
        private void ResizeWidth(SelectedBorder b, Point offset, MouseEventArgs e)
        {
            double angle = 0;
            double oldLeft    = (double)b.Child.GetValue(Canvas.LeftProperty);
            double oldWidth = (double)b.Child.GetValue(Canvas.WidthProperty);
            double oldTop = (double)b.Child.GetValue(Canvas.TopProperty);
            
            RotateTransform rt = b.Child.GetValue(Canvas.RenderTransformProperty) as RotateTransform;
            if(rt != null)
                angle = Math.Abs(rt.Angle % 360);
            
            Point rotatedOffset = new Point(offset.Y * Math.Sin(angle) + offset.X / Math.Sin(angle),
                                            offset.X * Math.Cos(angle) + offset.Y * Math.Sin(angle));
            rotatedOffset = offset;          
            // Check to see if we should be changing the 'Top' property
            if(b.Handle == b.WidthHandle2 && (angle < 90 || angle >= 270))
            {
                Console.WriteLine("A");
                Console.WriteLine("Delta: {0:0.00}", offset);
                if((oldWidth - rotatedOffset.X) >= 0)
                {
                    b.Child.SetValue<double>(Canvas.LeftProperty, oldLeft + rotatedOffset.X);
                    b.Child.SetValue<double>(Canvas.WidthProperty, oldWidth - rotatedOffset.X);
                    b.Child.SetValue<double>(Canvas.TopProperty, oldTop + rotatedOffset.X * Math.Sin(angle /360 * 2 * Math.PI) / 2 );
                    
                }
            }
            else if(b.Handle == b.WidthHandle2 && (angle >= 90 || angle < 270))
            {
                Console.WriteLine("C");
                 if((oldWidth + rotatedOffset.X) >= 0)
                    b.Child.SetValue<double>(Canvas.WidthProperty, oldWidth + rotatedOffset.X);
            }
            else if(b.Handle == b.WidthHandle1 && (angle < 90 || angle >= 270))
            {
                Console.WriteLine("B");
                if((oldWidth + rotatedOffset.X) >= 0)
                {
                    b.Child.SetValue<double>(Canvas.WidthProperty, oldWidth + rotatedOffset.X);
                    b.Child.SetValue<double>(Canvas.TopProperty, oldTop - rotatedOffset.X * Math.Sin(angle /360 * 2 * Math.PI) / 2 );
                }
            }
            else if(b.Handle == b.WidthHandle1 && (angle >= 90 || angle < 270))
            {
                Console.WriteLine("D");
                if((oldWidth - rotatedOffset.X) >= 0)
                {
                   b.Child.SetValue<double>(Canvas.LeftProperty, oldLeft + rotatedOffset.X);
                   b.Child.SetValue<double>(Canvas.WidthProperty, oldWidth -  rotatedOffset.X);
                }
            }
        }
        
        private void ResizeHeight(SelectedBorder b, Point offset, MouseEventArgs e)
        {
            double oldTop    = (double)b.Child.GetValue(Canvas.TopProperty);
            double oldHeight = (double)b.Child.GetValue(Canvas.HeightProperty);
            
            double angle = 0;
            RotateTransform rt = b.Child.GetValue(Canvas.RenderTransformProperty) as RotateTransform;
            if(rt != null)
                angle = Math.Abs(rt.Angle % 360);
            
            // Check to see if we should be changing the 'Top' property
            if(b.Handle == b.HeightHandle1 && (angle < 90 || angle >= 270))
            {
                Console.WriteLine("A");
                Console.WriteLine("Delta: {0:0.00}", offset);
                if((oldHeight - offset.Y) >= 0)
                {
                    b.Child.SetValue<double>(Canvas.TopProperty, oldTop + offset.Y);
                    b.Child.SetValue<double>(Canvas.HeightProperty, oldHeight - offset.Y);
                }
            }
            else if(b.Handle == b.HeightHandle1 && (angle >= 90 || angle < 270))
            {
                 Console.WriteLine("C");
                if((oldHeight + offset.Y) >= 0)
                    b.Child.SetValue<double>(Canvas.HeightProperty, oldHeight + offset.Y);
            }
            else if(b.Handle == b.HeightHandle2 && (angle < 90 || angle >= 270))
            {
                Console.WriteLine("B");
                if((oldHeight + offset.Y) >= 0)
                    b.Child.SetValue<double>(Canvas.HeightProperty, oldHeight + offset.Y);
            }
            else if(b.Handle == b.HeightHandle2 && (angle >= 90 || angle < 270))
            {
                Console.WriteLine("D");
                Console.WriteLine("Delta: {0:0.00}", offset);
                if((oldHeight - offset.Y) >= 0)
                {
                   b.Child.SetValue<double>(Canvas.TopProperty, oldTop + offset.Y);
                   b.Child.SetValue<double>(Canvas.HeightProperty, oldHeight -  offset.Y);
                }
            }
        }
    }
}
