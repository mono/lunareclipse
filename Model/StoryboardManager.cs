// StoryboardManager.cs created with MonoDevelop
// User: alan at 5:47 PM 8/9/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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
		private UndoGroup undos;

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
				RaiseEvent<StoryboardEventArgs>(CurrentChanged, new StoryboardEventArgs(value));
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
			
			controller.BeforeDrawChanged += new EventHandler<DrawChangeEventArgs>(BeforeDrawChange);
			controller.DrawChanged += new EventHandler<DrawChangeEventArgs>(AfterDrawChange);
			controller.Timeline.CurrentPositionChanged += new EventHandler(TimeChanged);
			controller.Timeline.KeyframeMoved += new EventHandler<LunarEclipse.Controls.KeyframeEventArgs>(KeyframeMoved);
			undos = new UndoGroup();
			LoadFromResources(controller.Canvas);
		}
		
		public void Add(Storyboard storyboard)
		{
			if(storyboard == null)
				throw new ArgumentNullException("storyboard");
			
			storyboards.Add(storyboard);
			controller.Canvas.Resources.Add(storyboard);
			RaiseEvent<StoryboardEventArgs>(StoryboardAdded, new StoryboardEventArgs(storyboard));
			
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
			
			RaiseEvent<EventArgs>(Loaded, EventArgs.Empty);
		}
		
		private void BeforeDrawChange(object sender, DrawChangeEventArgs e)
		{
			Selector s = e.Draw as Selector;
			if(s != null)
				HookSelector(s, false);
		}
		
		private void AfterDrawChange(object sender, DrawChangeEventArgs e)
		{
			Selector s = e.Draw as Selector;
			if(s != null)
				HookSelector(s, true);
		}
		
		private void HookSelector(Selector s, bool hook)
		{
			if(hook)
			{
				Toolbox.PropertyChanged += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
				
				s.MouseDown += delegate {
					Stop();
				};
				s.MouseUp += delegate {
					undos.Undo();
					undos.Clear();
					Seek(controller.Timeline.CurrentPosition);
				};
			}
			else
			{
				Toolbox.PropertyChanged -= new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
				s.MouseDown -= delegate {
					Stop();
				};
				s.MouseUp -= delegate {
					undos.Undo();
					undos.Clear();
					Seek(controller.Timeline.CurrentPosition);
				};
			}
		}
		
		private void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			DependencyObject target = null;
			DependencyProperty property = null;
			LinearDoubleKeyFrame keyframe = null;
			
			if(!Recording || Current == null)
				return;

			// Sometimes the dependency object we're changing the property of is not
			// the object passed in. We are actually changing one of it's children. In this
			// case we need to resolve down to that child and use it instead
			string path = ReflectionHelper.GetFullPath(e.Target, e.Property);
			ReflectionHelper.Resolve(path, e.Target, out target, out property);
			if(string.IsNullOrEmpty(target.Name))
				target.SetValue<string>(DependencyObject.NameProperty, NameGenerator.GetName(this.controller.Canvas, target));
			
			// Get a timeline: either an existing one or a new one
			DoubleAnimationUsingKeyFrames timeline = GetTimeline(target, property);
			
			// If this is a new timeline, we should add in a keyframe at 00:00 recording 
			// the initial value of the property. This way if the 'first' keyframe is at
			// a time other than 0:00, things will move correctly.
			if(timeline.KeyFrames.Count == 0)
			{
				LinearDoubleKeyFrame kf = new LinearDoubleKeyFrame();
				kf.KeyTime = TimeSpan.Zero;
				kf.Value = Convert.ToDouble(target == e.Target ? e.OldValue : target.GetValue(property));
				timeline.KeyFrames.Add(kf);
			}
			
			// For each of the keyframes, if any of them have exactly the same time
			// as the current time, we select that keyframe and update it's value
			foreach(LinearDoubleKeyFrame kf in timeline.KeyFrames)
				if(kf.KeyTime == controller.Timeline.CurrentPosition)
					keyframe = kf;
			
			// Otherwise we just create a new keyframe for this time
			if(keyframe == null)
			{
				keyframe = new LinearDoubleKeyFrame();
				keyframe.Value = Convert.ToDouble(target == e.Target ? e.OldValue : target.GetValue(property));
				keyframe.KeyTime = controller.Timeline.CurrentPosition;
				timeline.KeyFrames.Add(keyframe);
				controller.Timeline.AddKeyframe(timeline, keyframe.KeyTime.TimeSpan);
			}
			
			double difference = Convert.ToDouble(e.NewValue) - Convert.ToDouble(e.OldValue);
			keyframe.Value += difference;
			undos.Add(new UndoPropertyChange(e.Target, e.Property, e.OldValue, e.NewValue, true));
			//e.Target.SetValue<object>(e.Property, e.OldValue);
		}
		
		public void Remove(Storyboard storyboard)
		{
			if(storyboard == null)
				throw new ArgumentNullException("storyboard");
			
			storyboards.Remove(storyboard);
			controller.Canvas.Resources.Remove(storyboard);
			RaiseEvent<StoryboardEventArgs>(StoryboardRemoved, new StoryboardEventArgs(storyboard));
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
				
				return (DoubleAnimationUsingKeyFrames)t;
			}
			
			// There was no pre-existing timeline, so create a new one
			animation = new DoubleAnimationUsingKeyFrames();
			animation.SetValue<string>(Storyboard.NameProperty, NameGenerator.GetName(controller.Canvas, animation));
			animation.SetValue<string>(Storyboard.TargetNameProperty, target.Name);
			animation.SetValue<string>(Storyboard.TargetPropertyProperty, ReflectionHelper.GetFullPath(target, property));
			Current.Children.Add(animation);
			return animation;
		}
		
		private void KeyframeMoved(object sender, KeyframeEventArgs e)
		{
			foreach(DoubleAnimationUsingKeyFrames timeline in Current.Children)
				foreach(LinearDoubleKeyFrame kf in timeline.KeyFrames)
					if(kf.KeyTime == e.OldTime)
						kf.KeyTime = e.Marker.Time;
		}
		
		
		
		private void TimeChanged(object sender, EventArgs e)
		{
			Seek(controller.Timeline.CurrentPosition);
		}
		
		public void Play()
		{
			if(Current != null)
				Current.Begin();
		}
		
		private void Completed(object sender, EventArgs e)
		{
			Console.WriteLine("**** Done ****");
		}
		
		public void Seek(TimeSpan time)
		{
			if(Current == null)
				return;
			
			Current.Stop();
			Current.Begin();
			Current.Pause();
			Current.Seek(time);
		}
		
		public void Stop()
		{
			if(Current != null)
				Current.Stop();
		}
		
		private void RaiseEvent<T>(EventHandler<T> e, T args) where T : EventArgs
		{
			if(e != null)
				e(this, args);
		}
	}
}
