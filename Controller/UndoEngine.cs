//
// UndoEngine.cs
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
using LunarEclipse.Controller;

namespace LunarEclipse
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
			Console.WriteLine("Added: " + action.ToString());
			if(action is UndoGroup)
				Console.WriteLine("Count: {0}", ((UndoGroup)action).Count);
			
            this.undo.Push(action);
            RaiseUndoAdded();
            
            if(clearRedo)
            {
                this.redo.Clear();
                RaiseRedoRemoved();
            }
			
			Console.WriteLine("Undos: {0}", undo.Count);
        }
        
        private void PushRedo(UndoActionBase action)
        {
            this.redo.Push(action);
            RaiseRedoAdded();
			Console.WriteLine("Redos: {0}", redo.Count);
        }
        
        private UndoActionBase PopRedo()
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
			RaiseUndoRemoved();
            this.redo.Clear();
            RaiseRedoRemoved();
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
