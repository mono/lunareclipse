// AbstractControlPoint.cs
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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using Gtk.Moonlight;

namespace LunarEclipse.Model {	
	
	public abstract class AbstractControlPoint: Control, IControlPoint {
		
		public AbstractControlPoint(GtkSilver s, ISelectionBorder b)
		{
			silver = s;
			border = b;
			inner = s.InitializeFromXaml(GetXaml(), this);
			
			MouseLeftButtonDown += MouseStart;
			MouseLeftButtonUp += MouseEnd;
			MouseMove += MouseStep;
		}
		
		public abstract string GetXaml();
		
		public double Left {
			get { return (double) inner.GetValue(Canvas.LeftProperty); }
			set { inner.SetValue(Canvas.LeftProperty, value); }
		}
		
		public double Top {
			get { return (double) inner.GetValue(Canvas.TopProperty); }
			set { inner.SetValue(Canvas.TopProperty, value); }
		}
		
		public double Width {
			get { return (double) inner.GetValue(Canvas.WidthProperty); }
			set { inner.SetValue(Canvas.WidthProperty, value); }
		}
		
		public double Height {
			get { return (double) inner.GetValue(Canvas.HeightProperty); }
			set { inner.SetValue(Canvas.HeightProperty, value); }
		}
		
		protected ISelectionBorder Border {
			get { return border; }
		}
		
		protected Visual Element {
			get { return Border.Child; }
		}
		
		protected virtual void MouseStart(object sender, MouseEventArgs args)
		{
			CaptureMouse();
		}
		
		protected virtual void MouseEnd(object sender, MouseEventArgs args)
		{
			ReleaseMouseCapture();
		}
		
		protected virtual void MouseStep(object sender, MouseEventArgs args)
		{
		}
			
		private GtkSilver silver;
		private FrameworkElement inner;
		private ISelectionBorder border;
	}
}
