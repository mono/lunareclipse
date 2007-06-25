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
        
        private static bool IsCollection(object value)
        {
            if(value == null)
                return false;

            // FIXME: Should i just be checking for IEnumerable? That may be easier.
            return value.GetType().BaseType.FullName.StartsWith("MS.Internal.Collection");
        }
        
        
        private void SerialiseCollection(FieldInfo field, object value, XmlWriter writer)
        {
            //FIXME: Reflection hack
            
            int count = 0;
            Console.WriteLine("Serializing Collection: " + field.Name);
            
            
            Type t = value.GetType();
            MethodInfo info = t.GetMethod("GetEnumerator");
            IEnumerator enumerator = (IEnumerator)info.Invoke(value, null);
            
            enumerator.Reset();
            while(enumerator.MoveNext())
            {
                if((count++) == 0)
                    writer.WriteStartElement(CleanName(field.Name));
                Console.WriteLine("Moving Next: " + (count++));
                Console.WriteLine("Serializing child: " + enumerator.Current.GetType().Name);
                Serialize((DependencyObject)enumerator.Current, writer);
            }
            
            if(count > 0)
                writer.WriteEndElement();
        }
        
        public string Serialize(Canvas canvas)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            
            try
            {
            Serialize(canvas, writer);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            writer.Flush();

            sb.Insert(6, " xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            return sb.ToString();
        }
        
        private void Serialize(DependencyObject item, XmlWriter writer)
        {
            Console.WriteLine("Serializing object: " + item.GetType().Name);
            //Console.ReadLine();
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
                        

                        if(!IsCollection(value) && !IsDefaultValue(value))
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
                        
                        if(IsCollection(value))
                        {
                            SerialiseCollection(field, value, writer);
                            Console.WriteLine(string.Format("{0} is of value {1}", field.Name, value));
                        }
                    }
                }
                
                currentType = currentType.BaseType;
            }
            
            writer.WriteEndElement();
        }
        
 /*       private void Serialize(DependencyObject item, XmlWriter writer)
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
                        Console.WriteLine(string.Format("{0} is of value {1}", field.Name, value));
                        
                        if(IsCollection(value))
                            SerialiseCollection(field, value, writer);

                        else if(!IsDefaultValue(value))
                            writer.WriteAttributeString(CleanName(field.Name), value.ToString());
                    }
                }
                
                currentType = currentType.BaseType;
            }
            
            writer.WriteEndElement();
        }
*/
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
