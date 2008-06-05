// ProportionalShapeCreationTool.cs
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

using System;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public abstract class ProportionalShapeCreationTool: ShapeCreationTool {
		
		public ProportionalShapeCreationTool(MoonlightController controller):
			base(controller)
		{
		}
		
		protected override void NormalizeShape (out double x, out double y, out double width, out double height)
		{
			x = ShapeStart.X;
			y = ShapeStart.Y;
			width = ShapeEnd.X - ShapeStart.X;
			height = ShapeEnd.Y - ShapeStart.Y;
			
			double size = Math.Max(Math.Abs(width), Math.Abs(height));
			
			if (width < 0)
				x = ShapeStart.X - size;
			
			if (height < 0)
				y = ShapeStart.Y - size;

			width = size;
			height = size;
		}
	}
}
