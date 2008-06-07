// Selection.cs
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

using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using LunarEclipse.Model;

namespace LunarEclipse.Controller {	
	
	public class StandardSelection: ISelection {
		
		public StandardSelection(MoonlightController controller)
		{
			Controller = controller;
			HandleGroups = new Dictionary<UIElement, IHandleGroup>();
		}
		
		public void Add(UIElement element)
		{
			if (!HandleGroups.ContainsKey(element) ) {
				IHandleGroup group = FindHandleGroup(element);
				HandleGroups.Add(element, group);
				if (group != null)
					group.AddToCanvas();
			}
		}
		
		public void Remove(UIElement element)
		{
			IHandleGroup group = HandleGroups[element];
			HandleGroups.Remove(element);
			group.RemoveFromCanvas();
		}
		
		public void Clear()
		{
			foreach (IHandleGroup group in HandleGroups.Values)
				group.RemoveFromCanvas();
			
			HandleGroups.Clear();
		}
		
		protected Dictionary<UIElement, IHandleGroup> HandleGroups {
			get { return handle_groups; }
			set { handle_groups = value; }
		}

		protected MoonlightController Controller {
			get { return controller; }
			set { controller = value; }
		}
				
		private IHandleGroup FindHandleGroup(UIElement element)
		{
			if (element is Line)
				return new LineHandleGroup(Controller, element);
			return null;
		}
		
		private MoonlightController controller;
		private Dictionary<UIElement, IHandleGroup> handle_groups;		
	}
}
