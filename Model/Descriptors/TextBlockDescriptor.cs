// TextBlockDescriptor.cs
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
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class TextBlockDescriptor: StandardDescriptor {
		
		public TextBlockDescriptor(TextBlock element):
			this(element, null)
		{
		}
		
		public TextBlockDescriptor(TextBlock element, UndoGroup group):
			base(element, group)
		{
			text_block = element;
		}
		
		public override Rect GetBounds ()
		{
			double left = (double) Element.GetValue(Canvas.LeftProperty);
			double top = (double) Element.GetValue(Canvas.TopProperty);
			double width = text_block.ActualWidth;
			double height = text_block.ActualHeight;
			
			return new Rect(left, top, width, height);
		}
		
		private TextBlock text_block;
	}
}
