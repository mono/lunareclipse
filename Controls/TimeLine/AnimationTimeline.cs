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
		
		private DependencyObject clickedItem;
		private Point location;
		private TimelineMarker marker;
		private bool started;
		private TimeSpan startTime;

		
		
		public AnimationTimeline(int width, int height)
			:base (width, height)
		{
			Canvas c = new Canvas();
			c.Width = width;
			c.Height = height;
			Attach(c);
			//marker = new TimelineMarker();
			startTime = TimeSpan.Zero;
			//marker.MouseLeftButtonDown += delegate { clickedItem = marker; };
			
			//Canvas.Children.Add(marker);
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
		}
		
		private void AddMarker(TimelineMarker marker)
		{
			TimeSpan difference = marker.Time - startTime;
			double left = difference.TotalSeconds * PixelsPerDivision;
			marker.SetValue<double>(System.Windows.Controls.Canvas.LeftProperty, left);
			Canvas.Children.Add(marker);
			
			// Only add the timestamp for integer seconds
			if(marker.Height <= Height / 2.0)
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
				
			if(startTime.TotalMilliseconds + difference.TotalMilliseconds < 0)
				startTime = TimeSpan.Zero;
			else
				startTime = startTime.Add(difference);
			
			location = offset;
			Canvas.Children.Clear();
			DrawDivisions();
		}
		
		private void MouseUp(object sender, MouseEventArgs e)
		{
			if(!started)
				return;
			
			started = false;
			Canvas.ReleaseMouseCapture();
		}
	}
}
