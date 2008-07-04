//
// StoryboardManager.cs
//
// Authors:
//   Alan McGovern alan.mcgovern@gmail.com
//
// Copyright (C) 2007 Alan McGovern
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using LunarEclipse.Controls;
using LunarEclipse.Controller;
using LunarEclipse.Serialization;
	
namespace LunarEclipse.Model
{
	public class StoryboardManager
	{
#region Events
		
		public event EventHandler<StoryboardEventArgs> CurrentChanged;
		public event EventHandler<StoryboardEventArgs> StoryboardAdded;
		public event EventHandler<StoryboardEventArgs> StoryboardRemoved;
		public event EventHandler<StoryboardEventArgs> StoryboardStarted;
		public event EventHandler<StoryboardEventArgs> StoryboardStopped;
		public event EventHandler<EventArgs> Loaded;

#endregion Events
	
			
#region Member Variables
			
		private MoonlightController controller;   // The controller this manager is attached to
		private Storyboard current;               // The currently active storyboard
		private bool recording;                   // True if we are currently recording
		private List<Storyboard> storyboards;     // The list of storyboards that are declared
		private UndoGroup initialValues;

#endregion Member Variables
		
		
#region Properties
		
		public Storyboard Current
		{
			get { return current; }
			set 
			{
				if(!storyboards.Contains(value))
					throw new ArgumentException("The storyboard does not belong to this manager");

				current = value;
				Toolbox.RaiseEvent<StoryboardEventArgs>(CurrentChanged, this, new StoryboardEventArgs(value));
			}
		}
		
		public bool Recording
		{
			get { return recording; }
			set { recording = value; }
		}
		
#endregion Properties		

		internal StoryboardManager(MoonlightController controller)
		{
			this.controller = controller;
			storyboards = new List<Storyboard>();
			
			controller.Timeline.CurrentPositionChanged += new EventHandler(TimeChanged);
			controller.Timeline.KeyframeMoved += new EventHandler<LunarEclipse.Controls.KeyframeEventArgs>(KeyframeMoved);
			initialValues = new UndoGroup();
			LoadFromResources(controller.Canvas);
		}
		
		public void Add(Storyboard storyboard)
		{
			if(storyboard == null)
				throw new ArgumentNullException("storyboard");
			
			storyboards.Add(storyboard);
			controller.Canvas.Resources.Add(storyboard);
			Toolbox.RaiseEvent<StoryboardEventArgs>(StoryboardAdded, this, new StoryboardEventArgs(storyboard));
			
			if(Current == null)
				Current = storyboard;
		}
		                         
		private void LoadFromResources(Canvas canvas)
		{
			this.storyboards.Clear();
			
			// For every storyboard resource add it into our list
			foreach(DependencyObject o in canvas.Resources)
			{
				Storyboard b = o as Storyboard;
				if(o == null)
					continue;

				Add(b);
				
				// By default the first storyboard is selected
				if(Current == null)
					Current = b;
			}
			
			Toolbox.RaiseEvent<EventArgs>(Loaded, this, EventArgs.Empty);
		}
		
		private void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			DependencyObject target = null;
			DependencyProperty property = null;
			LinearDoubleKeyFrame keyframe = null;
			LinearDoubleKeyFrame previouskf = null;

			// If the name of something changed, then we need to update references to that item
			// in all our storyboards and update them accordingly
			if(e.Property == DependencyObject.NameProperty)
			{
				foreach(Timeline l in this.Current.Children)
					if(l.GetValue(Storyboard.TargetNameProperty).Equals(e.OldValue))
						l.SetValue(Storyboard.TargetNameProperty, e.NewValue);
				return;
			}
			
			// If recording is disabled or there is no active storyboard
			if(!Recording || Current == null)
				return;

			// Sometimes the dependency object we're changing the property of is not
			// the object passed in. We are actually changing one of it's children. In this
			// case we need to resolve down to that child and use it instead
			string path = ReflectionHelper.GetFullPath(e.Target, e.Property);
			ReflectionHelper.Resolve(path, e.Target, out target, out property);
			if(string.IsNullOrEmpty(target.Name))
				target.SetValue(DependencyObject.NameProperty, NameGenerator.GetName(this.controller.Canvas, target));
			
			// Get a timeline: either an existing one or a new one
			DoubleAnimationUsingKeyFrames timeline = GetTimeline(target, property);
			
			// Get the time of the keyframe previous to where the cursor is on the timeline.
			TimeSpan previous = controller.Timeline.GetKeyframeBefore(controller.Timeline.CurrentPosition);
			
			// If this is a new timeline, we should add in a keyframe at 00:00 recording 
			// the initial value of the property. This way if the 'first' keyframe is at
			// a time other than 0:00, things will move correctly.
			if(timeline.KeyFrames.Count == 0)
			{
				Console.WriteLine ("There were no keyframes");
				LinearDoubleKeyFrame kf = new LinearDoubleKeyFrame();
				kf.KeyTime = previous;
				kf.Value = Convert.ToDouble(target == e.Target ? e.OldValue : target.GetValue(property));
				timeline.KeyFrames.Add(kf);
				initialValues.Add(new UndoPropertyChange(target, property, kf.Value, kf.Value, true));
			}
			
			// For each of the keyframes, if any of them have exactly the same time
			// as the current time, we select that keyframe and update it's value
			Console.WriteLine("Checking for: {0}", previous);
			foreach(LinearDoubleKeyFrame kf in timeline.KeyFrames)
			{
				Console.WriteLine("We have a : {0}", kf.KeyTime.TimeSpan);
				if(kf.KeyTime.TimeSpan.Ticks == controller.Timeline.CurrentPosition.Ticks)
					keyframe = kf;
				if(kf.KeyTime.TimeSpan.Ticks == previous.Ticks)
					previouskf = kf;
			}
			// Otherwise we just create a new keyframe for this time
			if(keyframe == null)
			{
				Console.WriteLine("There was no existing keyframe 'kf'");
				keyframe = new LinearDoubleKeyFrame();
				keyframe.Value = Convert.ToDouble(target == e.Target ? e.OldValue : target.GetValue(property));
				keyframe.KeyTime = controller.Timeline.CurrentPosition;
				Console.WriteLine("Creating keyframe at: {0}", keyframe.KeyTime);
				timeline.KeyFrames.Add(keyframe);
				controller.Timeline.AddKeyframe(timeline, keyframe.KeyTime.TimeSpan);
			}
			if(previouskf == null && keyframe.KeyTime.TimeSpan.Ticks != controller.Timeline.CurrentPosition.Ticks)
			{
				Console.WriteLine ("There was no existing previous keyframe 'previouskf'");
				previouskf = new LinearDoubleKeyFrame();
				previouskf.Value = Convert.ToDouble(target == e.Target ? e.OldValue : target.GetValue(property));
				Console.WriteLine("Creating keyframe at: {0}", previouskf.Value);
				previouskf.KeyTime = previous;
				timeline.KeyFrames.Add(previouskf);
				controller.Timeline.AddKeyframe(timeline, keyframe.KeyTime.TimeSpan);
			}
			
			
			double difference = Convert.ToDouble(e.NewValue) - Convert.ToDouble(e.OldValue);
			keyframe.Value = Convert.ToDouble(e.NewValue);
			Console.WriteLine ("New value is: {0}", keyframe.Value);
			//undos.Add(new UndoPropertyChange(e.Target, e.Property, e.OldValue, e.NewValue, true));
		}
		
		public void Remove(Storyboard storyboard)
		{
			if(storyboard == null)
				throw new ArgumentNullException("storyboard");
			
			storyboards.Remove(storyboard);
			controller.Canvas.Resources.Remove(storyboard);
			Toolbox.RaiseEvent<StoryboardEventArgs>(StoryboardRemoved, this, new StoryboardEventArgs(storyboard));
		}
		
		private DoubleAnimationUsingKeyFrames GetTimeline(DependencyObject target, DependencyProperty property)
		{
			DoubleAnimationUsingKeyFrames animation;
			
			// If there is already a timeline for this target item
			// which is altering the same property, return that timeline
			foreach(Timeline t in Current.Children)
			{
				if(!target.Name.Equals(t.GetValue(Storyboard.TargetNameProperty)))
					continue;
				
				if(!ReflectionHelper.GetFullPath(target, property).Equals(t.GetValue(Storyboard.TargetPropertyProperty)))
					continue;
				
				Console.WriteLine("Found existing timeline");
				return (DoubleAnimationUsingKeyFrames)t;
			}
			Console.WriteLine("Creating new timeline");
			// There was no pre-existing timeline, so create a new one
			animation = new DoubleAnimationUsingKeyFrames();
			animation.SetValue(Storyboard.NameProperty, NameGenerator.GetName(controller.Canvas, animation));
			animation.SetValue(Storyboard.TargetNameProperty, target.Name);
			animation.SetValue(Storyboard.TargetPropertyProperty, ReflectionHelper.GetFullPath(target, property));
			Current.Children.Add(animation);
			return animation;
		}
		
		private void KeyframeMoved(object sender, KeyframeEventArgs e)
		{
			foreach(DoubleAnimationUsingKeyFrames timeline in Current.Children)
				foreach(LinearDoubleKeyFrame kf in timeline.KeyFrames)
					if(kf.KeyTime == e.OldTime)
				{
					Console.WriteLine("Moved keyframe: {0} to {1}", e.OldTime, e.Marker.Time);
						kf.KeyTime = e.Marker.Time;
				}
		}
		
		
		
		private void TimeChanged(object sender, EventArgs e)
		{
			Console.WriteLine("Seeking to: {0}", controller.Timeline.CurrentPosition);
			Seek(controller.Timeline.CurrentPosition);
		}
		
		public void Play()
		{
			initialValues.Undo();
			Console.WriteLine("Current is null? {0}", current == null);
			if(Current != null)
				Current.Begin();
		}
		
		private void Completed(object sender, EventArgs e)
		{
			Console.WriteLine("**** Done ****");
		}
		
		public void Seek(TimeSpan time)
		{
			initialValues.Undo();
			if(Current == null)
				return;
			
			Current.Stop();
			Current.Begin();
			Current.Pause();
			Current.Seek(time);
		}
		
		public void Stop()
		{
			Console.WriteLine("Current is null? {0}", current == null);
			if(Current != null)
				Current.Stop();
		}
	}
}
