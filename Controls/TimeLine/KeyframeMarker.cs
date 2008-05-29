//
// KeyframeMarker.cs
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
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Gtk.Moonlight;

namespace LunarEclipse.Controls
{
#warning FIXME
	public class KeyframeMarker : Control, IMarker
	{
		private Ellipse ellipse;
		private TimeSpan time;
		private Timeline timeline;
		
		public double Left
		{
			get { return (double)GetValue(Canvas.LeftProperty); }
			set { SetValue(Canvas.LeftProperty, value); }
		}
		
		public TimeSpan Time
		{
			get { return time; }
			set { time = value; }
		}
		
		public Timeline Timeline
		{
			get { return timeline; }
		}
		
		public KeyframeMarker(GtkSilver parent, Timeline timeline, TimeSpan time)
			: base()
		{
			ellipse = (Ellipse) parent.InitializeFromXaml("<Ellipse Name=\"Ellipse\"/>", this);
			ellipse.SetValue(Ellipse.FillProperty, new SolidColorBrush(Colors.Green)); 
			this.time = time;
			this.timeline = timeline;
		}
		
		public override void SetValue (DependencyProperty property, object obj)
		{
			if(property == Shape.WidthProperty || property == Shape.HeightProperty)
				ellipse.SetValue(property, obj);
			
			base.SetValue(property, obj);
		}
	}
}
