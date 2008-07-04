// LineDescriptor.cs
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

using System.Windows;
using System.Windows.Shapes;

namespace LunarEclipse.Model {	
	
	public class LineDescriptor: StandardDescriptor {
		
		public LineDescriptor(Line element):
			base(element)
		{
			line = element;
		}
		
		public override Rect GetBounds ()
		{
			double x = line.X1;
			double y = line.Y1;
			double width = line.X2 - line.X1;
			double height = line.Y2 - line.Y1;
			
			Toolbox.NormalizeRect(ref x, ref y, ref width, ref height);
			return new Rect(x, y, width, height);
		}
		
		public override void Move (double dx, double dy)
		{
			double x1 = line.X1 + dx;
			double x2 = line.X2 + dx;
			double y1 = line.Y1 + dy;
			double y2 = line.Y2 + dy;
			
			Toolbox.ChangeProperty(line, line, Line.X1Property, x1);
			Toolbox.ChangeProperty(line, line, Line.X2Property, x2);
			Toolbox.ChangeProperty(line, line, Line.Y1Property, y1);
			Toolbox.ChangeProperty(line, line, Line.Y2Property, y2);
		}
		
		Line line;
	}
}
