// /cvs/lunareclipse/Properties/Properties.cs created with MonoDevelop
// User: fejj at 1:54 PM 6/27/2007
//

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using LunarEclipse.Model;
using System.Windows;

using Gtk;

namespace LunarEclipse {
	public partial class Properties : Gtk.Bin, IPropertyGroup {
		DependencyProperty nameProp;
		SelectedBorder item;
		
		PropertyGroup brush;
		PropertyGroup appearance;
		PropertyGroup layout;
		PropertyGroup common;
		PropertyGroup text;
		PropertyGroup misc;

		public Properties ()
		{
			this.Build ();
			
			ObjectName.Text = "<No Object Selected>";
			ObjectName.Sensitive = false;
			ObjectName.Changed += new EventHandler (OnNameChanged);
			ObjectType.Text = "";
			
			// build the property groups
			
			// Brush
			brush = new PropertyGroupBrushes();
            vboxProperties.PackStart(brush, false, false, 0);
            brush.Hide ();
            
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

		public SelectedBorder SelectedObject {
			set {
				nameProp = null;
				item = value;
				
				if (item != null) {
					ObjectName.Text = item.Name != null ? item.Name : "";
					ObjectName.Sensitive = true;
					ObjectType.Text = value.Child.GetType ().Name;
					
					for (Type type = item.Child.GetType (); nameProp == null && type != null; type = type.BaseType) {
						FieldInfo[] fields = type.GetFields ();
						foreach (FieldInfo field in fields) {
							if (field.Name == "NameProperty") {
								nameProp = (DependencyProperty) field.GetValue (item.Child);
								break;
							}
						}
					}
				} else {
					ObjectName.Text = "<No Object Selected>";
					ObjectName.Sensitive = false;
					ObjectType.Text = "";
				}
				
				// build the property groups
				
				// Brush
				brush.SelectedObject = item;
                if(brush.HasProperties)
                    brush.Show ();
                else
                    brush.Hide ();

				// Appearance
				appearance.SelectedObject = item;
				if (appearance.HasProperties)
					appearance.Show ();
				else				
					appearance.Hide ();
				
				// Layout
				layout.SelectedObject = item;
				if (layout.HasProperties)
					layout.Show ();
				else
					layout.Hide ();
				
				// Common Properties
				
				// Text
				
				// Misc.
			}
		}
		
		void OnNameChanged (object o, EventArgs e)
		{
			Entry entry = (Entry) o;
			
			if (nameProp != null)
				item.SetValue<string> (nameProp, entry.Text);
		}
	}
}
