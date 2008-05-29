// AbstractHandle.cs
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
	
	public abstract class AbstractHandle: Control, IHandle{
		
		public AbstractHandle(GtkSilver silver, IHandleGroup group)
		{
			Group = group;
			Silver = silver;
			
			inner = silver.InitializeFromXaml(GetXaml(), this);
			
			MouseLeftButtonDown += MouseStart;
			MouseLeftButtonUp += MouseEnd;
			MouseMove += MouseStep;
			
			Update();
		}
		
		public IHandleGroup Group {
			get { return group; }
			protected set { group = value; }
		}
		
		public Rect CanvasAllocation {
			get {
				double left = (double) GetValue(Canvas.LeftProperty);
				double top = (double) GetValue(Canvas.TopProperty);
				double width = (double) GetValue(Canvas.WidthProperty);
				double height = (double) GetValue(Canvas.HeightProperty);
				
				return new Rect(left, top, width, height);
			}
			set {
				SetValue(Canvas.TopProperty, value.Top);
				SetValue(Canvas.LeftProperty, value.Left);
				SetValue(Canvas.WidthProperty, value.Width);
				SetValue(Canvas.HeightProperty, value.Height);
				
				inner.SetValue(Canvas.TopProperty, 0);
				inner.SetValue(Canvas.LeftProperty, 0);
				inner.SetValue(Canvas.WidthProperty, value.Width);
				inner.SetValue(Canvas.HeightProperty, value.Height);
			}
		}
		
		public virtual void Move(double dx, double dy)
		{
			Rect r = CanvasAllocation;
			r.X += dx;
			r.Y += dy;
		}
		
		public virtual void MouseStart(object sender, MouseEventArgs args)
		{
			CaptureMouse();
		}
		
		public virtual void MouseEnd(object sender, MouseEventArgs args)
		{
			ReleaseMouseCapture();
		}
		
		public virtual void MouseStep(object sender, MouseEventArgs args)
		{
			System.Console.WriteLine("Moving...");
		}
		
		public virtual void Update()
		{
			Rect r = CalculateElementBounds();
			CanvasAllocation = r;
		}
			
		// TODO: Candidate for Helper
		protected Rect CalculateElementBounds()
		{
			double top = (double) Element.GetValue(Canvas.TopProperty);
			double left = (double) Element.GetValue(Canvas.LeftProperty);
			double width = (double) Element.GetValue(Canvas.WidthProperty);
			double height = (double) Element.GetValue(Canvas.HeightProperty);
			
			return new Rect(left, top, width, height);
		}
		
		protected virtual string GetXaml()
		{
			return "<Rectangle />";
		}
		
		protected Visual Element {
			get { return Group.Child; }
		}
		
		protected GtkSilver Silver{
			get { return silver; }
			set { silver = value; }
		}
		
		private IHandleGroup group;
		private GtkSilver silver;
		private FrameworkElement inner;
	}
}
