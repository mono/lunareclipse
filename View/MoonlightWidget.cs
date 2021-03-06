// MoonlightWidget.cs
//
// Author:
//    Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 [copyright holders]
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
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using Gtk.Moonlight;

namespace LunarEclipse.View {	
	
	[System.ComponentModel.Category("LunarEclipse")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class MoonlightWidget : Gtk.Bin {
		
		public MoonlightWidget()
		{
			this.Build();
			
			silver = new GtkSilver(100, 100);
			silver.Transparent = true;
			canvas = new Canvas();
			canvas.Width = 100;
			canvas.Height = 100;
			canvas.Background = new SolidColorBrush(Colors.White);
			silver.Attach(canvas);
			
//			Image image = new Image();
//			image.Stretch = Stretch.Fill;
//			image.Width = 100;
//			image.Height = 100;
//			Downloader downloader = new Downloader();
//			downloader.Completed += delegate {
//				image.SetSource(downloader, null);
//			};
//			downloader.Open("GET", new Uri("file:///home/ceronman/Escritorio/images/bigbrother.png"));
//			downloader.Send();
//			
//			canvas.Children.Add(image);

			this.Add(silver);
		}
		
		public Canvas Canvas {
			get { return canvas; }
			set { 
				canvas = value;
				silver.Attach(canvas);
			}
		}

		public int Height {
			get { return height; }
			set {
				height = value;
				Canvas.Height = value;
				silver.Resize(width, height);
			}
		}

		public int Width {
			get { return width; }
			set {
				width = value;
				Canvas.Width = value;
				silver.Resize(width, height);
			}
		}

		public GtkSilver Silver {
			get { return silver; }
		}
		
		private int width;
		private int height;
		private Canvas canvas;
		private GtkSilver silver;
	}
}
