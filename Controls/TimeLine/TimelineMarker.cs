// Marker.cs created with MonoDevelop
// User: alan at 5:20 PM 7/20/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Shapes;

namespace LunarEclipse
{
	public class TimelineMarker : Rectangle
	{
		
		private int time;
		
		public int Time
		{
			get { return time; }
			internal set { time = value; }
		}
		
		public TimelineMarker(int time)
		{
			this.time = time;
		}
	}
}
