// RotateHandle.cs
//
// Author:
//	 Alan McGovern <alan.mcgovern@gmail.com>
//   Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 Alan McGover
// Copyrigth (c) 2008 Manuel Cerón.
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


using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using LunarEclipse.Controller;

namespace LunarEclipse.Model
{
	public class RotateHandle: TransformHandle	{
		
		public RotateHandle(MoonlightController controller, IHandleGroup group, ILocator locator):
			base(controller, group, locator)
		{
		}
		
		public override void MouseStep (object sender, MouseEventArgs args)
		{
			base.MouseStep (sender, args);
			
			if (!Dragging)
				return;
			
			Point center = ElementTransformCenter;
			Point position = args.GetPosition(null);
		
			// TODO: Explain this.
			double alpha = Math.Atan2(position.Y - center.Y, position.X - center.X);
			double beta = Math.Atan2(LastClick.Y - center.Y, LastClick.X - center.X);
			
			double angle = ElementRotation.Angle + Toolbox.RadiansToDegrees(alpha - beta);
			
			Toolbox.ChangeProperty(Element, ElementRotation, RotateTransform.AngleProperty, angle);
			Update();
			
			LastClick = position;			
		}
		
		protected override string GetXaml ()
		{
			return "<Ellipse Fill=\"#99FFFF00\" Stroke=\"#FF000000\"/>";
		}
	}
}
