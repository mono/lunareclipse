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
namespace LunarEclipse.Model
{
    public class SelectionDraw : DrawBase
    {
        private MoonlightController controller;
        private Point mouseStart;
        private bool clickedOnShape;
        private bool prepared;
        private Dictionary<Shape, SelectedBorder> selectedObjects;
        private bool shapeMoved;
        private bool shapeAdded;
        
        public Dictionary<Shape, SelectedBorder> SelectedObjects
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
            selectedObjects = new Dictionary<Shape, SelectedBorder>();
        }
        
        internal override void Cleanup ()
        {
            Shape[] shapes = new Shape[selectedObjects.Count];
            selectedObjects.Keys.CopyTo(shapes, 0);
            foreach(Shape s in shapes)
                Deselect(s);
        }

        internal override void DrawStart (Panel panel, MouseEventArgs e)
        {
            base.DrawStart(panel, e);
            Shape shape;
            this.clickedOnShape = this.ClickedOnShape(e.GetPosition(panel), out shape);
            mouseStart = Position;
            shapeMoved = false;
        }
        
        private List<Shape> GetSelectedObjects(MouseEventArgs e)
        {
            int i=0;
            List<Shape> shapes = new List<Shape>();
            Console.WriteLine("Contains: " + Panel.Children.Count.ToString());
            foreach(Visual visual in Panel.Children)
            {
                Console.WriteLine("Count: " + (++i).ToString());
                if(visual == Element || !(visual is Shape))
                    continue;
                Shape v = (Shape)visual;
                Rectangle rect = (Rectangle)Element;
                
                double top = (double)v.GetValue(Canvas.TopProperty);
                double left = (double)v.GetValue(Canvas.LeftProperty);
                double width = (double)v.GetValue(Shape.WidthProperty);
                double height = (double)v.GetValue(Shape.HeightProperty);
                
                double rectTop, rectLeft, rectWidth, rectHeight;
                GetNormalisedBounds(out rectTop, out rectLeft, out rectWidth, out rectHeight);
                                         
                if(((rectLeft < (left + width)) && (rectLeft + rectWidth) > left)
                   && (rectTop < (top + height)) && ((rectTop + rectHeight) > top))
                {
                    if(!selectedObjects.ContainsKey(v))
                        Select(v);
                }
            }
            
            return shapes;
        }
        
        private bool ClickedOnShape(Point point, out Shape shape)
        {
            foreach(Visual visual in Panel.Children)
            {
                if(visual == Element || !(visual is Shape))
                    continue;
                
                Shape v = (Shape)visual;
                Rectangle rect = (Rectangle)Element;
                
                double top = (double)v.GetValue(Canvas.TopProperty);
                double left = (double)v.GetValue(Canvas.LeftProperty);
                double width = (double)v.GetValue(Shape.WidthProperty);
                double height = (double)v.GetValue(Shape.HeightProperty);
                
                
                double rectTop = point.Y;
                double rectLeft = point.X;
                double rectWidth = 1;
                double rectHeight = 1;
                                         
                if(((rectLeft < (left + width)) && (rectLeft + rectWidth) > left)
                   && (rectTop < (top + height)) && ((rectTop + rectHeight) > top))
                {
                    shape = v;
                    return true;
                }
            }
            
            shape = null;
            return false;
        }
        
        internal override void Prepare ()
        {
            if(!prepared)
                base.Prepare();
        }


        internal override void Resize (MouseEventArgs e)
        {
            Shape clickedShape;
            Point mousePoint = e.GetPosition(Panel);
            mousePoint.Offset(-Position.X, -Position.Y);
            
            base.Resize(e);

            if(this.clickedOnShape)
            {
                ClickedOnShape(mouseStart, out clickedShape);
                
                if(!shapeAdded)
                {
                    shapeAdded = true;
                    if(clickedShape != null && !selectedObjects.ContainsKey(clickedShape))
                        DeselectAll();
                    
                    if(selectedObjects.Count == 0)
                        Select(clickedShape);
                }
                foreach(KeyValuePair<Shape, SelectedBorder> keypair in selectedObjects)
                    MoveShape(keypair.Key, mousePoint);
                
                Element.Width = 0;
                Element.Height = 0;
            }
            else
            {
                if(!e.Ctrl && !e.Shift)
                    DeselectAll();
                
                List<Shape> shapes = GetSelectedObjects(e);
                foreach(Shape s in shapes)
                    if(!selectedObjects.ContainsKey(s))
                        Select(s);
            }
        }

        internal override void DrawEnd (MouseEventArgs e)
        {
            Panel.Children.Remove(Element);
            Shape shape = null;
            shapeAdded = false;
            Point mouseLocation = e.GetPosition(Panel);
            bool clickedOnShape = ClickedOnShape(mouseLocation, out shape);
            bool mouseMoved = !mouseStart.Equals(Position);
            
            if(clickedOnShape && !mouseMoved)
            {
                if(!e.Ctrl && !e.Shift)
                    this.DeselectAll();
                
                if(this.selectedObjects.ContainsKey(shape))
                    Deselect(shape);
                else
                    Select(shape);
            }
                    
            if(!clickedOnShape && !mouseMoved)
                DeselectAll();
            
            if(shapeMoved)
            {
                Point start = mouseStart;
                start.Offset(-mouseLocation.X, -mouseLocation.Y);
                Console.WriteLine("Offset is: " + start.ToString());
                Shape[] movedShapes = new Shape[selectedObjects.Keys.Count];
                selectedObjects.Keys.CopyTo(movedShapes, 0);
                controller.UndoEngine.PushUndo(new UndoMoveShape(movedShapes, start));
            }
        }
        
        private void DeselectAll()
        {
            Shape[] shapes = new Shape[selectedObjects.Count];
            selectedObjects.Keys.CopyTo(shapes, 0);
            foreach(Shape s in shapes)
                Deselect(s);
        }
        
        private void Deselect(Shape s)
        {
            SelectedBorder border = this.selectedObjects[s];
            Visual v = border.Children[0];
            border.Children.Remove(v);
            controller.Canvas.Children.Add(v);
            controller.Canvas.Children.Remove(border);
            this.selectedObjects.Remove(s);
        }
        
        private void Select(Shape s)
        {
            SelectedBorder border = new SelectedBorder();
            border.Children.Add(s);
            this.controller.Canvas.Children.Remove(s);
            this.controller.Canvas.Children.Add(border);
            this.selectedObjects.Add(s, border);
            
            border.SetValue<double>(Canvas.TopProperty,
                   (double)s.GetValue(Canvas.TopProperty) - SelectedBorder.BorderWidth);
            border.SetValue<double>(Canvas.LeftProperty,
                   (double)s.GetValue(Canvas.LeftProperty) - SelectedBorder.BorderWidth);
        }
        
        private void MoveShape(Shape s, Point offset)
        {
            SelectedBorder b = selectedObjects[s];
            Point oldPoint = new Point((double)s.GetValue(Canvas.LeftProperty), (double)s.GetValue(Canvas.TopProperty));
            b.SetValue<double>(Canvas.LeftProperty, oldPoint.X + offset.X);
            b.SetValue<double>(Canvas.TopProperty, oldPoint.Y + offset.Y);
            shapeMoved = true;
        }
    }
}
