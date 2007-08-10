// DrawChangeEventArgs.cs created with MonoDevelop
// User: alan at 6:02 PM 8/9/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace LunarEclipse.Model
{
	public class DrawChangeEventArgs : EventArgs
	{
		private DrawBase draw;
		public DrawBase Draw
		{
			get { return draw; }
		}
		
		public DrawChangeEventArgs(DrawBase draw)
		{
			this.draw = draw;
		}
	}
}
