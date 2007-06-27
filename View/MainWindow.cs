// /home/alan/Projects/LunarEclipse/LunarEclipse/MainWindow.cs created with MonoDevelop
// User: alan at 4:48 PM 6/15/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Text;
using System.IO;

using Gtk;

using LunarEclipse.Controller;
using LunarEclipse.Model;

namespace LunarEclipse.View
{
    public partial class MainWindow: Gtk.Window
    {
        Box mainContainer;
        MoonlightController controller;
        TextBuffer buffer = new Gtk.TextBuffer(new TextTagTable());
        Button undo;
        Button redo;
        Notebook book;
        public MainWindow (): base (Gtk.WindowType.Toplevel)
    	{
    		Build ();
    		
            Canvas c = new Canvas();
            c.Width = 640;
            c.Height = 480;
            
    		GtkSilver moonlight = new GtkSilver(640, 480);
            moonlight.Attach(c);

    		mainContainer = new HBox();
    		mainContainer.Add(moonlight);
    		mainContainer.Add(InitialiseWidgets());
            
            TextView view = new Gtk.TextView(buffer);
            Gtk.ScrolledWindow scrolled = new ScrolledWindow();
            scrolled.Add(view);
            book = new Notebook();
    		Add(book);
            book.AppendPage(mainContainer, new Label("Canvas"));
            book.AppendPage(scrolled, new Label("Xaml"));
         
            controller = new MoonlightController(moonlight);
            moonlight.Load("~/Desktop/moonlight/moon/test/tester.xaml");
            HookEvents(true);
            ShowAll();
    	}


        private void HookEvents(bool hook)
        {
            if(hook)
            {
                book.SwitchPage += 
                    delegate (object sender, Gtk.SwitchPageArgs args) {
                    if(args.PageNum == 1)
                        buffer.Text = this.controller.SerializeCanvas();
                };
                
                controller.UndoEngine.UndoAdded += 
                    delegate { this.undo.Sensitive = true; };
                
                controller.UndoEngine.RedoAdded += 
                    delegate { this.redo.Sensitive = true; };
                
                controller.UndoEngine.RedoRemoved += 
                    delegate (object sender, EventArgs e) {
                    this.redo.Sensitive = ((UndoEngine)sender).RedoCount != 0; 
                };
                
                controller.UndoEngine.UndoRemoved += 
                    delegate (object sender, EventArgs e) {
                    this.undo.Sensitive = ((UndoEngine)sender).UndoCount != 0;
                };

            }
            else
            {
                //FIXME Unhook events ;)
            }
        }
        
    	private VBox InitialiseWidgets()
    	{
    	    VBox widgets = new VBox();
    	    
    	    Button b = new Button("Line");
    	    b.Clicked += delegate {
    	        controller.Current = new LineDraw(new Point());
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
    	    widgets.Add(b);
    	    
    	    b = new Button("Pen");
    	    b.Clicked += delegate {
    	        controller.Current = new PenDraw(new Point());
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
    	    widgets.Add(b);
    	    
    	    b = new Button("Rectangle");
    	    b.Clicked += delegate {
    	        controller.Current = new RectangleDraw(new Point());
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
    	    widgets.Add(b);
    	    
    	    b = new Button("Square");
    	    b.Clicked += delegate {
    	        controller.Current = new SquareDraw(new Point());
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };    	    
    	    widgets.Add(b);
    	    
    	    b = new Button("Circle");
    	    b.Clicked += delegate {
    	        controller.Current = new CircleDraw(new Point());
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
    	    widgets.Add(b);
    	    
    	    b = new Button("Ellipse");
    	    b.Clicked += delegate {
    	        controller.Current = new EllipseDraw(new Point());
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
    	    widgets.Add(b);
            
            undo = new Button("Undo");
            undo.Sensitive = false;
            undo.Clicked += delegate { controller.UndoEngine.Undo(); };
            widgets.Add(undo);
            
            redo = new Button("Redo");
            redo.Sensitive = false;
            redo.Clicked += delegate { controller.UndoEngine.Redo(); };
            widgets.Add(redo);
    	    
    	    b = new Button("Clear");
    	    b.Clicked += delegate {
    	        controller.Clear();
    	    };
    	    widgets.Add(b);
            
            return widgets;
        }
    	
    	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
    	{
    		Application.Quit ();
    		a.RetVal = true;
    	}
    }
}
