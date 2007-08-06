using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Controls;
using PropertyPairList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.Type, System.Reflection.FieldInfo>>;

namespace LunarEclipse.Serialization
{
	public class PropertyData
	{
		private List<Type> alternateTypes;
		private Type baseType;
		private Type declaringType;
		private DependencyProperty property;
		private FieldInfo propertyInfo;

		
		public List<Type> AlternateTypes
		{
			get { return alternateTypes; }
		}
		
		public Type BaseType
		{
			get { return baseType; } 
		}
		
		public Type DeclaringType
		{
			get { return declaringType; }
		}
		
		public DependencyProperty Property
		{
			get { return property; }
		}
		
		public FieldInfo PropertyInfo
		{
			get { return propertyInfo; }
		}


		public PropertyData(Type baseType, Type declaringType, DependencyProperty property, FieldInfo propertyInfo)
		{
			this.alternateTypes = new List<Type>();
			this.baseType = baseType;
			this.declaringType = declaringType;
			this.property = property;
			this.propertyInfo = propertyInfo;
		}
	}
	
    internal class ReflectionHelper
    {
		private static Dictionary<DependencyProperty, PropertyData> allProperties;
        private static Dictionary<Type, List<PropertyData>> cachedFields;
        
        
        static ReflectionHelper()
        {
			allProperties = new Dictionary<DependencyProperty, PropertyData>();
            cachedFields = new Dictionary<Type, List<PropertyData>>();
			SetUpList();
        }
		
		private static void SetUpList()
		{
			PropertyData data = null;
			DependencyObject instance = null;
			Assembly current = Assembly.GetAssembly(typeof(System.Windows.DependencyProperty));
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
				
				// For each of the FieldInfo's representing dependency properties, grab the instance
				// of the dependency property and add it to the dictionary
				List<PropertyData> fields = GetDependencyProperties(instance);
				foreach(PropertyData keypair in fields)
				{
					DependencyProperty val = keypair.Property;
					
					// If we don't already have this property in the dictionary, add it
					if(!allProperties.TryGetValue(val, out data))
					{
						data = new PropertyData(keypair.BaseType, keypair.DeclaringType, keypair.Property, keypair.PropertyInfo);
						allProperties.Add(val, data);
					}
					
					if(!data.DeclaringType.Equals(type) && !data.AlternateTypes.Contains(type))
						data.AlternateTypes.Add(type);
				}
			}
		}
		
        public static List<PropertyData> GetProps(DependencyObject item)
		{
			PropertyData[] data = new PropertyData[allProperties.Count];
			allProperties.Values.CopyTo(data, 0);
			return new List<PropertyData>(data);
		}
		
        private static List<PropertyData> GetDependencyProperties(DependencyObject item)
        {
            lock(cachedFields)
            {
				Type itemType = item.GetType();
                List<PropertyData> cached = null;
                List<PropertyData> fields = new List<PropertyData>();

                // Get all the fields for the type of this item
                if(!cachedFields.ContainsKey(itemType))
                    cachedFields.Add(itemType, FindFields(item));
                fields.AddRange(cachedFields[itemType]);
                
                return fields;
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
                    if(field.FieldType.Equals(typeof(DependencyProperty)))
                        fields.Add(new PropertyData(baseType, current, (DependencyProperty)field.GetValue(item), field));

                current = current.BaseType;
            }
            return fields;
        }
		
		private static bool HasProperty(Type type, DependencyProperty property)
		{
			return false;
		}
		
		public static string GetFullPath(DependencyObject target, DependencyProperty property)
		{
			string result = null;
			lock(cachedFields)
			{
				Type targetType = target.GetType();
				PropertyData t;
				if(!allProperties.TryGetValue(property, out t))
					return null;
				
				result += '(';
				if(t.AlternateTypes.Contains(targetType))
					result += targetType.Name;
				else
					result += t.DeclaringType.Name;
				result += '.';
				result += Serializer.CleanName(t.PropertyInfo.Name);
				result += ')';
			}
			return result;
		}
    }
}   

