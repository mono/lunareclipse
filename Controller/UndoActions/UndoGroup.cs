//
// UndoGroup.cs
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
using System.Collections.Generic;
using System.Collections;
using System.Windows;

namespace LunarEclipse.Controller
{
	public class UndoGroup : AbstractUndoAction, IEnumerable<IUndoAction>
	{
		private List<IUndoAction> undos;
		
		public int Count
		{
			get { return undos.Count; }
		}
		
		public UndoGroup()
			: base(null)
		{
			undos = new List<IUndoAction>();
		}
		
		public void AddPropertyChange(DependencyObject root, DependencyObject item, DependencyProperty prop, object oldValue, object newValue)
		{
			IUndoAction undo = new UndoPropertyChange(root, item, prop, oldValue, newValue);
			Add(undo);
		}
		
		public void AddPropertyChange(DependencyObject item, DependencyProperty prop, object oldValue, object newValue)
		{
			AddPropertyChange(item, item, prop, oldValue, newValue);
		}
		
		public void Add(IUndoAction undo)
		{
			// We need to do a check to see if this property has already been changed
			// in this group. If it has, then we update the existing undo, otherwise
			// we add it to the group
			if(undo is UndoPropertyChange)
			{
				if(!UpdateExisting((UndoPropertyChange)undo))
					undos.Add(undo);
			}
			else
			{
				undos.Add(undo);
			}
		}
		
		public void Clear()
		{
			this.undos.Clear();
		}
		
		public override void Redo ()
		{
			for (int i= 0; i < undos.Count; i++)
				undos[i].Redo();
		}
		
		public override void Undo ()
		{
			for(int i = undos.Count - 1; i >= 0; i--)
				undos[i].Undo();
		}
		
		private bool UpdateExisting(UndoPropertyChange undo)
		{
			for(int i=0; i < this.undos.Count; i++)
			{
				UndoPropertyChange existing = undos[i] as UndoPropertyChange;
				if(existing == null 
				   || !existing.GetType().Equals(undo.GetType())
				   || existing.Target != undo.Target
				   || existing.TargetProperty != undo.TargetProperty)
					continue;
				
				existing.NewValue = undo.NewValue;
				return true;
			}
			
			return false;
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)undos).GetEnumerator();
		}
		public System.Collections.Generic.IEnumerator<IUndoAction> GetEnumerator ()
		{
			return undos.GetEnumerator();
		}

	}
}