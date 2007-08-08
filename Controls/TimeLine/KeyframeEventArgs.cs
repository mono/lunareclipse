// KeyframeEventArgs.cs created with MonoDevelop
// User: alan at 5:42 PM 8/8/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace LunarEclipse.Controls
{
	public class KeyframeEventArgs : EventArgs
	{
		private TimeSpan oldTime;
		private KeyframeMarker marker;
		
		public KeyframeMarker Marker
		{
			get { return marker; }
		}
		
		public TimeSpan OldTime
		{
			get { return oldTime; }
		}
		public KeyframeEventArgs(KeyframeMarker marker, TimeSpan oldTime)
		{
			this.oldTime = oldTime;
			this.marker = marker;
		}
	}
}
