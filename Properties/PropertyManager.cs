// /cvs/lunareclipse/Properties/PropertyGroup.cs created with MonoDevelop
// User: fejj at 2:27 PM 6/27/2007
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
		public event EventHandler<EventArgs> SelectionChanged;
		
		private string name;
		private List<PropertyInfo> properties;
		private SelectedBorder selectedObject;
		private Selector selector;
		private static Dictionary<DependencyProperty, PropertyInfo> info;
		
		
		public bool HasProperties
		{
			get { return properties.Count != 0; }
		}
		
		public IList<PropertyInfo> Properties
		{
			get { return properties; }
		}
		
		public virtual SelectedBorder SelectedObject
		{
			get { return selectedObject; }
			set
			{
				selectedObject = value;
				UpdateProperties();
				Toolbox.RaiseEvent<EventArgs>(SelectionChanged, this, EventArgs.Empty);
			}
		}

		public PropertyManager(MoonlightController controller)
		{
			properties = new List<PropertyInfo>();
			
			controller.BeforeDrawChanged += delegate {
				selector = controller.Current as Selector;
				if(selector == null)
					return;
				
				selector.ItemSelected -= ItemSelectionChanged;
				selector.ItemDeselected -= ItemSelectionChanged;
			};
			
			controller.DrawChanged += delegate {
				selector = controller.Current as Selector;
				if(selector == null)
					return;
				
				selector.ItemSelected += ItemSelectionChanged;
				selector.ItemDeselected +=ItemSelectionChanged;
			};
			
		}
		private void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(selector.SelectedObjects.Count != 1)
				SelectedObject = null; 
			else
				SelectedObject = e.SelectedBorder;
			Toolbox.RaiseEvent<EventArgs>(SelectionChanged, this, EventArgs.Empty);
		}
		
		static PropertyManager()
		{
			info = new Dictionary<DependencyProperty, PropertyInfo>();
			info.Add(Rectangle.OpacityProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.OpacityProperty), PropertyType.Percent));
			info.Add(Rectangle.VisibilityProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.VisibilityProperty), PropertyType.Visibility));
			info.Add(Rectangle.RadiusXProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.RadiusXProperty), PropertyType.Double));
			info.Add(Rectangle.RadiusYProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.RadiusYProperty), PropertyType.Double));
			info.Add(Rectangle.StrokeThicknessProperty, new PropertyInfo(ReflectionHelper.GetData(Rectangle.StrokeThicknessProperty), PropertyType.Double));
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
		}
		
		private void UpdateProperties()
		{
			PropertyInfo item;
			properties.Clear();
			
			if(this.selectedObject == null)
				return;
			
			List<PropertyData> props = ReflectionHelper.GetProperties(this.selectedObject.Child);
			foreach(PropertyData data in props)
				if(info.TryGetValue(data.Property, out item))
					properties.Add(item);
			else
				Console.WriteLine("Item not usuable: {0}", data.ShortName);
			
			Console.WriteLine("WAHEY: {0}", properties.Count);
		}
	}
}
