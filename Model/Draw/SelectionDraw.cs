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
        private bool clickedOnShape;
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
        }

        internal override void DrawStart (Panel panel, MouseEventArgs e)
        {
            base.DrawStart(panel, e);
            List<Visual> selectedShapes = GetSelectedObjects(e);
            clickedOnShape = selectedShapes.Count != 0;
            mouseStart = Position;
            shapeMoved = false;
            Console.WriteLine("Mouse down, Clicked?: " + (clickedOnShape).ToString());
        }
        
        private List<Visual> GetSelectedObjects(MouseEventArgs e)
        {
            // This will contain the list of shapes that are selected
            List<Visual> shapes = new List<Visual>();
            
            double rectTop, rectLeft, rectWidth, rectHeight;
            GetNormalisedBounds(out rectTop, out rectLeft, out rectWidth, out rectHeight);
            
             //Console.WriteLine(string.Format("RectTop: {0}, Left: {1}, Width: {2}, Height: {3}",
             //                     rectTop, rectLeft, rectWidth, rectHeight));
            int count=0;
            foreach(Visual v in Panel.Children)
            {
                Visual visual = v;
                //Console.WriteLine("Iteration: " + (++count).ToString());
                if(visual == Element)
                    continue;
                
                //Console.WriteLine("Inside with: " + visual.ToString());
                double top = (double)visual.GetValue(Canvas.TopProperty);
                double left = (double)visual.GetValue(Canvas.LeftProperty);
                double width = (double)visual.GetValue(Shape.WidthProperty);
                double height = (double)visual.GetValue(Shape.HeightProperty);
                
                if(visual is SelectedBorder)
                {
                    SelectedBorder border = (SelectedBorder)visual;
                    visual = border.Child;
                    top -= (Double)((Canvas)border.Parent).GetValue(Canvas.TopProperty);
                    left -= (Double)((Canvas)border.Parent).GetValue(Canvas.LeftProperty);
                }
                
               //Console.WriteLine(string.Format("Top: {0}, Left: {1}, Width: {2}, Height: {3}",
               //                   top, left, width, height));
                if(((rectLeft < (left + width)) && (rectLeft + rectWidth) > left)
                   && (rectTop < (top + height)) && ((rectTop + rectHeight) > top))
                {
                    //Console.WriteLine("Found: " + visual.ToString());
                    shapes.Add(visual);
                }
            }
            
            
            shapes.Sort(new ZIndexComparer());
            Console.WriteLine("Found: ");
            foreach(Visual va in shapes)
            {
                Console.Write('\t');
                Console.WriteLine(va.ToString());
            }
            return shapes;
        }
        
        internal override void Prepare ()
        {
            if(!prepared)
                base.Prepare();
        }


        internal override void Resize (MouseEventArgs e)
        {
            List<Visual> clickedShapes = GetSelectedObjects(e);
            bool clickedShape = clickedShapes.Count != 0;
            Point mousePoint = e.GetPosition(Panel);
            mousePoint.Offset(-Position.X, -Position.Y);
            
            base.Resize(e);

            if(this.clickedOnShape)
            {
                if(!shapeAdded)
                {
                    shapeAdded = true;
                    if(!selectedObjects.ContainsKey(clickedShapes[0]))
                    {
                        DeselectAll();
                        Select(clickedShapes[0]);
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
                //Console.WriteLine("Offset is: " + start.ToString());
                int i=0;
                Visual[] movedShapes = new Visual[selectedObjects.Keys.Count];
                selectedObjects.Keys.CopyTo(movedShapes, 0);
                controller.UndoEngine.PushUndo(new UndoMoveShape(movedShapes, start));
            }
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
            SelectedBorder border = this.selectedObjects[s];
            Visual v = border.Children[0];
            border.Children.Remove(v);
            controller.Canvas.Children.Add(v);
            controller.Canvas.Children.Remove(border);
            this.selectedObjects.Remove(s);
            
            s.SetValue<double>(Canvas.TopProperty, 
                   (double)border.GetValue(Canvas.TopProperty) + SelectedBorder.BorderWidth);
            s.SetValue<double>(Canvas.LeftProperty,
                   (double)border.GetValue(Canvas.LeftProperty) + SelectedBorder.BorderWidth);
        }
        
        private void Select(Visual s)
        {
            SelectedBorder border = new SelectedBorder();
            controller.Canvas.Children.Remove(s);
            border.Child = s;
            controller.Canvas.Children.Add(border);
            selectedObjects.Add(s, border);
        }
        
        private void MoveShape(Visual s, Point offset)
        {
            SelectedBorder b = selectedObjects[s];
            Point oldPoint = new Point((double)b.GetValue(Canvas.LeftProperty), (double)b.GetValue(Canvas.TopProperty));
            b.SetValue<double>(Canvas.LeftProperty, oldPoint.X + offset.X);
            b.SetValue<double>(Canvas.TopProperty, oldPoint.Y + offset.Y);
            shapeMoved = true;
        }
    }
}
