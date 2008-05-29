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
using System.Windows.Input;
using Gtk.Moonlight;

namespace LunarEclipse.Model {	
	
	public class ResizeSelectionBorder: AbstractSelectionBorder {
		
		public ResizeSelectionBorder(GtkSilver silver, Visual child): base (silver, child)
		{
			//TODO: Probabley duplicate code from AbstractControlPoint
					
			Handles.Add(new ResizeControlPoint(silver, this));
			CreateFrame();
			Update();
		}
		
		public override void Update ()
		{
			SnapToChild();
		}

		
		private Rect GetChildBounds()
		{
			double top = (double) Child.GetValue(TopProperty);
			double left = (double) Child.GetValue(LeftProperty);
			double width = (double) Child.GetValue(WidthProperty);
			double height = (double) Child.GetValue(HeightProperty);
			
			return new Rect(left, top, width, height);
		}
		
		private void CreateFrame()
		{
			frame = new Rectangle();
			frame.StrokeThickness = DefaultStrokeThickness;
			frame.Stroke = new SolidColorBrush(DefaultStrokeColor);
		}
		
		private void SnapToChild()
		{
			Rect r = GetChildBounds();
			//TODO: Candidate for Helper
			frame.SetValue(TopProperty, r.Top);
			frame.SetValue(LeftProperty, r.Left);
			frame.Width = r.Width;
			frame.Height = r.Height;
			
			SetValue(TopProperty, r.Top);
			SetValue(LeftProperty, r.Left);
			SetValue(WidthProperty, r.Width);
			SetValue(HeightProperty, r.Height);
		}
		
		//TODO: This should be private in the future because of bug 386468
		public void AddFrame()
		{
			Update();
			Children.Add(frame);
			foreach (IControlPoint cp in Handles)
				Children.Add((Visual)cp);			
		}
		
		public const double DefaultStrokeThickness = 4;
		public Color DefaultStrokeColor = Colors.Black;

		private Rectangle frame;
	}
}
