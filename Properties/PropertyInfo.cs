// PropertyInfo.cs created with MonoDevelop
// User: alan at 2:35 PM 8/20/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace LunarEclipse
{
	public class PropertyInfo
	{
#region Fields

		private bool canAuto;
		private bool extended;
		private PropertyData propertyData;
		private PropertyType type;
		
#endregion Fields
		
		
#region Properties

		public bool Attached 
		{
			get { return propertyData.Attached; }
		}

		public bool CanAuto 
		{
			get { return canAuto; }
		}

		public bool Extended 
		{
			get { return extended; }
		}
		
		public string Name 
		{
			get { return propertyData.Name; }
		}

		public PropertyData PropertyData
		{
			get { return propertyData; }
		}
		
		public PropertyType Type 
		{
			get { return type; }
		}
		
#endregion Properties
		
		
#region Constructors
		
		public PropertyInfo (PropertyData propertyData, PropertyType type)
			: this(propertyData, type, false)
		{

		}
		
		public PropertyInfo(PropertyData propertyData, PropertyType type, bool canAuto)
			: this(propertyData, type, canAuto, false)
		{
			
		}
		
		public PropertyInfo(PropertyData propertyData, PropertyType type, bool canAuto, bool extended)
		{
			this.canAuto = canAuto;
			this.extended = extended;
			this.propertyData = propertyData;
			this.type = type;
		}
		
#endregion Constructors
		
	}
}
