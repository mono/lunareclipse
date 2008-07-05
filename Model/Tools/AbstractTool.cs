// AbstractTool.cs
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
using System.Windows.Input;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public abstract class AbstractTool: ITool {

		public AbstractTool(MoonlightController controller)
		{
			Controller = controller;
		}

		public virtual void MouseDown (MouseEventArgs ev)
		{
			Dragging = true;
		}
		
		public virtual void MouseUp (MouseEventArgs ev)
		{
			Dragging = false;
		}
		
		public virtual void MouseMove (MouseEventArgs ev)
		{
		}
		
		public virtual void MouseDrag (MouseEventArgs ev)
		{
		}
		
		public virtual void KeyDown (KeyEventArgs ev)
		{
		}
		
		public virtual void KeyUp (KeyEventArgs ev)
		{
		}
		
		public virtual void Activate ()
		{
		}
		
		public virtual void Deactivate ()
		{
		}

		public bool Activated { 
			get { return activated; }
			protected set { activated = value; }
		}
		
		public MoonlightController Controller {
			get { return controller; }
			protected set { controller = value; }
		}

		protected bool Dragging {
			get { return dragging; }
			set { dragging = value; }
		}
		
		protected virtual void PushUndo()
		{
		}
		
		protected bool CheckDoubleClick()
		{
			bool result = false;
			// Trick to detect double click
			// http://hackingsilverlight.blogspot.com/2008/02/silverlight-20-double-click-support.html
			if ((DateTime.Now.Ticks - last_ticks) < 2310000) {
				result = true;
			}
			last_ticks = DateTime.Now.Ticks;
			return result;
		}
	
		private bool activated;
		private MoonlightController controller;
		private bool dragging = false;
		
		// trick to support double click;
		private long last_ticks = 0;
	}
}
