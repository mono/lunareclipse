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
		
		public TransformHandle(MoonlightController controller, IHandleGroup group):
			base(controller, group)
		{
		}
		
		protected Point TransformOrigin {
			get {
				return (Point) Element.GetValue(RenderTransformOriginProperty);
			}
		}
		
		protected RotateTransform Rotation {
			get { return (RotateTransform) Transformations.Children[(int) Transform.Rotate]; }
		}
		
		protected TransformGroup Transformations {
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
	}
}
