// /cvs/lunareclipse/Properties/Properties.cs created with MonoDevelop
// User: fejj at 1:54 PM 6/27/2007
//

using System;

using System.Windows;

using Gtk;

namespace LunarEclipse {
	public partial class Properties : Gtk.Bin {
		PropertyGroup brush;
		PropertyGroup appearance;
		PropertyGroup layout;
		PropertyGroup common;
		PropertyGroup text;
		PropertyGroup misc;
		
		public Properties ()
		{
			this.Build ();
			
			ObjectName.Text = "<No Name>";
			ObjectType.Text = "";
			
			// build the property groups
			
			// Brush
			
			// Appearance
			appearance = new PropertyGroupAppearance ();
			appearance.Hide ();
			vboxProperties.PackStart (appearance);
			
			// Layout
			
			// Common Properties
			
			// Text
			
			// Misc.
		}
		
		public DependencyObject DependencyObject {
			set {
				ObjectName.Text = value.Name;
				ObjectType.Text = value.GetType ().Name;
				
				// build the property groups
				
				// Brush
				
				// Appearance
				appearance.DependencyObject = value;
				if (((PropertyGroup) appearance).HasProperties)
					appearance.Show ();
				else
					appearance.Hide ();
				
				// Layout
				
				// Common Properties
				
				// Text
				
				// Misc.
			}
		}
	}
}
