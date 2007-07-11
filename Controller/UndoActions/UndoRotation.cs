

using System;
using System.Windows.Controls;
using System.Windows.Media;
namespace LunarEclipse
{
    
    
    public class UndoRotation : UndoActionBase
    {
        private double angle;
        private double initialAngle;
        private Visual visual;
        
        public UndoRotation(Visual visual, double initialAngle, double angle)
        {
            this.angle = angle;
            this.initialAngle = initialAngle;
            this.visual = visual;
        }
        
        public override void Redo ()
        {
            RotateTransform t = visual.GetValue(Canvas.RenderTransformProperty) as RotateTransform;
            if(t != null)
                t.Angle = angle;
        }
        
        public override void Undo ()
        {
            RotateTransform t = visual.GetValue(Canvas.RenderTransformProperty) as RotateTransform;
            if(t != null)
                t.Angle = initialAngle;
        }
    }
}
