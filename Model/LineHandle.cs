// LineHandle.cs
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
using System.Windows.Shapes;
using Gtk.Moonlight;

namespace LunarEclipse.Model {
	
	public abstract class LineHandle: AbstractHandle	{
		
		public const double DefaultRadius = 4.0;
		
		public LineHandle(GtkSilver silver, IHandleGroup group):
			base(silver, group)
		{
		}
		
		public override void Update ()
		{
			Point position = LinePoint;
			CanvasAllocation = new Rect(position.X - DefaultRadius,
			                            position.Y - DefaultRadius,
			                            DefaultRadius * 2,
			                            DefaultRadius * 2);
		}
		
		public override void MouseStep (object sender, MouseEventArgs args)
		{
			base.MouseStep (sender, args);
			
			if (!Dragging)
				return;
			
			Point offset = CalculateOffset(args.GetPosition(null));			
			
			Point newpoint = LinePoint;
			
			newpoint.X += offset.X;
			newpoint.Y += offset.Y;
			
			LinePoint = newpoint;
			
			Update();
		}
		
		protected Line LineElement {
			get { return Element as Line; }
		}
		
		protected override string GetXaml ()
		{
			return "<Rectangle Fill=\"#99FFFF00\" Stroke=\"#FF000000\"/>";
		}

		protected abstract Point LinePoint {
			get;
			set;
		}
	}
}
