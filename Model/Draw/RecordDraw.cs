// RecordDraw.cs created with MonoDevelop
// User: alan at 11:54 AM 7/26/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LunarEclipse.Controller;

namespace LunarEclipse.Model
{
	public class RecordDraw : Selector
	{
		private Storyboard storyBoard;
		
		public RecordDraw(MoonlightController controller)
			: base(controller)
		{
			ChangedHeight += new EventHandler<PropertyChangedEventArgs>(HeightChanged);
			ChangedLeft += new EventHandler<PropertyChangedEventArgs>(LeftChanged);
			ChangedRotation += new EventHandler<PropertyChangedEventArgs>(RotationChanged);
			ChangedTop += new EventHandler<PropertyChangedEventArgs>(TopChanged);
			ChangedWidth += new EventHandler<PropertyChangedEventArgs>(WidthChanged);
		}
		
		private void HeightChanged(object sender, PropertyChangedEventArgs e)
		{
			
		}
		
		private void LeftChanged(object sender, PropertyChangedEventArgs e)
		{
			
		}
		
		private void RotationChanged(object sender, PropertyChangedEventArgs e)
		{
			
		}
		
		private void TopChanged(object sender, PropertyChangedEventArgs e)
		{
			
		}
		
		private void WidthChanged(object sender, PropertyChangedEventArgs e)
		{
			
		}
	}
}
