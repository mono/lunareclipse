// ResizeHandle.cs
//
// Author:
//    Manuel Cerón <ceronman@unicauca.edu.co>
//    Alan McGovern <alan.mcgovern@gmail.com>
//
// Copyright (c) 2007 Alan McGovern, 2008 Manuel Cerón
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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public abstract class ResizeHandle: TransformHandle {
		
		public ResizeHandle(MoonlightController controller, IHandleGroup group, ILocator locator):
			base(controller, group, locator)
		{
		}
		
		public override void MouseStep (object sender, MouseEventArgs args)
		{
			base.MouseStep (sender, args);
			
			if (!Dragging)
				return;
			
			IDescriptor descriptor = StandardDescriptor.CreateDescriptor(Element);
			
			Rect oldBounds = descriptor.GetBounds();
			Point offset = CalculateOffset(args.GetPosition(null));
			double angle = Toolbox.DegreesToRadians(Rotation.Angle);
			
			double cosAngle = Math.Cos(angle);
			double sinAngle = Math.Sin(angle);
			
			offset = TransformOffset(offset, angle);
			
			Rect newBounds = CalculateNewBounds(oldBounds, offset, cosAngle, sinAngle);
			descriptor.SetBounds(newBounds);
			
			UndoGroup.AddPropertyChange(Element, Canvas.LeftProperty, oldBounds.Left, newBounds.Left);
			UndoGroup.AddPropertyChange(Element, Canvas.TopProperty,  oldBounds.Top, newBounds.Top);
			UndoGroup.AddPropertyChange(Element, Canvas.WidthProperty, oldBounds.Width, newBounds.Width);
			UndoGroup.AddPropertyChange(Element, Canvas.HeightProperty, oldBounds.Height, newBounds.Height);
		}
		
		protected abstract Rect CalculateNewBounds(Rect oldBounds, Point offset, double cosAngle, double sinAngle);
		
		protected override string GetXaml ()
		{
			return "<Rectangle Fill=\"#99FFFF00\" Stroke=\"#FF000000\"/>";
		}
		
		private Point TransformOffset(Point offset, double angle)
		{
			// When a shape is rotated, we need to convert the mouse X, Y coordinates from
			// canvas coordinates into 'shape' coordinates. In 'shape' coordinates, the
			// X coord corresponds to moving perpendicularly to the width of the rotated
			// shape and the Y coordinate corresponds to moving perpendicularly to the Height
			// of the rotated shape.
			double mouseDistanceTravelled = Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);
			double mouseAngle = Math.Atan2(offset.Y, offset.X);
			offset = new Point(mouseDistanceTravelled * Math.Cos(mouseAngle - angle),
							   mouseDistanceTravelled * Math.Sin(mouseAngle - angle));
			
			return offset;
		}
	}
}
