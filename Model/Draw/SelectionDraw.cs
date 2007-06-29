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
        private List<Shape> selectedObjects;
        private bool shapeMoved;
        private bool shapeAdded;
        
        public List<Shape> SelectedObjects
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
            selectedObjects = new List<Shape>();
        }
        
        internal override void Cleanup ()
        {
            foreach(Shape s in selectedObjects)
            {
                s.Fill = new SolidColorBrush(Colors.White);
                s.Stroke = new SolidColorBrush(Colors.Red);
            }
            selectedObjects.Clear();
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
            List<Shape> shapes = new List<Shape>();
            
            foreach(Shape v in Panel.Children)
            {
                if(v == Element)
                    continue;
                
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
                   // Console.WriteLine("Hit");
                    if(!selectedObjects.Contains(v))
                        Select(v);
                }
            }
            
            return shapes;
        }
        
        private bool ClickedOnShape(Point point, out Shape shape)
        {
            foreach(Shape v in Panel.Children)
            {
                if(v == Element)
                    continue;
                
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
                    if(clickedShape != null && !selectedObjects.Contains(clickedShape))
                        DeselectAll();
                    
                    if(selectedObjects.Count == 0)
                        Select(clickedShape);
                }
                foreach(Shape s in this.selectedObjects)
                    MoveShape(s, mousePoint);
                
                Element.Width = 0;
                Element.Height = 0;
            }
            else
            {
                if(!e.Ctrl && !e.Shift)
                    DeselectAll();
                
                List<Shape> shapes = GetSelectedObjects(e);
                foreach(Shape s in shapes)
                    if(!selectedObjects.Contains(s))
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
                
                if(this.selectedObjects.Contains(shape))
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
                foreach(Shape s in selectedObjects)
                    controller.UndoEngine.PushUndo(new UndoMoveShape(s, start));
            }
        }
        
        private void DeselectAll()
        {
            while(this.selectedObjects.Count > 0)
                Deselect(this.selectedObjects[0]);
        }
        
        private void Deselect(Shape s)
        {
            s.Fill = new SolidColorBrush(Colors.White);
            s.Stroke = new SolidColorBrush(Colors.Red);
            this.selectedObjects.Remove(s);
        }
        
        private void Select(Shape s)
        {
            SolidColorBrush b = new SolidColorBrush(Colors.Green);
            s.Stroke = b;
            b = new SolidColorBrush(Colors.Black);
            b.Opacity = 0.3;
            s.Fill = b;
            this.selectedObjects.Add(s);
        }
        
        private void MoveShape(Shape s, Point offset)
        {
            Point oldPoint = new Point((double)s.GetValue(Canvas.LeftProperty), (double)s.GetValue(Canvas.TopProperty));
            s.SetValue<double>(Canvas.LeftProperty, oldPoint.X + offset.X);
            s.SetValue<double>(Canvas.TopProperty, oldPoint.Y + offset.Y);
            shapeMoved = true;
        }
    }
}
