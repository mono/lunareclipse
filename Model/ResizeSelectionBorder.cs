// ResizeSelectionBorder.cs
//
// Author:
//   Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 Manuel Cerón.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Gtk.Moonlight;

namespace LunarEclipse.Model {	
	
	public class ResizeSelectionBorder: AbstractSelectionBorder {
		
		public ResizeSelectionBorder(GtkSilver silver, Visual child): base (silver, child)
		{
			Handles.Add(new ResizeControlPoint(silver, this));
			frame = CreateFrame();
		}
		
		public override void Update ()
		{
			SnapFrame();
		}

		
		private Rect GetChildBounds()
		{
			double top = (double) Child.GetValue(TopProperty);
			double left = (double) Child.GetValue(LeftProperty);
			double width = (double) Child.GetValue(WidthProperty);
			double height = (double) Child.GetValue(HeightProperty);
			
			return new Rect(left, top, width, height);
		}
		
		private Rectangle CreateFrame()
		{
			Rectangle r = new Rectangle();
			r.StrokeThickness = DefaultStrokeThickness;
			r.Stroke = new SolidColorBrush(DefaultStrokeColor);
			
			return r;
		}
		
		private void SnapFrame()
		{
			Rect bounds = GetChildBounds();
			
			
			//TODO: Candidate for Helper
			frame.SetValue(TopProperty, bounds.Top);
			frame.SetValue(LeftProperty, bounds.Left);
			frame.Width = bounds.Width;
			frame.Height = bounds.Height;
		}
		
		//TODO: This should be private in the future because of bug 386468
		public void AddFrame()
		{
			Update();
			Children.Add(frame);
			foreach (IControlPoint cp in Handles)
				Children.Add((Visual)cp);			
		}

		public const double DefaultStrokeThickness = 1;
		public Color DefaultStrokeColor = Colors.Black;
			
		private Rectangle frame;
	}
}
