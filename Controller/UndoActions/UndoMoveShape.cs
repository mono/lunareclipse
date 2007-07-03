// /home/alan/Projects/LunarEclipse/Controller/UndoActions/UndoMoveShape.cs created with MonoDevelop
// User: alan at 2:20 PM 6/29/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LunarEclipse
{
    public class UndoMoveShape : UndoActionBase
    {
        private Point offset;
        private Visual[] shapes;
        
        internal UndoMoveShape(Visual[] shapes, Point offset)
        {
            this.shapes = shapes;
            this.offset = offset;
        }
        
        
        public override void Redo ()
        {
            foreach(Visual shape in shapes)
            {
                Point location = new Point((double)shape.GetValue(Canvas.LeftProperty), (double)shape.GetValue(Canvas.TopProperty));
                shape.SetValue<double>(Canvas.LeftProperty, location.X - offset.X);
                shape.SetValue<double>(Canvas.TopProperty, location.Y - offset.Y);
            }
        }


        public override void Undo ()
        {
            foreach(Visual shape in shapes)
            {
                Point location = new Point((double)shape.GetValue(Canvas.LeftProperty), (double)shape.GetValue(Canvas.TopProperty));
                shape.SetValue<double>(Canvas.LeftProperty, location.X + offset.X);
                shape.SetValue<double>(Canvas.TopProperty, location.Y + offset.Y);
            }
        }
    }
}
