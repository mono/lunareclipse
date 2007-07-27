

using System;
using System.Windows.Controls;
using System.Windows.Media;
using LunarEclipse.Model;

namespace LunarEclipse.Controller
{
    
    
    public class UndoRotation : UndoActionBase
    {
        private double finalAngle;
        private double initialAngle;
        private Visual visual;
        
        public UndoRotation(Visual visual, double initialAngle, double finalAngle)
        {
            this.finalAngle = finalAngle;
            this.initialAngle = initialAngle;
            this.visual = visual;
        }
        
        public override void Redo ()
        {
			TransformGroup g = (TransformGroup)visual.GetValue(Canvas.RenderTransformProperty);
			RotateTransform t = (RotateTransform)g.Children[(int)TransformType.Rotate];
			t.Angle = finalAngle;
		}
        
		public override void Undo ()
		{
			TransformGroup g = (TransformGroup)visual.GetValue(Canvas.RenderTransformProperty);
			RotateTransform t = (RotateTransform)g.Children[(int)TransformType.Rotate];
			t.Angle = initialAngle;
        }
    }
}
