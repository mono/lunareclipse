// IMarker.cs created with MonoDevelop
// User: alan at 12:47 PM 7/25/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace LunarEclipse.Controls
{
	public interface IMarker
	{
		TimeSpan Time { get; set; }
		double Left { get; set; }
		double Height { get; set; }
		double Width { get; set; }
	}
}
