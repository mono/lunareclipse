// /cvs/lunareclipse/Properties/PropertyGroupAppearance.cs created with MonoDevelop
// User: fejj at 12:21 PM 6/28/2007
//

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using LunarEclipse.Model;
using System.Windows;
using System.Windows.Media;
using LunarEclipse.Controller;
using Gtk;
using PropertyPairList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.Type, System.Reflection.FieldInfo>>;
using LunarEclipse.Serialization;

namespace LunarEclipse {
	public class PropertyGroupAppearance : PropertyGroup {
		enum PropType {
			Data,
			Double,
			Integer,
			Percent,
			Stretch,
			DashArray,
			PenLineCap,
			PenLineJoin,
			Visibility,
		}
		
		static string [] stretchEnums = Enum.GetNames(typeof(System.Windows.Media.Stretch));
		static string [] penLineCapEnums = Enum.GetNames(typeof(System.Windows.Media.PenLineCap));
		static string [] penLineJoinEnums = Enum.GetNames(typeof(System.Windows.Media.PenLineJoin));
		static string [] visibilityEnums = Enum.GetNames(typeof(System.Windows.Visibility));
		
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
			new PropInfo ("OpacityProperty",            PropType.Percent,     false),
			new PropInfo ("VisibilityProperty",         PropType.Visibility,  false),
			new PropInfo ("DataProperty",               PropType.Data,        false),
			new PropInfo ("RadiusXProperty",            PropType.Double,      false),
			new PropInfo ("RadiusYProperty",            PropType.Double,      false),
			new PropInfo ("StrokeThicknessProperty",    PropType.Integer,     false),
			// extended properties
			new PropInfo ("StretchProperty",            PropType.Stretch,     true),
			new PropInfo ("StrokeDashArrayProperty",    PropType.DashArray,   true),
			new PropInfo ("StrokeDashCapProperty",      PropType.PenLineCap,  true),
			new PropInfo ("StrokeDashOffsetProperty",   PropType.Integer,     true),
			new PropInfo ("StrokeStartLineCapProperty", PropType.PenLineCap,  true),
			new PropInfo ("StrokeEndLineCapProperty",   PropType.PenLineCap,  true),
			new PropInfo ("StrokeLineJoinProperty",     PropType.PenLineJoin, true),
			new PropInfo ("StrokeMiterLimitProperty",   PropType.Integer,     true)
		};
		
		DependencyObject item;
		bool hasProps = false;
		Hashtable propTable;
		
		public PropertyGroupAppearance () : base ("Appearance")
		{
			
		}
#warning FIX THIS
		void SetDependencyObject (SelectedBorder item)
		{/*
			uint rows = 0, erows = 0, i;
			uint top = 0, etop = 0;
			
			this.item = item;
			
			if (item == null) {
				Properties = null;
				return;
			}
			
			Hashtable props = new Hashtable ();
            PropertyPairList pairs = ReflectionHelper.GetProps(item.Child);
            foreach(KeyValuePair<Type, FieldInfo> keypair in pairs)
            {
                for(int j=0; j < info.Length; j++)
                {
                    if(keypair.Value.Name != info[j].Name)
                        continue;

                    if (info[j].Extended)
					    erows++;
			        else
				        rows++;
				    props.Add (info[j].Name, keypair.Value);
				    break;
                }
            }
			
			hasProps = props.Count > 0;
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
				FieldInfo field = (FieldInfo) props[info[i].Name];
				if (field == null)
					continue;
				
				string propName = info[i].Name.Substring (0, info[i].Name.Length - 8);
				DependencyProperty prop = (DependencyProperty) field.GetValue (((SelectedBorder)item).Child);
				object value = ((SelectedBorder)item).Child.GetValue (prop);
				Adjustment adj;
				
				Widget label = new Label (propName);
				label.Show ();
				
				Widget widget = null;
				
				switch (info[i].Type) {
				case PropType.PenLineCap:
					widget = new ComboBox (penLineCapEnums);
					((ComboBox) widget).Active = (int) value;
					((ComboBox) widget).Changed += new EventHandler (OnCapChanged);
					break;
				case PropType.Data:
					widget = new Entry ();
					((Entry) widget).Changed += new EventHandler (OnDataChanged);
					break;
				case PropType.Double:
					adj = new Adjustment (0.0, 0.0, Double.MaxValue, 1.0, 10.0, 100.0);
					widget = new SpinButton (adj, 1.0, 2);
					((SpinButton) widget).Numeric = true;
					if (value != null)
						((SpinButton) widget).Value = (double) value;
					((SpinButton) widget).Changed += new EventHandler (OnDoubleChanged);
					break;
				case PropType.Integer:
					adj = new Adjustment (0.0, 0.0, Int32.MaxValue, 1.0, 10.0, 100.0);
					widget = new SpinButton (adj, 1.0, 0);
					((SpinButton) widget).Numeric = true;
					if (value != null)
						((SpinButton) widget).Value = (double) value;
					((SpinButton) widget).Changed += new EventHandler (OnIntegerChanged);
					break;
				case PropType.Percent:
					adj = new Adjustment (0.0, 0.0, 1.0, 0.01, 0.1, 1.0);
					widget = new SpinButton (adj, 0.01, 2);
					((SpinButton) widget).Numeric = true;
					((SpinButton) widget).SetRange (0.0, 1.0);
					if (value != null)
						((SpinButton) widget).Value = (double) value;
					((SpinButton) widget).Changed += new EventHandler (OnDoubleChanged);
					break;
				case PropType.Stretch:
					widget = new ComboBox (stretchEnums);
					((ComboBox) widget).Active = (int) value;
					((ComboBox) widget).Changed += new EventHandler (OnStretchChanged);
					break;
				case PropType.PenLineJoin:
					widget = new ComboBox (penLineJoinEnums);
					((ComboBox) widget).Active = (int) value;
					((ComboBox) widget).Changed += new EventHandler (OnLineJoinChanged);
					break;
				case PropType.DashArray:
					widget = new Button ("...");
					break;
				case PropType.Visibility:
					widget = new ComboBox (visibilityEnums);
					((ComboBox) widget).Active = (int) value;
					((ComboBox) widget).Changed += new EventHandler (OnVisibilityChanged);
					break;
				}
				
				propTable.Add (widget, prop);
				widget.Show ();
				
				if (info[i].Extended) {
					((Table) extended).Attach (label, 0, 1, etop, etop + 1);
					((Table) extended).Attach (widget, 1, 2, etop, etop + 1);
					etop++;
				} else {
					((Table) main).Attach (label, 0, 1, top, top + 1);
					((Table) main).Attach (widget, 1, 2, top, top + 1);
					top++;
				}
			}
			
			main.Show ();
			
			Widget properties = new VBox (false, 3);
			((Box) properties).PackStart (main);

			if (erows > 0) {
				Widget expander = new Expander ("More...");
				((Expander) expander).Expanded = false;
				
				extended.Show ();
				((Container) expander).Add (extended);
				
				expander.Show ();
				((Box) properties).PackStart (expander);
			}
			
			properties.Show ();
			Properties = properties;*/
		}
		
		public override bool HasProperties {
			get { return hasProps; }
		}
		
		public override SelectedBorder SelectedObject {
			set {
				propTable = null;
				SetDependencyObject (value);
			}
		}
		
		// callbacks
		void OnCapChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			ComboBox combo = (ComboBox) o;
			
			((SelectedBorder)item).Child.SetValue<PenLineCap> (prop, (PenLineCap) combo.Active);
		    ((SelectedBorder)item).ResizeBorder();
        }
		
		void OnStretchChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			ComboBox combo = (ComboBox) o;
			
			((SelectedBorder)item).Child.SetValue<Stretch> (prop, (Stretch) combo.Active);
		    ((SelectedBorder)item).ResizeBorder();
        }
		
		void OnLineJoinChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			ComboBox combo = (ComboBox) o;
			
			((SelectedBorder)item).Child.SetValue<PenLineJoin> (prop, (PenLineJoin) combo.Active);
		    ((SelectedBorder)item).ResizeBorder();
        }
		
		void OnVisibilityChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			ComboBox combo = (ComboBox) o;
			
			((SelectedBorder)item).Child.SetValue<Visibility> (prop, (Visibility) combo.Active);
		    ((SelectedBorder)item).ResizeBorder();
        }
		
		void OnDoubleChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			SpinButton spin = (SpinButton) o;
			double v = spin.Value;
			
			((SelectedBorder)item).Child.SetValue<double> (prop, v);
            ((SelectedBorder)item).ResizeBorder();
            Console.WriteLine("Changed double");
		}
		
		void OnIntegerChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			SpinButton spin = (SpinButton) o;
			int v = (int) spin.Value;
			
			((SelectedBorder)item).Child.SetValue<int> (prop, v);
            ((SelectedBorder)item).ResizeBorder();
            Console.WriteLine("Changed integer");
		}
		
		void OnDataChanged (object o, EventArgs e)
		{
			DependencyProperty prop = (DependencyProperty) propTable[o];
			Entry entry = (Entry) o;
			
			// FIXME: is this really of type string? or is it an array of Points?
			((SelectedBorder)item).Child.SetValue<string> (prop, entry.Text);
		    ((SelectedBorder)item).ResizeBorder();
        }
	}
}
