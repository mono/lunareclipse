// /home/alan/Projects/LunarEclipse/LunarEclipse/Model/Draw/PenDraw.cs created with MonoDevelop
// User: alan at 4:11 PM 6/20/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows;
using System.Windows.Shapes;
using LunarEclipse.Model;
using System.Windows.Media;

namespace LunarEclipse
{
    public class PenDraw : DrawBase
    {
        PathGeometry geometry;
        PathFigure figure;
        
        public PenDraw(Point startPoint)
            :base(startPoint, new System.Windows.Shapes.Path())
        {
            Path path = (Path)Element;
            path.Stroke = new SolidColorBrush(Colors.Black);
            path.StrokeThickness = 1;
            path.Fill = new SolidColorBrush(Colors.Black);
                                        
            geometry = new PathGeometry();
            figure = new PathFigure();
            
            path.Data = geometry;
            geometry.Figures = new PathFigureCollection();
            geometry.Figures.Add(figure);
            
            figure.Segments = new PathSegmentCollection();
            figure.StartPoint = startPoint;
        }
        
        public override void Resize (Point end)
        {
            Console.WriteLine("Adding pen segment");
            LineSegment seg = new LineSegment();
            seg.Point = end;
            figure.Segments.Add(seg);
        }
    }
}
