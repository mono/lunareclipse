// RecordDraw.cs created with MonoDevelop
// User: alan at 11:54 AM 7/26/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows;
using LunarEclipse.Controls;
using LunarEclipse.Controller;
using LunarEclipse.Serialization;

namespace LunarEclipse.Model
{
	public class RecordDraw : Selector
	{
		private bool prepared;
		private Storyboard storyboard;

		public RecordDraw(MoonlightController controller)
			: base(controller)
		{
			controller.Timeline.CurrentPositionChanged += new EventHandler(TimeChanged);
			controller.Timeline.KeyframeMoved += new EventHandler<KeyframeEventArgs>(KeyframeMoved);
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
				Controller.Canvas.Resources.Add(storyboard);
			}
		}
		
		public override bool CanUndo {
			get { return false; }
		}

		internal override void Cleanup ()
		{
			base.Cleanup();
		}

		
		private void Completed(object sender, EventArgs e)
		{
			Console.WriteLine("**** Done ****");
		}

		public void Play()
		{
			storyboard.Begin();
		}
		
		public void Seek(TimeSpan time)
		{
			Console.WriteLine(time);
			storyboard.Begin();
			storyboard.Pause();
			storyboard.Seek(time);
		}
		
		public void Stop()
		{
			storyboard.Stop();
		}
		
		private void TimeChanged(object sender, EventArgs e)
		{
			Seek(this.Controller.Timeline.CurrentPosition);
		}
		
		private void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			LinearDoubleKeyFrame keyframe = null;
			
			storyboard.Stop();
			
			// Get a timeline: either an existing one or a new one
			DoubleAnimationUsingKeyFrames timeline = GetTimeline(e.Target, e.Property);
			
			// If this is a new timeline, we should add in a keyframe at 00:00 recording 
			// the initial value of the property. This way if the 'first' keyframe is at
			// a time other than 0:00, things will move correctly.
			if(timeline.KeyFrames.Count == 0)
			{
				LinearDoubleKeyFrame kf = new LinearDoubleKeyFrame();
				kf.KeyTime = TimeSpan.Zero;
				kf.Value = (double)e.OldValue;
				timeline.KeyFrames.Add(kf);
			}
			
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
				Controller.Timeline.AddKeyframe(timeline, keyframe.KeyTime.TimeSpan);
			}

			// Update the keyframes value and seek the animation to this keyframe
			keyframe.Value = (double)e.NewValue;
			Seek(keyframe.KeyTime.TimeSpan);
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
			animation.SetValue<string>(Storyboard.NameProperty, NameGenerator.GetName(Panel, animation));
			animation.SetValue<object>(Storyboard.TargetNameProperty, target.Name);
			animation.SetValue<object>(Storyboard.TargetPropertyProperty, ReflectionHelper.GetFullPath(target, property));
			storyboard.Children.Add(animation);
			return animation;
		}
		
		private void KeyframeMoved(object sender, KeyframeEventArgs e)
		{
			LinearDoubleKeyFrame k = new LinearDoubleKeyFrame();
			k.KeyTime = TimeSpan.FromDays(10);
			
			//DoubleAnimationUsingKeyFrames timeline = (DoubleAnimationUsingKeyFrames)e.Marker.Timeline;
			foreach(DoubleAnimationUsingKeyFrames timeline in storyboard.Children)
			{
				foreach(LinearDoubleKeyFrame kf in timeline.KeyFrames)
					if(kf.KeyTime == e.OldTime)
						kf.KeyTime = e.Marker.Time;
				
				timeline.KeyFrames.Add(k);
				timeline.KeyFrames.Remove(k);
			}
		}
	}
}
