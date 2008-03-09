// Timeline.cs created with MonoDevelop
// User: alan at 3:44 PM 7/20/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LunarEclipse.View;
using Gtk.Moonlight;


namespace LunarEclipse.Controls
{
	public class AnimationTimeline : GtkSilver
	{
		public event EventHandler CurrentPositionChanged;
		public event EventHandler<KeyframeEventArgs> KeyframeMoved;
		
		private const double PixelsPerDivision = 80;
		
		private IMarker clickedItem;   // If a [keyframe|timeline]marker is clicked on, it is stored here
		private Point location;        // The location of the mouse
		private PositionMarker marker; // The marker used to denote the current location in the timeline
		private bool started;          // True when the mouse is clicked, false when it is released again
		private TimeSpan startTime;    // The starting time displayed on the timeline
		private Point startLocation;   // The point at which the mouse originally was clicked at
		
		private List<IMarker> divisionMarkers;        // The markers used to indicate the second/quarter second division
		private List<TextBlock> divisionTextblocks;   // The textblocks used to indicate the time at each whole division
		private List<KeyframeMarker> keyframeMarkers; // The keyframes currently recorded on the timeline
		
		public TimeSpan CurrentPosition
		{
			get { return this.marker.Time; }
		}
		
		public AnimationTimeline(int width, int height)
			:base (width, height)
		{
			Canvas c = new Canvas();
			c.Width = width;
			c.Height = height;
			Attach(c);
			
			marker = new PositionMarker(TimeSpan.Zero, 0, 0);
			marker.ZIndex = 1;
			divisionMarkers = new List<IMarker>();
			divisionTextblocks = new List<TextBlock>();
			keyframeMarkers = new List<KeyframeMarker>();
			
			startTime = TimeSpan.Zero;
			marker.MouseLeftButtonDown += delegate { clickedItem = marker; };
			
			Canvas.MouseLeftButtonDown += new MouseEventHandler(MouseDown);
			Canvas.MouseMove += new MouseEventHandler(MouseMove);
			Canvas.MouseLeftButtonUp += new MouseEventHandler(MouseUp);
			
			Canvas.Children.Add(marker);
			Canvas.Background = new SolidColorBrush(Colors.Black);
			
			int divisions = (int)Math.Ceiling (width / PixelsPerDivision) * 4;
			for(int i=0; i <= (divisions + 1) * 4; i++)
			{
				divisionMarkers.Add(new TimelineMarker(0, TimeSpan.Zero));
				Canvas.Children.Add((Visual)divisionMarkers[i]);
			}
			
			divisions /= 4;
			for(int i=0; i <= divisions + 1; i++)
			{
				divisionTextblocks.Add(new TextBlock());
				Canvas.Children.Add(divisionTextblocks[i]);
			}
			
			PlaceDivisions();
		}
		
		private void PlaceDivisions()
		{
			// This is a hack so i can figure out what 'height' the textblocks are going to be
			// so i can perform my layout magic
			TextBlock b = new TextBlock(); b.Text = "3";
			double height = Allocation.Height - b.ActualHeight;
			
			// Calculate the next 'division' that we need to draw. It can be either
			// at 250ms, 500ms, 750ms, or 0/1000 ms. This rounds up to the nearest 250.
			long currentTime = (((long)startTime.TotalMilliseconds + 125) / 250) * 250;
			long endTime = (long)startTime.TotalMilliseconds + (long)(Allocation.Width / PixelsPerDivision) * 1000;

			int currentMarker = 0;
			int currentTextblock = 0;
			TextBlock block = null;
			for(long i = currentTime; i <=  endTime; i += 250, block = null)
			{
				IMarker marker = this.divisionMarkers[currentMarker++];
				marker.Time = TimeSpan.FromMilliseconds(i);
				
				switch(i % 1000)
				{
				case 0:
					marker.Height = height;
					block = divisionTextblocks[currentTextblock++];
					break;
				case 250:
				case 750:
					marker.Height = height / 4.0;
					break;
				case 500:
					marker.Height = height / 2.0;
					break;
				}
				
				PlaceMarker(marker, block);
			}
			
			// Push any of the unused markers or blocks outside of the visible area
			// on the canvas.
			while(currentMarker < divisionMarkers.Count)
				divisionMarkers[currentMarker++].Left = -200;
			
			while(currentTextblock < divisionTextblocks.Count)
				divisionTextblocks[currentTextblock++].SetValue<object>(System.Windows.Controls.Canvas.LeftProperty, -200);
			
			// Make sure that the position marker is placed correctly on
			// the canvas
			this.marker.Height = height;
			this.marker.Width = height / 8.0;
			PlaceMarker(this.marker, null);
			
			// Make sure all the keyframes are in the right place
			for(int i=0; i < keyframeMarkers.Count; i++)
				PlaceMarker(keyframeMarkers[i], null);
		}
		
		private void PlaceMarker(IMarker marker, TextBlock block)
		{
			TimeSpan difference = marker.Time - startTime;
			marker.Left = difference.TotalSeconds * PixelsPerDivision - marker.Width / 2;
			if(block == null)
				return;
			
			block.Text = marker.Time.ToString();
			block.Text = block.Text.Substring(block.Text.IndexOf(':') + 1);
			block.SetValue<double>(System.Windows.Controls.Canvas.LeftProperty, marker.Left - block.ActualWidth / 2.0);
			block.SetValue<double>(System.Windows.Controls.Canvas.TopProperty, Allocation.Height - block.ActualHeight);
			block.SetValue<Brush>(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.White));
		}
		
		private void MouseDown(object sender, MouseEventArgs e)
		{
			if(started)
				return;
			
			started = true;
			location = e.GetPosition(Canvas);
			startLocation = location;
			Canvas.CaptureMouse();
		}
		
		private void MouseMove(object sender, MouseEventArgs e)
		{
			IMarker marker = clickedItem;
			
			if(!started)
				return;
			
			Point offset = e.GetPosition(Canvas);
			location.X -= offset.X;
			location.Y -= offset.Y;
			
			TimeSpan difference = TimeSpan.FromSeconds(location.X / PixelsPerDivision);
			
			// This means that we clicked and dragged directly on the timeline
			if(this.clickedItem == null)
			{
				if(startTime.TotalMilliseconds + difference.TotalMilliseconds < 0)
					startTime = TimeSpan.Zero;
				else
					startTime = startTime.Add(difference);
			}
			// This means we tried to move either a position marker, keyframe
			// marker or whatever
			else
			{
				TimeSpan oldtime = marker.Time;
				if(marker.Time.TotalMilliseconds - difference.TotalMilliseconds < 0)
					marker.Time = TimeSpan.Zero;
				else
					marker.Time = marker.Time.Subtract(difference);
				
				if(marker is KeyframeMarker)
					KeyframeMoved(this, new KeyframeEventArgs((KeyframeMarker)marker, oldtime));
				else if (marker == this.marker)
					RaiseCurrentPositionChanged();
			}
			
			location = offset;
			PlaceDivisions();
		}
		
		public TimeSpan GetKeyframeBefore(TimeSpan span)
		{
			TimeSpan prev = TimeSpan.Zero;
			for(int i=0; i < this.keyframeMarkers.Count; i++)
				if(keyframeMarkers[i].Time >= prev && keyframeMarkers[i].Time <= span)
					prev = keyframeMarkers[i].Time;
			
			Console.WriteLine("Previous one is: {0}", prev);
			return prev;
		}
		
		private void MouseUp(object sender, MouseEventArgs e)
		{
			if(!started)
				return;

			bool moved = !startLocation.Equals(e.GetPosition(Canvas));
			if(!moved)
			{
				// If we clicked on a keyframe, we set the current position equal to the keyframes time.
				// Otherwise place the position marker at the point of clicking
				if(clickedItem is KeyframeMarker)
					marker.Time = clickedItem.Time;
				else
					marker.Time = startTime.Add(TimeSpan.FromSeconds(startLocation.X / AnimationTimeline.PixelsPerDivision));
				RaiseCurrentPositionChanged();
				
				PlaceMarker(marker, null);
			}
			
			started = false;
			clickedItem = null;
			Canvas.ReleaseMouseCapture();
		}
		
		public void AddKeyframe(Timeline timeline, TimeSpan time)
		{
			KeyframeMarker marker = new KeyframeMarker(timeline, time);

			// If we already have a marker at the same time for the same timeline, 
			// do not add another keyframe marker
			foreach(KeyframeMarker m in keyframeMarkers)
				if(m.Time.Equals(marker.Time))
				{
					Console.WriteLine("Already found: {0}", marker.Time);
					return;
				}
			Console.WriteLine("Adding to timeline: {0}", time);
			marker.Time = time;
			marker.Width = 15;
			marker.Height = 15;
			marker.SetValue<double>(System.Windows.Controls.Canvas.TopProperty, (this.Allocation.Height - 15.0) / 2.0);
			marker.MouseLeftButtonDown += delegate (object sender, MouseEventArgs e) {
				this.clickedItem = (IMarker)sender;
			};
			
			keyframeMarkers.Add(marker);
			Canvas.Children.Add(marker);
			PlaceMarker(marker, null);
		}
		
		public void RemoveSelectedKeyframe()
		{
			if(clickedItem is KeyframeMarker)
			{
				Canvas.Children.Remove((Visual)clickedItem);
				keyframeMarkers.Remove((KeyframeMarker)clickedItem);
			}
		}
		
		public void RemoveKeyframe(KeyframeMarker marker)
		{
			Canvas.Children.Remove((Visual)marker);
			this.keyframeMarkers.Remove(marker);
		}
		
		private void RaiseCurrentPositionChanged()
		{
			if(CurrentPositionChanged != null)
				CurrentPositionChanged(this, EventArgs.Empty);
		}
	}
}
