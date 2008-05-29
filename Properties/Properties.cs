//
// Properties.cs
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
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using LunarEclipse.Model;
using System.Windows;

using Gtk;

namespace LunarEclipse {
	public class PropertyManager {
//		DependencyProperty nameProp;
//		SelectedBorder item;
//		
//		PropertyGroup brush;
//		PropertyGroup appearance;
//		PropertyGroup layout;
//		PropertyGroup common;
//		PropertyGroup text;
//		PropertyGroup misc;

		public PropertyManager ()
		{
//			this.Build ();
//			
//			ObjectName.Text = "<No Object Selected>";
//			ObjectName.Sensitive = false;
//			ObjectName.Changed += new EventHandler (OnNameChanged);
//			ObjectType.Text = "";
//			
//			// build the property groups
//			
//			// Brush
//			brush = new PropertyGroupBrushes();
//            vboxProperties.PackStart(brush, false, false, 0);
//            brush.Hide ();
//            
//			// Appearance
//			appearance = new PropertyGroupAppearance ();
//			vboxProperties.PackStart (appearance, false, false, 0);
//			appearance.Hide ();
//			
//			// Layout
//			layout = new PropertyGroupLayout ();
//			vboxProperties.PackStart (layout, false, false, 0);
//			layout.Hide ();
//			
//			// Common Properties
//			
//			// Text
//			
//			// Misc.
		}
		
		public bool HasProperties {
			get { return null != null; }//item != null; }
		}

		public SelectedBorder SelectedObject {
			get {return null;}}
//			set {
//				nameProp = null;
//				item = value;
//				
//				if (item != null) {
//					ObjectName.Text = item.Name != null ? item.Name : "";
//					ObjectName.Sensitive = true;
//					ObjectType.Text = value.Child.GetType ().Name;
//					
//					for (Type type = item.Child.GetType (); nameProp == null && type != null; type = type.BaseType) {
//						FieldInfo[] fields = type.GetFields ();
//						foreach (FieldInfo field in fields) {
//							if (field.Name == "NameProperty") {
//								nameProp = (DependencyProperty) field.GetValue (item.Child);
//								break;
//							}
//						}
//					}
//				} else {
//					ObjectName.Text = "<No Object Selected>";
//					ObjectName.Sensitive = false;
//					ObjectType.Text = "";
//				}
//				
//				// build the property groups
//				
//				// Brush
//				brush.SelectedObject = item;
//                if(brush.HasProperties)
//                    brush.Show ();
//                else
//                    brush.Hide ();
//
//				// Appearance
//				appearance.SelectedObject = item;
//				if (appearance.HasProperties)
//					appearance.Show ();
//				else				
//					appearance.Hide ();
//				
//				// Layout
//				layout.SelectedObject = item;
//				if (layout.HasProperties)
//					layout.Show ();
//				else
//					layout.Hide ();
//				
//				// Common Properties
//				
//				// Text
//				
//				// Misc.
//			}
//		}
		
		void OnNameChanged (object o, EventArgs e)
		{
//			Entry entry = (Entry) o;
//			
//			if (nameProp != null)
//				item.SetValue<string> (nameProp, entry.Text);
		}
	}
}
