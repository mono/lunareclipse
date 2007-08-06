// RecordDraw.cs created with MonoDevelop
// User: alan at 11:54 AM 7/26/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LunarEclipse.Controller;
using System.Windows.Controls;
using System.Windows;
using LunarEclipse.Serialization;

namespace LunarEclipse.Model
{
	public class RecordDraw : Selector
	{
		private bool prepared;
		private static int nameCounter;
		private Storyboard storyboard;

		public RecordDraw(MoonlightController controller)
			: base(controller)
		{
			controller.Timeline.CurrentPositionChanged += new EventHandler(TimeChanged);
			ChangedHeight += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			ChangedLeft += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			ChangedRotation += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			ChangedTop += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			ChangedWidth += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
		}
		internal override void Prepare ()
		{
			base.Prepare();
			if(!prepared)
			{
				prepared = true;
				storyboard = new Storyboard();
				storyboard.Completed += new EventHandler(Completed);
				storyboard.Pause();
				Controller.Canvas.Resources.Add(storyboard);
			}
		}
		
		public override bool CanUndo {
			get { return false; }
		}

		internal override void Cleanup ()
		{
			base.Cleanup();
			//Controller.Canvas.Resources.Remove(storyboard);
		}

		
		private void Completed(object sender, EventArgs e)
		{
			Console.WriteLine("**** Done ****");
		}

		public void Play()
		{
			try{
				PrintStats();
			//Seek(TimeSpan.Zero);
			storyboard.Begin();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		
		private void PrintStats()
		{
			foreach(DoubleAnimationUsingKeyFrames l in this.storyboard.Children)
			{
				Console.WriteLine("Target: {0}", l.GetValue(Storyboard.TargetPropertyProperty));
				
				foreach(LinearDoubleKeyFrame kf in l.KeyFrames)
					Console.WriteLine("Time: {0} \t Value: {1}", kf.KeyTime.ToString(), kf.Value.ToString());
			}
				
		}
		
		public void Seek(TimeSpan time)
		{
			storyboard.Seek(time);
		}
		
		public void Stop()
		{
			storyboard.Stop();
		}
		
		private void TimeChanged(object sender, EventArgs e)
		{
			//Seek(this.Controller.Timeline.CurrentPosition);
		}
		
		private void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			LinearDoubleKeyFrame keyframe = null;
			
			// Make sure the 'name' of the shape isn't null. This is a hack for the moment.
			if(string.IsNullOrEmpty(e.Target.Name))
				e.Target.SetValue<string>(DependencyObject.NameProperty, (nameCounter++).ToString());
			
			// Get a timeline: either an existing one or a new one
			DoubleAnimationUsingKeyFrames timeline = GetTimeline(e.Target, e.Property);
			
			// For each of the keyframes, if any of them have exactly the same time
			// as the current time, we select that keyframe and update it's value
			foreach(LinearDoubleKeyFrame kf in timeline.KeyFrames)
				if(kf.KeyTime == Controller.Timeline.CurrentPosition)
					keyframe = kf;
			
			// Otherwise we just create a new keyframe for this time
			if(keyframe == null)
			{
				keyframe = new LinearDoubleKeyFrame();
				keyframe.KeyTime = Controller.Timeline.CurrentPosition;
				timeline.KeyFrames.Add(keyframe);
			}

			
			// Update the keyframes value and seek the animation to this keyframe
			keyframe.Value = (double)e.NewValue;
			//keyframe.Value = (double)e.NewValue - (double)e.OldValue;
			Console.WriteLine(keyframe.Value);
			//Seek(keyframe.KeyTime.TimeSpan);
		}
		
		private DoubleAnimationUsingKeyFrames GetTimeline(DependencyObject target, DependencyProperty property)
		{
			DoubleAnimationUsingKeyFrames animation;
			
			// If there is already a timeline for this target item
			// which is altering the same property, return that timeline
			foreach(Timeline t in storyboard.Children)
			{
				if(!target.Name.Equals(t.GetValue(Storyboard.TargetNameProperty)))
					continue;
				
				if(!ReflectionHelper.GetFullPath(target, property).Equals(t.GetValue(Storyboard.TargetPropertyProperty)))
					continue;

				return (DoubleAnimationUsingKeyFrames)t;
			}

			// There was no pre-existing timeline, so create a new one
			animation = new DoubleAnimationUsingKeyFrames();
			animation.BeginTime = TimeSpan.Zero;
			animation.SetValue<object>(Storyboard.TargetNameProperty, target.Name);
			animation.SetValue<object>(Storyboard.TargetPropertyProperty, ReflectionHelper.GetFullPath(target, property));
			storyboard.Children.Add(animation);
			return animation;
		}
	}
}
