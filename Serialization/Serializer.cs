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


namespace DesignerMoon
{
    public class Serializer
    {
        public Serializer()
        {
        }

        private static string CleanName(string fieldName)
        {
            return fieldName.Substring(0, fieldName.Length - 8);
        }
        
        
        private void SerialiseCollection(FieldInfo field, object value, XmlWriter writer)
        {
            ICollection collection = (ICollection)value;
            
            if(collection.Count == 0)
                return;
            
            writer.WriteStartElement(CleanName(field.Name));
            foreach(DependencyObject o in collection)
                Serialize(o, writer);
            writer.WriteEndElement();
        }
        
        public string Serialize(Canvas canvas)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding();   
            settings.OmitXmlDeclaration=true;
            XmlWriter writer = XmlWriter.Create(sb, settings);
            
            
            try
            {
                Serialize(canvas, writer);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            writer.Flush();
            
            sb.Replace("<Canvas", "<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            return sb.ToString();
        }
        
        private void Serialize(DependencyObject item, XmlWriter writer)
        {
            Type currentType = item.GetType();
            Type dependencyProperty = Canvas.BackgroundProperty.GetType();

            writer.WriteStartElement(currentType.Name);

            while(currentType != null)
            {
                FieldInfo[] fields = currentType.GetFields();
                
                foreach(FieldInfo field in fields)
                {
                    if(!field.FieldType.Equals(dependencyProperty))
                        continue;
                    
                    object dependencyValue = field.GetValue(item);
                    
                    if(dependencyValue == null)
                    {
                        Console.WriteLine(string.Format("{0} from {1} contained a null property", field.Name, field.ReflectedType.ToString()));
                    }
                    else
                    {
                        object value = item.GetValue((DependencyProperty)dependencyValue);
                        

                        if(!(value is DependencyObject) && !IsDefaultValue(value))
                        {
                            writer.WriteAttributeString(CleanName(field.Name), value.ToString());
                            Console.WriteLine(string.Format("{0} is of value {1}", field.Name, value));
                        }
                    }
                }
                
                currentType = currentType.BaseType;
            }

            currentType = item.GetType();
            while(currentType != null)
            {
                FieldInfo[] fields = currentType.GetFields();
                
                foreach(FieldInfo field in fields)
                {
                    if(!field.FieldType.Equals(dependencyProperty))
                        continue;
                    
                    object dependencyValue = field.GetValue(item);
                    
                    if(dependencyValue == null)
                    {
                        Console.WriteLine(string.Format("{0} from {1} contained a null property", field.Name, field.ReflectedType.ToString()));
                    }
                    else
                    {
                        object value = item.GetValue((DependencyProperty)dependencyValue);

                        if(value is ICollection)
                        {
                            SerialiseCollection(field, value, writer);
                            Console.WriteLine(string.Format("{0} is of value {1}", field.Name, value));
                        }
                        else if(value is DependencyObject)
                        {
                            writer.WriteStartElement(field.ReflectedType.Name + "." + CleanName(field.Name));
                            Serialize((DependencyObject)value, writer);
                            writer.WriteEndElement();
                        }
                    }
                }
                
                currentType = currentType.BaseType;
            }
            
            writer.WriteEndElement();
        }
        
        private static bool IsDefaultValue(object value)
        {
            if(value == null)
                return true;
            

            try
            {
                return Convert.ToDouble(value) == 0;
            }
            catch
            {
               return false;
            }
        }
    }
}
