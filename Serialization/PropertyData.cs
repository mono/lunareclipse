// PropertyData.cs created with MonoDevelop
// User: alan at 18:58 06/08/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
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
