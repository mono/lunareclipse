using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections.Generic;
using PropertyPairList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.Type, System.Reflection.FieldInfo>>;

namespace LunarEclipse
{
    internal class ReflectionHelper
    {
        private static Dictionary<Type, PropertyPairList> cachedFields;
        
        
        static ReflectionHelper()
        {
            cachedFields = new Dictionary<Type, PropertyPairList>();
        }
        
        
        public static PropertyPairList GetDependencyProperties(DependencyObject item)
        {
            lock(cachedFields)
            {
                Type itemType = item.GetType();
                PropertyPairList cached;
                PropertyPairList fields = new PropertyPairList();

                // Get all the fields for the type of this item
                if(!cachedFields.ContainsKey(itemType))
                    cachedFields.Add(itemType, FindFields(itemType));
                fields.AddRange(cachedFields[itemType]);
                
                // Check to see if there are any attached properties from
                // a parent element which need to added to the list aswell
                FrameworkElement e = item as FrameworkElement;
                while(e != null && e.Parent != null)
                {
                    Type parentType = e.Parent.GetType();
                    
                    if(!cachedFields.ContainsKey(parentType))
                        cachedFields.Add(parentType, FindFields(parentType));       
                    
                    cached = cachedFields[parentType];
                    foreach(KeyValuePair<Type, FieldInfo> keypair in cached)
                        if(!ContainsField(fields, keypair))
                            fields.Add(keypair);
                    
                    e = e.Parent as FrameworkElement;
                }
                
                return fields;
            }
        }
        
        private static bool ContainsField(PropertyPairList fields, KeyValuePair<Type, FieldInfo> item)
        {
            foreach(KeyValuePair<Type, FieldInfo> keypair in fields)
                if(keypair.Value.Equals(item.Value))
                    return true;
            
            return false;
        }

        private static PropertyPairList FindFields(Type type)
        {
            Type current = type;
            PropertyPairList fields = new PropertyPairList();
            
            // We get all the DependencyProperty fields in the current type
            // and it's parent type, and keep going up the tree until
            // we reach the top of the inheritence tree
            while(current != null)
            {
                FieldInfo[] currentFields = current.GetFields();
                foreach(FieldInfo field in currentFields)
                    if(field.FieldType.Equals(typeof(DependencyProperty)))
                        fields.Add(new KeyValuePair<Type, FieldInfo>(type, field));

                current = current.BaseType;
            }
            return fields;
        }
		
		public static string GetFullPath(DependencyObject target, DependencyProperty property)
		{
			lock(cachedFields)
			{
				if(property == System.Windows.Controls.Canvas.TopProperty)
					return "(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)";
				
				if(property == System.Windows.Controls.Canvas.LeftProperty)
					return "(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)";
			}
			return null;
		}
    }
}   

