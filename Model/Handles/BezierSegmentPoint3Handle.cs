// BezierSegmentPoint3Handle.cs
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

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {
		
	public class BezierSegmentPoint3Handle: PointHandle {
		
		public BezierSegmentPoint3Handle(MoonlightController controller, IHandleGroup group, BezierSegment seg):
			base(controller, group)
		{
			segment = seg;
		}
		
		public override Point Location {
			get { return segment.Point3; }
			set { 
				double dx = value.X - segment.Point3.X;
				double dy = value.Y - segment.Point3.Y;
				
				MovePoint(BezierSegment.Point1Property, dx, dy);
				MovePoint(BezierSegment.Point2Property, dx, dy);
				MovePoint(BezierSegment.Point3Property, dx, dy);
			}
		}
		
		private void MovePoint(DependencyProperty pointProperty, double dx, double dy)
		{
			Point point = (Point) segment.GetValue(pointProperty);
			point.X += dx;
			point.Y += dy;
			ChangeProperty(segment, pointProperty, point);		
		}
		
		protected override UIElement CreateContent ()
		{
			Rectangle rect = new Rectangle();
			rect.Fill = new SolidColorBrush(Colors.Yellow);
			rect.Stroke = new SolidColorBrush(Colors.Black);
			return rect;
		}
		
		BezierSegment segment;
	}
}
