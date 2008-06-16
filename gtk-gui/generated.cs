// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Stetic {
    
    
    internal class Gui {
        
        private static bool initialized;
        
        internal static void Initialize(Gtk.Widget iconRenderer) {
            if ((Stetic.Gui.initialized == false)) {
                Stetic.Gui.initialized = true;
                Gtk.IconFactory w1 = new Gtk.IconFactory();
                Gtk.IconSet w2 = new Gtk.IconSet(new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "./Icons/circle-tool.png")));
                w1.Add("circle-tool", w2);
                Gtk.IconSet w3 = new Gtk.IconSet(new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "./Icons/ellipse-tool.png")));
                w1.Add("ellipse-tool", w3);
                Gtk.IconSet w4 = new Gtk.IconSet(new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "./Icons/line-tool.png")));
                w1.Add("line-tool", w4);
                Gtk.IconSet w5 = new Gtk.IconSet(new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "./Icons/path-tool.png")));
                w1.Add("path-tool", w5);
                Gtk.IconSet w6 = new Gtk.IconSet(new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "./Icons/rectangle-tool.png")));
                w1.Add("rectangle-tool", w6);
                Gtk.IconSet w7 = new Gtk.IconSet(new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "./Icons/selection-tool.png")));
                w1.Add("selection-tool", w7);
                Gtk.IconSet w8 = new Gtk.IconSet(new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "./Icons/square-tool.png")));
                w1.Add("square-tool", w8);
                w1.AddDefault();
            }
        }
    }
    
    internal class BinContainer {
        
        private Gtk.Widget child;
        
        private Gtk.UIManager uimanager;
        
        public static BinContainer Attach(Gtk.Bin bin) {
            BinContainer bc = new BinContainer();
            bin.SizeRequested += new Gtk.SizeRequestedHandler(bc.OnSizeRequested);
            bin.SizeAllocated += new Gtk.SizeAllocatedHandler(bc.OnSizeAllocated);
            bin.Added += new Gtk.AddedHandler(bc.OnAdded);
            return bc;
        }
        
        private void OnSizeRequested(object sender, Gtk.SizeRequestedArgs args) {
            if ((this.child != null)) {
                args.Requisition = this.child.SizeRequest();
            }
        }
        
        private void OnSizeAllocated(object sender, Gtk.SizeAllocatedArgs args) {
            if ((this.child != null)) {
                this.child.Allocation = args.Allocation;
            }
        }
        
        private void OnAdded(object sender, Gtk.AddedArgs args) {
            this.child = args.Widget;
        }
        
        public void SetUiManager(Gtk.UIManager uim) {
            this.uimanager = uim;
            this.child.Realized += new System.EventHandler(this.OnRealized);
        }
        
        private void OnRealized(object sender, System.EventArgs args) {
            if ((this.uimanager != null)) {
                Gtk.Widget w;
                w = this.child.Toplevel;
                if (((w != null) && typeof(Gtk.Window).IsInstanceOfType(w))) {
                    ((Gtk.Window)(w)).AddAccelGroup(this.uimanager.AccelGroup);
                    this.uimanager = null;
                }
            }
        }
    }
    
    internal class ActionGroups {
        
        public static Gtk.ActionGroup GetActionGroup(System.Type type) {
            return Stetic.ActionGroups.GetActionGroup(type.FullName);
        }
        
        public static Gtk.ActionGroup GetActionGroup(string name) {
            return null;
        }
    }
}
