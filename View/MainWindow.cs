// /home/alan/Projects/LunarEclipse/LunarEclipse/MainWindow.cs created with MonoDevelop
// User: alan at 4:48 PM 6/15/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        Button undo;
        Button redo;
        Notebook book;
		AnimationTimeline timeline;
		Box animationWidgets;
		VBox propertyPane;
		
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
			
			propertyPane = new VBox ();
			propertyPane.ShowAll ();
    		mainContainer.Add (propertyPane);
			
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
			
            controller = new MoonlightController (moonlight, timeline);
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
				
				controller.PropertyManager.PropertiesUpdated += delegate {
					UpdateProperties();
				};
				
			}
			else
			{
				//FIXME Unhook events ;)
			}
        }
		
		private void UpdateProperties()
		{
			propertyPane.FreezeChildNotify();
			
		    Widget[] widgets = (Widget[])propertyPane.Children.Clone();
			foreach(Widget widget in widgets)
				propertyPane.Remove(widget);
			
			foreach(PropertyInfo info in this.controller.PropertyManager.Properties)
				propertyPane.PackEnd(CreatePropertyWidget(controller.PropertyManager.SelectedObject.Child, info));
			
			propertyPane.ThawChildNotify();
			propertyPane.ShowAll();
		}
		
		private Widget CreatePropertyWidget(DependencyObject o, PropertyInfo info)
		{
			HBox b = new HBox(true, 0);
			Adjustment adj = null;
			SpinButton spin = null;
			Label l  = new Label(info.PropertyData.ShortName);
			l.SetAlignment(0.0f, 0.5f);
			b.PackStart(l);
			
			switch(info.Type)
			{
				
			case PropertyType.Double:
			case PropertyType.Integer:
				adj = new Adjustment (0.0, 0.0, Int32.MaxValue, 1.0, 10.0, 100.0);
				spin = new SpinButton (adj, 1.0, 0);
				spin.SetRange (0.0, (double) Int32.MaxValue);
				spin.Numeric = true;
				if(info.Type == PropertyType.Integer)
				{
					spin.Value = (int)o.GetValue(info.PropertyData.Property);
					spin.ValueChanged += delegate (object sender, EventArgs e) {
						Toolbox.ChangeProperty(o, info.PropertyData.Property, spin.ValueAsInt);
					};
				}
				else
				{
					spin.Value = (double)o.GetValue(info.PropertyData.Property);
					spin.ValueChanged += delegate (object sender, EventArgs e) {
						Toolbox.ChangeProperty(o, info.PropertyData.Property, spin.Value);
					};
				}
				b.PackEnd(spin);
				break;
			
			case PropertyType.Percent:
				adj = new Adjustment (0.0, 0.0, 1.0, 0.01, 0.1, 1.0);
				spin = new SpinButton (adj, 0.01, 2);
				spin.Numeric = true;
				spin.SetRange (0.0, 1.0);
				spin.Value = (double)o.GetValue(info.PropertyData.Property);
				spin.ValueChanged += delegate (object sender, EventArgs e) {
					Toolbox.ChangeProperty(o, info.PropertyData.Property, spin.Value);
				};
				b.PackEnd(spin);
				break;
				
			case PropertyType.PenLineCap:
				b.PackEnd(CreateEnumPropertyWidget(o, info, typeof(PenLineCap)));
				break;
				
			case PropertyType.PenLineJoin:
				b.PackEnd(CreateEnumPropertyWidget(o, info, typeof(PenLineJoin)));
				break;
				
			case PropertyType.Visibility:
				b.PackEnd(CreateEnumPropertyWidget(o, info, typeof(Visibility)));
				break;
			
			case PropertyType.Stretch:
				b.PackEnd(CreateEnumPropertyWidget(o, info, typeof(Stretch)));
				break;
				
			case PropertyType.DashArray:
			case PropertyType.Data:
				b.PackEnd(new Entry());
				break;
				
			default:
				b.PackEnd(new Label(string.Format("Unsupported: {0}", info.Type)));
				break;
			}
			
			return b;
		}
		
		private Widget CreateEnumPropertyWidget(DependencyObject o, PropertyInfo info, Type type)
		{
			string[] names = Enum.GetNames(type);
			int[] values = (int[])Enum.GetValues(type);
			int value = (int)o.GetValue(info.PropertyData.Property);
			
			ComboBox b = new ComboBox(names);
			
			for(int i=0; i < values.Length; i++)
				if(values[i] == value)
					b.Active = i;
			
			b.Changed += delegate (object sender, EventArgs e) {
				Toolbox.ChangeProperty(o, info.PropertyData.Property, Enum.Parse(type, b.ActiveText));
			};
			
			return b;
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
