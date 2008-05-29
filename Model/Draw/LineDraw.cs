//
// LineDraw.cs
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
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;


namespace LunarEclipse.Model
{
    public class LineDraw : DrawBase
    {
        public LineDraw()
            : base(new System.Windows.Shapes.Line())
        {
            
        }
        
        internal override void Prepare ()
        {
            base.Prepare();
            Element.SetValue(Shape.FillProperty, new SolidColorBrush(Colors.Red));
        }

        internal override void DrawStart (Panel panel, MouseEventArgs e)
        {
            base.DrawStart(panel, e);
            Line l = Element as Line;
            l.X1 = (l.X2 = Position.X);
            l.Y1 = (l.Y2 = Position.Y);
            l.SetValue(Canvas.TopProperty, 0);
            l.SetValue(Canvas.LeftProperty, 0);
        }
        
        internal override void MouseMove (MouseEventArgs end)
        {
            Line l = Element as Line;
            Point p = end.GetPosition(Panel);
            l.X2 = p.X;
            l.Y2 = p.Y;
        }
		
		internal override void DrawEnd (MouseEventArgs point)
		{
		}

    }
}
