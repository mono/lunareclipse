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
                this.current = value;
            }
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
    	
        public MoonlightController(GtkSilver moonlight, IPropertyGroup properties)
        {
            this.moonlight = moonlight;
			this.properties = properties;
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
            if(current == null || active)
                return;
            
            active = true;
            current.DrawStart(this.moonlight.Canvas, e);
            
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
            if(!active)
                return;

            current.DrawEnd(e);
            active = false;
			
			if (current is SelectionDraw) {
				SelectionDraw selection = (SelectionDraw) current;
				if (selection.SelectedObjects.Count == 1)
                    foreach(KeyValuePair<Visual, SelectedBorder> keypair in selection.SelectedObjects)
                    {
                        properties.DependencyObject = keypair.Key;
                        Console.WriteLine("Selected: " + keypair.Key.ToString());
                    }
			}
                    
                    
            Console.WriteLine("Children: " + this.Canvas.Children.Count.ToString());
        }
        

        public string SerializeCanvas()
        {
            if(Current != null)
                Current.Cleanup();
              
            return serializer.Serialize(this.moonlight.Canvas);
        }
    }
}
