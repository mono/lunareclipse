//
// MainWindow.cs
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
using Gtk.Moonlight;

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
			c.Background = new SolidColorBrush(Colors.White);
			
			Console.WriteLine ("Moonlight created");
    		GtkSilver moonlight = new GtkSilver(800, 600);
            moonlight.Attach(c);
			moonlight.Show ();
			
//			System.Windows.Shapes.Ellipse circulo = new System.Windows.Shapes.Ellipse();
//			circulo.Width = 100.0;
//			circulo.Height = 200.0;
//			circulo.SetValue(Canvas.TopProperty, 50.0);
//			circulo.SetValue(Canvas.LeftProperty, 50.0);
//			circulo.Fill = new SolidColorBrush(Colors.White);
//			circulo.Stroke = new SolidColorBrush(Colors.Blue);
//			c.Children.Add(circulo);
			
			System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();
			
			line.X1 = 50;
			line.Y1 = 50;
			line.X2 = 200;
			line.Y2 = 100;
			line.Stroke = new SolidColorBrush(Colors.Blue);
			c.Children.Add(line);
			
			ResizeHandleGroup hg = new ResizeHandleGroup(moonlight, line);
			hg.AddToCanvas(c);
			
			Console.WriteLine ("Animation");
			timeline = new AnimationTimeline(800, 70);
			timeline.SizeAllocated += delegate {
				timeline.UpdateSize();
			};
    		mainContainer = new HBox ();
			Gtk.VBox leftpane = new VBox ();
			Widget toolbox = InitialiseWidgets ();
			toolbox.ShowAll ();
			leftpane.PackStart (toolbox, false, false, 0);
			leftpane.ShowAll ();
			mainContainer.PackStart (leftpane, false, false, 0);
			animationWidgets = InitialiseAnimationWidgets();
			mainContainer.PackStart(animationWidgets, false, false, 0);
			VBox vbox = new VBox();
			vbox.PackStart(moonlight);
			vbox.PackEnd(timeline);
			vbox.ShowAll();
			mainContainer.Add (vbox);
			
			propertyPane = new VBox ();
			propertyPane.ShowAll ();
			
			ScrolledWindow propertyScroll = new ScrolledWindow();
			propertyScroll.AddWithViewport(propertyPane);
			propertyScroll.ShowAll();
    		leftpane.PackStart (propertyScroll, true, true, 0);
			
            TextView view = new Gtk.TextView(buffer);
			view.Show ();
			
			Gtk.Button save_as = new Gtk.Button (Gtk.Stock.SaveAs);
			save_as.Clicked += HandleSaveasClicked;
			save_as.Show ();
			leftpane.PackEnd (save_as, false, false, 0);
			
            Gtk.ScrolledWindow scrolled = new ScrolledWindow();
            scrolled.Add (view);
            
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
            ShowAll ();
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
				
				controller.PropertyManager.SelectionChanged += delegate {
					GeneratePropertyWidgets();
				};
				
				Toolbox.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) {
					UpdatePropertyWidget(e);
				};
				
			}
			else
			{
				//FIXME Unhook events ;)
			}
        }
		
		private void UpdatePropertyWidget(PropertyChangedEventArgs e)
		{
			if(controller.PropertyManager.SelectedObject == null || e.Target != controller.PropertyManager.SelectedObject.Child)
				return;
			
			PropertyData data = LunarEclipse.Serialization.ReflectionHelper.GetData(e.Property);
			foreach(HBox box in propertyPane.Children)
			{
				if(((Label)box.Children[0]).Text == data.ShortName)
				{
					Widget widget = box.Children[1];
					if(widget is SpinButton)
						((SpinButton)widget).Value = Convert.ToDouble(e.NewValue);
					
					else if(widget is ComboBox)
						((ComboBox)widget).Active = Convert.ToInt32(e.NewValue);
					
					else if(widget is Entry)
						((Entry)widget).Text = Convert.ToString(e.NewValue);
					
					else
						Console.WriteLine("Couldn't Update");
				}
			}
		}
		
		private void GeneratePropertyWidgets()
		{
			propertyPane.FreezeChildNotify();
			
		    Widget[] widgets = (Widget[])propertyPane.Children.Clone();
			foreach(Widget widget in widgets)
				widget.Destroy();
			
			foreach(PropertyInfo info in this.controller.PropertyManager.Properties)
				propertyPane.PackEnd(CreatePropertyWidget(controller.PropertyManager.SelectedObject.Child, info));
			
			if(propertyPane.Children.Length == 0)
			{
//				HBox h = new HBox(true, 0);
//				Label l = new Label("No object selected...");
//				h.PackStart(l, true, true, 0);
//				l = new Label("...");
//				h.PackStart(l, true, true, 0);
//				propertyPane.Add(h);
			}
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
				
			case PropertyType.String:
				Entry entry = new Entry(o.GetValue(info.PropertyData.Property).ToString());
				entry.Changed += delegate (object sender, EventArgs e) {
					Toolbox.ChangeProperty(o, info.PropertyData.Property, ((Entry)sender).Text);
				};
				b.PackEnd(entry);
				break;
				
			case PropertyType.DashArray:
			case PropertyType.Data:
				b.PackEnd(new Entry());
				break;
				
			case PropertyType.Point:
				b.PackEnd(CreatePointPropertyWidget(o, info));
				break;
				
			case PropertyType.Brush:
				b.PackEnd(CreateBrushPropertyWidget(o, info));
				break;
				
			default:
				b.PackEnd(new Label(string.Format("Unsupported: {0}", info.Type)));
				break;
			}
			
			return b;
		}
		
		private Widget CreateBrushPropertyWidget(DependencyObject o, PropertyInfo info)
		{
			Entry e = new Entry();
			e.Changed += delegate {
				uint val;
				if(!uint.TryParse(e.Text, System.Globalization.NumberStyles.HexNumber, null, out val))
					val = 0x555555;
				
				Color c = Color.FromArgb((byte)(val>>24), (byte)(val>>16), (byte)(val>>8), (byte)val);
				Console.WriteLine(c.ToString());
				o.SetValue(info.PropertyData.Property, new SolidColorBrush(c));
			};
			return e;
		}
		
		private Widget CreatePointPropertyWidget(DependencyObject o, PropertyInfo info)
		{
			VBox b = new VBox(true, 0);
			HBox h = new HBox(true, 0);
			h.Add(new Label("X"));
			Adjustment adj = new Adjustment (0.0, 0, 1, 0.01, 0.1, 1.0);
			Point p = (Point)o.GetValue(info.PropertyData.Property);
			
			SpinButton spinner = new SpinButton(adj, 1.0, 2);
			spinner.Numeric = true;
			spinner.Value = p.X;
			spinner.Changed += delegate(object sender, EventArgs e) {
				Point point = (Point)o.GetValue(info.PropertyData.Property);
				point.X = ((SpinButton)sender).Value;
				o.SetValue(info.PropertyData.Property, point);
			};
			h.Add(spinner);
			b.Add(h);
			
			h = new HBox(true, 0);
			h.Add(new Label("Y"));
			adj = new Adjustment (0.0, 0, 1, 0.01, 0.1, 1.0);
			spinner = new SpinButton(adj, 1.0, 2);
			spinner.Numeric = true;
			spinner.Value = p.Y;
			spinner.Changed += delegate (object sender, EventArgs e) {
				Point point = (Point)o.GetValue(info.PropertyData.Property);
				point.Y = ((SpinButton)sender).Value;
				o.SetValue(info.PropertyData.Property, point);
			};
			h.Add(spinner);
			b.Add(h);
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
		
		private Table InitialiseWidgets()
		{
			Table toolbox = new Table (4, 3, true);
			Button b;
			
			b = new Button("Selection");
			b.Clicked += delegate {
                controller.Current = new SelectionDraw(this.controller);
				Console.WriteLine("Draw is" + controller.Current.GetType().Name.ToString());
			};
			toolbox.Attach (b, 0, 1, 0, 1);
			
			b = new Button("Circle");
			b.Clicked += delegate {
				controller.Current = new CircleDraw();
				Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
			toolbox.Attach (b, 1, 2, 0, 1);
			
            b = new Button("Ellipse");
    	    b.Clicked += delegate {
    	        controller.Current = new EllipseDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
	    toolbox.Attach (b, 2, 3, 0, 1);
            
    	    b = new Button("Line");
    	    b.Clicked += delegate {
    	        controller.Current = new LineDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
	    toolbox.Attach (b, 0, 1, 1, 2);
    	    
    	    b = new Button("Pen");
    	    b.Clicked += delegate {
    	        controller.Current = new PenDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
	    toolbox.Attach (b, 1, 2, 1, 2);
    	    
    	    b = new Button("Rectangle");
    	    b.Clicked += delegate {
    	        controller.Current = new RectangleDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };
	    toolbox.Attach (b, 2, 3, 1, 2);
    	    
    	    b = new Button("Square");
    	    b.Clicked += delegate {
    	        controller.Current = new SquareDraw();
    	        Console.WriteLine("Draw is: " + controller.Current.GetType().Name);
    	    };    	    
	    toolbox.Attach (b, 0, 1, 2, 3);
			
			Button record = new Button ();
			record.Image = new Gtk.Image ("gtk-media-record", IconSize.Button);
			record.Clicked += delegate (object sender, EventArgs e) {
				controller.StoryboardManager.Recording = !controller.StoryboardManager.Recording;
				animationWidgets.Visible = controller.StoryboardManager.Recording;
//				((Button)sender).Label = animationWidgets.Visible ? "Stop Recording" : "Record";
				record.Image = animationWidgets.Visible ? new Gtk.Image ("gtk-media-stop", IconSize.Button) : new Gtk.Image ("gtk-media-record", IconSize.Button);
			};
	    toolbox.Attach (record, 1, 2, 2, 3);
			
            undo = new Button(Stock.Undo);
            undo.Sensitive = false;
            undo.Clicked += delegate {
                controller.Undo(); 
            };
	    toolbox.Attach (undo, 0, 1, 3, 4);
            
            redo = new Button(Stock.Redo);
            redo.Sensitive = false;
            redo.Clicked += delegate { controller.UndoEngine.Redo(); };
	    toolbox.Attach (redo, 1, 2, 3, 4);
    	    
    	    b = new Button(Stock.Clear);
    	    b.Clicked += delegate {
    	        controller.Clear();
    	    };
	    toolbox.Attach (b, 2, 3, 3, 4);
			
            return toolbox;
        }
    	
    	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
    	{
    		Gtk.Application.Quit ();
    		a.RetVal = true;
    	}
		private void HandleSaveasClicked (object sender, EventArgs e)
		{
			Gtk.FileChooserDialog fc= new Gtk.FileChooserDialog("Choose the save location",
			                                                    this,
			                                                    FileChooserAction.Open,
			                                                    "Cancel",ResponseType.Cancel,
			                                                    "Save",ResponseType.Accept);

			if (fc.Run() == (int)ResponseType.Accept) 
			{
				string xaml = this.controller.SerializeCanvas ();
				System.IO.FileStream fs = new FileStream (fc.Filename, FileMode.OpenOrCreate, FileAccess.Write);
				StreamWriter stream = new StreamWriter (fs);
				stream.Write (xaml);
				stream.Close();
			}
			fc.Destroy();
		}
    }
}
