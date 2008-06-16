// MainWindow2.cs
//
// Author:
//    Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 [copyright holders]
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
using LunarEclipse.Controller;
using LunarEclipse.Controls;
using LunarEclipse.Model;
using Gtk.Moonlight;
using System.Windows.Controls;
using System.Windows.Media;

namespace LunarEclipse.View
{
	public partial class MainWindow : Gtk.Window
	{
		
		public MainWindow() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
			AnimationTimeline timeline = new AnimationTimeline(800, 70);
			timeline.SizeAllocated += delegate {
				timeline.UpdateSize();
			};
			//vbox2.PackEnd(timeline, false, false, 0);
			controller = new MoonlightController(moonlightwidget.Silver, timeline);
		}

		protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			Gtk.Application.Quit();
		}

		protected virtual void OnSelectionToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new SelectionTool(controller);
		}

		protected virtual void OnRectangleToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new RectangleCreationTool(controller);
		}

		protected virtual void OnSquareToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new SquareCreationTool(controller);
		}

		protected virtual void OnCircleToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new CircleCreationTool(controller);
		}

		protected virtual void OnEllipseToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new EllipseCreationTool(controller);
		}

		protected virtual void OnPathToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new PenCreationTool(controller);
		}

		protected virtual void OnLineToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new LineCreationTool(controller);
		}

		protected virtual void OnLimpiarActionActivated (object sender, System.EventArgs e)
		{
			controller.Clear();
		}

		protected virtual void OnNotebookSwitchPage (object o, Gtk.SwitchPageArgs args)
		{
			xaml_textview.Buffer.Text = controller.SerializeCanvas();
		}

		protected virtual void OnPolylineToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new PolyLineCreationTool(controller);
		}

		protected virtual void OnTextToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new TextBlockCreationTool(controller);
		}

		protected virtual void OnImageToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new ImageCreationTool(controller);
		}
		
		private MoonlightController controller;
	}
}
