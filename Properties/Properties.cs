// /cvs/lunareclipse/Properties/Properties.cs created with MonoDevelop
// User: fejj at 1:54 PM 6/27/2007
//

using System;

using System.Windows;

using Gtk;

namespace LunarEclipse {
	public partial class Properties : Gtk.Bin, IPropertyGroup {
		DependencyObject item;
		
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
			ObjectName.Sensitive = false;
			ObjectName.Changed += new EventHandler (OnNameChanged);
			ObjectType.Text = "";
			
			// build the property groups
			
			// Brush
			
			// Appearance
			appearance = new PropertyGroupAppearance ();
			vboxProperties.PackStart (appearance, false, false, 0);
			appearance.Hide ();
			
			// Layout
			layout = new PropertyGroupLayout ();
			vboxProperties.PackStart (layout, false, false, 0);
			layout.Hide ();
			
			// Common Properties
			
			// Text
			
			// Misc.
		}
		
		public bool HasProperties {
			get { return item != null; }
		}
		
		public DependencyObject DependencyObject {
			set {
				item = value;
				
				if (item != null) {
					ObjectName.Text = item.Name;
					ObjectName.Sensitive = true;
					ObjectType.Text = item.GetType ().Name;
				} else {
					ObjectName.Text = "<No Name>";
					ObjectName.Sensitive = false;
					ObjectType.Text = "";
				}
				
				// build the property groups
				
				// Brush
				
				// Appearance
				appearance.DependencyObject = item;
				if (((PropertyGroup) appearance).HasProperties)
					appearance.Show ();
				else
					appearance.Hide ();
				
				// Layout
				layout.DependencyObject = item;
				if (((PropertyGroup) layout).HasProperties)
					layout.Show ();
				else
					layout.Hide ();
				
				// Common Properties
				
				// Text
				
				// Misc.
			}
		}
		
		void OnNameChanged (object sender, EventArgs e)
		{
			
		}
	}
}
