// SelectionTool.cs
//
// Author:
//   Alan McGovern <alan.mcgovern@gmail.com>
//   Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2007 Alan McGovern.
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class SelectionTool: AbstractTool {
		
		public SelectionTool(MoonlightController controller):
			base (controller)
		{
			SelectionRect = new Rectangle();
			SelectionRect.Opacity = 0.2;
            SelectionRect.Fill = new SolidColorBrush(Colors.Blue);
            SelectionRect.Stroke = new SolidColorBrush(Colors.Black);
            SelectionRect.StrokeThickness = 1.0;
            SelectionRect.SetValue(UIElement.ZIndexProperty, int.MaxValue);
		}
		
		public override void Activate()
		{
			base.Activate();
			
			Controller.Selection.Show();
			
			foreach (UIElement element in Controller.Canvas.Children) {
				if (!Selectable(element))
					continue;
				element.MouseLeftButtonDown += OnElementClicked;
				element.Cursor = Cursors.Hand;
			}
			
			Controller.Selection.HandleMouseDown += OnHandleMouseDown;
		}
		
		public override void Deactivate ()
		{
			base.Deactivate ();
			
			Controller.Selection.Hide();
			
			foreach (UIElement element in Controller.Canvas.Children) {
				if (!Selectable(element))
					continue;
				element.MouseLeftButtonDown -= OnElementClicked;
				element.Cursor = Cursors.Default;
			}
			
			Controller.Selection.HandleMouseDown -= OnHandleMouseDown;	
		}

		
		public override void MouseDown (MouseEventArgs ev)
		{
			base.MouseDown (ev);
			
			last_click = ev.GetPosition(null);
			
			if (clicked_element != null) {
				if (!Controller.Selection.Contains(clicked_element) ){
					Controller.Selection.Clear();
					Controller.Selection.Add(clicked_element);
				}
				return;
			}
			
			if (clicked_handle != null)
				return;
			
			StartSelection(ev.GetPosition(null));
		}
		
		public override void MouseMove (MouseEventArgs ev)
		{
			base.MouseMove (ev);
			
			if (!Dragging)
				return;
			
			if (clicked_element != null) {
				Point offset = new Point(0.0, 0.0);
				Point current = ev.GetPosition(null);
				offset.X = current.X - last_click.X;
				offset.Y = current.Y - last_click.Y;
				foreach (UIElement element in Controller.Selection.Elements) {
					IDescriptor descriptor = StandardDescriptor.CreateDescriptor(element);
					descriptor.Move(offset);
				}
				last_click = current;
				return;
			}
			
			MoveSelection(ev.GetPosition(null));
		}

		
		public override void MouseUp (MouseEventArgs ev)
		{
			base.MouseUp (ev);
			
			if (clicked_element == null && clicked_handle == null)
				EndSelection();
			
			clicked_element = null;
			clicked_handle = null;
		}
		
		protected Rectangle SelectionRect {
			get { return selection_rect; }
			set { selection_rect = value; }
		}

		protected Point SelectionStart {
			get { return selection_start; }
			set { selection_start = value; }
		}

		protected Point SelectionEnd {
			get { return selection_end; }
			set { selection_end = value; }
		}

		protected void OnHandleMouseDown(object sender, MouseEventArgs args)
		{
			clicked_handle = sender as IHandle;
		}
		
		private bool Selectable(UIElement element)
		{
			return ( (element != null) &&
			        !(element is IHandle) &&
			        !(element is IFrame) &&
			        element != SelectionRect);
		}
		
		private void OnElementClicked(object sender, MouseEventArgs e)
		{
			clicked_element = sender as UIElement;
		}
		
		private void UpdateSelectionRect()
		{
			double x = SelectionStart.X;
			double y = SelectionStart.Y;
			double width = SelectionEnd.X - SelectionStart.X;
			double height = SelectionEnd.Y - SelectionStart.Y;
			
			Toolbox.NormalizeRect(ref x, ref y, ref width, ref height);
			
			SelectionRect.SetValue(Canvas.LeftProperty, x);
			SelectionRect.SetValue(Canvas.TopProperty, y);
			SelectionRect.SetValue(Canvas.WidthProperty, width);
			SelectionRect.SetValue(Canvas.HeightProperty, height);
		}
		
		private void StartSelection(Point start)
		{
			SelectionStart = start;
			SelectionEnd = start;
			UpdateSelectionRect();
			Controller.Canvas.Children.Add(SelectionRect);
		}
		
		private void MoveSelection(Point end)
		{
			SelectionEnd = end;
			UpdateSelectionRect();
		}
		
		private void EndSelection()
		{
			Controller.Canvas.Children.Remove(SelectionRect);
			
			IDescriptor rectDescriptor = StandardDescriptor.CreateDescriptor(SelectionRect);
			Rect selection = rectDescriptor.GetBounds();
			
			List<UIElement> selectedElements = new List<UIElement>();
			foreach (UIElement element in Controller.Canvas.Children) {
				IDescriptor descriptor = StandardDescriptor.CreateDescriptor(element);
				if (descriptor.IsInside(selection) && Selectable(element))
					selectedElements.Add(element);
			}
			
			Controller.Selection.Clear();
			
			foreach (UIElement element in selectedElements)
				Controller.Selection.Add(element);
		}
		
		private UIElement clicked_element;
		private IHandle clicked_handle;
		private Point last_click;
		private Rectangle selection_rect;
		private Point selection_start;
		private Point selection_end;
	}
}
