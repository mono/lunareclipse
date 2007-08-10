// StoryboardAddedEventArgs.cs created with MonoDevelop
// User: alan at 4:46 PM 8/9/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Media.Animation;

namespace LunarEclipse
{
	public class StoryboardEventArgs : EventArgs
	{
		private Storyboard storyboard;
		
		public Storyboard Storyboard
		{
			get { return storyboard; }
		}
				
		public StoryboardEventArgs(Storyboard storyboard)
		{
			this.storyboard = storyboard;
		}
	}
}
