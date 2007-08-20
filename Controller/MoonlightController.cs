// /home/alan/Projects/LunarEclipse/LunarEclipse/Controller/MoonlightController.cs created with MonoDevelop
// User: alan at 10:56 AM 6/18/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
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

namespace LunarEclipse.Controller
{
    public class MoonlightController
    {
#region Events
		
		public event EventHandler<DrawChangeEventArgs> BeforeDrawChanged;
		public event EventHandler<DrawChangeEventArgs> DrawChanged;
		
#endregion Events
		
		
#region Member Variables
		
		//IPropertyGroup properties;
		bool active;
		private DrawBase current;
        private GtkSilver moonlight;
        private Serializer serializer;
		private StoryboardManager storyboardManager;
		private AnimationTimeline timeline;
		private UndoEngine undo;

#endregion Member Variables
        
		internal Canvas Canvas
		{
			get { return this.moonlight.Canvas; }
		}
        
    	public DrawBase Current
        {
            get { return this.current; }
            set 
            { 
				Toolbox.RaiseEvent<DrawChangeEventArgs>(BeforeDrawChanged, this, new DrawChangeEventArgs(current));
				current = value;
				Toolbox.RaiseEvent<DrawChangeEventArgs>(DrawChanged, this, new DrawChangeEventArgs(current));
            }
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

    	
        public MoonlightController(GtkSilver moonlight, AnimationTimeline timeline)
        {
			this.timeline = timeline;
            this.moonlight = moonlight;
			//this.properties = properties;
			
			BeforeDrawChanged += delegate {
				if(Current != null) Current.Cleanup();
			};
			
			DrawChanged += delegate {
				if(Current != null) Current.Prepare();
			};
			
            moonlight.Canvas.MouseLeftButtonDown += new MouseEventHandler(MouseLeftDown);
            moonlight.Canvas.MouseMove += new MouseEventHandler(MouseMove);
            moonlight.Canvas.MouseLeftButtonUp += new MouseEventHandler(MouseLeftUp);
            serializer = new Serializer();
			storyboardManager = new StoryboardManager(this);
			storyboardManager.Add(new Storyboard());
            undo = new UndoEngine();
        }
        
		
        public void Clear()
		{
			if(Current != null)
				Current.Cleanup();
			
			moonlight.Canvas.Children.Clear();
			moonlight.Canvas.Resources.Clear();
			moonlight.Canvas.Triggers.Clear();
			undo.Clear();
		}
        
        
        private void MouseLeftDown(object sender, MouseEventArgs e)
        {
           Console.WriteLine("Mouse down");
            if(current == null || active)
                return;
            
            active = true;
            this.Canvas.CaptureMouse();
            current.DrawStart(this.moonlight.Canvas, e);
            
			Console.WriteLine("Current: {0}", current.ToString());
            if(current.CanUndo)
                undo.PushUndo(new UndoAddObject(moonlight.Canvas.Children, current.Element));
        }
        
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if(!active)
                return;

            current.MouseMove(e);
        }
        
        private void MouseLeftUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse up");
            if(!active)
                return;

            current.DrawEnd(e);
            active = false;
            this.Canvas.ReleaseMouseCapture();
            
			if (!(current is SelectionDraw))
                return;
            
			SelectionDraw selection = (SelectionDraw) current;
			if (selection.SelectedObjects.Count == 1)
            {
               foreach(KeyValuePair<Visual, SelectedBorder> keypair in selection.SelectedObjects)
               {
                   //properties.SelectedObject = keypair.Value;
                   Console.WriteLine("Selected: " + keypair.Key.ToString());
               }
            }
            else
            {
                //properties.SelectedObject = null;
            }
        }
		
		
        public string SerializeCanvas()
        {
			////if(Current != null)
			//	Current.Cleanup();
			
			return serializer.Serialize(this.moonlight.Canvas);
        }
		
		public void Undo()
        {
            if(Current != null)
				Current.Cleanup();
            
            undo.Undo();
        }
	}
}
