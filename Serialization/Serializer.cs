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

namespace LunarEclipse
{
    public sealed class Serializer
    {
        private static Type dependencyProperty = Canvas.BackgroundProperty.GetType();
        private static Dictionary<Type, DependencyObject> defaultValues = new Dictionary<Type, DependencyObject>();
        
        public Serializer()
        {
            
        }

        private string CleanName(string fieldName)
        {
            return fieldName.Substring(0, fieldName.Length - 8);
        }
        
        private void SerialiseCollection(FieldInfo field, object value, XmlWriter writer)
        {
            IEnumerable collection = (IEnumerable)value;
            
            //FIXME: Nasty hack as the collections don't have a common interface
            IEnumerator enumerator = collection.GetEnumerator();
            enumerator.Reset();
            if(!enumerator.MoveNext())
                return;
            
            foreach(DependencyObject o in collection)
                Serialize(o, writer);
        }
        
        public string Serialize(Canvas canvas)
        {
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
            // We get the type of the current DependencyObject
            // and then walk up the inheritance tree and pick
            // out all the DependencyProperties it has and
            // then serialise the values of those properties to XAML
            Type baseType = item.GetType();
            List<Type> checkableTypes = new List<Type>();

            // The first type we need to check is the type of the object supplied
            checkableTypes.Add(baseType);
            
            // If the object is a Framework Element, then it has a Parent property.
            // We add the type of the parent to the list of types we need to scan.
            // We then check if that item is a Framework element in order to check 
            // its parent and so on. The Parent objects are needed so we can grab 
            // attached properties.
            FrameworkElement e = item as FrameworkElement;
            while(e != null && e.Parent != null)
            {
                Type parentType = e.Parent.GetType();
                if(!checkableTypes.Contains(parentType))
                    checkableTypes.Add(parentType);
                
                e = e.Parent as FrameworkElement;
            }

            // Once we have a list of all the types that need to be checked for
            // we use reflection to scan through them all and pick out all the
            // DependencyProperties. We have to make sure we don't add the same
            // field twice.
            List<KeyValuePair<Type, FieldInfo>> fields = new List<KeyValuePair<Type, FieldInfo>>();
            writer.WriteStartElement(baseType.Name);
            foreach(Type t in checkableTypes)
                for(Type current = t; current != null; current = current.BaseType)
                {
                    FieldInfo[] currentFields = current.GetFields();
                    foreach(FieldInfo field in currentFields)
                        if(field.FieldType.Equals(dependencyProperty) &&
                           !ContainsField(fields, field))
                            fields.Add(new KeyValuePair<Type, FieldInfo>(t, field));
                }

            // Now that we have all the dependency properties, we need to write
            // all the ones whose value is *not* a dependency object as attributes.
            // Every DependencyProperty whose value is a dependency object must
            // be written as an element
            foreach(KeyValuePair<Type, FieldInfo> keypair in fields)
            {
                DependencyProperty dependencyProperty = (DependencyProperty)keypair.Value.GetValue(item);
                object value = item.GetValue((DependencyProperty)dependencyProperty);
                    
                if(!(value is DependencyObject) && (value != null) && !IsDefaultValue(item, dependencyProperty, value))
                {
                    string name = CleanName(keypair.Value.Name);
                    if(!keypair.Key.Equals(baseType))
                        name = keypair.Key.Name + "." + name;
                    writer.WriteAttributeString(name, value.ToString());
                }
            }
            
            // After we write out all the attributes we can then write out
            // the child elements.
            foreach(KeyValuePair<Type, FieldInfo> keypair in fields)
            {
                object dependencyValue = keypair.Value.GetValue(item);
                object value = item.GetValue((DependencyProperty)dependencyValue);

                if(value is IEnumerable)
                    SerialiseCollection(keypair.Value, value, writer);
                
                else if(value is DependencyObject)
                {
                    writer.WriteStartElement(baseType.Name + "." + CleanName(keypair.Value.Name));
                    Serialize((DependencyObject)value, writer);
                    writer.WriteEndElement();
                }
            }
            
            writer.WriteEndElement();
        }
        
        private bool ContainsField(List<KeyValuePair<Type, FieldInfo>> fields, FieldInfo field)
        {
            foreach(KeyValuePair<Type, FieldInfo> keypair in fields)
                if(keypair.Value.Equals(field))
                    return true;
            
            return false;
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
