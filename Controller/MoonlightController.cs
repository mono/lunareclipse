//
// MoonlightController.cs
//
// Authors:
//   Alan McGovern alan.mcgovern@gmail.com
//
// Copyright (C) 2007 Alan McGovern
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Input;
using System.Xml;
using System.Collections.Generic;
using LunarEclipse.Serialization;
using LunarEclipse.View;
using LunarEclipse.Model;
using LunarEclipse.Controls;
using Gtk.Moonlight;

namespace LunarEclipse.Controller
{
    public class MoonlightController
    {
#region Member Variables
		
		//IPropertyGroup properties;
		bool active;
        private GtkSilver moonlight;
		private PropertyManager propertyManager;
        private Serializer serializer;
		private StoryboardManager storyboardManager;
		private AnimationTimeline timeline;
		private UndoEngine undo;
		private ITool current_tool;
		private ISelection selection;

#endregion Member Variables
        
		internal Canvas Canvas
		{
			get { return this.moonlight.Canvas; }
		}
		
		internal GtkSilver GtkSilver {
			get { return this.moonlight; }
		}
        
		public ITool CurrentTool
		{
			get { return current_tool; }
			set {
				if (current_tool != null)
					current_tool.Deactivate();
				current_tool = value;
				if (current_tool != null)
					current_tool.Activate();
			}
		}
		
		public PropertyManager PropertyManager
		{
			get { return propertyManager; }
		}
		
		public StoryboardManager StoryboardManager
		{
			get { return storyboardManager; }
		}
		
		public AnimationTimeline Timeline
		{
			get { return timeline; }
		}
        
        public UndoEngine UndoEngine
        {
            get { return undo; }
        }

        public ISelection Selection {
        	get { return selection; }
			protected set { selection = value; }
        }

    	
        public MoonlightController(GtkSilver moonlight, AnimationTimeline timeline)
        {
			this.timeline = timeline;
            this.moonlight = moonlight;
			//this.properties = properties;
			
            moonlight.Canvas.MouseLeftButtonDown += new MouseEventHandler(MouseLeftDown);
            moonlight.Canvas.MouseMove += new MouseEventHandler(MouseMove);
            moonlight.Canvas.MouseLeftButtonUp += new MouseEventHandler(MouseLeftUp);
			serializer = new Serializer();
			storyboardManager = new StoryboardManager(this);
			storyboardManager.Add(new Storyboard());
            undo = new UndoEngine();
			Selection = new StandardSelection(this);
			CurrentTool = new SelectionTool(this);
			propertyManager = new PropertyManager(this);
        }
        
		
        public void Clear()
		{
			Selection.Clear();			
			moonlight.Canvas.Children.Clear();
			moonlight.Canvas.Resources.Clear();
			moonlight.Canvas.Triggers.Clear();
			undo.Clear();
		}
        
        
        private void MouseLeftDown(object sender, MouseEventArgs e)
        {
            this.Canvas.CaptureMouse();
            //current.DrawStart(this.moonlight.Canvas, e);
			CurrentTool.MouseDown(e);
        }
        
        private void MouseMove(object sender, MouseEventArgs e)
        {
            //current.MouseMove(e);
			CurrentTool.MouseMove(e);
        }
        
        private void MouseLeftUp(object sender, MouseEventArgs e)
        {
			CurrentTool.MouseUp(e);
            this.Canvas.ReleaseMouseCapture();
        }
		
		
		public string SerializeCanvas()
        {
			return serializer.Serialize(this.moonlight.Canvas);
		}
		
		public void Undo()
        {
            undo.Undo();
			Selection.Update();
        }
		
		public void Redo()
		{
			undo.Redo();
			Selection.Update();
		}
	}
}
