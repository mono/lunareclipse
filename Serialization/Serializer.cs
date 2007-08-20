// /home/alan/Projects/LunarEclipse/Serialization/Serializer.cs created with MonoDevelop
// User: alan at 2:19 PM 6/25/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LunarEclipse.Model;


namespace LunarEclipse.Serialization
{
    internal class Serializer
    {
		private Canvas canvas;
        private Dictionary<Type, DependencyObject> defaultValues;
		
        internal Serializer()
        {
            defaultValues = new Dictionary<Type, DependencyObject>();
        }

        internal static string CleanName(string fieldName)
        {
            // The name of all DependencyProperties ends in "Property", i.e.
            // NameProperty, FillProperty. this method removes "Property" from
            // the end of each name so i serialise the correct name into XAML
            return fieldName.Substring(0, fieldName.Length - 8);
        }
        
        private void SerialiseCollection(PropertyData propertyData, object value, XmlWriter writer)
        {
            IEnumerable collection = (IEnumerable)value;
            
            //FIXME: Nasty hack as the collections don't have a common interface
            IEnumerator enumerator = collection.GetEnumerator();
            enumerator.Reset();
			if(!enumerator.MoveNext())
				return;

			// We need to write the full qualified name for correct serializing, meaning:
			// <TransformGroup.Children><TransformCollection>..... /> <TransformGroup.Children/>
			string name = null;
			if(propertyData.Attached)
				name = propertyData.DeclaringType.Name;
			else
				name = propertyData.BaseType.Name;
			
			name += '.' + CleanName(propertyData.PropertyInfo.Name);
			writer.WriteStartElement(name);
			writer.WriteStartElement(value.GetType().Name);
			
            foreach(DependencyObject o in collection)
                Serialize(o, writer);
			
			writer.WriteEndElement();
			writer.WriteEndElement();
        }
        
        public string Serialize(Canvas canvas)
        {
			this.canvas = canvas;
			
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings(); 
            settings.OmitXmlDeclaration=true;
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.NewLineOnAttributes = false;
            using(XmlWriter writer = XmlWriter.Create(sb, settings))
                Serialize(canvas, writer);
            
            Regex r = new Regex("<Canvas");
            return r.Replace(sb.ToString(), "<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"", 1);
        }
        
        private void Serialize(DependencyObject item, XmlWriter writer)
        {
            Type baseType = item.GetType();
			
			if(string.IsNullOrEmpty(item.Name))
				item.SetValue<string>(DependencyObject.NameProperty, NameGenerator.GetName(this.canvas, item));
            
            // Gets all the dependency properties for this item type
            // and any relevant attached properties.
            List<PropertyData> fields = ReflectionHelper.GetProperties(item);
            
            // First we need to write all the dependency properties whose value is *not*
            // a dependency object as attributes. Every DependencyProperty whose value
            // is a dependency object must be written as an element.
            writer.WriteStartElement(baseType.Name);
            foreach(PropertyData prop in fields)
            {
                DependencyProperty dependencyProperty = prop.Property;
                object value = item.GetValue((DependencyProperty)dependencyProperty);

                if(!(value is DependencyObject) && (value != null) && !IsDefaultValue(item, dependencyProperty, value))
                {
                    string name = CleanName(prop.PropertyInfo.Name);
					if(prop.Attached)
                        name = prop.DeclaringType.Name + "." + name;
                    writer.WriteAttributeString(name, value.ToString());
                }
            }
            
            // After we write out all the attributes we can then write out
            // the child elements.
            foreach(PropertyData prop in fields)
            {
                object dependencyValue = prop.Property;
                object value = item.GetValue((DependencyProperty)dependencyValue);

				// We've already serialised non-dependency objects, so now serialise
				// only the dependency objects
				if(!(value is DependencyObject))
					continue;
				
                if(value is IEnumerable)
				{
					SerialiseCollection(prop, value, writer);
				}
				else if(!IsDefaultValue(item, prop.Property, value))
                {
                    writer.WriteStartElement(baseType.Name + "." + CleanName(prop.PropertyInfo.Name));
                    Serialize((DependencyObject)value, writer);
                    writer.WriteEndElement();
                }
            }
            
            writer.WriteEndElement();
        }

        
        private bool IsDefaultValue(DependencyObject item, DependencyProperty property, object value)
        {
            Type itemType = item.GetType();
            DependencyObject defaultItem;
            
            // If we have not cached an instance of the supplied DependencyObject,
            // we create a new instance of it and add it to the cache
            if(!defaultValues.TryGetValue(itemType, out defaultItem))
            {
                defaultItem = (DependencyObject)Activator.CreateInstance(itemType);
                defaultValues.Add(itemType, defaultItem);
            }
            
            // Using the cached instance, we check to see if the value supplied
            // is the same as the default value of a fresh item.
            object defaultValue = defaultItem.GetValue(property);
            if(value == null && defaultValue == null)
                return true;
            
            if(value == null || defaultValue == null)
                return false;
            
            return value.Equals(defaultValue);
        }
    }
}
