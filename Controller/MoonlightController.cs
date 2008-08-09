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
using System.IO;
using LunarEclipse.Serialization;
using LunarEclipse.View;
using LunarEclipse.Model;
using LunarEclipse.Controls;
using Gtk.Moonlight;

namespace LunarEclipse.Controller
{
    public class MoonlightController
    {
		public event EventHandler ZoomChanged;
		
		bool active;
        private GtkSilver moonlight;
		private PropertyManager propertyManager;
        private Serializer serializer;
		private AnimationTimeline timeline;
		private UndoEngine undo;
		private ITool current_tool;
		private ISelection selection;
		private double current_scale = 1.0;
        
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

        public double CurrentScale {
        	get { return current_scale; }
        	set { current_scale = value; }
        }
    	
        public MoonlightController(GtkSilver moonlight, AnimationTimeline timeline)
        {
			this.timeline = timeline;
            this.moonlight = moonlight;
			//this.properties = properties;
			
			serializer = new Serializer();
            undo = new UndoEngine();
			Selection = new StandardSelection(this);
			CurrentTool = new SelectionTool(this);
			propertyManager = new PropertyManager(this);
			
			SetupCanvas();
        }
        
		
        public void Clear()
		{
			Selection.Clear();			
			moonlight.Canvas.Children.Clear();
			undo.Clear();
		}
		
		private void SetupCanvas()
		{
			moonlight.Canvas.MouseLeftButtonDown += MouseLeftDown;
            moonlight.Canvas.MouseMove += MouseMove;
            moonlight.Canvas.MouseLeftButtonUp += MouseLeftUp;
		}
        
        private void MouseLeftDown(object sender, MouseEventArgs e)
        {
            this.Canvas.CaptureMouse();
            CurrentTool.MouseDown(e);
        }
        
        private void MouseMove(object sender, MouseEventArgs e)
        {
			CurrentTool.MouseMove(e);
        }
        
        private void MouseLeftUp(object sender, MouseEventArgs e)
        {
			CurrentTool.MouseUp(e);
            this.Canvas.ReleaseMouseCapture();
        }
		
		
		public string SerializeCanvas()
        {
			return serializer.Serialize(Canvas);
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
		
		public void Zoom(int zoom, Point center)
		{
			double scale = 	zoom / 100.0;
			TransformGroup transforms = new TransformGroup();
			ScaleTransform scaleTransform = new ScaleTransform();
			scaleTransform.ScaleX = scale;
			scaleTransform.ScaleY = scale;
			current_scale = scale;
			transforms.Children.Add(scaleTransform);
			Canvas.SetValue(UIElement.RenderTransformOriginProperty, center);
			Canvas.SetValue(UIElement.RenderTransformProperty, transforms);			
			
			moonlight.Resize((int)(Canvas.Width * current_scale), (int)(Canvas.Height * current_scale));
		}
		
		public void Zoom(int zoom)
		{
			Zoom(zoom, new Point(0.0, 0.0));
			
			if (ZoomChanged != null)
				ZoomChanged(this, new EventArgs());
		}
		
		public double ZoomCorrection(double value)
		{
			return value / current_scale;
		}
		
		public Point ZoomCorrection(Point point)
		{
			return new Point(ZoomCorrection(point.X), ZoomCorrection(point.Y));
		}
		
		public void SaveToFile(string file)
		{
			CurrentTool.Deactivate();
			TextWriter writer = new StreamWriter(file);
			writer.Write(serializer.Serialize(Canvas));
			writer.Close();
			CurrentTool.Activate();
		}
		
		public void LoadFromFile(string file)
		{
			CurrentTool.Deactivate();
			Selection.Clear();
			moonlight.LoadFile(file);
			SetupCanvas();
			CurrentTool.Activate();
		}
		
		public void LoadXaml(string xaml)
		{
			CurrentTool.Deactivate();
			Selection.Clear();
			Canvas canvas;
			moonlight.LoadXaml(xaml, out canvas);
			SetupCanvas();
			CurrentTool.Activate();
		}
	}
}
