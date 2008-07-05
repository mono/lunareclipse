//
// UndoPropertyChange.cs
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

namespace LunarEclipse.Controller
{
    public class UndoPropertyChange : AbstractUndoAction
    {
        private DependencyObject root;
		private DependencyProperty property;
        private object oldvalue;
        private object newvalue;
		private bool silent;
		
		public object OldValue
		{
			get { return oldvalue; }
			set { oldvalue = value; }
		}
		public object NewValue
		{
			get { return newvalue; }
			set { newvalue = value; }
		}
		
		public DependencyProperty TargetProperty
		{
			get { return property; }
		}

		public bool Silent
		{
			get { return silent; }
		}
		
		public UndoPropertyChange(DependencyObject item, DependencyProperty property, object oldvalue, object newvalue)
			: this(item, item, property, oldvalue, newvalue, false)
		{
        }
        
        public UndoPropertyChange(DependencyObject root, DependencyObject item, DependencyProperty property, object oldvalue, object newvalue)
			: this(root, item, property, oldvalue, newvalue, false)
		{	
        }
		
		public UndoPropertyChange(DependencyObject root, DependencyObject item, DependencyProperty property, object oldvalue, object newvalue, bool silent)
			:base(item)
		{
			this.root = root;
            this.property = property;
            this.oldvalue = oldvalue;
            this.newvalue = newvalue;
			this.silent = silent;
        }
        
        public override void Redo ()
        {
			if(silent)
				Target.SetValue(property, newvalue);
			else
				Toolbox.ChangeProperty((UIElement)root, Target, property, newvalue);
        }

        
        public override void Undo ()
        {
			if(silent)
				Target.SetValue(property, oldvalue);
			else
				Toolbox.ChangeProperty((UIElement)root, Target, property, oldvalue);
        }
    }
}
