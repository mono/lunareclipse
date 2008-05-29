//
// PropertyGroupBrushes.cs
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
using System.Collections.Generic;
using System.Reflection;
using Gtk;
using LunarEclipse.Model;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using LunarEclipse.Serialization;
using PropertyPairList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.Type, System.Reflection.FieldInfo>>;

namespace LunarEclipse
{
    public class PropertyGroupBrushes
    {
        private Button active;
        
        private bool hasProperties;
        private Button fillButton;
        private Button strokeButton;
        private Button backgroundButton;
        private ColorSelection colorPicker;
        
        public PropertyGroupBrushes ()
        {
            VBox box = new VBox();
            
            fillButton = new Button("Fill");
            fillButton.Show ();
            fillButton.Clicked +=
                delegate (object sender,   EventArgs e){ this.active = (Button)sender; };
			box.PackStart(fillButton);

            strokeButton = new Button("Stroke");
            strokeButton.Show ();
            strokeButton.Clicked += 
                delegate (object sender, EventArgs e){ this.active = (Button)sender; };
            box.PackStart(strokeButton);
            
            backgroundButton = new Button("Background");
            backgroundButton.Show ();
            backgroundButton.Clicked += 
                delegate (object sender, EventArgs e){ this.active = (Button)sender; };
            box.PackStart(backgroundButton);
            
            colorPicker = new ColorSelection();
            colorPicker.ColorChanged += delegate {
                Gdk.Color current = colorPicker.CurrentColor;
				Brush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
				                 (byte)(colorPicker.CurrentAlpha * (255.0 / ushort.MaxValue)),
	                             (byte)(current.Red * (255.0 / ushort.MaxValue)), 
	                             (byte)(current.Green * (255.0 / ushort.MaxValue)), 
	                             (byte)(current.Blue * (255.0 / ushort.MaxValue))));
                
				if(active == fillButton)
                    SelectedObject.Child.SetValue(Shape.FillProperty, brush);
                else if(active == backgroundButton)
                    SelectedObject.Child.SetValue(Canvas.BackgroundProperty, brush);
                else if(active == strokeButton)
                    SelectedObject.Child.SetValue(Shape.StrokeProperty, brush);
            };
			
			colorPicker.Show ();
            box.PackEnd(colorPicker);
            box.Show ();
        }
        
        public SelectedBorder SelectedObject {
            set 
            {
                SelectedObject = value;
                SetProperties(value);
            }
			get { return null;}
        }
#warning FIX THIS
        private void SetProperties(SelectedBorder border)
        {/*
            hasProperties = false;
            fillButton.Visible = false;
            strokeButton.Visible = false;
            backgroundButton.Visible = false;
			Properties.Hide ();
            
            if(border == null)
                return;
            
            PropertyPairList properties =  ReflectionHelper.GetDependencyProperties(border.Child);
			foreach(KeyValuePair<Type, FieldInfo> keypair in properties)
            {
                if(keypair.Value.Name == "StrokeProperty")
                    hasProperties = (strokeButton.Visible = true);
				
                else if(keypair.Value.Name == "FillProperty")
                    hasProperties = (fillButton.Visible = true);
				
                else if(keypair.Value.Name == "BackgroundProperty")
                    hasProperties = (backgroundButton.Visible = true);
            }
			
			if(fillButton.Visible)
				active = fillButton;
            else if(backgroundButton.Visible)
				active = backgroundButton;

            if(hasProperties)
                Properties.Show();*/
        }
    }
}
