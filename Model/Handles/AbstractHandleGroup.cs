// AbstractHandleGroup.cs
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
using System.Windows.Controls;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {
	
	public class AbstractHandleGroup: IHandleGroup {
		
		public event MouseEventHandler HandleMouseDown;
		
		public AbstractHandleGroup(MoonlightController controller, UIElement child)
		{
			if (child == null)
				throw new ArgumentNullException("child");
			
			if (controller == null)
				throw new ArgumentNullException("controller");
			
			Child = child;
			Controller = controller;
			handles = new List<IHandle>();
		}
		
		public UIElement Child {
			get { return child; }
			protected set { child = value; }
		}
		
		public IEnumerable<IHandle> HandlesEnumerator {
			get {
				foreach (IHandle handle in handles)
					yield return handle;
			}
		}
		
		public void AddToCanvas()
		{
			foreach (IHandle handle in Handles) {
				Controller.Canvas.Children.Add(handle as UIElement);
				handle.MouseLeftButtonDown += OnHandleMouseDown;
			}
		}
		
		public void RemoveFromCanvas()
		{
			foreach (IHandle handle in Handles) {
				Controller.Canvas.Children.Remove(handle as UIElement);
				handle.MouseLeftButtonDown -= OnHandleMouseDown;
			}
		}
		
		protected void UpdateHandles()
		{
			foreach(IHandle handle in Handles)
				handle.Update();
		}
		
		protected void AddHandle(IHandle handle)
		{
			Handles.Add(handle);
		}
		
		protected void OnHandleMouseDown(object sender, MouseEventArgs args)
		{
			if (HandleMouseDown != null)
				HandleMouseDown(sender, args);
		}
		
		protected List<IHandle> Handles
		{
			get { return handles; }
			set { handles = value; }
		}

		protected MoonlightController Controller {
			get { return controller; }
			set { controller = value; }
		}
		
		private UIElement child;
		private MoonlightController controller;
		List<IHandle> handles;
	}
}
