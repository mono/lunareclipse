// UndoGroup.cs created with MonoDevelop
// User: alan at 1:29 PM 7/27/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Collections;

namespace LunarEclipse.Controller
{
	public class UndoGroup : UndoActionBase, IEnumerable<UndoActionBase>
	{
		private List<UndoActionBase> undos;
		
		public int Count
		{
			get { return undos.Count; }
		}
		
		public UndoGroup()
			: base(null)
		{
			undos = new List<UndoActionBase>();
		}
		
		public void Add(UndoActionBase undo)
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

		System.Collections.IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		public System.Collections.Generic.IEnumerator<UndoActionBase> GetEnumerator ()
		{
				for(int i=0; i < undos.Count; i++)
				yield return undos[i];
		}

	}
}