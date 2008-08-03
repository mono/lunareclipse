// PathFrame.cs
//
// Author:
//    Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 Manuel Cerón
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace LunarEclipse.Model {	
	
	public class BezierSegmentFrame: AbstractFrame{
		
		public BezierSegmentFrame(UIElement child, PathFigure fig, BezierSegment bez):
			base(child)
		{
			bezier = bez;
			figure = fig;
			
			previous = null;
			
			foreach (PathSegment segment in figure.Segments) {
				BezierSegment bs = segment as BezierSegment;
				if (segment == bezier)
					break;
				previous = segment;
			}
			
			point1_line = CreateLine();
			point2_line = CreateLine();
		}
		
		public override void Update ()
		{
			double posx = (double) Child.GetValue(Canvas.LeftProperty);
			double posy = (double) Child.GetValue(Canvas.TopProperty);
			point1_line.X1 = bezier.Point3.X + posx;
			point1_line.Y1 = bezier.Point3.Y + posy;
			point1_line.X2 = bezier.Point2.X + posx;
			point1_line.Y2 = bezier.Point2.Y + posy;
			
			Point start = GetPreviousPoint();
				
			point2_line.X1 = start.X + posx;
			point2_line.Y1 = start.Y + posy;
			point2_line.X2 = bezier.Point1.X + posx;
			point2_line.Y2 = bezier.Point1.Y + posy;
		}
		
		public override void AddToCanvas ()
		{
			Children.Add(point1_line);
			Children.Add(point2_line);
		}

		private Line CreateLine()
		{
			Line line = new Line();
			line.Stroke = new SolidColorBrush(Colors.LightGray);
			line.StrokeDashArray = new DoubleCollection();
			line.StrokeDashArray.Add(5.0);
			line.StrokeDashArray.Add(5.0);
            line.StrokeThickness = 1.0;
			
			return line;
		}
		
		private Point GetPreviousPoint()
		{
			if (previous == null)
				return figure.StartPoint;
			
			if (previous is BezierSegment) {
				BezierSegment pbezier = previous as BezierSegment;
				return pbezier.Point3;
			}
			
			if (previous is LineSegment) {
				LineSegment pline = previous as LineSegment;
				return pline.Point;
			}
			
			return bezier.Point1;
		}
		
		private Line point1_line;
		private Line point2_line;
		private BezierSegment bezier;
		private PathSegment previous;
		private PathFigure figure;
	}
}
