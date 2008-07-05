// PolyLineDescriptor.cs
//
// Author:
//    Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 Manuel Cerón.
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

using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class PathDescriptor: StandardDescriptor {
		
		public PathDescriptor(Path element):
			this(element, null)
		{
		}
		
		public PathDescriptor(Path element, UndoGroup group):
			base(element, group)
		{
			path = element;
		}
		
		public override Rect GetBounds ()
		{
			double minx = Double.MaxValue;
			double miny = Double.MaxValue;
			double maxx = Double.MinValue;
			double maxy = Double.MinValue;
			
			foreach (DependencyObject obj in PointObjects) {
				Point p = new Point(minx, miny);
				if (obj is PathFigure)
					p = (Point) obj.GetValue(PathFigure.StartPointProperty);
				if (obj is LineSegment)
					p = (Point) obj.GetValue(LineSegment.PointProperty);
				if (obj is BezierSegment)
					p = (Point) obj.GetValue(BezierSegment.Point3Property);
				
				if (p.X < minx)
					minx = p.X;
				if (p.X > maxx)
					maxx = p.X;
				if (p.Y < miny)
					miny = p.Y;
				if (p.Y > maxy)
					maxy = p.Y;
			}
			
			return new Rect(new Point(minx, miny), new Point(maxx, maxy));
		}
		
		public override void Move (double dx, double dy)
		{
			foreach (DependencyObject obj in PointObjects){
				if (obj is PathFigure)
					MovePoint(obj, PathFigure.StartPointProperty, dx, dy);
				if (obj is LineSegment)
					MovePoint(obj, LineSegment.PointProperty, dx, dy);
				if (obj is BezierSegment) {
					MovePoint(obj, BezierSegment.Point1Property, dx, dy);
					MovePoint(obj, BezierSegment.Point2Property, dx, dy);
					MovePoint(obj, BezierSegment.Point3Property, dx, dy);
				}
			}
		}
		
		private void MovePoint(DependencyObject element, DependencyProperty prop, double dx, double dy)
		{
			Point p = (Point) element.GetValue(prop);
			p.X += dx;
			p.Y += dy;
			ChangeProperty(element, prop, p);
		}
		
		private IEnumerable<DependencyObject> PointObjects {
			get {
				PathGeometry geometry = path.Data as PathGeometry;
			
				if (geometry == null)
					return false;
				
				foreach (PathFigure fig in geometry.Figures) {
					yield return fig;
					foreach (PathSegment segment in fig.Segments) {
						yield return segment;
					}
				}
			}
		}

		private Path path;
	}
}
