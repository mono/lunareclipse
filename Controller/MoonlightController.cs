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

namespace LunarEclipse.Controller
{
    public class MoonlightController
    {
        private UndoEngine undo;
        private GtkSilver moonlight;
    	private DrawBase current;
    	private XmlDocument xaml;
    	
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
    	
        public MoonlightController(GtkSilver moonlight)
        {
            this.moonlight = moonlight;
            moonlight.Canvas.MouseLeftButtonDown += new MouseEventHandler(MouseLeftDown);
            moonlight.Canvas.MouseMove += new MouseEventHandler(MouseMove);
            moonlight.Canvas.MouseLeftButtonUp += new MouseEventHandler(MouseLeftUp);
            undo = new UndoEngine();
        }
        
        public void Clear()
        {
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
        }
        

        public string SerializeCanvas()
        {
            Serializer s = new Serializer();
            return s.Serialize(this.moonlight.Canvas);
        }
    }
}
