// ShapeCreationTool.cs
//
// Author:
//   Manuel Cerón <ceronman@unicauca.edu.co>
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

using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public abstract class ShapeCreationTool: AbstractTool {
		
		public ShapeCreationTool(MoonlightController controller):
			base (controller)
		{
		}
		
		public override void MouseDown (MouseEventArgs ev)
		{
			base.MouseDown (ev);
			
			CreatedShape = CreateShape();
			Controller.Canvas.Children.Add(CreatedShape);
			
			Point position = ev.GetPosition(Controller.Canvas);
			
			ShapeStart = position;
			ShapeEnd = position;
			
			UpdateShape();
		}
		
		public override void MouseMove (MouseEventArgs ev)
		{
			base.MouseMove (ev);
			
			Point position = ev.GetPosition(Controller.Canvas);
			ShapeEnd = position;
			UpdateShape();
		}

		
		protected abstract Shape CreateShape();
		
		protected virtual void UpdateShape() 
		{
			double x, y, width, height;
			
			NormalizeShape(out x, out y, out width, out height);
			
			CreatedShape.SetValue(Canvas.LeftProperty, x);
			CreatedShape.SetValue(Canvas.TopProperty, y);
			CreatedShape.SetValue(Shape.WidthProperty, width);
			CreatedShape.SetValue(Shape.HeightProperty, height);
		}
		
		protected virtual void NormalizeShape(out double x, out double y, out double width, out double height)
		{
			x = ShapeStart.X;
			y = ShapeStart.Y;
			width = ShapeEnd.X - ShapeStart.X;
			height = ShapeEnd.Y - ShapeStart.Y;
			
			if (width < 0) {
				x = ShapeEnd.X;
				width = -width;
			}
			
			if (height < 0) {
				y = ShapeEnd.Y;
				height = -height;
			}
		}
		
		protected Shape CreatedShape {
			get { return created_shape; }
			set { created_shape = value; }
		}

		protected Point ShapeStart {
			get {
				return shape_start;
			}
			set {
				shape_start = value;
			}
		}

		protected Point ShapeEnd {
			get {
				return shape_end;
			}
			set {
				shape_end = value;
			}
		}
		
		private Shape created_shape;
		private Point shape_start;
		private Point shape_end;
	}
}
