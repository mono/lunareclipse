// TrasnformHandle.cs
//
// Author:
//   Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 Manuel Cerón.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System.Windows;
using System.Windows.Media;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	enum Transform {
		Rotate,
		Translate,
		Skew,
		Scale
	}
	
	public abstract class TransformHandle: AbstractHandle {
		
		public TransformHandle(MoonlightController controller, IHandleGroup group, ILocator locator):
			base(controller, group)
		{
			Locator = locator;
			
			TransformGroup transforms = new TransformGroup();
			Inner.SetValue(RenderTransformProperty, transforms);
			rotation = new RotateTransform();
			transforms.Children.Add(rotation);
		}
		
		public override Point Location {
			get { return locator.Locate(); }
			set {  }
		}

		public override void Update ()
		{
			base.Update ();
			
			Point center = ElementTransformCenter;
			Rect allocation = CanvasAllocation;
			rotation.CenterX = center.X - allocation.X;
			rotation.CenterY = center.Y - allocation.Y;
			
			rotation.Angle = ElementRotation.Angle;
		}
		
		protected Point ElementTransformOrigin {
			get {
				return (Point) Element.GetValue(RenderTransformOriginProperty);
			}
		}
		
		protected Point ElementTransformCenter {
			get {
				IDescriptor descriptor = StandardDescriptor.CreateDescriptor(Element);
				Rect bounds = descriptor.GetBounds();
				Point origin = ElementTransformOrigin;
				return new Point(bounds.X + bounds.Width * origin.X,
								 bounds.Y + bounds.Height * origin.Y);
			}
		}
		
		protected RotateTransform ElementRotation {
			get { return (RotateTransform) ElementTransformGroup.Children[(int) Transform.Rotate]; }
		}
		
		protected TransformGroup ElementTransformGroup {
			get {
				TransformGroup group = (TransformGroup) Element.GetValue(RenderTransformProperty);
				
				if (group == null) {
					group = new TransformGroup();
					group.Children.Add(new RotateTransform());
					Element.SetValue(RenderTransformProperty, group);
				}
				
				return group;
			}
		}

		protected ILocator Locator {
			get { return locator; }
			set { locator = value; }
		}

		protected RotateTransform Rotation {
			get { return rotation; }
		}
	
		private RotateTransform rotation;
		private ILocator locator;
	}
}
