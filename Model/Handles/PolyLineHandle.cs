// PolyLineHandle.cs
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
using System.Windows.Media;
using System.Windows.Shapes;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {
	
	public class PolyLineHandle: PointHandle {
		
		public PolyLineHandle(MoonlightController controller, IHandleGroup group, int i):
			base(controller, group)
		{
			index = i;
		}
		
		public override Point Location {
			get {
				return Points[index];
			}
			set {
				PointCollection points = Points;
				points[index] = value;
				Points = points;
			}
		}
		
		private PointCollection Points {
			get {
				return (PointCollection) Element.GetValue(Polyline.PointsProperty);
			}
			
			set {
				ChangeProperty(Element, Polyline.PointsProperty, value);
			}
		}
		
		private int index;
	}
}
