// /cvs/lunareclipse/Properties/PropertyGroupLayout.cs created with MonoDevelop
// User: fejj at 11:37 AM 6/29/2007
//

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Gtk;
using LunarEclipse.Model;
using LunarEclipse.Serialization;


namespace LunarEclipse {
	public class PropertyGroupLayout : PropertyGroup {

		

		
		static PropertyInfo [] info = GeneratePropertyInfo();
		
		static PropertyInfo [] GeneratePropertyInfo()
		{
			// name, type, attached, can auto
			PropertyInfo info = null;
			List<PropertyInfo> props = new List<PropertyInfo>();
			
			props.Add(new PropertyInfo(ReflectionHelper.GetData(Canvas.WidthProperty), PropertyType.Double, false, true));
			props.Add(new PropertyInfo(ReflectionHelper.GetData(Canvas.HeightProperty), PropertyType.Double, false, true));
			props.Add(new PropertyInfo(ReflectionHelper.GetData(Canvas.LeftProperty), PropertyType.Double, true, false));
			props.Add(new PropertyInfo(ReflectionHelper.GetData(Canvas.TopProperty), PropertyType.Double, true, false));
			props.Add(new PropertyInfo(ReflectionHelper.GetData(Canvas.ZIndexProperty), PropertyType.Integer, false, false));
			
			return props.ToArray();
		}
		
		
		DependencyObject item;
		Hashtable propTable;
		Hashtable spinTable;
		
		public PropertyGroupLayout () : base ("Layout")
		{
		}
#warning Fix This
		void SetDependencyObject (DependencyObject item)
		{/*
			int i;
			
			this.item = item;
			
			if (item == null) {
				Properties = null;
				return;
			}
			
            PropertyPairList pairs = ReflectionHelper.GetDependencyProperties(item);
			Hashtable props = new Hashtable();
            foreach(KeyValuePair<Type, FieldInfo> keypair in pairs)
                props.Add(keypair.Value.Name, keypair.Value);
            
			if (props.Count == 0) {
				Properties = null;
				return;
			}
			
			Table table = new Table ((uint) props.Count, 2, false);
			propTable = new Hashtable ();
			spinTable = new Hashtable ();
			uint top = 0;
			
			for (i = 0; i < info.Length; i++) {
				FieldInfo field = (FieldInfo) props[info[i].Name];
				if (field == null && !info[i].Attached)
					continue;
				
				string propName = info[i].Name.Substring (0, info[i].Name.Length - 8);
				DependencyProperty prop = null;
				object value = null;
				
				if (!info[i].Attached) {
					prop = (DependencyProperty) field.GetValue (item);
					value = ((SelectedBorder)item).Child.GetValue (prop);
				} else {
					// Attached properties
					switch (info[i].Name) {
					case "LeftProperty":
						prop = Canvas.LeftProperty;
						value = ((SelectedBorder)item).Child.GetValue (prop);
						break;
					case "TopProperty":
						prop = Canvas.TopProperty;
						value = ((SelectedBorder)item).Child.GetValue (prop);
						break;
					}
				}
				
				Widget label = new Label (propName);
				label.Show ();
				
				Adjustment adj = new Adjustment (0.0, 0.0, Int32.MaxValue, 1.0, 10.0, 100.0);
				Widget widget = new SpinButton (adj, 1.0, 0);
				((SpinButton) widget).SetRange (0.0, (double) Int32.MaxValue);
				((SpinButton) widget).Numeric = true;
				
				if (value != null) {
					switch (value.GetType ().Name) {
					case "Double":
						((SpinButton) widget).Value = (double) value;
						break;
					case "Int32":
						((SpinButton) widget).Value = (int) value;
						break;
					}
				}

				((SpinButton) widget).ValueChanged += new EventHandler(OnIntegerChanged);
				
				propTable.Add (widget, prop);
				widget.Show ();
				
				if (info[i].CanAuto) {
					Box hbox = new HBox (false, 6);
					hbox.PackStart (widget, true, true, 0);
					Button button = new Button ("Auto");
					button.Clicked += new EventHandler (OnAutoClicked);
					spinTable.Add (button, widget);
					button.Show ();
					hbox.PackStart (button, false, false, 0);
					hbox.Show ();
					
					widget = hbox;
				}
				
				table.Attach (label, 0, 1, top, top + 1);
				table.Attach (widget, 1, 2, top, top + 1);
				top++;
			}
			
			table.Show ();
			
			Properties = table;*/
		}

		public override SelectedBorder SelectedObject {
			set {
				propTable = null;
				spinTable = null;
				SetDependencyObject (value);
			}
		}
		
		// callbacks
		void OnIntegerChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			SpinButton spin = (SpinButton) o;
			
			if (((Entry) spin).Text == "Auto") {
				((SelectedBorder)item).Child.SetValue<double> (prop, 0.0);
			} else {
				double v = spin.Value;
				((SelectedBorder)item).Child.SetValue<double> (prop, v);
                ((SelectedBorder)item).ResizeBorder();
                Console.WriteLine("Changed other integer: {0:0}", v);
			}
		}
		
		void OnAutoClicked (object o, EventArgs e)
		{
			SpinButton spin = (SpinButton) spinTable[o];
			((Entry) spin).Text = "Auto";
		}
	}
}
