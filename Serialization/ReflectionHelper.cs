//
// ReflectionHelper.cs
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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LunarEclipse.Serialization
{	
    internal static class ReflectionHelper
    {
		// This is a list of all 'attached' properties
		private static List<PropertyData> attachedProperties;
		
		// This dictionary stores a list of all dependencyproperties exposed by a DependencyObject
		// and is keyed by the System.Type of the DependencyObject.
		// i.e. allProperties[typeof(System.Windows.Controls.Canvas)] will return a list of all dependency properties
		// exposed by Canvas.
		private static Dictionary<Type, List<PropertyData>> allProperties;
		
		// This dictionary is keyed by the 'fully' qualified name of the dependency property
		// i.e. the string "Canvas.LeftProperty" is paired with the DependencyProperty 'Canvas.LeftProperty' etc
		// Note, the properties are stored by both the type the DependencyProperty is declared in
		// and the Type it is accessible from i.e. 'UIElement.Visibility' and 'Rectangle.Visibility'
		// both point towards the same thing
		private static Dictionary<string, DependencyProperty> propertyByName;
		
		// This dictionary contains all the PropertyData objects except it is 
		// keyed by DependencyProperty
		private static Dictionary<DependencyProperty, PropertyData> propertyDataByProperty;
		
		static ReflectionHelper()
		{
			allProperties = new Dictionary<Type, List<PropertyData>>();
			attachedProperties = new List<PropertyData>();
			propertyByName = new Dictionary<string, DependencyProperty>();
			propertyDataByProperty = new Dictionary<DependencyProperty, PropertyData>();
			SetUpList();
		}
		
		public static PropertyData GetData(DependencyProperty property)
		{
			return propertyDataByProperty[property];
		}
		
		public static List<PropertyData> GetProperties(DependencyObject item)
		{
			return GetProperties(item, true);
		}
		
		
		public static List<PropertyData> GetProperties(DependencyObject item, bool withAttached)
		{
			List<PropertyData> result;
			Type t = item.GetType();
			if(!allProperties.ContainsKey(t))
				return new List<PropertyData>();
			
			List<PropertyData> properties = allProperties[item.GetType()];
			
			result = new List<PropertyData>();
			
			if(withAttached)
				result.AddRange(attachedProperties);

			
			foreach (PropertyData prop in properties) {
				if (!prop.Attached)
					result.Add(prop);
			}			
			
			return result;
		}
		

		public static string GetFullPath(DependencyObject target, DependencyProperty property)
		{
			if(target == null)
				throw new ArgumentNullException("target");
			
			if(property == null)
				throw new ArgumentNullException("property");
			
			StringBuilder result = new StringBuilder(64);
			
			Type targetType = target.GetType();
			List<PropertyData> properties = ReflectionHelper.GetProperties(target, true);
			
			if(SpecialCase(target, property, result))
				return result.ToString();
			
			for(int i=0; i < properties.Count; i++)
			{
				if(properties[i].Property != property)
					continue;
				
				result.Append('(');
				if(properties[i].Attached)
					result.Append(properties[i].DeclaringType.Name);
				else
					result.Append(targetType.Name);
				result.Append('.');
				result.Append(properties[i].ShortName);
				result.Append(')');
				break;
			}
			
			return result.ToString();
		}

		public static void Resolve(string propertyPath, DependencyObject original, out DependencyObject target, out DependencyProperty property)
		{
			if(string.IsNullOrEmpty(propertyPath))
				throw new ArgumentException("propertyPath cannot be null or empty");
			
			if(original == null)
				throw new ArgumentNullException("original");
			
			// Initially we assume that the final object we will be applying the property to is the same as
			// the original object
			target = original;
			
			// Match a string like (a.b).(c.d).(e.f)[4]  or  (a.b)[4].(c.d)[2].(e.f)
			// and split it into it's componant parts. In the case of the second string, i'll get this
			// (a.b)[4]   and    (c.d)[2]   and (e.f)
			Regex e = new Regex("(\\([a-zA-Z]+.[a-zA-Z]+\\)(\\[\\d+\\])?)");
			MatchCollection matches = e.Matches(propertyPath);
			
			for(int i=0; i < matches.Count; i++)
			{
				// Get the property that is targeted by this string
				string[] parts = matches[i].Value.Split(')');
				string propertyName = parts[0].Substring(1, parts[0].Length - 1);
				property = ReflectionHelper.propertyByName[propertyName];

				// When this condition is true, we know we have found the target object and the property
				// which we need to apply to the object
				if(i == (matches.Count - 1))
					return;

				target = (DependencyObject)target.GetValue(property);
				
				
				if(parts.Length > 1 && parts[1].Length > 2)
					target = GetFromIndex(parts[1], target);
			}

			Console.WriteLine("Couldn't resolve");
			target = null;
			property = null;
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
			
			foreach(List<PropertyData> list in allProperties.Values)
				foreach(PropertyData propData in list)
				{
					string name = propData.DeclaringType.Name + '.' + propData.ShortName;
					if(!propertyByName.ContainsKey(name))
						propertyByName.Add(name, propData.Property);
					
					name = propData.BaseType.Name + '.' + propData.ShortName;
					if(!propertyByName.ContainsKey(name))
						propertyByName.Add(name, propData.Property);
				
					if(!propertyDataByProperty.ContainsKey(propData.Property)) {
					    if (propData.Property == null) {
						    continue;
					    }
						propertyDataByProperty.Add(propData.Property, propData);
				    }
				}
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
		
		private static DependencyObject GetFromIndex(string s, DependencyObject o)
		{
			s = s.Substring(1, s.Length - 2);
			int i = 0;
			int index = int.Parse(s);
			IEnumerable enumerable = (IEnumerable)o;
			foreach(object obj in enumerable)
			{
				if(i++ != index)
					continue;
				return (DependencyObject)obj;
			}
			
			return null;
		}
		
		private static bool SpecialCase(DependencyObject target, DependencyProperty property, StringBuilder sb)
		{
			// Special casing is a bitch. Recording animations becomes really hard if i use this.
			// It's going to require a lot more thought to get right
			return false;
			if(property == Canvas.LeftProperty)
				sb.Append("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)");
			else if(property == Canvas.TopProperty)
				sb.Append("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)");
			
			return sb.Length != 0;
		}
    }
}   

