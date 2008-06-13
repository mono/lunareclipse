// PenCreationTool.cs
//
// Author:
//   Alan McGovern <alan.mcgovern@gmail.com>
//   Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2007 Alan McGovern.
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
using System.Windows.Media;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {
	
	public class PenCreationTool: ShapeCreationTool {
		
		public PenCreationTool(MoonlightController controller):
			base (controller)
		{
		}
		
		protected override Shape CreateShape ()
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
			if (ShapeStart == ShapeEnd)
				return;
			figure.StartPoint = ShapeStart;
			
			LineSegment segment = new LineSegment();
			segment.Point = ShapeEnd;
			
			figure.Segments.Add(segment);
		}

		
		private PathFigure figure;
	}
}
