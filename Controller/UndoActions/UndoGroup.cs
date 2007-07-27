// UndoGroup.cs created with MonoDevelop
// User: alan at 1:29 PM 7/27/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace LunarEclipse.Controller
{
	public class UndoGroup : UndoActionBase
	{
		private List<UndoActionBase> undos;
		
		public List<UndoActionBase> Undos
		{
			get { return undos; }
		}
		
		public UndoGroup()
		{
			undos = new List<UndoActionBase>();
		}
		
		public override void Undo ()
		{
			for(int i = undos.Count - 1; i >= 0; i--)
				undos[i].Undo();
		}
		
		public override void Redo ()
		{
			for (int i= 0; i < undos.Count; i++)
				undos[i].Redo();
		}
	}
}
