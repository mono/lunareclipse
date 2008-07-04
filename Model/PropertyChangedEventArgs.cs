//
// PropertyChangedEventArgs.cs
//
// Authors:
//   Alan McGovern alan.mcgovern@gmail.com
//
// Copyright (C) 2007 Alan McGovern
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


using System;
using System.Windows;

namespace LunarEclipse.Model
{
	public class PropertyChangedEventArgs : EventArgs
	{
		private DependencyObject target;
		private DependencyProperty property;
		private object oldValue;
		private object newValue;
		private UIElement element;
		
		public object NewValue
		{
			get { return newValue; }
		}
		
		public object OldValue
		{
			get { return oldValue; }
		}
		
		public DependencyProperty Property
		{
			get { return property; }
		}
		
		public DependencyObject Target
		{
			get { return target; }
		}

		public UIElement Element {
			get { return element; }
		}
		
		public PropertyChangedEventArgs(UIElement element, DependencyObject target, DependencyProperty property, object oldValue, object newValue)
		{
			this.newValue = newValue;
			this.oldValue = oldValue;
			this.property = property;
			this.target = target;
			this.element = element;
		}
	}
}
