// /home/alan/Projects/LunarEclipse/Controller/UndoActions/UndoActionBase.cs created with MonoDevelop
// User: alan at 3:47 PM 6/26/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;

namespace LunarEclipse.Controller
{
    public abstract class UndoActionBase
    {
		private DependencyObject target;
		
		internal DependencyObject Target
		{
			get { return target; }
			set { target = value; }
		}
        
		protected UndoActionBase(DependencyObject target)
		{
			this.target = target;
		}
		
		public abstract void Undo();
		public abstract void Redo();
	}
}
