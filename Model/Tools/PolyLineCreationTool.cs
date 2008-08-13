// PolyLineCreationTool.cs
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
	
	public class PolyLineCreationTool: ShapeCreationTool {
		
		public PolyLineCreationTool(MoonlightController controller):
			base (controller)
		{
		}
		
		public override void MouseDown (MouseEventArgs ev)
		{
			if (CheckDoubleClick()) {
				create_new = true;
				try  { points.RemoveAt(current_index + 1); }
				catch {}
				return;
			}
			
			if (create_new) {
				base.MouseDown (ev);
				create_new = false;
				return;
			}
			
			Point position = ev.GetPosition(Controller.Canvas);
			
			ShapeStart = position;
			ShapeEnd = position;
			
			points.Add(position);
			
			foreach (Point p in points)
				System.Console.Write(p.ToString() + " | ");
			System.Console.WriteLine();
			
			
			foreach (Point p in ((Polyline)CreatedShape).Points)
				System.Console.Write(p.ToString() + " | ");
			System.Console.WriteLine();
			current_index++;
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
			Polyline polyline = new Polyline();
			points = new PointCollection();
			polyline.Points = points;
			points.Add(new Point());
			points.Add(new Point());
			current_index = 0;
			return polyline;
		}
		
		protected override void UpdateShape ()
		{
			points[current_index] = ShapeStart;
			points[current_index+1] = ShapeEnd;
		}
		
		private PointCollection points;
		private bool create_new = true;
		private int current_index;
		
		// trick to support double click;
		private long last_ticks = 0;
	}
}
