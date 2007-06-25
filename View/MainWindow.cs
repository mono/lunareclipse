// /home/alan/Projects/DesignerMoon/DesignerMoon/MainWindow.cs created with MonoDevelop
// User: alan at 4:48 PM 6/15/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Input;
using DesignerMoon.Controller;
using DesignerMoon.Model;

namespace DesignerMoon.View
{
    public partial class MainWindow: Gtk.Window
    {
        Box mainContainer;
        MoonlightController controller;
        
        public MainWindow (): base (Gtk.WindowType.Toplevel)
    	{
    		Build ();
    		
            Canvas c = new Canvas();
            c.Width = 640;
            c.Height = 480;
            Rectangle r = new Rectangle();
            r.Width = 100;
            r.Height = 50;
            r.SetValue<double>(Canvas.TopProperty, 100);
            r.SetValue<double>(Canvas.TopProperty, 200);
            
            Ellipse e = new Ellipse();
            e.Fill = new SolidColorBrush(Colors.Red);
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(55,66);
            brush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;
            e.Stroke = brush;
            e.StrokeThickness = 5;
            c.Children.Add(e);
                
            c.Background = new ImageBrush();
            c.SetValue<string>(Canvas.NameProperty, "Canvas Name");
            c.Background.Opacity = 5;
            
            c.RenderTransformOrigin = new Point(55, 55);
            
            c.Children.Add(r);
            Serializer s = new Serializer();
            Console.WriteLine(s.Serialize(c));
            return;

    		GtkSilver moonlight = new GtkSilver(640, 480);
            moonlight.Attach(c);

    		mainContainer = new HBox();
    		mainContainer.Add(moonlight);
    		mainContainer.Add(InitialiseWidgets());
    		Add(mainContainer);
    		
            controller = new MoonlightController(moonlight);
    		ShowAll();
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
