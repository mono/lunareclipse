// Recorder.cs created with MonoDevelop
// User: alan at 7:14 PM 7/25/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using LunarEclipse.View;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Media.Animation;

namespace LunarEclipse.Controller
{
	public class Recorder : UndoEngine
	{
		private Storyboard storyboard;
		private Dictionary<Visual, Timeline> timelines;
		
		internal TimelineCollection Children
		{
			get { return (TimelineCollection)storyboard.GetValue(Storyboard.ChildrenProperty); }
		}
		
		public Recorder()
		{
			storyboard = new Storyboard();
			storyboard.BeginTime = TimeSpan.Zero;
			storyboard.Duration = TimeSpan.Zero;
			storyboard.SetValue<object>(Storyboard.ChildrenProperty, new TimelineCollection());
		}
		
	}
}
