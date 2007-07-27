// /home/alan/Projects/LunarEclipse/Controller/UndoActions/AddObjectAction.cs created with MonoDevelop
// User: alan at 3:46 PM 6/26/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Windows;
namespace LunarEclipse.Controller
{
    public class UndoAddObject : UndoActionBase
    {
        private DependencyObject collection;
        private DependencyObject item;
        
        public UndoAddObject(DependencyObject collection, DependencyObject item)
        {
            this.collection = collection;
            this.item = item;
        }
        
        public override void Redo ()
        {
            Type t = collection.GetType();
            t.GetMethod("Add").Invoke(collection, new object[] { item });
        }
        
        public override void Undo ()
        {
            Type t = collection.GetType();
            t.GetMethod("Remove").Invoke(collection, new object[] { item });
        }
    }
}
