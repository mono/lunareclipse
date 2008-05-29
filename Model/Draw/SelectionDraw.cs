//
// SelectionDraw.cs
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
using System.Windows.Input;
using System.Collections.Generic;
using LunarEclipse.Controller;
using LunarEclipse;

namespace LunarEclipse.Model
{
    public class SelectionDraw : Selector
	{
		private UndoGroup undos;
		
		public SelectionDraw(MoonlightController controller)
			: base(controller)
		{
			this.ChangedHeight += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			this.ChangedLeft += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			this.ChangedRotation += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			this.ChangedTop += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			this.ChangedWidth += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
		}

		
		public override bool CanUndo 
		{
			get { return false; }
		}
			
		internal override void DrawEnd (MouseEventArgs point)
		{
			base.DrawEnd(point);
			
			if(undos.Count == 0)
				return;
			
			Controller.UndoEngine.PushUndo(undos);
			undos = new UndoGroup();
		}
		
		internal override void DrawStart (Panel panel, MouseEventArgs point)
		{
			base.DrawStart(panel, point);
			undos = new UndoGroup();
		}
		
		private void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			undos.Add(new UndoPropertyChange(e.Target, e.Property, e.OldValue, e.NewValue));
		}
	}
}
