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
		
		public StandardDescriptor(UIElement element):
			this(element, null)
		{
		}
			
		public StandardDescriptor(UIElement element, UndoGroup undoGroup)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			Element = element;
			UndoGroup = undoGroup;
		}
		
		// TODO: this should be changed with SL 2.0 custom properties
		public static IDescriptor CreateDescriptor(UIElement element, UndoGroup group)
		{
			if (element is TextBlock)
				return new TextBlockDescriptor(element as TextBlock, group);
			if (element is Line)
				return new LineDescriptor(element as Line, group);
			if (element is Polyline)
				return new PolyLineDescriptor(element as Polyline, group);
			if (element is Path)
				return new PathDescriptor(element as Path, group);
			return new StandardDescriptor(element, group);
		}
		public static IDescriptor CreateDescriptor(UIElement element)
		{
			 return CreateDescriptor(element, null);
		}
		
		// TODO: this should be changed with SL 2.0 custom properties
		public static IHandleGroup CreateHandleGroup(MoonlightController controller, UIElement element)
		{
			if (element is Line)
				return new LineHandleGroup(controller, element as Line);
			if (element is Path)
				return new PathHandleGroup(controller, element as Path);
			if (element is Polyline)
				return new PolyLineHandleGroup(controller, element as Polyline);
			return new ResizeRotateHandleGroup(controller, element);
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
			ChangeProperty(Element, Canvas.LeftProperty, left);
			ChangeProperty(Element, Canvas.TopProperty, top);
			ChangeProperty(Element, Canvas.WidthProperty, width);
			ChangeProperty(Element, Canvas.HeightProperty, height);
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
			
			ChangeProperty(element, Canvas.TopProperty, top);
			ChangeProperty(element, Canvas.LeftProperty, left);
		}
		
		public virtual void Move(Point offset)
		{
			Move(offset.X, offset.Y);
		}
		
		public void ChangeProperty(DependencyObject item, DependencyProperty prop, object value)
		{
			object oldValue = item.GetValue(prop);
			Toolbox.ChangeProperty(Element, item, prop, value);
			if (UndoGroup != null)
				UndoGroup.AddPropertyChange(Element, item, prop, oldValue, value);
		}
		
		protected UIElement Element {
			get { return element; }
			set { element = value; }
		}

		protected UndoGroup UndoGroup {
			get { return undo_group; }
			set { undo_group = value; }
		}
		
		private UIElement element;
		private UndoGroup undo_group;
	}
}
