// /cvs/lunareclipse/Properties/PropertyGroup.cs created with MonoDevelop
// User: fejj at 2:27 PM 6/27/2007
//

using System;

using System.Windows;
using LunarEclipse.Model;
using Gtk;

namespace LunarEclipse {
	public abstract partial class PropertyGroup : Bin, IPropertyGroup {
		Widget properties = null;
		
		public PropertyGroup (string name)
		{
			this.Build ();
			
			PropertyGroupLabel.Text = "<b>" + name + "</b>";
			PropertyGroupLabel.UseMarkup = true;
			expander.Expanded = false;
		}
		
		protected Widget Properties {
			get { return properties; }
			set {
				if (properties != null)
					properties.Destroy ();
				properties = value;
				expander.Add (value);
			}
		}
		
		protected bool Expanded {
			get { return expander.Expanded; }
			set { expander.Expanded = value; }
		}
		
		public virtual bool HasProperties {
			get { return false; }
		}
		
		public virtual SelectedBorder SelectedObject {
			set {
				if (properties != null)
					properties.Destroy ();
				properties = null;
			}
		}
	}
}
