// /cvs/lunareclipse/Properties/IPropertyGroup.cs created with MonoDevelop
// User: fejj at 2:34 PM 6/28/2007
//

using System;
using System.Windows;
using LunarEclipse.Model;
using Gtk;

namespace LunarEclipse {
	public interface IPropertyGroup {
		SelectedBorder SelectedObject { set; }
		bool HasProperties { get; }
	}
}
