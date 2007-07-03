

using System;
using System.Windows;
using System.Windows.Media;
namespace LunarEclipse
{
    public class ZIndexComparer : System.Collections.Generic.IComparer<Visual>
    {
        public ZIndexComparer()
        {

        }

        public int Compare(Visual left, Visual right)
        {
            //Console.WriteLine("Compare Start");
            int leftValue = (int)left.GetValue(UIElement.ZIndexProperty);
            int rightValue = (int)right.GetValue(UIElement.ZIndexProperty);
            return leftValue.CompareTo(rightValue);
        }
    }
}
