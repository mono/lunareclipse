// /home/alan/Projects/DesignerMoon/DesignerMoon/IDraw.cs created with MonoDevelop
// User: alan at 6:02 PM 6/15/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace DesignerMoon.Model
{
    public abstract class DrawBase
    {
        private Shape uiElement;
        private Point endPoint;
        private Point startPoint;

        
        public Point Start
        {
            get { return startPoint; }
            set { startPoint = value; }
        }
        
        public Point End
        {
            get { return endPoint; }
        }
        
        public Shape Element
        {
            get { return uiElement; }
        }
        
        internal DrawBase(Point startPoint, Shape element)
        {
            this.startPoint = startPoint;
            uiElement = element;
            
            uiElement.Stroke = new SolidColorBrush(Colors.Red);
        }
        
        public virtual void Resize(Point end)
        {
            uiElement.Width = end.X - Start.X;
            uiElement.Height = end.Y - Start.Y;
        }
        
        public virtual DrawBase Clone()
        {
            return (DrawBase)Activator.CreateInstance(this.GetType(), new object[] { this.startPoint});
        }
    }
}
