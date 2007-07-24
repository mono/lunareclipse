// KeyframeMarker.cs created with MonoDevelop
// User: alan at 1:32 PM 7/24/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace LunarEclipse
{
	public class KeyframeMarker
	{
		private int id;
		private TimeSpan time;
		
		
		public int Id
		{
			get { return id; }
		}
		
		public TimeSpan Time
		{
			get { return time; }
			internal set { time = value; }
		}
		
		public KeyframeMarker(int id, TimeSpan time)
		{
			this.id = id;
			this.time = time;
		}
	}
}
