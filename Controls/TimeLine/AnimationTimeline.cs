// Timeline.cs created with MonoDevelop
// User: alan at 3:44 PM 7/20/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using LunarEclipse.View;


namespace LunarEclipse.View
{
	public class AnimationTimeline : GtkSilver
	{
		private const double PixelsPerDivision = 80;
		
		private IMarker clickedItem;
		private Point location;
		private PositionMarker marker;
		private bool started;
		private TimeSpan startTime;
		private Point startLocation;
		
		
		public AnimationTimeline(int width, int height)
			:base (width, height)
		{
			Canvas c = new Canvas();
			c.Width = width;
			c.Height = height;
			Attach(c);
			marker = new PositionMarker(TimeSpan.Zero, 0, 0);
			startTime = TimeSpan.Zero;
			marker.MouseLeftButtonDown += delegate { clickedItem = marker; };
			
			Canvas.MouseLeftButtonDown += new MouseEventHandler(MouseDown);
			Canvas.MouseMove += new MouseEventHandler(MouseMove);
			Canvas.MouseLeftButtonUp += new MouseEventHandler(MouseUp);
			
			Canvas.Background = new SolidColorBrush(Colors.Black);
			DrawDivisions();
		}
		
		private void DrawDivisions()
		{
			TextBlock b = new TextBlock(); b.Text = "3";
			TimelineMarker marker = null;
			double height = Height - b.ActualHeight;
			
			// Calculate the next 'division' that we need to draw. It can be either
			// at 250ms, 500ms, 750ms, or 0/1000 ms.
			long currentTime = startTime.Seconds * 1000 + ((startTime.Milliseconds + 125) / 250) * 250;
			long endTime = (long)startTime.TotalMilliseconds + (long)(Width / PixelsPerDivision) * 1000;
			
			for(long i = currentTime; i <=  endTime; i += 250)
			{
				TimeSpan time = TimeSpan.FromMilliseconds(i);
				switch(i % 1000)
				{
					case 0:
						marker = new TimelineMarker(height, time);
						break;
					case 250:
					case 750:
						marker = new TimelineMarker(height / 4, time);
						break;
					case 500:
						marker = new TimelineMarker(height / 2, time);
						break;
				}

				AddMarker(marker);
			}
			
			this.marker.Height = height;
			this.marker.Width = height / 8.0;
			AddMarker(this.marker);
		}
		
		private void AddMarker(IMarker marker)
		{
			TimeSpan difference = marker.Time - startTime;
			double left = difference.TotalSeconds * PixelsPerDivision;
			marker.Left = left - marker.Width / 2;
			Canvas.Children.Add((Visual)marker);
			
			// Only add the timestamp for integer seconds
			if(marker.Height <= Height / 2.0 || !(marker is TimelineMarker))
				return;
			
			TextBlock block = new TextBlock();
			block.Text = marker.Time.ToString();
			block.Text = block.Text.Substring(block.Text.IndexOf(':') + 1);
			block.SetValue<double>(System.Windows.Controls.Canvas.LeftProperty, left - block.ActualWidth / 2);
			block.SetValue<double>(System.Windows.Controls.Canvas.TopProperty, Height - block.ActualHeight);
			Canvas.Children.Add(block);
			block.Foreground = new SolidColorBrush(Colors.White);
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
				if(marker.Time.TotalMilliseconds - difference.TotalMilliseconds < 0)
					marker.Time = TimeSpan.Zero;
				else
					marker.Time = marker.Time.Subtract(difference);
			}
			
			location = offset;
			Canvas.Children.Clear();
			DrawDivisions();
		}
		
		private void MouseUp(object sender, MouseEventArgs e)
		{
			if(!started)
				return;

			bool moved = !startLocation.Equals(e.GetPosition(Canvas));
			if(!moved)
			{
				this.marker.Time = this.startTime.Add(TimeSpan.FromSeconds(startLocation.X / AnimationTimeline.PixelsPerDivision));
				Canvas.Children.Remove(this.marker);
				AddMarker(this.marker);
			}
			
			started = false;
			this.clickedItem = null;
			Canvas.ReleaseMouseCapture();
		}
	}
}
