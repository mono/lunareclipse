// PropertyPanel.cs
//
// Authors:
//   Alan McGovern <alan.mcgovern@gmail.com>
//   Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (C) 2007 Alan McGovern, 2008 Manuel Cerón
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
using System.Windows.Media;
using Gtk;
using LunarEclipse.Controller;
using LunarEclipse.Model;

namespace LunarEclipse.View
{	
	[System.ComponentModel.Category("LunarEclipse")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PropertyPanel : Gtk.Bin
	{
		public PropertyPanel()
		{
			this.Build();
			
			main_box = new VBox();
			main_box.BorderWidth = 6;
			scrolledwindow.AddWithViewport(main_box);
			Toolbox.PropertyChanged += UpdatePropertyWidget;
		}
		
		public MoonlightController Controller {
			get { return controller; }
			set {
				controller = value;
				controller.Selection.SelectionChanged += delegate {
					GenerateWidgets();
				};
			}
		}
		
		private void GenerateWidgets()
		{
			main_box.FreezeChildNotify();
			
			CleanMainBox();
			
			foreach (PropertyInfo info in Controller.PropertyManager.Properties)
				main_box.PackEnd(CreateWidget(controller.PropertyManager.SelectedObject, info));
			
			main_box.ThawChildNotify();
			main_box.ShowAll();
		}
		
		private Widget CreateWidget(DependencyObject o, PropertyInfo info)
		{
			HBox b = new HBox(true, 0);
			Adjustment adj = null;
			SpinButton spin = null;
			Label l  = new Label(info.PropertyData.ShortName);
			l.SetAlignment(0.0f, 0.5f);
			b.PackStart(l);
			
			switch(info.Type)
			{
				
			case PropertyType.Double:
			case PropertyType.Integer:
				adj = new Adjustment (0.0, 0.0, Int32.MaxValue, 1.0, 10.0, 100.0);
				spin = new SpinButton (adj, 1.0, 0);
				spin.SetRange (0.0, (double) Int32.MaxValue);
				spin.Numeric = true;
				if(info.Type == PropertyType.Integer)
				{
					spin.Value = (int)o.GetValue(info.PropertyData.Property);
					spin.ValueChanged += delegate (object sender, EventArgs e) {
						Toolbox.ChangeProperty((UIElement) o, o, info.PropertyData.Property, spin.ValueAsInt);
					};
				}
				else
				{
					spin.Value = (double)o.GetValue(info.PropertyData.Property);
					spin.ValueChanged += delegate (object sender, EventArgs e) {
						Toolbox.ChangeProperty((UIElement) o, o, info.PropertyData.Property, spin.Value);
					};
				}
				b.PackEnd(spin);
				break;
			
			case PropertyType.Percent:
				adj = new Adjustment (0.0, 0.0, 1.0, 0.01, 0.1, 1.0);
				spin = new SpinButton (adj, 0.01, 2);
				spin.Numeric = true;
				spin.SetRange (0.0, 1.0);
				spin.Value = (double)o.GetValue(info.PropertyData.Property);
				spin.ValueChanged += delegate (object sender, EventArgs e) {
					Toolbox.ChangeProperty((UIElement) o, o, info.PropertyData.Property, spin.Value);
				};
				b.PackEnd(spin);
				break;
				
			case PropertyType.PenLineCap:
				b.PackEnd(CreateEnumPropertyWidget(o, info, typeof(PenLineCap)));
				break;
				
			case PropertyType.PenLineJoin:
				b.PackEnd(CreateEnumPropertyWidget(o, info, typeof(PenLineJoin)));
				break;
				
			case PropertyType.Visibility:
				b.PackEnd(CreateEnumPropertyWidget(o, info, typeof(Visibility)));
				break;
			
			case PropertyType.Stretch:
				b.PackEnd(CreateEnumPropertyWidget(o, info, typeof(Stretch)));
				break;
				
			case PropertyType.String:
				Entry entry = new Entry(o.GetValue(info.PropertyData.Property).ToString());
				entry.Changed += delegate (object sender, EventArgs e) {
					Toolbox.ChangeProperty((UIElement)o, o, info.PropertyData.Property, ((Entry)sender).Text);
				};
				b.PackEnd(entry);
				break;
				
			case PropertyType.DashArray:
			case PropertyType.Data:
				b.PackEnd(new Entry());
				break;
				
			case PropertyType.Point:
				b.PackEnd(CreatePointPropertyWidget(o, info));
				break;
				
			case PropertyType.Brush:
				b.PackEnd(CreateBrushPropertyWidget(o, info));
				break;
				
			default:
				b.PackEnd(new Label(string.Format("Unsupported: {0}", info.Type)));
				break;
			}
			
			return b;
		}
		
		private Widget CreateBrushPropertyWidget(DependencyObject o, PropertyInfo info)
		{
			Entry e = new Entry();
			e.Changed += delegate {
				uint val;
				if(!uint.TryParse(e.Text, System.Globalization.NumberStyles.HexNumber, null, out val))
					val = 0x555555;
				
				Color c = Color.FromArgb((byte)(val>>24), (byte)(val>>16), (byte)(val>>8), (byte)val);
				Console.WriteLine(c.ToString());
				o.SetValue(info.PropertyData.Property, new SolidColorBrush(c));
			};
			return e;
		}
		
		private Widget CreatePointPropertyWidget(DependencyObject o, PropertyInfo info)
		{
			VBox b = new VBox(true, 0);
			HBox h = new HBox(true, 0);
			h.Add(new Label("X"));
			Adjustment adj = new Adjustment (0.0, 0, 1, 0.01, 0.1, 1.0);
			Point p = (Point)o.GetValue(info.PropertyData.Property);
			
			SpinButton spinner = new SpinButton(adj, 1.0, 2);
			spinner.Numeric = true;
			spinner.Value = p.X;
			spinner.Changed += delegate(object sender, EventArgs e) {
				Point point = (Point)o.GetValue(info.PropertyData.Property);
				point.X = ((SpinButton)sender).Value;
				o.SetValue(info.PropertyData.Property, point);
			};
			h.Add(spinner);
			b.Add(h);
			
			h = new HBox(true, 0);
			h.Add(new Label("Y"));
			adj = new Adjustment (0.0, 0, 1, 0.01, 0.1, 1.0);
			spinner = new SpinButton(adj, 1.0, 2);
			spinner.Numeric = true;
			spinner.Value = p.Y;
			spinner.Changed += delegate (object sender, EventArgs e) {
				Point point = (Point)o.GetValue(info.PropertyData.Property);
				point.Y = ((SpinButton)sender).Value;
				o.SetValue(info.PropertyData.Property, point);
			};
			h.Add(spinner);
			b.Add(h);
			return b;
		}
		
		private Widget CreateEnumPropertyWidget(DependencyObject o, PropertyInfo info, Type type)
		{
			string[] names = Enum.GetNames(type);
			int[] values = (int[])Enum.GetValues(type);
			int value = (int)o.GetValue(info.PropertyData.Property);
			
			ComboBox b = new ComboBox(names);
			
			for(int i=0; i < values.Length; i++)
				if(values[i] == value)
					b.Active = i;
			
			b.Changed += delegate (object sender, EventArgs e) {
				Toolbox.ChangeProperty((UIElement)o, o, info.PropertyData.Property, Enum.Parse(type, b.ActiveText));
			};
			
			return b;
		}
		
		private void UpdatePropertyWidget(object sender, PropertyChangedEventArgs e)
		{
			if(e.Target != controller.PropertyManager.SelectedObject)
				return;
			
			PropertyData data = LunarEclipse.Serialization.ReflectionHelper.GetData(e.Property);
			foreach(HBox box in main_box.Children)
			{
				if(((Label)box.Children[0]).Text == data.ShortName)
				{
					Widget widget = box.Children[1];
					if(widget is SpinButton)
						((SpinButton)widget).Value = Convert.ToDouble(e.NewValue);
					
					else if(widget is ComboBox)
						((ComboBox)widget).Active = Convert.ToInt32(e.NewValue);
					
					else if(widget is Entry)
						((Entry)widget).Text = Convert.ToString(e.NewValue);
					
					else
						Console.WriteLine("Couldn't Update");
				}
			}
		}
		
		private void CleanMainBox()
		{
			Widget[] widgets = (Widget[]) main_box.Children.Clone();
			
			foreach (Widget widget in widgets)
				widget.Destroy();
		}
		
		private MoonlightController controller;
		private VBox main_box;
	}
}
