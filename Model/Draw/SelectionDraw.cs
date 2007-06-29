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


namespace LunarEclipse.Model
{
    public class SelectionDraw : DrawBase
    {
        private bool clickedOnShape;
        private bool prepared;
        private List<Shape> selectedObjects;
        
        public override bool CanUndo
        {
            get { return false; }
        }

        public SelectionDraw()
            :base(new SelectionRectangle())
        {
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
            clickedOnShape = GetSelectedObjects(e).Count != 0;
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
        
        
        internal override void Prepare ()
        {
            if(!prepared)
                base.Prepare();
        }


        internal override void Resize (MouseEventArgs e)
        {
            base.Resize(e);
            
            if(!e.Ctrl && !e.Shift)
                DeselectAll();
            
            List<Shape> shapes = GetSelectedObjects(e);
            foreach(Shape s in shapes)
                if(!selectedObjects.Contains(s))
                    Select(s);
        }

        internal override void DrawEnd (MouseEventArgs e)
        {
            Panel.Children.Remove(Element);
            
            if(clickedOnShape && !e.Ctrl && !e.Shift && e.GetPosition(Panel).Equals(Position))
            {
                this.DeselectAll();
                
                List<Shape> shapes = this.GetSelectedObjects(e);
                foreach(Shape s in shapes)
                    if(!selectedObjects.Contains(s))
                        Select(s);
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
              s.Stroke = new SolidColorBrush(Colors.Green);
              s.Fill = new SolidColorBrush(Colors.Black);
              this.selectedObjects.Add(s);
        }
    }
}
