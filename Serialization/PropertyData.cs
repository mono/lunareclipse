// PropertyData.cs created with MonoDevelop
// User: alan at 18:58 06/08/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace LunarEclipse
{
	internal class PropertyData
	{
		private bool attached;
		private Type baseType;
		private Type declaringType;
		private DependencyProperty property;
		private FieldInfo propertyInfo;

		
		public bool Attached
		{
			get { return attached; }
		}
		
		public Type BaseType
		{
			get { return baseType; } 
		}
		
		public Type DeclaringType
		{
			get { return declaringType; }
		}
		
		public DependencyProperty Property
		{
			get { return property; }
		}
		
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
