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
			frames = new List<BezierSegmentFrame>();
		}
		
		public override void MouseDown (MouseEventArgs ev)
		{
			Dragging = true;
			
			if (CheckDoubleClick() ) {
				create_new = true;
				figure.Segments.Remove(current_segment);
				RemoveAllFrames();
				return;
			}
			
			Point position = ev.GetPosition(Controller.Canvas);
			
			if (create_new) {
				SetupElement();
				AddSegment();
				ShapeStart = position;
				ShapeEnd = position;
				create_new = false;
			}
			else {
				AddSegment();
			}
			UpdateShape();
		}
		
		public override void MouseUp (MouseEventArgs ev)
		{
			Dragging = false;
		}

		public override void MouseMove (MouseEventArgs ev)
		{
			if (create_new)
				return;
			
			Point position = ev.GetPosition(Controller.Canvas);
			
			if (Dragging) {
				Point center = current_segment.Point3;
				Point offset = center;
				
				offset.X = position.X - center.X;
				offset.Y = position.Y - center.Y;
				
				Point simetric = new Point();
				
				simetric.X = center.X - offset.X;
				simetric.Y = center.Y - offset.Y;
				
				current_segment.Point1 = position;
				if (previous_segment != null) {
					previous_segment.Point2 = simetric;
				}
				
				dragging_bezier = true;
				UpdateFrames();
				return;
			}
			
			ShapeEnd = position;
			UpdateShape();
		}
		
		protected Point PreviousPoint {
			get {
				if (previous_segment != null) {
					return previous_segment.Point3;
				}
				else {
					return figure.StartPoint;
				}
			}
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
			
			return path;
		}
		
		protected override void UpdateShape ()
		{
			figure.StartPoint = ShapeStart;
			
			current_segment.Point3 = ShapeEnd;
			current_segment.Point2 = ShapeEnd;
			
			if (!dragging_bezier) {
				current_segment.Point1 = PreviousPoint;
			}
		}
		
		private void AddSegment()
		{
			previous_segment = current_segment;
			current_segment = new BezierSegment();
			figure.Segments.Add(current_segment);
			BezierSegmentFrame frame = new BezierSegmentFrame(CreatedShape, figure, current_segment);
			Controller.Canvas.Children.Add(frame);
			frame.AddToCanvas();
			frames.Add(frame);
			
			dragging_bezier = false;
		}
		
		private void RemoveAllFrames()
		{
			foreach (BezierSegmentFrame frame in frames)
				Controller.Canvas.Children.Remove(frame);
			
			frames.Clear();
		}
		
		private void UpdateFrames()
		{
			foreach (BezierSegmentFrame frame in frames)
				frame.Update();
		}
		
		private PathFigure figure;
		private BezierSegment current_segment;
		private BezierSegment previous_segment;
		private Point bezier_point;
		private List<BezierSegmentFrame> frames;
		private bool create_new = true;
		private bool dragging_bezier = false;
	}
}
