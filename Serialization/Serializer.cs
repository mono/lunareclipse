//
// Serializer.cs
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
// distribute, sublicense, and/or sell copies of Rthe Software, and to
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
using System.Threading;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
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
        private static Dictionary<Type, DependencyObject> defaultValues;
		
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
			if (value is PointCollection)
				return; 
			
            IEnumerable collection = (IEnumerable)value;
			
            //FIXME: Nasty hack as the collections don't have a common interface
			IEnumerator enumerator;
		 	try {
            	enumerator = collection.GetEnumerator();
			}
			catch (NotImplementedException e) {
				System.Diagnostics.Debug.WriteLine("Something is not implemented in moonlight");
				return;
			}
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
			
			name += '.' + propertyData.ShortName;
			writer.WriteStartElement(name);
			writer.WriteStartElement(value.GetType().Name);
			
            foreach(DependencyObject o in collection)
                Serialize(o, writer);
			
			writer.WriteEndElement();
			writer.WriteEndElement();
        }
        
        public string Serialize(Canvas canvas)
        {
			// This is used to prevent bad serializations on other cultures
			// For example in spanish, doubles would be serialized like
			// "1,0" instead of "1.0"
			CultureInfo culture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
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
			Thread.CurrentThread.CurrentCulture = culture;
        }
        
        private void Serialize(DependencyObject item, XmlWriter writer)
        {
			System.Diagnostics.Debug.WriteLine(item);
			System.Diagnostics.Debug.Indent();
			if (item is IHandle || item is IFrame)
				return;
			
            Type baseType = item.GetType();
			
//			if(string.IsNullOrEmpty(item.Name)) {
//				item.SetValue(DependencyObject.NameProperty, NameGenerator.GetName(this.canvas, item));
//				System.Console.WriteLine("#### "+ baseType.Name);
//				System.Console.WriteLine(item.Name);
//			}
            
			// Gets all the dependency properties for this item type
            // and any relevant attached properties.
            List<PropertyData> fields = ReflectionHelper.GetProperties(item);
            
            // First we need to write all the dependency properties whose value is *not*
            // a dependency object as attributes. Every DependencyProperty whose value
            // is a dependency object must be written as an element.
            writer.WriteStartElement(baseType.Name);
			System.Diagnostics.Debug.WriteLine("properties:");
			System.Diagnostics.Debug.Indent();
            foreach(PropertyData prop in fields)
            {
                System.Diagnostics.Debug.WriteLine(prop.ShortName);
				DependencyProperty dependencyProperty = prop.Property;
				object value = null;
			 	try {
                		value = item.GetValue(dependencyProperty);
				}
				catch {
					System.Diagnostics.Debug.WriteLine(string.Format("Error getting value: {0}.{1} on {2}", prop.DeclaringType, prop.ShortName, item));
				}
				
				if (value is PointCollection) {
					PointCollection collection = value as PointCollection;
					string[] points = new string[collection.Count];
					for (int i=0; i<collection.Count; i++) {
						Point p = collection[i];
						points[i] = string.Format("{0},{1}", p.X, p.Y);
					}
					writer.WriteAttributeString(prop.ShortName, string.Join(" ", points));
					continue;
				}
				
                if(!(value is DependencyObject) && (value != null) && !IsDefaultValue(item, dependencyProperty, value))
                {
                    string name = prop.ShortName;
					if(prop.Attached)
                        name = prop.DeclaringType.Name + "." + name;
                    writer.WriteAttributeString(name, value.ToString());
                }
            }
			System.Diagnostics.Debug.Unindent();
            
            // After we write out all the attributes we can then write out
            // the child elements.
            foreach(PropertyData prop in fields)
            {
                object dependencyValue = prop.Property;
                object value = null;
				
			 	try {
					value = item.GetValue((DependencyProperty)dependencyValue);
				}
				catch {
					System.Diagnostics.Debug.WriteLine(string.Format("Error getting value: {0}.{1} on {2}", prop.DeclaringType, prop.ShortName, item));
				}

				// We've already serialised non-dependency objects, so now serialise
				// only the dependency objects
				if(!(value is DependencyObject))
					continue;
				
                if (value is IEnumerable) {
					SerialiseCollection(prop, value, writer);
				}
				else if(!IsDefaultValue(item, prop.Property, value))
                {
                    writer.WriteStartElement(baseType.Name + "." + prop.ShortName);
                    Serialize((DependencyObject)value, writer);
                    writer.WriteEndElement();
                }
            }
     
            writer.WriteEndElement();
			System.Diagnostics.Debug.Unindent();
        }
		
		public static DependencyObject Clone(Canvas root, DependencyObject item)
        {
            System.Diagnostics.Debug.WriteLine("Starting Cloning:");
			System.Diagnostics.Debug.Indent();
			Type type = item.GetType();
            DependencyObject newObject = (DependencyObject) Activator.CreateInstance(type);
			
            List<PropertyData> fields = ReflectionHelper.GetProperties(item);
			
			foreach(PropertyData prop in fields) {
				System.Diagnostics.Debug.WriteLine("* Property: " + prop.ShortName);
				System.Diagnostics.Debug.Indent();
				
                DependencyProperty dependencyProperty = prop.Property;
				
                object value = null;
				
				try {
					value = item.GetValue(dependencyProperty);
				}
				catch {
					System.Diagnostics.Debug.WriteLine(string.Format("Error getting value: {0}.{1} on {2}", prop.DeclaringType, prop.ShortName, item));
				}
				
				if(!IsDefaultValue(item, dependencyProperty, value))
                {
                    object newValue = null;

					if (value is PointCollection) {
						PointCollection newPoints = new PointCollection();
						foreach (Point p in (value as PointCollection) )
							newPoints.Add(p);
						newValue = newPoints;
					}
					
					else if(value is IEnumerable && !(value is string)) {
						Type collectionType = value.GetType();
						System.Diagnostics.Debug.WriteLine("* IEnumerable: " + collectionType.ToString());
						System.Diagnostics.Debug.Indent();
						newValue = Activator.CreateInstance(collectionType);
						try {
							foreach (DependencyObject o in value as IEnumerable) {
								DependencyObject member = Clone(root, o);
								newValue.GetType().GetMethod("Add").Invoke(newValue, new object[] { member });
							}
						}
						catch (NotImplementedException e) {
							System.Diagnostics.Debug.WriteLine("Something is not implemented in moonlight");
						}
						System.Diagnostics.Debug.Unindent();
					}
					
					else if (value is DependencyObject) {
						newValue = Clone(root, value as DependencyObject);
					}
					
					else {
						newValue = value;
					}
					
					try {
						newObject.SetValue(dependencyProperty, newValue);
					}
					catch (Exception e){
						System.Diagnostics.Debug.WriteLine(string.Format("*** couldn't clone {0}: {1}", prop.ShortName, e.Message));
					}
                }
				
				System.Diagnostics.Debug.Unindent();
            }
			System.Diagnostics.Debug.Unindent();
			
			//if (!string.IsNullOrEmpty(item.Name))
			//	newObject.Name = NameGenerator.GetName(root, newObject);

			return newObject;
		}

        public static bool IsDefaultValue(DependencyObject item, DependencyProperty property, object value)
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
            object defaultValue = null;
			try {
				defaultValue = defaultItem.GetValue(property);
			}
			catch {
				System.Diagnostics.Debug.WriteLine("Error getting value: ");
			}
            if(value == null && defaultValue == null)
                return true;
            
            if(value == null || defaultValue == null)
                return false;
			
            return value.Equals(defaultValue);
        }
    }
}
