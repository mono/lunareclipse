using System;
using System.Runtime.InteropServices;
using Gtk;
using System.Windows;
using System.Windows.Controls;
using Mono;
using System.Reflection;
using System.IO;
using System.Net;
using System.Collections;
using System.Threading;

namespace LunarEclipse.View
{
    public class GtkSilver : DrawingArea {
    	[DllImport ("moon")]
    	extern static IntPtr surface_new (int w, int h);

	    [DllImport ("moon")]
	    extern static IntPtr surface_get_drawing_area (IntPtr surface);

	    [DllImport ("moon")]
	    extern static IntPtr xaml_create_from_file (string file, ref int kind_type);

    	IntPtr surface;
        public Canvas Canvas
        {
            get { return c; }
        }
    	private Canvas c;

    	public static readonly string filePath = System.IO.Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "file.xaml");
                        
                             
                    
    	public GtkSilver (int w, int h)
    	{
    		surface = surface_new (w, h);
    		Raw = surface_get_drawing_area (surface);
    	}

    	public void Attach (Canvas canvas)
    	{
    		if (canvas == null)
    			throw new ArgumentNullException ("canvas");

    		// Dynamically invoke our get-the-handle-code
    		MethodInfo m = typeof (Canvas).Assembly.GetType ("Mono.Hosting").
    			GetMethod ("SurfaceAttach", BindingFlags.Static | BindingFlags.NonPublic);
    		m.Invoke (null, new object [] { surface, canvas });
            
            this.c = canvas;
    	}
    	


        public bool Load (string file)
	    {
    		string xaml = null;

    		Console.WriteLine ("Loading: {0}", file);
    		try {
    			using (FileStream fs = File.OpenRead (file)){
    				using (StreamReader sr = new StreamReader (fs)){
    					xaml = sr.ReadToEnd ();
    				}
    			}
    		} catch (Exception e) {
    			Console.Error.WriteLine ("Error loading XAML file {0}: {1}", file, e.GetType());
    			return false;
    		}
    		
    		if (xaml == null){
    			Console.Error.WriteLine ("Error loading XAML file {0}", file);
    			return false;
    		}
    		Console.WriteLine("XAML");
    		Console.WriteLine("XAML");
    		Console.WriteLine("XAML");
    		Console.WriteLine(xaml);
    		DependencyObject d = XamlReader.Load (xaml);
    		if (d == null){
    			Console.Error.WriteLine ("No dependency object returned from XamlReader");
    			return false;
    		}
    		
    		if (!(d is Canvas)){
    			Console.Error.WriteLine ("No Canvas as root");
    			return false;
    		}
    		
    		c = (Canvas)d;
    		return true;
        }
    }
}
