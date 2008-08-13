//
// Toolbox.cs
//
// Authors:
//   Alan McGovern alan.mcgovern@gmail.com
//
// Copyright (C) 2007 Alan McGovern
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LunarEclipse.Model;
using LunarEclipse.Controller;

namespace LunarEclipse
{
	public static class Toolbox
	{
		// TODO: move this to controller
		public static event EventHandler<PropertyChangedEventArgs> PropertyChanged;

		public static void ChangeProperty(UIElement element, DependencyObject target, DependencyProperty property, object value)
		{
			PropertyChangedEventArgs e = new PropertyChangedEventArgs(element, target, property, target.GetValue(property), value);
			target.SetValue(property, value);
			RaiseEvent<PropertyChangedEventArgs>(PropertyChanged, target, e);
		}
		
		public static void ChangeProperty(DependencyObject target, DependencyProperty property, object value)
		{
			ChangeProperty(target as UIElement, target, property, value);
		}
		
		public static double DegreesToRadians(double angle)
		{
			return (angle * Math.PI) / 180.0 ;
		}
		
		public static double RadiansToDegrees(double angle)
		{
			return (angle / Math.PI) * 180.0;
		}
		
		public static void NormalizeRect(ref double x, ref double y, ref double width, ref double height)
		{
			if (width < 0) {
				width = -width;
				x -= width;
			}
			
			if (height < 0) {
				height = -height;
				y -= height;
			}
		}
		
		internal static void RaiseEvent<T>(EventHandler<T> e, object sender, T args) where T : EventArgs
		{
			if(e != null)
				e(sender, args);
		}
		
		public static int MaxZ(UIElementCollection elements)
		{
			if (elements.Count == 0)
				return 0;
			
			int max = int.MinValue;
			
			foreach (UIElement element in elements) {
				int z = 0; //(int) element.GetValue(Canvas.ZIndexProperty);
				max = Math.Max(max, z);
			}
			
			return max;
		}
		
		public static int MinZ(UIElementCollection elements)
		{
			if (elements.Count == 0)
				return 0;
		
			int min = int.MaxValue;
			
			foreach (UIElement element in elements) {
				int z = 0; //(int) element.GetValue(Canvas.ZIndexProperty);
				min = Math.Max(min, z);
			}
			
			return min;
		}
	}
}
