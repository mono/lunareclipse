//
// PropertyData.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace LunarEclipse
{
	public class PropertyData
	{
		private bool attached;
		private Type baseType;
		private Type declaringType;
		private string shortName;
		private DependencyProperty property;
		private FieldInfo propertyInfo;

		
		public bool Attached
		{
			get { return attached; }
		}
		
		/// <value>
		/// This is the type that the DependencyProperty is accessible from.
		/// i.e. Rectangle.NameProperty
		/// </value>
		public Type BaseType
		{
			get { return baseType; } 
		}
		
		/// <value>
		/// This is the type that the DependencyProperty is declared in.
		/// i.e. DependencyObject.NameProperty
		/// </value>
		public Type DeclaringType
		{
			get { return declaringType; }
		}
		
		/// <value>
		/// The trimmed name of the property. i.e. 'NameProperty' has 'Name' as it's shortname.
		/// </value>
		public string ShortName
		{
			get { return shortName; }
		}
		
		
		/// <value>
		/// The actual dependency property
		/// </value>
		public DependencyProperty Property
		{
			get { return property; }
		}
		
		
		/// <value>
		/// The fieldinfo related to this dependency property
		/// </value>
		public FieldInfo PropertyInfo
		{
			get { return propertyInfo; }
		}


		public PropertyData(Type baseType, Type declaringType, DependencyProperty property, FieldInfo propertyInfo, bool attached)
		{
			this.attached = attached;
			this.baseType = baseType;
			this.declaringType = declaringType;
			this.property = property;
			this.propertyInfo = propertyInfo;
			
			this.shortName = LunarEclipse.Serialization.Serializer.CleanName(propertyInfo.Name);
		}
		
		
		public override bool Equals (object o)
		{
			PropertyData other = o as PropertyData;
			return other == null ? false : this.property == other.property;
		}
		
		public override int GetHashCode ()
		{
			return property.GetHashCode();
		}
	}
}
