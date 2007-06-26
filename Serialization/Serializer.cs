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

namespace DesignerMoon
{
    public sealed class Serializer
    {
        private static Type dependencyProperty = Canvas.BackgroundProperty.GetType();
        
        public Serializer()
        {
        }

        private string CleanName(string fieldName)
        {
            return fieldName.Substring(0, fieldName.Length - 8);
        }
        
        private void SerialiseCollection(FieldInfo field, object value, XmlWriter writer)
        {
            ICollection collection = (ICollection)value;
            
            if(collection.Count == 0)
                return;
            
            string cleanedName = CleanName(field.Name);
            // FIXME: Is this the only "Children" property? Is this safe?
            if(!cleanedName.Equals("Children"))
                writer.WriteStartElement(cleanedName);
            
            foreach(DependencyObject o in collection)
                Serialize(o, writer);
            
            if(!cleanedName.Equals("Children"))
                writer.WriteEndElement();
        }
        
        public string Serialize(Canvas canvas)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings(); 
            settings.OmitXmlDeclaration=true;
            
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
            Type currentType = baseType;
            List<FieldInfo> fields = new List<FieldInfo>();
            
            writer.WriteStartElement(currentType.Name);
            while(currentType != null)
            {
                FieldInfo[] currentFields = currentType.GetFields();
                foreach(FieldInfo field in currentFields)
                    if(field.FieldType.Equals(dependencyProperty))
                        fields.Add(field);
                
                currentType = currentType.BaseType;
            }

            // We first have to go through all the fields and write out
            // all the attributes first.
            foreach(FieldInfo field in fields)
            {
                object dependencyValue = field.GetValue(item);
                object value = item.GetValue((DependencyProperty)dependencyValue);
                    
                if(!(value is DependencyObject) && !IsDefaultValue(value))
                    writer.WriteAttributeString(CleanName(field.Name), value.ToString());
            }
            
            // After we write out all the attributes we can then write out
            // the child elements.
            foreach(FieldInfo field in fields)
            {
                object dependencyValue = field.GetValue(item);
                object value = item.GetValue((DependencyProperty)dependencyValue);

                if(value is ICollection)
                    SerialiseCollection(field, value, writer);
                
                else if(value is DependencyObject)
                {
                    writer.WriteStartElement(baseType.Name + "." + CleanName(field.Name));
                    Serialize((DependencyObject)value, writer);
                    writer.WriteEndElement();
                }
            }
            
            writer.WriteEndElement();
        }
        
        //FIXME: check default value properly
        private bool IsDefaultValue(object value)
        {
            if(value == null)
                return true;
            
            try { return Convert.ToDouble(value) == 0;  }
            catch  { return false; }
        }
    }
}
