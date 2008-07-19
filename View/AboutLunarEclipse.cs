// AboutDialog.cs
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
using System.Reflection;
using Gtk;

namespace LunarEclipse.View {	
	
	public class AboutLunarEclipse: Gtk.AboutDialog
	{
		public AboutLunarEclipse(): base ()
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			
			Name = (asm.GetCustomAttributes (typeof (AssemblyTitleAttribute), false) [0] as AssemblyTitleAttribute).Title;
		    Version = asm.GetName ().Version.ToString ();
			Comments = (asm.GetCustomAttributes (typeof (AssemblyDescriptionAttribute), false) [0] as AssemblyDescriptionAttribute).Description;
			Copyright = (asm.GetCustomAttributes (typeof (AssemblyCopyrightAttribute), false) [0] as AssemblyCopyrightAttribute).Copyright;
			License = license;
			Authors = authors;
		}
		
		private string [] authors = new string [] {
			"Alan McGovern <alan.mcgovern@gmail.com>",
			"Manuel Cerón <ceronman@unicauca.edu.co>"
		};
		
		private string license =
@"Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
""Software""), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";
	}
}
