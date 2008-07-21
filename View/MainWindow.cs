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
using System.Windows;
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
			
			propertypanel.Controller = controller;
			SetupUndoButtons(controller.UndoEngine);
			SetupFiguresButtons(controller.Selection);
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
			controller.CurrentTool = new PathCreationTool(controller);
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

		protected virtual void OnPenToolActionActivated (object sender, System.EventArgs e)
		{
			controller.CurrentTool = new PenCreationTool(controller);
		}

		protected virtual void OnDeshacerActionActivated (object sender, System.EventArgs e)
		{
			controller.Undo();
		}
		
		private void SetupUndoButtons(UndoEngine e)
		{
			e.UndoAdded += delegate {
				UndoAction.Sensitive = true;
			};
				
			e.RedoAdded += delegate {
				RedoAction.Sensitive = true;
			};
				
			e.RedoRemoved += delegate (object sender, EventArgs args) {
				RedoAction.Sensitive = ((UndoEngine)sender).RedoCount != 0; 
			};
				
			e.UndoRemoved += delegate (object sender, EventArgs args) {
				UndoAction.Sensitive = ((UndoEngine)sender).UndoCount != 0;
			};
		}
		
		private void SetupFiguresButtons(ISelection selection)
		{
			selection.SelectionChanged += delegate {
				bool selecting = selection.Count > 0;
				BringForwardsAction.Sensitive = selecting;
				BringToFrontAction.Sensitive = selecting;
				SendBackwarsAction.Sensitive = selecting;
				SendToBackAction.Sensitive = selecting;
			};
		}

		protected virtual void OnRedoActionActivated (object sender, System.EventArgs e)
		{
			controller.Redo();
		}

		protected virtual void OnBringToFrontActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.BringToFront();
		}

		protected virtual void OnBringForwardsActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.BringForwards();
		}

		protected virtual void OnSendToBackActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.SendToBack();
		}

		protected virtual void OnSendBackwarsActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.SendBackwards();
		}

		protected virtual void OnLeftActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.AlignLeft();
		}

		protected virtual void OnRightActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.AlignRight();
		}

		protected virtual void OnTopActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.AlignTop();
		}

		protected virtual void OnBottomActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.AlignBottom();
		}

		protected virtual void OnHorizontalCenterActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.AlignHorizontalCenter();
		}

		protected virtual void OnVerticalCenterActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.AlignVerticalCenter();
		}

		protected virtual void OnBorrarActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.DeleteFromCanvas();
		}

		protected virtual void OnAcercaDeActionActivated (object sender, System.EventArgs e)
		{
			AboutLunarEclipse dialog = new AboutLunarEclipse();
			dialog.Run();
			dialog.Destroy();
		}

		protected virtual void OnDebug1Activated (object sender, System.EventArgs e)
		{
			controller.Selection.CloneMainElement();
		}

		private MoonlightController controller;
	}
}
