// MainWindow2.cs
//
// Author:
//    Manuel Cerón <ceronman@unicauca.edu.co>
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
using LunarEclipse.Controller;
using LunarEclipse.Model;
using System.Windows.Controls;
using System.Windows.Media;
using Gtk;
using Gtk.Moonlight;
using Gettext = Mono.Unix.Catalog;

namespace LunarEclipse.View
{
	public partial class MainWindow : Gtk.Window
	{
		
		public MainWindow() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
			controller = new MoonlightController(moonlightwidget.Silver);
			
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
			if (args.PageNum == 1)
				xaml_textview.Buffer.Text = controller.SerializeCanvas();
			else if (args.PageNum == 0)
				controller.LoadXaml(xaml_textview.Buffer.Text);
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

		protected virtual void OnUndoActionActivated (object sender, System.EventArgs e)
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

		protected virtual void OnDeleteActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.DeleteFromCanvas();
		}

		protected virtual void OnAboutActionActivated (object sender, System.EventArgs e)
		{
			AboutLunarEclipse dialog = new AboutLunarEclipse();
			dialog.Run();
			dialog.Destroy();
		}

		protected virtual void OnDebug1Activated (object sender, System.EventArgs e)
		{
			controller.Zoom(200);
		}
		
		protected virtual void OnDebug2Activated (object sender, System.EventArgs e)
		{
			controller.Zoom(100);
		}

		protected virtual void OnCutActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.Cut();
		}

		protected virtual void OnCopyActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.Copy();
		}

		protected virtual void OnPasteActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.Paste();
		}

		protected virtual void OnCloneActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.Clone();
		}

		protected virtual void OnSelectAllActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.SelectAll();
		}

		protected virtual void OnClearSelectionActionActivated (object sender, System.EventArgs e)
		{
			controller.Selection.Clear();
		}

		protected virtual void OnZoomScaleValueChanged (object sender, System.EventArgs e)
		{
			controller.Zoom((int)zoomScale.Value);
		}

		protected virtual void OnSaveActionActivated (object sender, System.EventArgs e)
		{
			FileChooserDialog dialog = new FileChooserDialog(Gettext.GetString("Save..."),
			                                                 this,
			                                                 FileChooserAction.Save,
			                                                 Stock.Cancel, ResponseType.Cancel,
			                                                 Stock.Ok, ResponseType.Accept);
			
			dialog.DoOverwriteConfirmation = true;
			
			FileFilter filter = new FileFilter();
			filter.Name = Gettext.GetString("XAML Silverlight 1.0");
			filter.AddMimeType("application/xaml+xml");
			filter.AddPattern("*.xaml");
			
			dialog.AddFilter(filter);
			
			if (dialog.Run() == (int)ResponseType.Accept) {
				controller.SaveToFile(dialog.Filename);
			}
			dialog.Destroy();
		}

		protected virtual void OnOpenActionActivated (object sender, System.EventArgs e)
		{
			FileChooserDialog dialog = new FileChooserDialog(Gettext.GetString("Open File..."),
			                                                 this,
			                                                 FileChooserAction.Open,
			                                                 Stock.Cancel, ResponseType.Cancel,
			                                                 Stock.Ok, ResponseType.Accept);
			
			FileFilter filter = new FileFilter();
			filter.Name = Gettext.GetString("XAML Silverlight 1.0");
			filter.AddMimeType("application/xaml+xml");
			filter.AddPattern("*.xaml");
			dialog.AddFilter(filter);
			
			if (dialog.Run() == (int)ResponseType.Accept) {
				controller.LoadFromFile(dialog.Filename);
			}
			
			dialog.Destroy();
		}

		private MoonlightController controller;
	}
}
