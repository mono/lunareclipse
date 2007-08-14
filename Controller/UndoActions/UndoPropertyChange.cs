// /home/alan/Projects/LunarEclipse/Controller/UndoActions/ChangePropertyAction.cs created with MonoDevelop
// User: alan at 3:46 PM 6/26/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;

namespace LunarEclipse.Controller
{
    public class UndoPropertyChange : UndoActionBase
    {
        private DependencyProperty property;
        private object oldvalue;
        private object newvalue;
		
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
        
        public UndoPropertyChange(DependencyObject item, DependencyProperty property, object oldvalue, object newvalue)
			:base(item)
		{
            this.property = property;
            this.oldvalue = oldvalue;
            this.newvalue = newvalue;
        }
        
        public override void Redo ()
        {
			Toolbox.ChangeProperty(Target, property, newvalue);
        }

        
        public override void Undo ()
        {
			Toolbox.ChangeProperty(Target, property, oldvalue);
        }
    }
}
