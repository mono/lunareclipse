//
// PropertyManager.cs
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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

using LunarEclipse.Controller;
using LunarEclipse.Model;
using LunarEclipse.Serialization;

namespace LunarEclipse.Model
{
	public class PropertyManager
	{
		private List<PropertyInfo> properties;
		private UIElement selected_object;
		private static Dictionary<DependencyProperty, PropertyInfo> info;
		
		
		public bool HasProperties
		{
			get { return properties.Count != 0; }
		}
		
		public IList<PropertyInfo> Properties
		{
			get { return properties; }
		}
		
		public virtual UIElement SelectedObject
		{
			get { return selected_object; }
		}

		public PropertyManager(MoonlightController controller)
		{
			properties = new List<PropertyInfo>();
			
			controller.Selection.SelectionChanged += OnSelectionChanged;
		}
		
		static PropertyManager()
		{
			info = new Dictionary<DependencyProperty, PropertyInfo>();
			info.Add(Rectangle.OpacityProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.OpacityProperty), PropertyType.Percent));
			info.Add(Rectangle.VisibilityProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.VisibilityProperty), PropertyType.Visibility));
			info.Add(Rectangle.RadiusXProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.RadiusXProperty), PropertyType.Double));
			info.Add(Rectangle.RadiusYProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.RadiusYProperty), PropertyType.Double));
			info.Add(Rectangle.StrokeThicknessProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.StrokeThicknessProperty), PropertyType.Double));
			info.Add(Rectangle.NameProperty, new PropertyInfo(ReflectionHelper.GetData(Canvas.NameProperty), PropertyType.String));
			info.Add(Path.DataProperty, new PropertyInfo(ReflectionHelper.GetData(Path.DataProperty), PropertyType.Data));
			
			// extended properties
			info.Add(Path.StretchProperty, new PropertyInfo(ReflectionHelper.GetData(Path.StretchProperty), PropertyType.Stretch, false, true));
			info.Add(Path.StrokeDashArrayProperty, new PropertyInfo(ReflectionHelper.GetData(Path.StrokeDashArrayProperty), PropertyType.DashArray, false, true));
			info.Add(Path.StrokeDashCapProperty, new PropertyInfo(ReflectionHelper.GetData(Path.StrokeDashCapProperty), PropertyType.PenLineCap, false, true));
			info.Add(Path.StrokeDashOffsetProperty, new PropertyInfo(ReflectionHelper.GetData(Path.StrokeDashOffsetProperty), PropertyType.Double, false, true));
			//info.Add(Path.StrokeStartLineCapProperty, new PropertyInfo(ReflectionHelper.GetData(Path.StrokeStartLineCapProperty), PropertyType.stroke, false, true));
			//info.Add(Path.StrokeEndLineCapProperty, new PropertyInfo(ReflectionHelper.GetData(Path.StrokeEndLineCapProperty), PropertyType.str, false, true));
			info.Add(Path.StrokeLineJoinProperty, new PropertyInfo(ReflectionHelper.GetData(Path.StrokeLineJoinProperty), PropertyType.PenLineJoin, false, true));
			info.Add(Path.StrokeMiterLimitProperty, new PropertyInfo(ReflectionHelper.GetData(Path.StrokeMiterLimitProperty), PropertyType.Double, false, true));
			
			info.Add(Canvas.WidthProperty, new PropertyInfo(ReflectionHelper.GetData(Canvas.WidthProperty), PropertyType.Double, false, true));
			info.Add(Canvas.HeightProperty, new PropertyInfo(ReflectionHelper.GetData(Canvas.HeightProperty), PropertyType.Double, false, true));
			info.Add(Canvas.LeftProperty, new PropertyInfo(ReflectionHelper.GetData(Canvas.LeftProperty), PropertyType.Double, true, false));
			info.Add(Canvas.TopProperty, new PropertyInfo(ReflectionHelper.GetData(Canvas.TopProperty), PropertyType.Double, true, false));
			info.Add(Canvas.ZIndexProperty, new PropertyInfo(ReflectionHelper.GetData(Canvas.ZIndexProperty), PropertyType.Integer, false, false));
			
			info.Add(UIElement.RenderTransformOriginProperty, new PropertyInfo(ReflectionHelper.GetData(Canvas.RenderTransformOriginProperty), PropertyType.Point));
			info.Add(Shape.FillProperty, new PropertyInfo(ReflectionHelper.GetData(Shape.FillProperty), PropertyType.Brush));
			info.Add(Shape.StrokeProperty, new PropertyInfo(ReflectionHelper.GetData(Shape.StrokeProperty), PropertyType.Brush));
			info.Add(Canvas.BackgroundProperty, new PropertyInfo(ReflectionHelper.GetData(Canvas.BackgroundProperty), PropertyType.Brush));
		}
		
		private void OnSelectionChanged(object sender, EventArgs args)
		{
			ISelection selection = sender as ISelection;
			selected_object = selection.MainElement;
			UpdateProperties();
		}
		
		private void UpdateProperties()
		{
			PropertyInfo item;
			properties.Clear();
			
			if(SelectedObject == null)
				return;
			
			List<PropertyData> props = ReflectionHelper.GetProperties(SelectedObject);
			foreach(PropertyData data in props)
				if(info.TryGetValue(data.Property, out item))
					properties.Add(item);
			else
				Console.WriteLine("Item not usuable: {0}", data.ShortName);
		}
	}
}
