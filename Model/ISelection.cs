// ISelection.cs
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

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LunarEclipse.Model {	
	
	public interface ISelection {
		
		event MouseEventHandler HandleMouseDown;
		event EventHandler SelectionChanged;
		
		void Add(UIElement element);
		void Remove(UIElement element);
		void Hide();
		void Show();
		void Clear();
		void SelectAll();
		void Update();
		
		void BringToFront();
		void BringForwards();
		void SendToBack();
		void SendBackwards();
		
		void AlignLeft();
		void AlignHorizontalCenter();
		void AlignRight();
		void AlignTop();
		void AlignVerticalCenter();
		void AlignBottom();
		
		void DeleteFromCanvas();
		void Copy();
		void Cut();
		void Paste();
		void Clone();
		
		Rect GetBounds();
		
		UIElement MainElement { get; }
		bool Contains(UIElement element);
		IEnumerable<UIElement> Elements {get;}
		int Count { get; }
	}
}
