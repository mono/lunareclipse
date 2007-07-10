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
            Console.WriteLine("Mouse down, Clicked?: " + (clickedOnShape != null).ToString());
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
                
                if(((rectLeft < (right)) && (rectLeft + rectWidth) > left)
                   && (rectTop < (bottom)) && ((rectTop + rectHeight) > top))
                {
                    Console.WriteLine("Selected: " + visual.ToString());
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
            if(prepared)
                return;
            
            prepared = true;
            base.Prepare();
            
            Console.WriteLine("Preparing");
            foreach(Visual v in this.controller.Canvas.Children)
            {
                UIElement e = v as UIElement;
                
                if(e == null)
                    continue;
                
                e.MouseLeftButtonDown += new MouseEventHandler(ClickedOnVisual);
            }
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
                    MoveShape(keypair.Key, mousePoint);
                
                Element.Width = 0;
                Element.Height = 0;
            }
            else
            {
                if(!e.Ctrl && !e.Shift)
                    DeselectAll();
                
                foreach(Visual s in clickedShapes)
                    if(!selectedObjects.ContainsKey(s))
                        Select(s);
            }
        }

        internal override void DrawEnd (MouseEventArgs e)
        {
            Panel.Children.Remove(Element);
            List<Visual> shapes = GetSelectedObjects(e);
            Point mouseLocation = e.GetPosition(Panel);
            bool clickedOnShape = shapes.Count != 0;
            bool mouseMoved = !mouseStart.Equals(Position);
            shapeAdded = false;

            if(clickedOnShape && !mouseMoved)
            {
                if(!e.Ctrl && !e.Shift)
                    this.DeselectAll();
                
                foreach(Visual s in shapes)
                    if(this.selectedObjects.ContainsKey(s))
                        Deselect(s);
                    else
                        Select(s);
            }
                    
            if(!clickedOnShape && !mouseMoved)
                DeselectAll();
            
            if(shapeMoved)
            {
                Point start = mouseStart;
                start.Offset(-mouseLocation.X, -mouseLocation.Y);

                Visual[] movedShapes = new Visual[selectedObjects.Keys.Count];
                selectedObjects.Keys.CopyTo(movedShapes, 0);
                controller.UndoEngine.PushUndo(new UndoMoveShape(movedShapes, start));
            }
            
            foreach(KeyValuePair<Visual, SelectedBorder> keypair in selectedObjects)
                keypair.Value.MoveType = MoveType.Standard;
            
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
            SelectedBorder b = this.selectedObjects[s];
            b.MouseLeftButtonDown -= new MouseEventHandler(ClickedOnVisual);   
            
            // Remove the border from the canvas
            controller.Canvas.Children.Remove(b);
            // Remove the selection from the list of currently selected items
            this.selectedObjects.Remove(s);
        }
        
        private void Select(Visual s)
        {
            SelectedBorder border = new SelectedBorder();
            border.Child = s;
            controller.Canvas.Children.Add(border);
            selectedObjects.Add(s, border);
            border.MouseLeftButtonDown += new MouseEventHandler(ClickedOnVisual);
        }
        
        private void MoveShape(Visual s, Point offset)
        {
            SelectedBorder b = selectedObjects[s];
            double oldLeft = (double)b.Child.GetValue(Canvas.LeftProperty);
            double oldTop = (double)b.Child.GetValue(Canvas.TopProperty);
            double oldHeight = (double)b.Child.GetValue(Canvas.HeightProperty);
            double oldWidth = (double)b.Child.GetValue(Canvas.WidthProperty);

            switch(b.MoveType)
            {
                case MoveType.Rotate:
                Point center = new Point((double)b.GetValue(Canvas.WidthProperty) * 0.5 + 
                                         (double)b.GetValue(Canvas.LeftProperty),
                                         (double)b.GetValue(Canvas.HeightProperty) * 0.5 +
                                         (double)b.GetValue(Canvas.TopProperty));

                center = new Point((double)b.Child.GetValue(Canvas.WidthProperty) * 0.5 + (double)b.Child.GetValue(Canvas.LeftProperty),
                                   (double)b.Child.GetValue(Canvas.HeightProperty) * 0.5 + (double)b.Child.GetValue(Canvas.TopProperty));
               
                double beginAngle = Math.Atan2(mouseStart.Y - (center.Y), mouseStart.X - (center.X));
                double currentAngle = Math.Atan2((Position.Y - center.Y) , (Position.X - center.X));
                b.RotateTransform.Angle = (currentAngle - beginAngle) * 360 / (2 * Math.PI);
                break;
                
                case MoveType.Standard:
                    b.Child.SetValue<double>(Canvas.LeftProperty, oldLeft + offset.X);
                    b.Child.SetValue<double>(Canvas.TopProperty, oldTop + offset.Y);
                    break;
                
                case MoveType.StretchHeight:
                    b.Child.SetValue<double>(Canvas.HeightProperty, oldHeight + offset.Y);
                    break;
                
                case MoveType.StretchLeft:
                    b.Child.SetValue<double>(Canvas.LeftProperty, oldLeft + offset.X);
                    b.Child.SetValue<double>(Canvas.WidthProperty, oldWidth - offset.X);    
                    break;
                
                case(MoveType.StretchTop):
                    b.Child.SetValue<double>(Canvas.TopProperty, oldTop + offset.Y);
                    b.Child.SetValue<double>(Canvas.HeightProperty, oldHeight - offset.Y);
                    break;
                
                case MoveType.StretchWidth:
                    b.Child.SetValue<double>(Canvas.WidthProperty, oldWidth + offset.X);
                    break;
            }

            b.ResizeBorder();
            shapeMoved = true;
        }
    }
}
