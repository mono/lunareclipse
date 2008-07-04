// RightResizeWidthHandle.cs
//
// Author:
//    Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 Manuel Cerón
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

using System;
using System.Windows;
using LunarEclipse.Controller;

namespace LunarEclipse.Model {	
	
	public class RightResizeWidthHandle: ResizeHandle {
		
		public RightResizeWidthHandle(MoonlightController controller, IHandleGroup group, ILocator locator):
			base(controller, group, locator)
		{
		}
		
		protected override Rect CalculateNewBounds (Rect oldBounds, Point offset, double cosAngle, double sinAngle)
		{
			Rect newBounds = oldBounds;
			
			if ((oldBounds.Width + offset.X) >= 0) {
				newBounds.Width = oldBounds.Width + offset.X;
				newBounds.X = oldBounds.Left  - offset.X * (1 - cosAngle) / 2;
				newBounds.Y =  oldBounds.Top + offset.X * sinAngle / 2.0;
			}
			
			return newBounds;
		}

	}
}
