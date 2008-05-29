//
// PenDraw.cs
//
// Authors:
//   Alan McGovern alan.mcgovern@gmail.com
//
// Copyright (C) 2007 Alan McGovern
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


using System;
using System.Windows;
using System.Windows.Shapes;
using LunarEclipse.Model;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;



namespace LunarEclipse
{
    public class PenDraw : DrawBase
    {
        PathFigure figure;
        
        public PenDraw()
            :base(new System.Windows.Shapes.Path())
        {
			PathSegmentCollection beziers = new PathSegmentCollection();
			beziers.Add(new BezierSegment());
        }
        
        internal override void DrawStart (Panel panel, MouseEventArgs point)
        {
            base.DrawStart(panel, point);
           
            Path path = (Path)Element;
            
            PathGeometry geometry = new PathGeometry();
            figure = new PathFigure();
            
            path.Data = geometry;
            geometry.Figures = new PathFigureCollection();
            geometry.Figures.Add(figure);
            
            figure.Segments = new PathSegmentCollection();
        }


        internal override void MouseMove (MouseEventArgs e)
        {
            Point end = e.GetPosition(Panel);
            double top = (double)Element.GetValue(Canvas.TopProperty);
            double left = (double)Element.GetValue(Canvas.LeftProperty);
			end.X -= left;
			end.Y -= top;
            LineSegment seg = new LineSegment();
            seg.Point = end;
            figure.Segments.Add(seg);
        }
        
        internal override void DrawEnd (MouseEventArgs point)
        {
            BezierSegment bezier;
            PathSegmentCollection beziers = new PathSegmentCollection();
            
            List<LineSegment> current = new List<LineSegment>();
            foreach(LineSegment line in figure.Segments)
            {
                if(current.Count == 3)
                {
                    bezier = new BezierSegment();
                    bezier.Point1 = current[0].Point;
                    bezier.Point2 = current[1].Point;
                    bezier.Point3 = current[2].Point;
                    beziers.Add(bezier);
                    current.Clear();
                }
                
                current.Add(line);
            }
                    
            this.figure.Segments = beziers;
            if(current.Count == 0)
                return;
            
            bezier = new BezierSegment();
            bezier.Point1 = current[0].Point;
            current.RemoveAt(0);

            if(current.Count > 0)
            {
                bezier.Point2 = current[0].Point;
                current.RemoveAt(0);
            }
            else
            {
                bezier.Point2 = bezier.Point1;
            }
            if(current.Count > 0)
            {
                bezier.Point3 = current[0].Point;
                current.RemoveAt(0);
            }
            else
            {
                bezier.Point3 = bezier.Point2;
            }
            
            beziers.Add(bezier);
        }

    }
}
