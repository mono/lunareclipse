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
using System.Windows.Media;
using System.Windows.Shapes;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class PolyLineDescriptor: AbstractLineDescriptor {
		
		public PolyLineDescriptor(Polyline element):
			this(element, null)
		{
		}
		
		public PolyLineDescriptor(Polyline element, UndoGroup group):
			base(element, group)
		{
			polyline = element;
		}
		
		public override Rect GetBounds ()
		{
			PointCollection points = polyline.Points;
			double minx = Double.MaxValue;
			double miny = Double.MaxValue;
			double maxx = Double.MinValue;
			double maxy = Double.MinValue;
			
			foreach (Point p in points) {
				minx = Math.Min(minx, p.X);
				maxx = Math.Max(maxx, p.X);
				miny = Math.Min(miny, p.Y);
				maxy = Math.Max(maxy, p.Y);
			}
			
			return new Rect(new Point(minx, miny), new Point(maxx, maxy));
		}
		
		public override void Move (double dx, double dy)
		{
			PointCollection points = polyline.Points;
			
			for (int i=0; i< points.Count; i++) {
				Point p = points[i];
				p.X += dx;
				p.Y += dy;
				points[i] = p;
			}

			ChangeProperty(polyline, Polyline.PointsProperty, points);
		}

		private Polyline polyline;
	}
}
