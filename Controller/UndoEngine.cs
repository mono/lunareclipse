// /home/alan/Projects/LunarEclipse/Controller/UndoEngine.cs created with MonoDevelop
// User: alan at 3:42 PM 6/26/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using DesignerMoon.Controller;

namespace DesignerMoon
{
    public class UndoEngine
    {
        public event EventHandler UndoAdded;
        public event EventHandler RedoAdded;
        public event EventHandler UndoRemoved;
        public event EventHandler RedoRemoved;
        
        private Stack<UndoActionBase> undo;
        private Stack<UndoActionBase> redo;
        
        public int RedoCount
        {
            get { return this.redo.Count;}
        }
        
        public int UndoCount
        {
            get { return this.undo.Count;}
        }
        
        internal UndoEngine()
        {
            redo = new Stack<UndoActionBase>();
            undo = new Stack<UndoActionBase>();
        }
        
        
        internal void PushUndo(UndoActionBase action)
        {
            PushUndo(action, true);
        }
        
        internal void PushUndo(UndoActionBase action, bool clearRedo)
        {
            this.undo.Push(action);
            RaiseUndoAdded();
            
            if(clearRedo)
            {
                this.redo.Clear();
                RaiseRedoRemoved();
            }
        }
        
        internal void PushRedo(UndoActionBase action)
        {
            this.redo.Push(action);
            RaiseRedoAdded();
        }
        
        internal UndoActionBase PopRedo()
        {
            UndoActionBase b = this.redo.Pop();
            RaiseRedoRemoved();
            return b;
        }
        
        internal UndoActionBase PopUndo()
        {
            UndoActionBase b = this.undo.Pop();
            this.RaiseUndoRemoved();
            return b;
        }
        
        internal void Redo()
        {
            UndoActionBase b = PopRedo();
            b.Redo();
            PushUndo(b, false);
        }
        
        internal void Undo()
        {
            UndoActionBase b = PopUndo();
            b.Undo();
            PushRedo(b);
        }
        
        internal void Clear()
        {
            this.undo.Clear();
            this.redo.Clear();
        }
        
        
        private void RaiseUndoAdded()
        {
            if(this.UndoAdded != null)
                this.UndoAdded(this, EventArgs.Empty);
        }
        
        private void RaiseRedoAdded()
        {
            if(this.RedoAdded != null)
                this.RedoAdded(this, EventArgs.Empty);
        }
        
        private void RaiseUndoRemoved()
        {
            if(this.UndoRemoved != null)
                this.UndoRemoved(this, EventArgs.Empty);
        }
        
        private void RaiseRedoRemoved()
        {
            if(this.RedoRemoved != null)
                this.RedoRemoved(this, EventArgs.Empty);
        }
    }
}
