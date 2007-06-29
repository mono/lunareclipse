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
		struct PropInfo {
			public string Name;
			public bool Attached;
			public bool CanAuto;
			
			public PropInfo (string name, bool attached, bool canAuto)
			{
				Name = name;
				Attached = attached;
				CanAuto = canAuto;
			}
		}
		
		static PropInfo [] info = new PropInfo [5] {
			new PropInfo ("WidthProperty",  false, true),
			new PropInfo ("HeightProperty", false, true),
			new PropInfo ("LeftProperty",   true,  false),
			new PropInfo ("TopProperty",    true,  false),
			new PropInfo ("ZIndexProperty", false, false),
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
			
			List<FieldInfo> props = new List<FieldInfo> ();
			for (Type current = item.GetType (); current != null; current = current.BaseType) {
				FieldInfo[] currentFields = current.GetFields ();
				foreach (FieldInfo field in currentFields) {
					if (!field.FieldType.Equals (typeof (DependencyProperty)))
						continue;
					
					for (i = 0; i < info.Length; i++) {
						if (field.Name == info[i].Name) {
							props.Add (field);
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
				foreach (FieldInfo field in props) {
					if (field.Name == info[i].Name) {
						string propName = info[i].Name.Substring (0, info[i].Name.Length - 8);
						DependencyProperty prop = (DependencyProperty) field.GetValue (item);
						Widget label = new Label (propName);
						label.Show ();
						
						Widget value = new SpinButton (null, 1.0, 0);
						((SpinButton) value).Changed += new EventHandler (OnIntegerChanged);
						((SpinButton) value).SetRange (0.0, Int32.MaxValue);
						((SpinButton) value).Numeric = true;
						
						propTable.Add (value, prop);
						value.Show ();
						
						if (info[i].CanAuto) {
							Box hbox = new HBox (false, 6);
							hbox.PackStart (value, true, true, 0);
							Button button = new Button ("Auto");
							button.Clicked += new EventHandler (OnAutoClicked);
							spinTable.Add (button, value);
							button.Show ();
							hbox.PackStart (button, false, false, 0);
							hbox.Show ();
							
							value = hbox;
						}
						
						table.Attach (label, 0, 1, top, top + 1);
						table.Attach (value, 1, 2, top, top + 1);
						top++;
					}
				}
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
				item.SetValue<int> (prop, 0);
			} else {
				int v = (int) spin.Value;
				item.SetValue<int> (prop, v);
			}
		}
		
		void OnAutoClicked (object o, EventArgs e)
		{
			SpinButton spin = (SpinButton) spinTable[o];
			((Entry) spin).Text = "Auto";
		}
	}
}
