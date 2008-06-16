// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace LunarEclipse.View {
    
    
    public partial class MainWindow {
        
        private Gtk.Action FileAction;
        
        private Gtk.Action AbrirAction;
        
        private Gtk.Action NuevoAction;
        
        private Gtk.Action GuardarAction;
        
        private Gtk.Action GuardarComoAction;
        
        private Gtk.Action SalirAction;
        
        private Gtk.Action EditAction;
        
        private Gtk.Action ToolsAction;
        
        private Gtk.Action HelpAction;
        
        private Gtk.Action DeshacerAction;
        
        private Gtk.Action RehacerAction;
        
        private Gtk.Action CortarAction;
        
        private Gtk.Action CopiarAction;
        
        private Gtk.Action PegarAction;
        
        private Gtk.Action BorrarAction;
        
        private Gtk.Action AcercaDeAction;
        
        private Gtk.RadioAction SelectionToolAction;
        
        private Gtk.RadioAction RectangleToolAction;
        
        private Gtk.RadioAction SquareToolAction;
        
        private Gtk.RadioAction EllipseToolAction;
        
        private Gtk.RadioAction CircleToolAction;
        
        private Gtk.RadioAction PathToolAction;
        
        private Gtk.RadioAction TextToolAction;
        
        private Gtk.RadioAction ImageToolAction;
        
        private Gtk.Action AnimationAction;
        
        private Gtk.Action GrabarAction;
        
        private Gtk.Action ReproducirAction;
        
        private Gtk.Action DetenerAction;
        
        private Gtk.RadioAction LineToolAction;
        
        private Gtk.Action DrawingAction;
        
        private Gtk.Action LimpiarAction;
        
        private Gtk.VBox vbox1;
        
        private Gtk.MenuBar menubar1;
        
        private Gtk.Toolbar toolbar1;
        
        private Gtk.Notebook notebook;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Toolbar toolbar2;
        
        private Gtk.VBox vbox2;
        
        private Gtk.ScrolledWindow scrolledwindow1;
        
        private LunarEclipse.View.MoonlightWidget moonlightwidget;
        
        private LunarEclipse.Controls.AnimationTimelineWidget animationtimelinewidget1;
        
        private Gtk.Label label2;
        
        private Gtk.ScrolledWindow scrolledwindow2;
        
        private Gtk.TextView xaml_textview;
        
        private Gtk.Label label1;
        
        private Gtk.Statusbar statusbar1;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget LunarEclipse.View.MainWindow
            Gtk.UIManager w1 = new Gtk.UIManager();
            Gtk.ActionGroup w2 = new Gtk.ActionGroup("Default");
            this.FileAction = new Gtk.Action("FileAction", Mono.Unix.Catalog.GetString("_File"), null, null);
            this.FileAction.ShortLabel = Mono.Unix.Catalog.GetString("_File");
            w2.Add(this.FileAction, null);
            this.AbrirAction = new Gtk.Action("AbrirAction", Mono.Unix.Catalog.GetString("_Abrir"), null, "gtk-open");
            this.AbrirAction.ShortLabel = Mono.Unix.Catalog.GetString("_Abrir");
            w2.Add(this.AbrirAction, null);
            this.NuevoAction = new Gtk.Action("NuevoAction", Mono.Unix.Catalog.GetString("_Nuevo"), null, "gtk-new");
            this.NuevoAction.ShortLabel = Mono.Unix.Catalog.GetString("_Nuevo");
            w2.Add(this.NuevoAction, null);
            this.GuardarAction = new Gtk.Action("GuardarAction", Mono.Unix.Catalog.GetString("_Guardar"), null, "gtk-save");
            this.GuardarAction.ShortLabel = Mono.Unix.Catalog.GetString("_Guardar");
            w2.Add(this.GuardarAction, null);
            this.GuardarComoAction = new Gtk.Action("GuardarComoAction", Mono.Unix.Catalog.GetString("Guardar _como"), null, "gtk-save-as");
            this.GuardarComoAction.ShortLabel = Mono.Unix.Catalog.GetString("Guardar _como");
            w2.Add(this.GuardarComoAction, null);
            this.SalirAction = new Gtk.Action("SalirAction", Mono.Unix.Catalog.GetString("_Salir"), null, "gtk-quit");
            this.SalirAction.ShortLabel = Mono.Unix.Catalog.GetString("_Salir");
            w2.Add(this.SalirAction, null);
            this.EditAction = new Gtk.Action("EditAction", Mono.Unix.Catalog.GetString("_Edit"), null, null);
            this.EditAction.ShortLabel = Mono.Unix.Catalog.GetString("_Edit");
            w2.Add(this.EditAction, null);
            this.ToolsAction = new Gtk.Action("ToolsAction", Mono.Unix.Catalog.GetString("_Tools"), null, null);
            this.ToolsAction.ShortLabel = Mono.Unix.Catalog.GetString("_Tools");
            w2.Add(this.ToolsAction, null);
            this.HelpAction = new Gtk.Action("HelpAction", Mono.Unix.Catalog.GetString("_Help"), null, null);
            this.HelpAction.ShortLabel = Mono.Unix.Catalog.GetString("_Help");
            w2.Add(this.HelpAction, null);
            this.DeshacerAction = new Gtk.Action("DeshacerAction", Mono.Unix.Catalog.GetString("_Deshacer"), null, "gtk-undo");
            this.DeshacerAction.ShortLabel = Mono.Unix.Catalog.GetString("_Deshacer");
            w2.Add(this.DeshacerAction, null);
            this.RehacerAction = new Gtk.Action("RehacerAction", Mono.Unix.Catalog.GetString("_Rehacer"), null, "gtk-redo");
            this.RehacerAction.ShortLabel = Mono.Unix.Catalog.GetString("_Rehacer");
            w2.Add(this.RehacerAction, null);
            this.CortarAction = new Gtk.Action("CortarAction", Mono.Unix.Catalog.GetString("Cor_tar"), null, "gtk-cut");
            this.CortarAction.ShortLabel = Mono.Unix.Catalog.GetString("Cor_tar");
            w2.Add(this.CortarAction, null);
            this.CopiarAction = new Gtk.Action("CopiarAction", Mono.Unix.Catalog.GetString("_Copiar"), null, "gtk-copy");
            this.CopiarAction.ShortLabel = Mono.Unix.Catalog.GetString("_Copiar");
            w2.Add(this.CopiarAction, null);
            this.PegarAction = new Gtk.Action("PegarAction", Mono.Unix.Catalog.GetString("_Pegar"), null, "gtk-paste");
            this.PegarAction.ShortLabel = Mono.Unix.Catalog.GetString("_Pegar");
            w2.Add(this.PegarAction, null);
            this.BorrarAction = new Gtk.Action("BorrarAction", Mono.Unix.Catalog.GetString("_Borrar"), null, "gtk-delete");
            this.BorrarAction.ShortLabel = Mono.Unix.Catalog.GetString("_Borrar");
            w2.Add(this.BorrarAction, null);
            this.AcercaDeAction = new Gtk.Action("AcercaDeAction", Mono.Unix.Catalog.GetString("Acerca _de"), null, "gtk-about");
            this.AcercaDeAction.ShortLabel = Mono.Unix.Catalog.GetString("Acerca _de");
            w2.Add(this.AcercaDeAction, null);
            this.SelectionToolAction = new Gtk.RadioAction("SelectionToolAction", Mono.Unix.Catalog.GetString("_Selection Tool"), null, "selection-tool", 0);
            this.SelectionToolAction.Group = new GLib.SList(System.IntPtr.Zero);
            this.SelectionToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Selection Tool");
            w2.Add(this.SelectionToolAction, null);
            this.RectangleToolAction = new Gtk.RadioAction("RectangleToolAction", Mono.Unix.Catalog.GetString("_Rectangle Tool"), null, "rectangle-tool", 0);
            this.RectangleToolAction.Group = this.SelectionToolAction.Group;
            this.RectangleToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Rectangle Tool");
            w2.Add(this.RectangleToolAction, null);
            this.SquareToolAction = new Gtk.RadioAction("SquareToolAction", Mono.Unix.Catalog.GetString("S_quare Tool"), null, "square-tool", 0);
            this.SquareToolAction.Group = this.RectangleToolAction.Group;
            this.SquareToolAction.ShortLabel = Mono.Unix.Catalog.GetString("S_quare Tool");
            w2.Add(this.SquareToolAction, null);
            this.EllipseToolAction = new Gtk.RadioAction("EllipseToolAction", Mono.Unix.Catalog.GetString("_Ellipse Tool"), null, "ellipse-tool", 0);
            this.EllipseToolAction.Group = this.RectangleToolAction.Group;
            this.EllipseToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Ellipse Tool");
            w2.Add(this.EllipseToolAction, null);
            this.CircleToolAction = new Gtk.RadioAction("CircleToolAction", Mono.Unix.Catalog.GetString("_Circle Tool"), null, "circle-tool", 0);
            this.CircleToolAction.Group = this.RectangleToolAction.Group;
            this.CircleToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Circle Tool");
            w2.Add(this.CircleToolAction, null);
            this.PathToolAction = new Gtk.RadioAction("PathToolAction", Mono.Unix.Catalog.GetString("_Path Tool"), null, "path-tool", 0);
            this.PathToolAction.Group = this.RectangleToolAction.Group;
            this.PathToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Path Tool");
            w2.Add(this.PathToolAction, null);
            this.TextToolAction = new Gtk.RadioAction("TextToolAction", Mono.Unix.Catalog.GetString("_Text Tool"), null, null, 0);
            this.TextToolAction.Group = this.RectangleToolAction.Group;
            this.TextToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Text Tool");
            w2.Add(this.TextToolAction, null);
            this.ImageToolAction = new Gtk.RadioAction("ImageToolAction", Mono.Unix.Catalog.GetString("Image Tool"), null, null, 0);
            this.ImageToolAction.Group = this.RectangleToolAction.Group;
            this.ImageToolAction.ShortLabel = Mono.Unix.Catalog.GetString("Image Tool");
            w2.Add(this.ImageToolAction, null);
            this.AnimationAction = new Gtk.Action("AnimationAction", Mono.Unix.Catalog.GetString("_Animation"), null, null);
            this.AnimationAction.ShortLabel = Mono.Unix.Catalog.GetString("_Animation");
            w2.Add(this.AnimationAction, null);
            this.GrabarAction = new Gtk.Action("GrabarAction", Mono.Unix.Catalog.GetString("_Grabar"), null, "gtk-media-record");
            this.GrabarAction.ShortLabel = Mono.Unix.Catalog.GetString("_Grabar");
            w2.Add(this.GrabarAction, null);
            this.ReproducirAction = new Gtk.Action("ReproducirAction", Mono.Unix.Catalog.GetString("_Reproducir"), null, "gtk-media-play");
            this.ReproducirAction.ShortLabel = Mono.Unix.Catalog.GetString("_Reproducir");
            w2.Add(this.ReproducirAction, null);
            this.DetenerAction = new Gtk.Action("DetenerAction", Mono.Unix.Catalog.GetString("_Detener"), null, "gtk-media-stop");
            this.DetenerAction.ShortLabel = Mono.Unix.Catalog.GetString("_Detener");
            w2.Add(this.DetenerAction, null);
            this.LineToolAction = new Gtk.RadioAction("LineToolAction", Mono.Unix.Catalog.GetString("_Line Tool"), null, "line-tool", 0);
            this.LineToolAction.Group = this.RectangleToolAction.Group;
            this.LineToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Line Tool");
            w2.Add(this.LineToolAction, null);
            this.DrawingAction = new Gtk.Action("DrawingAction", Mono.Unix.Catalog.GetString("Drawing"), null, null);
            this.DrawingAction.ShortLabel = Mono.Unix.Catalog.GetString("Drawing");
            w2.Add(this.DrawingAction, null);
            this.LimpiarAction = new Gtk.Action("LimpiarAction", Mono.Unix.Catalog.GetString("_Limpiar"), null, "gtk-clear");
            this.LimpiarAction.ShortLabel = Mono.Unix.Catalog.GetString("_Limpiar");
            w2.Add(this.LimpiarAction, null);
            w1.InsertActionGroup(w2, 0);
            this.AddAccelGroup(w1.AccelGroup);
            this.Name = "LunarEclipse.View.MainWindow";
            this.Title = Mono.Unix.Catalog.GetString("Lunar Eclipse");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Container child LunarEclipse.View.MainWindow.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            w1.AddUiFromString("<ui><menubar name='menubar1'><menu action='FileAction'><menuitem action='NuevoAction'/><menuitem action='AbrirAction'/><menuitem action='GuardarAction'/><menuitem action='GuardarComoAction'/><separator/><menuitem action='SalirAction'/></menu><menu action='EditAction'><menuitem action='DeshacerAction'/><menuitem action='RehacerAction'/><separator/><menuitem action='CortarAction'/><menuitem action='CopiarAction'/><menuitem action='PegarAction'/><menuitem action='BorrarAction'/></menu><menu action='DrawingAction'><menuitem action='LimpiarAction'/></menu><menu action='ToolsAction'><menuitem action='SelectionToolAction'/><menuitem action='RectangleToolAction'/><menuitem action='SquareToolAction'/><menuitem action='EllipseToolAction'/><menuitem action='CircleToolAction'/><menuitem action='LineToolAction'/><menuitem action='PathToolAction'/><menuitem action='TextToolAction'/><menuitem action='ImageToolAction'/></menu><menu action='AnimationAction'><menuitem action='GrabarAction'/><menuitem action='ReproducirAction'/><menuitem action='DetenerAction'/></menu><menu action='HelpAction'><menuitem action='AcercaDeAction'/></menu></menubar></ui>");
            this.menubar1 = ((Gtk.MenuBar)(w1.GetWidget("/menubar1")));
            this.menubar1.Name = "menubar1";
            this.vbox1.Add(this.menubar1);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox1[this.menubar1]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            w1.AddUiFromString("<ui><toolbar name='toolbar1'><toolitem action='NuevoAction'/><toolitem action='AbrirAction'/><toolitem action='GuardarAction'/><separator/><toolitem action='DeshacerAction'/><toolitem action='RehacerAction'/><separator/><toolitem action='PegarAction'/><toolitem action='CopiarAction'/><toolitem action='CortarAction'/><toolitem action='BorrarAction'/><separator/><toolitem action='ReproducirAction'/><toolitem action='DetenerAction'/><toolitem action='GrabarAction'/></toolbar></ui>");
            this.toolbar1 = ((Gtk.Toolbar)(w1.GetWidget("/toolbar1")));
            this.toolbar1.Name = "toolbar1";
            this.toolbar1.ShowArrow = false;
            this.toolbar1.ToolbarStyle = ((Gtk.ToolbarStyle)(0));
            this.vbox1.Add(this.toolbar1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox1[this.toolbar1]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.notebook = new Gtk.Notebook();
            this.notebook.CanFocus = true;
            this.notebook.Name = "notebook";
            this.notebook.CurrentPage = 0;
            this.notebook.TabPos = ((Gtk.PositionType)(3));
            // Container child notebook.Gtk.Notebook+NotebookChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            w1.AddUiFromString("<ui><toolbar name='toolbar2'><toolitem action='SelectionToolAction'/><toolitem action='RectangleToolAction'/><toolitem action='SquareToolAction'/><toolitem action='EllipseToolAction'/><toolitem action='CircleToolAction'/><toolitem action='PathToolAction'/><toolitem action='LineToolAction'/><separator/><toolitem action='LimpiarAction'/></toolbar></ui>");
            this.toolbar2 = ((Gtk.Toolbar)(w1.GetWidget("/toolbar2")));
            this.toolbar2.Name = "toolbar2";
            this.toolbar2.Orientation = ((Gtk.Orientation)(1));
            this.toolbar2.ShowArrow = false;
            this.toolbar2.ToolbarStyle = ((Gtk.ToolbarStyle)(0));
            this.hbox2.Add(this.toolbar2);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.hbox2[this.toolbar2]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.VscrollbarPolicy = ((Gtk.PolicyType)(0));
            this.scrolledwindow1.HscrollbarPolicy = ((Gtk.PolicyType)(0));
            // Container child scrolledwindow1.Gtk.Container+ContainerChild
            Gtk.Viewport w6 = new Gtk.Viewport();
            w6.ShadowType = ((Gtk.ShadowType)(0));
            // Container child GtkViewport.Gtk.Container+ContainerChild
            this.moonlightwidget = new LunarEclipse.View.MoonlightWidget();
            this.moonlightwidget.Events = ((Gdk.EventMask)(256));
            this.moonlightwidget.Name = "moonlightwidget";
            this.moonlightwidget.Height = 600;
            this.moonlightwidget.Width = 600;
            w6.Add(this.moonlightwidget);
            this.scrolledwindow1.Add(w6);
            this.vbox2.Add(this.scrolledwindow1);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox2[this.scrolledwindow1]));
            w9.Position = 0;
            // Container child vbox2.Gtk.Box+BoxChild
            this.animationtimelinewidget1 = new LunarEclipse.Controls.AnimationTimelineWidget();
            this.animationtimelinewidget1.WidthRequest = 70;
            this.animationtimelinewidget1.HeightRequest = 70;
            this.animationtimelinewidget1.Events = ((Gdk.EventMask)(256));
            this.animationtimelinewidget1.Name = "animationtimelinewidget1";
            this.vbox2.Add(this.animationtimelinewidget1);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.vbox2[this.animationtimelinewidget1]));
            w10.PackType = ((Gtk.PackType)(1));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
            this.hbox2.Add(this.vbox2);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox2]));
            w11.Position = 1;
            this.notebook.Add(this.hbox2);
            // Notebook tab
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Design");
            this.notebook.SetTabLabel(this.hbox2, this.label2);
            this.label2.ShowAll();
            // Container child notebook.Gtk.Notebook+NotebookChild
            this.scrolledwindow2 = new Gtk.ScrolledWindow();
            this.scrolledwindow2.CanFocus = true;
            this.scrolledwindow2.Name = "scrolledwindow2";
            // Container child scrolledwindow2.Gtk.Container+ContainerChild
            Gtk.Viewport w13 = new Gtk.Viewport();
            w13.ShadowType = ((Gtk.ShadowType)(0));
            // Container child GtkViewport1.Gtk.Container+ContainerChild
            this.xaml_textview = new Gtk.TextView();
            this.xaml_textview.CanFocus = true;
            this.xaml_textview.Name = "xaml_textview";
            w13.Add(this.xaml_textview);
            this.scrolledwindow2.Add(w13);
            this.notebook.Add(this.scrolledwindow2);
            Gtk.Notebook.NotebookChild w16 = ((Gtk.Notebook.NotebookChild)(this.notebook[this.scrolledwindow2]));
            w16.Position = 1;
            // Notebook tab
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Xaml Code");
            this.notebook.SetTabLabel(this.scrolledwindow2, this.label1);
            this.label1.ShowAll();
            this.vbox1.Add(this.notebook);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.vbox1[this.notebook]));
            w17.Position = 2;
            // Container child vbox1.Gtk.Box+BoxChild
            this.statusbar1 = new Gtk.Statusbar();
            this.statusbar1.Name = "statusbar1";
            this.statusbar1.Spacing = 6;
            this.vbox1.Add(this.statusbar1);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.vbox1[this.statusbar1]));
            w18.Position = 3;
            w18.Expand = false;
            w18.Fill = false;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 630;
            this.DefaultHeight = 528;
            this.Show();
            this.DeleteEvent += new Gtk.DeleteEventHandler(this.OnDeleteEvent);
            this.SelectionToolAction.Activated += new System.EventHandler(this.OnSelectionToolActionActivated);
            this.RectangleToolAction.Activated += new System.EventHandler(this.OnRectangleToolActionActivated);
            this.SquareToolAction.Activated += new System.EventHandler(this.OnSquareToolActionActivated);
            this.EllipseToolAction.Activated += new System.EventHandler(this.OnEllipseToolActionActivated);
            this.CircleToolAction.Activated += new System.EventHandler(this.OnCircleToolActionActivated);
            this.PathToolAction.Activated += new System.EventHandler(this.OnPathToolActionActivated);
            this.LineToolAction.Activated += new System.EventHandler(this.OnLineToolActionActivated);
            this.LimpiarAction.Activated += new System.EventHandler(this.OnLimpiarActionActivated);
            this.notebook.SwitchPage += new Gtk.SwitchPageHandler(this.OnNotebookSwitchPage);
        }
    }
}
