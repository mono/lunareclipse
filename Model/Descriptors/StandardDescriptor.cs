// BasicShapeDescriptor.cs
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

using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class StandardDescriptor: IDescriptor {
		
		public StandardDescriptor(UIElement element)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			Element = element;
		}
		
		// TODO: this should be changed with SL 2.0 custom properties
		public static IDescriptor CreateDescriptor(UIElement element)
		{
			if (element is TextBlock)
				return new TextBlockDescriptor(element as TextBlock);
			if (element is Line)
				return new LineDescriptor(element as Line);
			return new StandardDescriptor(element);
		}
		
		// TODO: this should be changed with SL 2.0 custom properties
		public static IHandleGroup CreateHandleGroup(MoonlightController controller, UIElement element)
		{
			if (element is Line)
				return new LineHandleGroup(controller, element as Line);
			if (element is Path)
				return new PathHandleGroup(controller, element as Path);
			return new RotateHandleGroup(controller, element);
		}
		
		public virtual Rect GetBounds ()
		{
			double left = (double) Element.GetValue(Canvas.LeftProperty);
			double top = (double) Element.GetValue(Canvas.TopProperty);
			double width = (double) Element.GetValue(Canvas.WidthProperty);
			double height = (double) Element.GetValue(Canvas.HeightProperty);
			
			return new Rect(left, top, width, height);
		}
		
		public virtual void SetBounds(double left, double top, double width, double height)
		{
			Element.SetValue(Canvas.TopProperty, top);
			Element.SetValue(Canvas.LeftProperty, left);
			Element.SetValue(Canvas.WidthProperty, width);
			Element.SetValue(Canvas.HeightProperty, height);
		}
		
		public virtual void SetBounds(Rect bounds)
		{
			SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
		}
		
		public virtual bool IsInside(Rect rect)
		{
			Rect element = GetBounds();
			if (element.X > rect.X && element.X + element.Width < rect.X + rect.Width &&
			    element.Y > rect.Y && element.Y + element.Height < rect.Y + rect.Height) {
				System.Console.WriteLine("True");
				return true;
			}
			return false;
		}
		
		public virtual void Move(double dx, double dy)
		{
			double top = (double) element.GetValue(Canvas.TopProperty);
			double left = (double) element.GetValue(Canvas.LeftProperty);
			
			left += dx;
			top += dy;
			
			Toolbox.ChangeProperty(element, Canvas.TopProperty, top);
			Toolbox.ChangeProperty(element, Canvas.LeftProperty, left);
		}
		
		public virtual void Move(Point offset)
		{
			Move(offset.X, offset.Y);
		}
		
		protected UIElement Element {
			get { return element; }
			set { element = value; }
		}
		
		private UIElement element;
	}
}
