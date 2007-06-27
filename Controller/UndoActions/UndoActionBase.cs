// /home/alan/Projects/LunarEclipse/Controller/UndoActions/UndoActionBase.cs created with MonoDevelop
// User: alan at 3:47 PM 6/26/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace LunarEclipse
{
    public abstract class UndoActionBase
    {
        protected UndoActionBase()
        {
        }
        
        public abstract void Undo();
        public abstract void Redo();
    }
}
