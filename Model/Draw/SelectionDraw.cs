using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;
using LunarEclipse.Controller;
using LunarEclipse;

namespace LunarEclipse.Model
{
    public class SelectionDraw : Selector
	{
		private UndoGroup undos;
		
		public SelectionDraw(MoonlightController controller)
			: base(controller)
		{
			this.ChangedHeight += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			this.ChangedLeft += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			this.ChangedRotation += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			this.ChangedTop += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
			this.ChangedWidth += new EventHandler<PropertyChangedEventArgs>(PropertyChanged);
		}

		
		public override bool CanUndo 
		{
			get { return false; }
		}
			
		internal override void DrawEnd (MouseEventArgs point)
		{
			base.DrawEnd(point);
			
			if(undos.Count == 0)
				return;
			
			Controller.UndoEngine.PushUndo(undos);
			undos = new UndoGroup();
		}
		
		internal override void DrawStart (Panel panel, MouseEventArgs point)
		{
			base.DrawStart(panel, point);
			undos = new UndoGroup();
		}
		
		private void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			undos.Add(new UndoPropertyChange(e.Target, e.Property, e.OldValue, e.NewValue));
		}
	}
}
