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

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {
	
	public abstract class AbstractHandle: UserControl, IHandle {
		
		public const double DefaultRadius = 5.0;
	
		public AbstractHandle(MoonlightController controller, IHandleGroup group):
			base()
		{
			Group = group;
			
			//inner = controller.GtkSilver.InitializeFromXaml(GetXaml(), this);
			//Application.LoadComponent(this, new Uri("ellipse.xml", UriKind.Relative));
			Content = CreateContent();
			
			Content.MouseLeftButtonDown += MouseStart;
			Content.MouseLeftButtonUp += MouseEnd;
			Content.MouseMove += MouseStep;
			
			highlight_fill = new SolidColorBrush(Colors.Red);
			normal_fill = (Brush) Content.GetValue(Shape.FillProperty);
			
			Content.MouseEnter += delegate {
				Content.SetValue(Shape.FillProperty, highlight_fill);
			};
			
			Content.MouseLeave += delegate {
				Content.SetValue(Shape.FillProperty, normal_fill);
			};
			
			SetValue(Canvas.ZIndexProperty, int.MaxValue);
			
			transforms = new TransformGroup();
			scaleTransform = new ScaleTransform();
			transforms.Children.Add(scaleTransform);
			Content.SetValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));
		 	Content.SetValue(UIElement.RenderTransformProperty, transforms);
		
			undo_group = new UndoGroup();
			
			Controller.ZoomChanged += delegate {
				Update();
			};
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
			Content.SetValue(Shape.FillProperty, normal_fill);
			Dragging = false;
			PushUndo();
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
			
		 	ZoomCorrection();
		}
			
		protected Rect CanvasAllocation {
			get {
				IDescriptor descriptor = StandardDescriptor.CreateDescriptor(this);
				return descriptor.GetBounds();
			}
			set {
				IDescriptor descriptor = StandardDescriptor.CreateDescriptor(this);
				descriptor.SetBounds(value);
				descriptor = StandardDescriptor.CreateDescriptor(Content);
				descriptor.SetBounds(0, 0, value.Width, value.Height);
			}
		}
			
		protected virtual UIElement CreateContent()
		{
			Rectangle rect = new Rectangle();
			rect.Fill = new SolidColorBrush(Colors.Yellow);
			rect.Stroke = new SolidColorBrush(Colors.Black);
			return rect;
		}
		
		protected UIElement Element {
			get { return Group.Child; }
		}
		
		protected MoonlightController Controller {
			get { return Group.Controller; }
		}
		
		protected Point LastClick {
			get { return last_click; }
			set { last_click = value; }
		}

		protected bool Dragging {
			get { return dragging; }
			set { dragging = value; }
		}

		protected UndoGroup UndoGroup {
			get { return undo_group; }
		}

		public TransformGroup Transforms {
			get { return transforms; }
			set { transforms = value; }
		}
		
		protected Point CalculateOffset(Point current)
		{
			Point offset = new Point(0.0, 0.0);
			
			offset.X = current.X - LastClick.X;
			offset.Y = current.Y - LastClick.Y;
			
			LastClick = current;
			
			return Controller.ZoomCorrection(offset);
		}
		
		protected virtual void PushUndo()
		{
			Controller.UndoEngine.PushUndo(undo_group);
		}
		
		protected virtual void ZoomCorrection()
		{
			double scale = 	1.0 / Controller.CurrentScale;
			scaleTransform.ScaleX = scale;
			scaleTransform.ScaleY = scale;
		}
		
		protected void ChangeProperty(DependencyObject item, DependencyProperty prop, object value)
		{
			object oldValue = item.GetValue(prop);
			Toolbox.ChangeProperty(Element, item, prop, value);
			UndoGroup.AddPropertyChange(Element, item, prop, oldValue, value);
		}
		
		private IHandleGroup group;
		private Point last_click;
		private bool dragging = false;
		private Brush normal_fill;
		private Brush highlight_fill;
		private TransformGroup transforms;
		private ScaleTransform scaleTransform;
		private UndoGroup undo_group;
	}
}
