using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Controls;

namespace LunarEclipse.Serialization
{	
    internal static class ReflectionHelper
    {
		private static List<PropertyData> attachedProperties;
		private static Dictionary<Type, List<PropertyData>> allProperties;
		
		static ReflectionHelper()
        {
			allProperties = new Dictionary<Type, List<PropertyData>>();
			attachedProperties = new List<PropertyData>();
			SetUpList();
        }
		
		private static void SetUpList()
		{
			DependencyObject instance = null;
			Assembly current = Assembly.GetAssembly(typeof(DependencyProperty));
			Type[] types = current.GetTypes();
			
			// For every type in the assembly, check to see if it is instantiable and it
			// has dependency properties. If both conditions are true, instantiate it and 
			// grab all it's dependency properties into our list.
			foreach(Type type in types)
			{
				if(type.IsGenericTypeDefinition || type.IsAbstract || !type.IsPublic)
					continue;
				
				// If i can't instantiate the object, that doesn't matter.
				try { instance = Activator.CreateInstance(type) as DependencyObject; }
				catch { instance = null; }

				if(instance == null)
					continue;
				
				// Add any attached properties into a special list so all types can get access to it
				List<PropertyData> fields = FindFields(instance);
				foreach(PropertyData property in fields)
					if(property.Attached && !attachedProperties.Contains(property))
						attachedProperties.Add(property);
				
				allProperties.Add(type, fields);
			}
		}
		
        public static List<PropertyData> GetProps(DependencyObject item)
		{
			return GetProps(item, true);
		}
		
		public static List<PropertyData> GetProps(DependencyObject item, bool withAttached)
		{
			List<PropertyData> result;
			List<PropertyData> properties = allProperties[item.GetType()];
			
			if(withAttached)
			{
				result = new List<PropertyData>(properties.Count + attachedProperties.Count);
				result.AddRange(attachedProperties);
			}
			else
			{
				result = new List<PropertyData>(properties.Count);
			}
			
			result.AddRange(properties);
			return result;
		}
		
		private static List<PropertyData> FindFields(DependencyObject item)
        {
			Type baseType = item.GetType();
			Type current = baseType;
			List<PropertyData> fields = new List<PropertyData>();
			
            // We get all the DependencyProperty fields in the current type
			// and it's parent type, and keep going up the tree until
			// we reach the top of the inheritence tree
			while(current != null)
			{
                FieldInfo[] currentFields = current.GetFields();
				foreach(FieldInfo field in currentFields)
				{
					if(!field.FieldType.Equals(typeof(DependencyProperty)))
						continue;
					
					// NOTE: My definition of an 'attached' property is a DependencyProperty
					// which has no normal CLR Property (with a get/set) to expose it.
					DependencyProperty property = (DependencyProperty)field.GetValue(item);
					bool attached = current.GetProperty(Serializer.CleanName(field.Name)) == null;
					fields.Add(new PropertyData(baseType, current, property, field, attached));
				}
				current = current.BaseType;
			}
			
			return fields;
		}
		
		public static string GetFullPath(DependencyObject target, DependencyProperty property)
		{
			string result = null;
			Type targetType = target.GetType();
			List<PropertyData> properties = ReflectionHelper.GetProps(target, true);
			
			for(int i=0; i < properties.Count; i++)
			{
				if(properties[i].Property != property)
					continue;
				
				result += '(';
				if(properties[i].Attached)
					result += properties[i].DeclaringType.Name;
				else
					result += targetType.Name;
				result += '.';
				result += Serializer.CleanName(properties[i].PropertyInfo.Name);
				result += ')';
			}
			
			return result;
		}
    }
}   

