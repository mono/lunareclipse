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
using LunarEclipse.View;
using LunarEclipse.Model;
using System.Xml;
using System.Collections.Generic;


namespace LunarEclipse.Controller
{
    public class MoonlightController
    {
		IPropertyGroup properties;
        private UndoEngine undo;
        private GtkSilver moonlight;
    	private DrawBase current;
        private Serializer serializer;
		private AnimationTimeline timeline;
        
    	internal Canvas Canvas
        {
            get { return this.moonlight.Canvas; }
        }
        
    	public DrawBase Current
        {
            get { return this.current; }
            set 
            { 
                if(current != null) 
                    current.Cleanup();
				
                current = value;
				
				if(current != null)
					current.Prepare();
            }
        }
		
		public AnimationTimeline Timeline
		{
			get { return timeline; }
		}
        
        public UndoEngine UndoEngine
        {
            get { return undo; }
        }
        
        public void Undo()
        {
            if(Current != null)
                Current.Cleanup();
            
            undo.Undo();
        }
    	
        public MoonlightController(GtkSilver moonlight, AnimationTimeline timeline, IPropertyGroup properties)
        {
            this.moonlight = moonlight;
			this.properties = properties;
			this.timeline = timeline;
            moonlight.Canvas.MouseLeftButtonDown += new MouseEventHandler(MouseLeftDown);
            moonlight.Canvas.MouseMove += new MouseEventHandler(MouseMove);
            moonlight.Canvas.MouseLeftButtonUp += new MouseEventHandler(MouseLeftUp);
            serializer = new Serializer();
            undo = new UndoEngine();
        }
        
        public void Clear()
        {
            if(Current != null)
                Current.Cleanup();
            moonlight.Canvas.Children.Clear();
            undo.Clear();
        }
        
        bool active = false;
        private void MouseLeftDown(object sender, MouseEventArgs e)
        {
           Console.WriteLine("Mouse down");
            if(current == null || active)
                return;
            
            active = true;
            try{this.Canvas.CaptureMouse();}catch{}
            current.DrawStart(this.moonlight.Canvas, e);
            
			Console.WriteLine("Current: {0}", current.ToString());
            if(current.CanUndo)
                undo.PushUndo(new UndoAddObject(moonlight.Canvas.Children, current.Element));
        }
        
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if(!active)
                return;

            current.Resize(e);
        }
        
        private void MouseLeftUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse up");
            if(!active)
                return;

            current.DrawEnd(e);
            active = false;
            try{this.Canvas.ReleaseMouseCapture();}catch{}
            
			if (!(current is SelectionDraw))
                return;
            
			SelectionDraw selection = (SelectionDraw) current;
			if (selection.SelectedObjects.Count == 1)
            {
               foreach(KeyValuePair<Visual, SelectedBorder> keypair in selection.SelectedObjects)
               {
                   properties.SelectedObject = keypair.Value;
                   Console.WriteLine("Selected: " + keypair.Key.ToString());
               }
            }
            else
            {
                properties.SelectedObject = null;
            }
        }

        public string SerializeCanvas()
        {
			Current = null;
			return serializer.Serialize(this.moonlight.Canvas);
        }
    }
}
