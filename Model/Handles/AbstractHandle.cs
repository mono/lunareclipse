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
using LunarEclipse.Controller;

namespace LunarEclipse.Model {
	
	public abstract class AbstractHandle: Control, IHandle {
		
		public const double DefaultRadius = 5.0;
		
		public AbstractHandle(MoonlightController controller, IHandleGroup group):
			base()
		{
			Group = group;
			
			inner = controller.GtkSilver.InitializeFromXaml(GetXaml(), this);
			
			MouseLeftButtonDown += MouseStart;
			MouseLeftButtonUp += MouseEnd;
			MouseMove += MouseStep;
		}
		
		public IHandleGroup Group {
			get { return group; }
			protected set { group = value; }
		}
		
		public abstract Point Location {
			get;
			set;
		}
		
		public virtual void Move(double dx, double dy)
		{
			Rect r = CanvasAllocation;
			r.X += dx;
			r.Y += dy;
			CanvasAllocation = r;
		}
		
		public virtual void MouseStart(object sender, MouseEventArgs args)
		{
			CaptureMouse();
			LastClick = args.GetPosition(null);
			Dragging = true;
		}
		
		public virtual void MouseEnd(object sender, MouseEventArgs args)
		{
			ReleaseMouseCapture();
			Dragging = false;
		}
		
		public virtual void MouseStep(object sender, MouseEventArgs args)
		{
		}
		
		public virtual void Update ()
		{
			Point position = Location;
			position.X += (double) Element.GetValue(Canvas.LeftProperty);
			position.Y += (double) Element.GetValue(Canvas.TopProperty);
			CanvasAllocation = new Rect(position.X - DefaultRadius,
			                            position.Y - DefaultRadius,
			                            DefaultRadius * 2,
			                            DefaultRadius * 2);
		}
			
		protected Rect CanvasAllocation {
			get {
				IDescriptor descriptor = StandardDescriptor.CreateDescriptor(this);
				return descriptor.GetBounds();
			}
			set {
				IDescriptor descriptor = StandardDescriptor.CreateDescriptor(this);
				descriptor.SetBounds(value);
				descriptor = StandardDescriptor.CreateDescriptor(inner);
				descriptor.SetBounds(0, 0, value.Width, value.Height);
			}
		}
			
		protected virtual string GetXaml()
		{
			return "<Rectangle Fill=\"#00FFFFFF\" Stroke=\"#FF000000\"/>";
		}
		
		protected UIElement Element {
			get { return Group.Child; }
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
		
		protected Point LastClick {
			get { return last_click; }
			set { last_click = value; }
		}

		protected bool Dragging {
			get { return dragging; }
			set { dragging = value; }
		}

		protected FrameworkElement Inner {
			get { return inner; }
			set { inner = value; }
		}
		
		protected Point CalculateOffset(Point current)
		{
			Point offset = new Point(0.0, 0.0);
			
			offset.X = current.X - LastClick.X;
			offset.Y = current.Y - LastClick.Y;
			
			LastClick = current;
			
			return offset;
		}
		
		private IHandleGroup group;
		private FrameworkElement inner;
		private Point last_click;
		private bool dragging = false;
	}
}
