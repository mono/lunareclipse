//
// SelectionRectangle.cs
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
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using Gtk.Moonlight;

namespace LunarEclipse.Model
{
    public class SelectionRectangle : Control
    {
		private Rectangle rect;
		
        public SelectionRectangle()
            :base()
        {
			// FIXME: Find a proper way to do this.
			GtkSilver silver = new GtkSilver(); 
			rect = (Rectangle)silver.InitializeFromXaml("<Rectangle Name=\"Rect\" />", this);
			
            rect.Opacity = 0.33;
            rect.Fill = new SolidColorBrush(Colors.Blue);
            rect.Stroke = new SolidColorBrush(Colors.Green);
            rect.StrokeDashArray = new double[] {5, 5 };
            rect.StrokeThickness = 2;
            rect.SetValue(ZIndexProperty, int.MaxValue);
        }
		
		public override void SetValue (DependencyProperty property, object obj)
		{
			if(property == Shape.WidthProperty || property == Shape.HeightProperty)
				rect.SetValue(property, obj);
			
			base.SetValue(property, obj);
		}

    }
}
