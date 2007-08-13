// /home/alan/Projects/LunarEclipse/LunarEclipse/MainWindow.cs created with MonoDevelop
// User: alan at 4:48 PM 6/15/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using Gtk;

using LunarEclipse.Controls;
using LunarEclipse.Controller;
using LunarEclipse.Model;

namespace LunarEclipse.View
{
    public partial class MainWindow: Gtk.Window
    {
        Box mainContainer;
        MoonlightController controller;
        TextBuffer buffer = new Gtk.TextBuffer(new TextTagTable());
		IPropertyGroup properties;
        Button undo;
        Button redo;
        Notebook book;
		AnimationTimeline timeline;
		Box animationWidgets;
		
		public MainWindow (): base (Gtk.WindowType.Toplevel)
    	{
    		Build ();
    		
            Canvas c = new Canvas();
            c.Width = 800;
            c.Height = 600;

    		GtkSilver moonlight = new GtkSilver(800, 600);
            moonlight.Attach(c);
			moonlight.Show ();
			timeline = new AnimationTimeline(800, 70);
    		mainContainer = new HBox ();
			Widget toolbox = InitialiseWidgets ();
			toolbox.ShowAll ();
			mainContainer.Add (toolbox);
			animationWidgets = InitialiseAnimationWidgets();
			mainContainer.Add(animationWidgets);
			VBox vbox = new VBox();
			vbox.PackStart(moonlight);
			vbox.PackEnd(timeline);
			vbox.ShowAll();
    		mainContainer.Add (vbox);
			
			Widget propertyPane = new Properties ();
			propertyPane.Show ();
    		mainContainer.Add (propertyPane);
            
			properties = (IPropertyGroup) propertyPane;
			
            TextView view = new Gtk.TextView(buffer);
			view.Show ();
			
            Gtk.ScrolledWindow scrolled = new ScrolledWindow();
            scrolled.Add(view);
            
			book = new Notebook();
			Widget label = new Label("Canvas");
			mainContainer.Show ();
			label.Show ();
            book.AppendPage (mainContainer, label);
			label = new Label ("Xaml");
			scrolled.Show ();
			label.Show ();
            book.AppendPage (scrolled, label);
			book.Show ();
			Add (book);
			
            controller = new MoonlightController (moonlight, timeline, properties);
            HookEvents(true);
            Show ();
    	}

		private Box InitialiseAnimationWidgets()
		{
			Box box = new VBox();
			
			Button b = new Button("Play");
			b.Clicked += delegate (object sender, EventArgs e) {
				if(b.Label == "Play")
				{
					this.controller.StoryboardManager.Seek(TimeSpan.Zero);
					Console.WriteLine(controller.SerializeCanvas());
					this.controller.StoryboardManager.Play();
				}
				else if(b.Label == "Stop");
				{
					controller.StoryboardManager.Stop();
				}

				b.Label = b.Label == "Play" ? "Stop" : "Play";
			};
			box.Add(b);
			box.ShowAll();
			box.Visible = false;
			return box;
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
			Button b;
			
			b = new Button("Selection");
			b.Clicked += delegate {
                controller.Current = new SelectionDraw(this.controller);
				Console.WriteLine("Draw is" + controller.Current.GetType().Name.ToString());
			};
			widgets.Add(b);
			
			b = new Button("Circle");
			b.Clicked += delegate {
				controller.Current = new CircleDraw();
				Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
			widgets.Add(b);
			
            b = new Button("Ellipse");
    	    b.Clicked += delegate {
    	        controller.Current = new EllipseDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
    	    widgets.Add(b);
            
    	    b = new Button("Line");
    	    b.Clicked += delegate {
    	        controller.Current = new LineDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
    	    widgets.Add(b);
    	    
    	    b = new Button("Pen");
    	    b.Clicked += delegate {
    	        controller.Current = new PenDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
    	    widgets.Add(b);
    	    
    	    b = new Button("Rectangle");
    	    b.Clicked += delegate {
    	        controller.Current = new RectangleDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
    	    widgets.Add(b);
    	    
    	    b = new Button("Square");
    	    b.Clicked += delegate {
    	        controller.Current = new SquareDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };    	    
    	    widgets.Add(b);
			
			b = new Button("Record");
			b.Clicked += delegate (object sender, EventArgs e) {
				controller.StoryboardManager.Recording = !controller.StoryboardManager.Recording;
				animationWidgets.Visible = controller.StoryboardManager.Recording;
				((Button)sender).Label = animationWidgets.Visible ? "Stop Recording" : "Record";
			};
			widgets.Add(b);
			
            undo = new Button("Undo");
            undo.Sensitive = false;
            undo.Clicked += delegate {
                controller.Undo(); 
            };
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
