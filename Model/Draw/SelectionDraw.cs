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
                DrawBase.GetNormalisedBounds(Element, out rectTop, out rectLeft, out rectWidth, out rectHeight);
                                         
                if(((rectLeft < (left + width)) && (rectLeft + rectWidth) > left)
                   && (rectTop < (top + height)) && ((rectTop + rectHeight) > top))
                {
                   // Console.WriteLine("Hit");
                    if(!selectedObjects.Contains(v))
                    {
                        v.Stroke = new SolidColorBrush(Colors.Green);
                        v.Fill = new SolidColorBrush(Colors.Black);
                        shapes.Add(v);
                    }
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
            foreach(Shape s in this.selectedObjects)
            {
                s.Fill = new SolidColorBrush(Colors.White);
                s.Stroke = new SolidColorBrush(Colors.Red);
            }
            this.selectedObjects.Clear();
            List<Shape> shapes = GetSelectedObjects(e);
            

                base.Resize(e);
                
                foreach(Shape s in shapes)
                    if(!selectedObjects.Contains(s))
                        selectedObjects.Add(s);
        }

        internal override void DrawEnd (MouseEventArgs e)
        {
            Panel.Children.Remove(Element);
        }
    }
}
