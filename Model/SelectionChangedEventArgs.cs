// ItemSelectedEventArgs.cs created with MonoDevelop
// User: alan at 6:40 PM 8/9/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Media;

namespace LunarEclipse.Model
{
	public class SelectionChangedEventArgs : EventArgs
	{
		private SelectedBorder selectedBorder;
		private Visual item;
		
		public SelectedBorder SelectedBorder
		{
			get { return selectedBorder; }
		}
		
		public Visual Item
		{
			get { return item; }
		}

		public SelectionChangedEventArgs(Visual visual, SelectedBorder border)
		{
			item = visual;
			selectedBorder = border;
		}
	}
}
