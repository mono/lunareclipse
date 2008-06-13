// PathHandleGroup.cs
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
using System.Windows.Media;
using System.Windows.Shapes;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class PathHandleGroup: AbstractHandleGroup {
		
		public PathHandleGroup(MoonlightController controller, Path child):
			base(controller, child)
		{
			PathGeometry geometry = child.Data as PathGeometry;
			
			if (geometry == null)
				return;
			
			foreach (PathFigure fig in geometry.Figures) {
				foreach (LineSegment segment in fig.Segments) {
					AddHandle(new LineSegmentHandle(Controller, this, segment) );
				}
				AddHandle(new StartPointHandle(Controller, this, fig) );
			}
			
			UpdateHandles();
		}
	}
}
