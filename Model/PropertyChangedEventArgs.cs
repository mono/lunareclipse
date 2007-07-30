// PropertyChangedEventArgs.cs created with MonoDevelop
// User: alan at 1:08 PM 7/30/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;

namespace LunarEclipse.Model
{
	public class PropertyChangedEventArgs : EventArgs
	{
		private DependencyObject target;
		private DependencyProperty property;
		private object oldValue;
		private object newValue;
		
		
		public object NewValue
		{
			get { return newValue; }
		}
		
		public object OldValue
		{
			get { return oldValue; }
		}
		
		public DependencyProperty Property
		{
			get { return property; }
		}
		
		public DependencyObject Target
		{
			get { return target; }
		}
		
		public PropertyChangedEventArgs(DependencyObject target, DependencyProperty property, object oldValue, object newValue)
		{
			this.newValue = newValue;
			this.oldValue = oldValue;
			this.property = property;
			this.target = target;
		}
	}
}
