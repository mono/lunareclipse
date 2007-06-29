// /home/alan/Projects/LunarEclipse/Controller/UndoActions/UndoMoveShape.cs created with MonoDevelop
// User: alan at 2:20 PM 6/29/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LunarEclipse
{
    
    
    public class UndoMoveShape : UndoActionBase
    {
        private Point offset;
        private Shape shape;
        
        
        public UndoMoveShape(Shape shape, Point offset)
        {
            this.shape = shape;
            this.offset = offset;
        }
        
        public override void Redo ()
        {
            Point location = new Point((double)shape.GetValue(Canvas.LeftProperty), (double)shape.GetValue(Canvas.TopProperty));
            shape.SetValue<double>(Canvas.LeftProperty, location.X - offset.X);
            shape.SetValue<double>(Canvas.TopProperty, location.Y - offset.Y);
        }

        
        public override void Undo ()
        {
            Point location = new Point((double)shape.GetValue(Canvas.LeftProperty), (double)shape.GetValue(Canvas.TopProperty));
            shape.SetValue<double>(Canvas.LeftProperty, location.X + offset.X);
            shape.SetValue<double>(Canvas.TopProperty, location.Y + offset.Y);
        }
    }
}
