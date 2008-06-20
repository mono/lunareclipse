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

using System;
using System.Windows;
using System.Windows.Shapes;

namespace LunarEclipse.Model {	
	
	public class PolyLineDescriptor: StandardDescriptor {
		
		public PolyLineDescriptor(Polyline element):
			base(element)
		{
			polyline = element;
		}
		
		public override Rect GetBounds ()
		{
			Point[] points = (Point[]) polyline.GetValue(Polyline.PointsProperty);
			double minx = Double.MaxValue;
			double miny = Double.MaxValue;
			double maxx = Double.MinValue;
			double maxy = Double.MinValue;
			
			foreach (Point p in points) {
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
			Point[] points = (Point[]) polyline.GetValue(Polyline.PointsProperty);
			
			for (int i=0; i< points.Length; i++) {
				Point p = points[i];
				p.X += dx;
				p.Y += dy;
				points[i] = p;
			}
			
			Toolbox.ChangeProperty(polyline, Polyline.PointsProperty, points);
		}

		
		private Polyline polyline;
	}
}
