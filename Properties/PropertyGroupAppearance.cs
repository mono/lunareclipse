// /cvs/lunareclipse/Properties/PropertyGroupAppearance.cs created with MonoDevelop
// User: fejj at 12:21 PM 6/28/2007
//

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using System.Windows;

using Gtk;

namespace LunarEclipse {
	public class PropertyGroupAppearance : PropertyGroup {
		enum PropType {
			Cap,
			Data,
			Double,
			Integer,
			Percent,
			Stretch,
			LineJoin,
			DashArray,
			Visibility,
		}
		
		static string [] capEntries = { "Flat", "Square", "Round", "Triangle" };
		static string [] stretchEntries = { "None", "Fill", "Uniform", "UniformToFill" };
		static string [] lineJoinEntries = { "Miter", "Bevel", "Round" };
		static string [] visibilityEntries = { "Visible", "Hidden", "Collapsed" };
		
		struct PropInfo {
			public string Name;
			public PropType Type;
			public bool Extended;
			
			public PropInfo (string name, PropType type, bool extended)
			{
				Name = name;
				Type = type;
				Extended = extended;
			}
		}
		
		static PropInfo [] info = new PropInfo [14] {
			new PropInfo ("OpacityProperty",            PropType.Percent,    false),
			new PropInfo ("VisibilityProperty",         PropType.Visibility, false),
			new PropInfo ("DataProperty",               PropType.Data,       false),
			new PropInfo ("RadiusXProperty",            PropType.Double,     false),
			new PropInfo ("RadiusYProperty",            PropType.Double,     false),
			new PropInfo ("StrokeThicknessProperty",    PropType.Integer,    false),
			// extended properties
			new PropInfo ("StretchProperty",            PropType.Stretch,    true),
			new PropInfo ("StrokeDashArrayProperty",    PropType.DashArray,  true),
			new PropInfo ("StrokeDashCapProperty",      PropType.Cap,        true),
			new PropInfo ("StrokeDashOffsetProperty",   PropType.Integer,    true),
			new PropInfo ("StrokeStartLineCapProperty", PropType.Cap,        true),
			new PropInfo ("StrokeEndLineCapProperty",   PropType.Cap,        true),
			new PropInfo ("StrokeLineJoinProperty",     PropType.LineJoin,   true),
			new PropInfo ("StrokeMiterLimitProperty",   PropType.Integer,    true)
		};
		
		DependencyObject item;
		bool hasProps = false;
		Hashtable propTable;
		
		public PropertyGroupAppearance () : base ("Appearance")
		{
		}
		
		void SetDependencyObject (DependencyObject item)
		{
			uint rows = 0, erows = 0, i;
			uint top = 0, etop = 0;
			
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
							if (info[i].Extended)
								erows++;
							else
								rows++;
							props.Add (field);
							hasProps = true;
							break;
						}
					}
				}
			}
			
			if (!hasProps) {
				Properties = null;
				return;
			}
			
			if (rows == 0)
				Console.WriteLine ("rows == 0; this should not happen?");
			
			Widget main = new Table (rows, 2, false);
			Widget extended = erows > 0 ? new Table (erows, 2, false) : null;
			
			propTable = new Hashtable ();
			
			for (i = 0; i < info.Length; i++) {
				foreach (FieldInfo field in props) {
					if (field.Name == info[i].Name) {
						string propName = info[i].Name.Substring (0, info[i].Name.Length - 8);
						DependencyProperty prop = (DependencyProperty) field.GetValue (item);
						Widget label = new Label (propName);
						label.Show ();
						
						Widget value = null;
						
						switch (info[i].Type) {
						case PropType.Cap:
							value = new ComboBox (capEntries);
							((ComboBox) value).Changed += new EventHandler (OnComboChanged);
							break;
						case PropType.Data:
							value = new Entry ();
							((Entry) value).Changed += new EventHandler (OnDataChanged);
							break;
						case PropType.Double:
							value = new SpinButton (null, 1.0, 2);
							((SpinButton) value).Numeric = true;
							((SpinButton) value).SetRange (0.0, Double.MaxValue);
							((SpinButton) value).Changed += new EventHandler (OnDoubleChanged);
							break;
						case PropType.Integer:
							value = new SpinButton (null, 1.0, 0);
							((SpinButton) value).Numeric = true;
							((SpinButton) value).SetRange (0.0, Int32.MaxValue);
							((SpinButton) value).Changed += new EventHandler (OnIntegerChanged);
							break;
						case PropType.Percent:
							value = new SpinButton (null, 0.01, 2);
							((SpinButton) value).Numeric = true;
							((SpinButton) value).SetRange (0.0, 1.0);
							((SpinButton) value).Changed += new EventHandler (OnDoubleChanged);
							break;
						case PropType.Stretch:
							value = new ComboBox (stretchEntries);
							((ComboBox) value).Changed += new EventHandler (OnComboChanged);
							break;
						case PropType.LineJoin:
							value = new ComboBox (lineJoinEntries);
							((ComboBox) value).Changed += new EventHandler (OnComboChanged);
							break;
						case PropType.DashArray:
							value = new Button ("...");
							break;
						case PropType.Visibility:
							value = new ComboBox (visibilityEntries);
							((ComboBox) value).Changed += new EventHandler (OnComboChanged);
							break;
						}
						
						propTable.Add (value, prop);
						value.Show ();
						
						if (info[i].Extended) {
							((Table) extended).Attach (label, 0, 1, etop, etop + 1);
							((Table) extended).Attach (value, 1, 2, etop, etop + 1);
							etop++;
						} else {
							((Table) main).Attach (label, 0, 1, top, top + 1);
							((Table) main).Attach (value, 1, 2, top, top + 1);
							top++;
						}
					}
				}
			}
			
			main.Show ();
			
			Widget properties = new VBox (false, 3);
			((Box) properties).PackStart (main);
			
			if (erows > 0) {
				Widget expander = new Expander ("");
				((Expander) expander).Expanded = false;
				
				extended.Show ();
				((Container) expander).Add (extended);
				
				expander.Show ();
				((Box) properties).PackStart (expander);
			}
			
			properties.Show ();
			Properties = properties;
		}
		
		public override bool HasProperties {
			get { return hasProps; }
		}
		
		public override DependencyObject DependencyObject {
			set {
				propTable = null;
				SetDependencyObject (value);
			}
		}
		
		// callbacks
		void OnComboChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			ComboBox combo = (ComboBox) o;
			
			item.SetValue (prop, combo.ActiveText);
		}
		
		void OnDoubleChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			SpinButton spin = (SpinButton) o;
			double v = spin.Value;
			
			item.SetValue<double> (prop, v);
		}
		
		void OnIntegerChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			SpinButton spin = (SpinButton) o;
			int v = (int) spin.Value;
			
			item.SetValue<int> (prop, v);
		}
		
		void OnDataChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			Entry entry = (Entry) o;
			
			// FIXME: is this really of type string? or is it an array of Points?
			item.SetValue<string> (prop, entry.Text);
		}
	}
}
