

using System;
using System.Collections.Generic;
using System.Reflection;
using Gtk;
using LunarEclipse.Model;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

using PropertyPairList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.Type, System.Reflection.FieldInfo>>;

namespace LunarEclipse
{
    public class PropertyGroupBrushes : PropertyGroup
    {
        private Button active;
        
        private bool hasProperties;
        private Button fillButton;
        private Button strokeButton;
        private Button backgroundButton;
        private ColorSelection colorPicker;
        
        public PropertyGroupBrushes () : base ("Colors")
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
                    SelectedObject.Child.SetValue<Brush>(Shape.FillProperty, brush);
                else if(active == backgroundButton)
                    SelectedObject.Child.SetValue<Brush>(Canvas.BackgroundProperty, brush);
                else if(active == strokeButton)
                    SelectedObject.Child.SetValue<Brush>(Shape.StrokeProperty, brush);
            };
			
			colorPicker.Show ();
            box.PackEnd(colorPicker);
            box.Show ();
            Properties = box;
        }
        
        public override bool HasProperties {
            get { return hasProperties; }
        }
        
        public override SelectedBorder SelectedObject {
            set 
            {
                base.SelectedObject = value;
                SetProperties(value);
            }
        }
        
        private void SetProperties(SelectedBorder border)
        {
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
                Properties.Show();
        }
    }
}
