// NameGenerator.cs created with MonoDevelop
// User: alan at 4:06 PM 8/8/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;
using LunarEclipse.Controller;

namespace LunarEclipse.Model
{
	public static class NameGenerator
	{
		public static string GetName(Panel container, DependencyObject item)
		{
			int count = 0;
			string basename = item.GetType().Name;
			string name = basename;
			
			while(container.FindName(name) != null)
				name = string.Format("{0}{1}", basename, ++count);
			
			return name;
		}
	}
}
