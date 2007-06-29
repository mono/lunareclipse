// /cvs/lunareclipse/Properties/PropertyGroupLayout.cs created with MonoDevelop
// User: fejj at 11:37 AM 6/29/2007
//

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using System.Windows;

using Gtk;

namespace LunarEclipse {
	public class PropertyGroupLayout : PropertyGroup {
		enum PropType {
			Double,
			Integer
		}
		
		struct PropInfo {
			public string Name;
			public PropType Type;
			public bool Attached;
			public bool CanAuto;
			
			public PropInfo (string name, PropType type, bool attached, bool canAuto)
			{
				Name = name;
				Type = type;
				Attached = attached;
				CanAuto = canAuto;
			}
		}
		
		static PropInfo [] info = new PropInfo [5] {
			new PropInfo ("WidthProperty",  PropType.Double,  false, true),
			new PropInfo ("HeightProperty", PropType.Double,  false, true),
			new PropInfo ("LeftProperty",   PropType.Double,  true,  false),
			new PropInfo ("TopProperty",    PropType.Double,  true,  false),
			new PropInfo ("ZIndexProperty", PropType.Integer, false, false),
		};
		
		DependencyObject item;
		Hashtable propTable;
		Hashtable spinTable;
		
		public PropertyGroupLayout () : base ("Layout")
		{
		}
		
		void SetDependencyObject (DependencyObject item)
		{
			int i;
			
			this.item = item;
			
			if (item == null) {
				Properties = null;
				return;
			}
			
			Hashtable props = new Hashtable ();
			for (Type type = item.GetType (); type != null; type = type.BaseType) {
				FieldInfo[] fields = type.GetFields ();
				foreach (FieldInfo field in fields) {
					if (!field.FieldType.Equals (typeof (DependencyProperty)))
						continue;
					
					for (i = 0; i < info.Length; i++) {
						if (field.Name == info[i].Name) {
							props.Add (field.Name, field);
							break;
						}
					}
				}
			}
			
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
				if (field == null)
					continue;
				
				string propName = info[i].Name.Substring (0, info[i].Name.Length - 8);
				DependencyProperty prop = (DependencyProperty) field.GetValue (item);
				object value = item.GetValue (prop);
				Widget label = new Label (propName);
				label.Show ();
				
				Adjustment adj = new Adjustment (0.0, 0.0, Int32.MaxValue, 1.0, 10.0, 100.0);
				Widget widget = new SpinButton (adj, 1.0, 0);
				((SpinButton) widget).SetRange (0.0, (double) Int32.MaxValue);
				((SpinButton) widget).Numeric = true;
				
				Console.WriteLine ("Layout: {0} = {1} ({2})", propName, value, value.GetType ());
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
				
				((SpinButton) widget).Changed += new EventHandler (OnIntegerChanged);
				((SpinButton) widget).ValueChanged += new EventHandler (OnIntegerChanged);
				
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
			
			Properties = table;
		}
		
		public override bool HasProperties {
			get { return item != null; }
		}
		
		public override DependencyObject DependencyObject {
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
				item.SetValue<double> (prop, 0.0);
			} else {
				double v = spin.Value;
				item.SetValue<double> (prop, v);
			}
		}
		
		void OnAutoClicked (object o, EventArgs e)
		{
			SpinButton spin = (SpinButton) spinTable[o];
			((Entry) spin).Text = "Auto";
		}
	}
}
