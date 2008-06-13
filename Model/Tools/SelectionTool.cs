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
using System.Windows;
using System.Windows.Input;
using LunarEclipse;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class SelectionTool: AbstractTool {
		
		public SelectionTool(MoonlightController controller):
			base (controller)
		{
		}
		
		public override void Activate()
		{
			base.Activate();
			
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
			
			if (clicked_element != null) {
				Controller.Selection.Add(clicked_element);
				action_flag = true;
			}
			
			if (clicked_handle != null) {
				System.Console.WriteLine("*****Handle");
				action_flag = true;
			}
		}
		
		public override void MouseUp (MouseEventArgs ev)
		{
			base.MouseUp (ev);
			
			clicked_element = null;
			clicked_handle = null;
			
			if (!action_flag)
				Controller.Selection.Clear();
			
			action_flag = false;
		}

		protected void OnHandleMouseDown(object sender, MouseEventArgs args)
		{
			clicked_handle = sender as IHandle;
		}
		
		private bool Selectable(UIElement element)
		{
			return ( (element != null) &&
			        !(element is IHandle) );
		}
		
		private void OnElementClicked(object sender, MouseEventArgs e)
		{
			clicked_element = sender as UIElement;
		}
				
		private UIElement clicked_element;
		private IHandle clicked_handle;
		private bool action_flag = false;
	}
}
