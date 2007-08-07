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
using LunarEclipse.View;


namespace LunarEclipse.View
{
	public class AnimationTimeline : GtkSilver
	{
		public event EventHandler CurrentPositionChanged;
		
		private const double PixelsPerDivision = 80;
		
		private IMarker clickedItem;
		private Point location;
		private PositionMarker marker;
		private bool started;
		private TimeSpan startTime;
		private Point startLocation;
		
		private List<IMarker> divisionMarkers;
		private List<TextBlock> divisionTextblocks;
		
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
			
			startTime = TimeSpan.Zero;
			marker.MouseLeftButtonDown += delegate { clickedItem = marker; };
			
			Canvas.MouseLeftButtonDown += new MouseEventHandler(MouseDown);
			Canvas.MouseMove += new MouseEventHandler(MouseMove);
			Canvas.MouseLeftButtonUp += new MouseEventHandler(MouseUp);
			
			Canvas.Children.Add(marker);
			Canvas.Background = new SolidColorBrush(Colors.Black);
			
			int divisions = (int)Math.Ceiling(Width / PixelsPerDivision) * 4;
			for(int i=0; i <= divisions * 4; i++)
			{
				divisionMarkers.Add(new TimelineMarker(0, TimeSpan.Zero));
				Canvas.Children.Add((Visual)divisionMarkers[i]);
			}
			
			divisions /= 4;
			for(int i=0; i <= divisions; i++)
			{
				divisionTextblocks.Add(new TextBlock());
				Canvas.Children.Add(divisionTextblocks[i]);
			}
			
			PlaceDivisions();
		}
		
		private void PlaceDivisions()
		{
			TextBlock b = new TextBlock(); b.Text = "3";
			double height = Height - b.ActualHeight;
			
			// Calculate the next 'division' that we need to draw. It can be either
			// at 250ms, 500ms, 750ms, or 0/1000 ms.
			long currentTime = (((long)startTime.TotalMilliseconds + 125) / 250) * 250;
			long endTime = (long)startTime.TotalMilliseconds + (long)(Width / PixelsPerDivision) * 1000;

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
				divisionTextblocks[currentTextblock++].SetValue<double>(System.Windows.Controls.Canvas.LeftProperty, -200);
			
			// Make sure that the position marker is placed correctly on
			// the canvas
			this.marker.Height = height;
			this.marker.Width = height / 8.0;
			PlaceMarker(this.marker, null);
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
			block.SetValue<double>(System.Windows.Controls.Canvas.TopProperty, Height - block.ActualHeight);
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
			PlaceDivisions();
		}
		
		private void MouseUp(object sender, MouseEventArgs e)
		{
			if(!started)
				return;

			bool moved = !startLocation.Equals(e.GetPosition(Canvas));
			if(!moved)
			{
				marker.Time = startTime.Add(TimeSpan.FromSeconds(startLocation.X / AnimationTimeline.PixelsPerDivision));
				if(this.CurrentPositionChanged != null)
					CurrentPositionChanged(this, EventArgs.Empty);
				
				PlaceMarker(marker, null);
			}
			
			started = false;
			clickedItem = null;
			Canvas.ReleaseMouseCapture();
		}
	}
}
