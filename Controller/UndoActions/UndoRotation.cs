

using System;
using System.Windows.Controls;
using System.Windows.Media;
using LunarEclipse.Model;

namespace LunarEclipse.Controller
{
    
    
    public class UndoRotation : UndoPropertyChange
    {
		private Visual Visual
		{
			get { return (Visual)Target; }
		}
		
        public UndoRotation(Visual visual, double initialAngle, double finalAngle)
			:base(visual, null, initialAngle, finalAngle)
        {

        }
        
        public override void Redo ()
        {
			TransformGroup g = (TransformGroup)Visual.GetValue(Canvas.RenderTransformProperty);
			RotateTransform t = (RotateTransform)g.Children[(int)TransformType.Rotate];
			t.Angle = (double)NewValue;
		}
        
		public override void Undo ()
		{
			TransformGroup g = (TransformGroup)Visual.GetValue(Canvas.RenderTransformProperty);
			RotateTransform t = (RotateTransform)g.Children[(int)TransformType.Rotate];
			t.Angle = (double)OldValue;
        }
		
    }
}
