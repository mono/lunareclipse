// ResizeControlPoint.cs
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
using System.Windows.Input;
using System.Windows.Controls;
using Gtk.Moonlight;

namespace LunarEclipse.Model {	
	
	public class ResizeControlPoint: AbstractControlPoint {
		
		public ResizeControlPoint(GtkSilver silver, UIElement element): base(silver, element)
		{
			Width = DefaultWidth;
			Height = DefaultHeight;
		}
		
		public override string GetXaml ()
		{
			return "<Rectangle Fill=\"#FFFF0000\" Stroke=\"#FF000000\" />";
		}
		
		protected override void MouseStart (object sender, MouseEventArgs args)
		{
			base.MouseStart (sender, args);
			lastPoint = args.GetPosition(null);
			drag = true;
			
		}
		
		protected override void MouseStep (object sender, MouseEventArgs args)
		{
			base.MouseStep (sender, args);
			if (!drag)
				return;
			Point currentPoint = args.GetPosition(null);
			Point offset = new Point(0.0, 0.0);
			offset.X = currentPoint.X - lastPoint.X;
			offset.Y = currentPoint.Y - lastPoint.Y;
			
			System.Console.WriteLine(offset);
			
			Left += offset.X;
			Top += offset.Y;
			
			MoveElement(offset);
			
			lastPoint = currentPoint;
		}
		
		protected override void MouseEnd (object sender, MouseEventArgs args)
		{
			base.MouseEnd (sender, args);
			drag = false;
		}

		private void MoveElement(Point offset)
		{
			double left = (double) Element.GetValue(Canvas.LeftProperty);
			double top = (double) Element.GetValue(Canvas.TopProperty);
			
			Element.SetValue(Canvas.LeftProperty, left + offset.X);
			Element.SetValue(Canvas.TopProperty, top + offset.Y);
		}
		
		public const double DefaultWidth = 10.0;
		public const double DefaultHeight = 10.0;
		
		private Point lastPoint;
		private bool drag = false;
	}
}
