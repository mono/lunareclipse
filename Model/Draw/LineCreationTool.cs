// LineCreationTool.cs
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

using System.Windows.Shapes;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class LineCreationTool: ShapeCreationTool {
		
		public LineCreationTool(MoonlightController controller):
			base(controller)
		{
		}
		
		protected override Shape CreateShape ()
		{
			return new Line();
		}
		
		protected override void UpdateShape ()
		{
			Line line = (Line) CreatedShape;
			
			line.X1 = ShapeStart.X;
			line.Y1 = ShapeStart.Y;
			line.X2 = ShapeEnd.X;
			line.Y2 = ShapeEnd.Y;
		}

	}
}
