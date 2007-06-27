// /cvs/lunareclipse/Properties/Property.cs created with MonoDevelop
// User: fejj at 2:27 PM 6/27/2007
//

using System;
using Gtk;

namespace LunarEclipse {
	public partial class PropertyGroup : Gtk.Bin {
		Gtk.Widget properties;
		bool expanded;
		
		public PropertyGroup (string name, bool expanded, Gtk.Widget properties)
		{
			this.Build ();
			
			PropertyGroupName.Text = "<b>" + name + "</b>";
			
			this.expanded = expanded;
			PropertyGroupExpander.ArrowType = expanded ? ArrowType.Down : ArrowType.Right;
			if (!expanded)
				properties.Hide ();
			
			this.properties = properties;
			Add (properties);
			
			PropertyGroupTitle.ButtonReleaseEvent += new ButtonReleaseEventHandler (OnPropertyGroupTitleClicked);
		}
		
		bool OnPropertyGroupTitleClicked (object sender, ButtonReleaseEventArgs e)
		{
			if (expanded) {
				PropertyGroupExpander.ArrowType = ArrowType.Right;
				properties.Hide ();
				expanded = false;
			} else {
				PropertyGroupExpander.ArrowType = ArrowType.Down;
				properties.Show ();
				expanded = true;
			}
			
			return true;
		}
	}
}
