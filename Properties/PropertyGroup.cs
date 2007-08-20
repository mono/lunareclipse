// /cvs/lunareclipse/Properties/PropertyGroup.cs created with MonoDevelop
// User: fejj at 2:27 PM 6/27/2007
//

using System;
using System.Collections.Generic;
using System.Windows;

using LunarEclipse.Model;

namespace LunarEclipse
{
	public abstract class PropertyGroup
	{
		private string name;
		private List<PropertyInfo> properties;
		private SelectedBorder selectedObject;
		
		public PropertyGroup (string name)
		{
			this.name = name;
			properties = new List<PropertyInfo>();
		}
		
		public bool HasProperties
		{
			get { return properties.Count != 0; }
		}
		
		public List<PropertyInfo> Properties
		{
			get { return properties; }
		}
		
		public virtual SelectedBorder SelectedObject
		{
			get { return selectedObject; }
			set { selectedObject = value; }
		}
	}
}
