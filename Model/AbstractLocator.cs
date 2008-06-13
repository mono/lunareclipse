// AbstractLocator.cs
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
using System.Windows.Controls;

namespace LunarEclipse.Model {
	
	public abstract class AbstractLocator: ILocator {
		
		public AbstractLocator(UIElement element)
		{
			this.element = element;
		}
		
		public virtual Point Locate()
		{
			return new Point(0.0, 0.0);
		}
		
		protected Rect ElementBounds {
			get {
				double top = (double) Element.GetValue(Canvas.TopProperty);
				double left = (double) Element.GetValue(Canvas.LeftProperty);
				double width = (double) Element.GetValue(Canvas.WidthProperty);
				double height = (double) Element.GetValue(Canvas.HeightProperty);
				
				return new Rect(left, top, width, height);
			}
		}
		
		protected UIElement Element {
			get { return element; }
		}
		
		private UIElement element;
	}
}
