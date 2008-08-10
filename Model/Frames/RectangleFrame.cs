// RectangleFrame.cs
//
// Author:
//    Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 Manuel Cerón
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
using System.Windows.Media;

namespace LunarEclipse.Model {
	
	public class RectangleFrame: AbstractFrame {
		
		public RectangleFrame(UIElement child):
			base(child)
		{
			rectangle = new Rectangle();
			
			rectangle.Stroke = new SolidColorBrush(Colors.LightGray);
			rectangle.StrokeDashArray = new System.Windows.Media.DoubleCollection();
			rectangle.StrokeDashArray.Add(5.0);
			rectangle.StrokeDashArray.Add(5.0);
            rectangle.StrokeThickness = 1;
		}
		
		public override void Update ()
		{
			IDescriptor descriptor = StandardDescriptor.CreateDescriptor(Child);
			Rect bounds = descriptor.GetBounds();
			
			rectangle.SetValue(LeftProperty, bounds.X);
			rectangle.SetValue(TopProperty, bounds.Y);
			rectangle.SetValue(WidthProperty, bounds.Width);
			rectangle.SetValue(HeightProperty, bounds.Height);
			
			rectangle.SetValue(UIElement.RenderTransformProperty, Child.RenderTransform);
			rectangle.SetValue(UIElement.RenderTransformOriginProperty, Child.RenderTransformOrigin);
		}
		
		// FIXME: Workarround for bug #386468
		public override void AddToCanvas()
		{
			Children.Add(rectangle);
		}

		private Rectangle rectangle;
	}
}
