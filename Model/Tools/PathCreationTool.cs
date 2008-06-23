// PathCreationTool.cs
//
// Author:
//    Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 [copyright holders]
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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class PathCreationTool: ShapeCreationTool{
		
		public PathCreationTool(MoonlightController controller):
			base (controller)
		{
		}
		
		public override void MouseDown (MouseEventArgs ev)
		{
			if (CheckDoubleClick() ) {
				create_new = true;
				return;
			}
			
			Point position = ev.GetPosition(Controller.Canvas);
			
			if (create_new) {
				SetupElement();
				ShapeStart = position;
				ShapeEnd = position;
				create_new = false;
				return;
			}
			else {
				AddSegment();
			}
			UpdateShape();
		}
		
		public override void MouseMove (MouseEventArgs ev)
		{
			if (create_new)
				return;
			
			Point position = ev.GetPosition(Controller.Canvas);
			ShapeEnd = position;
			UpdateShape();
		}

		
		protected override UIElement CreateShape ()
		{
			Path path = new Path();
			PathGeometry geometry =  new PathGeometry();
			path.Data = geometry;
			geometry.Figures = new PathFigureCollection();
			figure = new PathFigure();
			geometry.Figures.Add(figure);
			figure.Segments = new PathSegmentCollection();
			AddSegment();
			
			return path;
		}
		
		protected override void UpdateShape ()
		{
			figure.StartPoint = ShapeStart;
			current_segment.Point1 = ShapeEnd;
			current_segment.Point2 = ShapeEnd;
			current_segment.Point3 = ShapeEnd;
		}
		
		private void AddSegment()
		{
			current_segment = new BezierSegment();
			figure.Segments.Add(current_segment);
		}
		
		private PathFigure figure;
		private BezierSegment current_segment;
		private Point start_point;
		private Point current_point;
		private bool create_new = true;
	}
}
