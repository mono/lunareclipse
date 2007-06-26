// /home/alan/Projects/LunarEclipse/Controller/UndoActions/ChangePropertyAction.cs created with MonoDevelop
// User: alan at 3:46 PM 6/26/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;

namespace DesignerMoon
{
    public class UndoPropertyChange : UndoActionBase
    {
        private DependencyObject item;
        private DependencyProperty property;
        private object oldvalue;
        private object newvalue;
        
        public UndoPropertyChange(DependencyObject item, DependencyProperty property, object oldvalue, object newvalue)
        {
            this.item = item;
            this.property = property;
            this.oldvalue = oldvalue;
            this.newvalue = newvalue;
        }
        
        public override void Redo ()
        {
            item.SetValue<object>(property, newvalue);
        }

        
        public override void Undo ()
        {
            item.SetValue<object>(property, oldvalue);
        }
    }
}
