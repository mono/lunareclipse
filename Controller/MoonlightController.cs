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
            set { this.current = value; } 
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
            // Horrible hack to make the screen redraw
            moonlight.Canvas.Opacity = moonlight.Canvas.Opacity;
        }
        
        bool active = false;
        private void MouseLeftDown(object sender, MouseEventArgs e)
        {
            active = true;
            Console.WriteLine("MouseDown");
            current = current.Clone();
            Point position = e.GetPosition(moonlight.Canvas);
            Console.WriteLine("Mouse at: " + position.ToString());
            moonlight.Canvas.Children.Add(current.Element);
            
            undo.PushUndo(new UndoAddObject(moonlight.Canvas.Children, current.Element));
            
            if(current is LineDraw)
            {
                Line l = (Line)current.Element;
                l.X1 = position.X;
                l.Y1 = position.Y;
            }
            else
            {
                current.Element.SetValue<double>(Canvas.TopProperty, position.Y);
                current.Element.SetValue<double>(Canvas.LeftProperty, position.X);
                current.Start = position;
                current.Resize(position);
            }

        }
        
        private void MouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse at: " + e.GetPosition(moonlight.Canvas).ToString());
            if(!active)
                return;
                
            Console.WriteLine("MouseMove");
            current.Resize(e.GetPosition(moonlight.Canvas));

        }
        
        private void MouseLeftUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse at: " + e.GetPosition(moonlight.Canvas).ToString());
            if(!active)
                return;
                
            Console.WriteLine("MouseUp");
            current.Resize(e.GetPosition(moonlight.Canvas));
            active = false;
        }
        

        public string SerializeCanvas()
        {
            Serializer s = new Serializer();
            return s.Serialize(this.moonlight.Canvas);
        }
    }
}
