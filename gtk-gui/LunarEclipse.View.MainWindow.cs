// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace LunarEclipse.View {
    
    
    public partial class MainWindow {
        
        private Gtk.UIManager UIManager;
        
        private Gtk.Action FileAction;
        
        private Gtk.Action OpenAction;
        
        private Gtk.Action NewAction;
        
        private Gtk.Action SaveAction;
        
        private Gtk.Action SaveAsAction;
        
        private Gtk.Action QuitAction;
        
        private Gtk.Action EditAction;
        
        private Gtk.Action ToolsAction;
        
        private Gtk.Action HelpAction;
        
        private Gtk.Action UndoAction;
        
        private Gtk.Action RedoAction;
        
        private Gtk.Action CutAction;
        
        private Gtk.Action CopiarAction;
        
        private Gtk.Action PasteAction;
        
        private Gtk.Action DeleteAction;
        
        private Gtk.Action AboutAction;
        
        private Gtk.RadioAction SelectionToolAction;
        
        private Gtk.RadioAction RectangleToolAction;
        
        private Gtk.RadioAction SquareToolAction;
        
        private Gtk.RadioAction EllipseToolAction;
        
        private Gtk.RadioAction CircleToolAction;
        
        private Gtk.RadioAction PathToolAction;
        
        private Gtk.RadioAction TextToolAction;
        
        private Gtk.RadioAction ImageToolAction;
        
        private Gtk.Action AnimationAction;
        
        private Gtk.Action RecordAction;
        
        private Gtk.Action ReproducirAction;
        
        private Gtk.Action StopAction;
        
        private Gtk.RadioAction LineToolAction;
        
        private Gtk.Action DrawingAction;
        
        private Gtk.Action CleanAction;
        
        private Gtk.RadioAction PolylineToolAction;
        
        private Gtk.RadioAction PenToolAction;
        
        private Gtk.Action debug1;
        
        private Gtk.Action FiguresAction;
        
        private Gtk.Action OrderAction;
        
        private Gtk.Action AlignAction;
        
        private Gtk.Action BringToFrontAction;
        
        private Gtk.Action SendToBackAction;
        
        private Gtk.Action BringForwardsAction;
        
        private Gtk.Action SendBackwarsAction;
        
        private Gtk.Action LeftAction;
        
        private Gtk.Action HorizontalCenterAction;
        
        private Gtk.Action RightAction;
        
        private Gtk.Action TopAction;
        
        private Gtk.Action VerticalCenterAction;
        
        private Gtk.Action BottomAction;
        
        private Gtk.Action debug2;
        
        private Gtk.Action CloneAction;
        
        private Gtk.Action SelectAllAction;
        
        private Gtk.Action ClearSelectionAction;
        
        private Gtk.VBox vbox1;
        
        private Gtk.MenuBar menubar1;
        
        private Gtk.Toolbar toolbar1;
        
        private Gtk.Notebook notebook;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Toolbar toolbar2;
        
        private Gtk.VBox vbox2;
        
        private Gtk.HPaned hpaned1;
        
        private Gtk.VBox vbox3;
        
        private Gtk.ScrolledWindow scrolledwindow1;
        
        private LunarEclipse.View.MoonlightWidget moonlightwidget;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Label label3;
        
        private Gtk.HScale zoomScale;
        
        private LunarEclipse.View.PropertyPanel propertypanel;
        
        private Gtk.Label label2;
        
        private Gtk.ScrolledWindow scrolledwindow2;
        
        private Gtk.TextView xaml_textview;
        
        private Gtk.Label label1;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget LunarEclipse.View.MainWindow
            this.UIManager = new Gtk.UIManager();
            Gtk.ActionGroup w1 = new Gtk.ActionGroup("Default");
            this.FileAction = new Gtk.Action("FileAction", Mono.Unix.Catalog.GetString("_File"), null, null);
            this.FileAction.ShortLabel = Mono.Unix.Catalog.GetString("_File");
            w1.Add(this.FileAction, null);
            this.OpenAction = new Gtk.Action("OpenAction", Mono.Unix.Catalog.GetString("_Abrir"), null, "gtk-open");
            this.OpenAction.ShortLabel = Mono.Unix.Catalog.GetString("_Abrir");
            w1.Add(this.OpenAction, null);
            this.NewAction = new Gtk.Action("NewAction", Mono.Unix.Catalog.GetString("_Nuevo"), null, "gtk-new");
            this.NewAction.ShortLabel = Mono.Unix.Catalog.GetString("_Nuevo");
            w1.Add(this.NewAction, null);
            this.SaveAction = new Gtk.Action("SaveAction", Mono.Unix.Catalog.GetString("_Guardar"), null, "gtk-save");
            this.SaveAction.ShortLabel = Mono.Unix.Catalog.GetString("_Guardar");
            w1.Add(this.SaveAction, null);
            this.SaveAsAction = new Gtk.Action("SaveAsAction", Mono.Unix.Catalog.GetString("Guardar _como"), null, "gtk-save-as");
            this.SaveAsAction.ShortLabel = Mono.Unix.Catalog.GetString("Guardar _como");
            w1.Add(this.SaveAsAction, null);
            this.QuitAction = new Gtk.Action("QuitAction", Mono.Unix.Catalog.GetString("_Salir"), null, "gtk-quit");
            this.QuitAction.ShortLabel = Mono.Unix.Catalog.GetString("_Salir");
            w1.Add(this.QuitAction, null);
            this.EditAction = new Gtk.Action("EditAction", Mono.Unix.Catalog.GetString("_Edit"), null, null);
            this.EditAction.ShortLabel = Mono.Unix.Catalog.GetString("_Edit");
            w1.Add(this.EditAction, null);
            this.ToolsAction = new Gtk.Action("ToolsAction", Mono.Unix.Catalog.GetString("_Tools"), null, null);
            this.ToolsAction.ShortLabel = Mono.Unix.Catalog.GetString("_Tools");
            w1.Add(this.ToolsAction, null);
            this.HelpAction = new Gtk.Action("HelpAction", Mono.Unix.Catalog.GetString("_Help"), null, null);
            this.HelpAction.ShortLabel = Mono.Unix.Catalog.GetString("_Help");
            w1.Add(this.HelpAction, null);
            this.UndoAction = new Gtk.Action("UndoAction", Mono.Unix.Catalog.GetString("_Deshacer"), null, "gtk-undo");
            this.UndoAction.Sensitive = false;
            this.UndoAction.ShortLabel = Mono.Unix.Catalog.GetString("_Deshacer");
            w1.Add(this.UndoAction, "<Control>z");
            this.RedoAction = new Gtk.Action("RedoAction", Mono.Unix.Catalog.GetString("_Rehacer"), null, "gtk-redo");
            this.RedoAction.Sensitive = false;
            this.RedoAction.ShortLabel = Mono.Unix.Catalog.GetString("_Rehacer");
            w1.Add(this.RedoAction, "<Control>y");
            this.CutAction = new Gtk.Action("CutAction", Mono.Unix.Catalog.GetString("Cor_tar"), null, "gtk-cut");
            this.CutAction.ShortLabel = Mono.Unix.Catalog.GetString("Cor_tar");
            w1.Add(this.CutAction, null);
            this.CopiarAction = new Gtk.Action("CopiarAction", Mono.Unix.Catalog.GetString("_Copiar"), null, "gtk-copy");
            this.CopiarAction.ShortLabel = Mono.Unix.Catalog.GetString("_Copiar");
            w1.Add(this.CopiarAction, null);
            this.PasteAction = new Gtk.Action("PasteAction", Mono.Unix.Catalog.GetString("_Pegar"), null, "gtk-paste");
            this.PasteAction.ShortLabel = Mono.Unix.Catalog.GetString("_Pegar");
            w1.Add(this.PasteAction, null);
            this.DeleteAction = new Gtk.Action("DeleteAction", Mono.Unix.Catalog.GetString("_Borrar"), null, "gtk-delete");
            this.DeleteAction.ShortLabel = Mono.Unix.Catalog.GetString("_Borrar");
            w1.Add(this.DeleteAction, "<Mod2>Delete");
            this.AboutAction = new Gtk.Action("AboutAction", Mono.Unix.Catalog.GetString("Acerca _de"), null, "gtk-about");
            this.AboutAction.ShortLabel = Mono.Unix.Catalog.GetString("Acerca _de");
            w1.Add(this.AboutAction, null);
            this.SelectionToolAction = new Gtk.RadioAction("SelectionToolAction", Mono.Unix.Catalog.GetString("_Selection Tool"), null, "selection-tool", 0);
            this.SelectionToolAction.Group = new GLib.SList(System.IntPtr.Zero);
            this.SelectionToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Selection Tool");
            w1.Add(this.SelectionToolAction, null);
            this.RectangleToolAction = new Gtk.RadioAction("RectangleToolAction", Mono.Unix.Catalog.GetString("_Rectangle Tool"), null, "rectangle-tool", 0);
            this.RectangleToolAction.Group = this.SelectionToolAction.Group;
            this.RectangleToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Rectangle Tool");
            w1.Add(this.RectangleToolAction, null);
            this.SquareToolAction = new Gtk.RadioAction("SquareToolAction", Mono.Unix.Catalog.GetString("S_quare Tool"), null, "square-tool", 0);
            this.SquareToolAction.Group = this.RectangleToolAction.Group;
            this.SquareToolAction.ShortLabel = Mono.Unix.Catalog.GetString("S_quare Tool");
            w1.Add(this.SquareToolAction, null);
            this.EllipseToolAction = new Gtk.RadioAction("EllipseToolAction", Mono.Unix.Catalog.GetString("_Ellipse Tool"), null, "ellipse-tool", 0);
            this.EllipseToolAction.Group = this.RectangleToolAction.Group;
            this.EllipseToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Ellipse Tool");
            w1.Add(this.EllipseToolAction, null);
            this.CircleToolAction = new Gtk.RadioAction("CircleToolAction", Mono.Unix.Catalog.GetString("_Circle Tool"), null, "circle-tool", 0);
            this.CircleToolAction.Group = this.EllipseToolAction.Group;
            this.CircleToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Circle Tool");
            w1.Add(this.CircleToolAction, null);
            this.PathToolAction = new Gtk.RadioAction("PathToolAction", Mono.Unix.Catalog.GetString("_Path Tool"), null, "path-tool", 0);
            this.PathToolAction.Group = this.CircleToolAction.Group;
            this.PathToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Path Tool");
            w1.Add(this.PathToolAction, null);
            this.TextToolAction = new Gtk.RadioAction("TextToolAction", Mono.Unix.Catalog.GetString("_Text Tool"), null, "text-tool", 0);
            this.TextToolAction.Group = this.CircleToolAction.Group;
            this.TextToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Text Tool");
            w1.Add(this.TextToolAction, null);
            this.ImageToolAction = new Gtk.RadioAction("ImageToolAction", Mono.Unix.Catalog.GetString("Image Tool"), null, "image-tool", 0);
            this.ImageToolAction.Group = this.CircleToolAction.Group;
            this.ImageToolAction.ShortLabel = Mono.Unix.Catalog.GetString("Image Tool");
            w1.Add(this.ImageToolAction, null);
            this.AnimationAction = new Gtk.Action("AnimationAction", Mono.Unix.Catalog.GetString("_Animation"), null, null);
            this.AnimationAction.ShortLabel = Mono.Unix.Catalog.GetString("_Animation");
            w1.Add(this.AnimationAction, null);
            this.RecordAction = new Gtk.Action("RecordAction", Mono.Unix.Catalog.GetString("_Grabar"), null, "gtk-media-record");
            this.RecordAction.ShortLabel = Mono.Unix.Catalog.GetString("_Grabar");
            w1.Add(this.RecordAction, null);
            this.ReproducirAction = new Gtk.Action("ReproducirAction", Mono.Unix.Catalog.GetString("_Reproducir"), null, "gtk-media-play");
            this.ReproducirAction.ShortLabel = Mono.Unix.Catalog.GetString("_Reproducir");
            w1.Add(this.ReproducirAction, null);
            this.StopAction = new Gtk.Action("StopAction", Mono.Unix.Catalog.GetString("_Detener"), null, "gtk-media-stop");
            this.StopAction.ShortLabel = Mono.Unix.Catalog.GetString("_Detener");
            w1.Add(this.StopAction, null);
            this.LineToolAction = new Gtk.RadioAction("LineToolAction", Mono.Unix.Catalog.GetString("_Line Tool"), null, "line-tool", 0);
            this.LineToolAction.Group = this.CircleToolAction.Group;
            this.LineToolAction.ShortLabel = Mono.Unix.Catalog.GetString("_Line Tool");
            w1.Add(this.LineToolAction, null);
            this.DrawingAction = new Gtk.Action("DrawingAction", Mono.Unix.Catalog.GetString("Drawing"), null, null);
            this.DrawingAction.ShortLabel = Mono.Unix.Catalog.GetString("Drawing");
            w1.Add(this.DrawingAction, null);
            this.CleanAction = new Gtk.Action("CleanAction", Mono.Unix.Catalog.GetString("_Limpiar"), null, "gtk-clear");
            this.CleanAction.ShortLabel = Mono.Unix.Catalog.GetString("_Limpiar");
            w1.Add(this.CleanAction, null);
            this.PolylineToolAction = new Gtk.RadioAction("PolylineToolAction", Mono.Unix.Catalog.GetString("Polyline Tool"), null, "polyline-tool", 0);
            this.PolylineToolAction.Group = this.CircleToolAction.Group;
            this.PolylineToolAction.ShortLabel = Mono.Unix.Catalog.GetString("Polyline Tool");
            w1.Add(this.PolylineToolAction, null);
            this.PenToolAction = new Gtk.RadioAction("PenToolAction", Mono.Unix.Catalog.GetString("P_en Tool"), null, "pen-tool", 0);
            this.PenToolAction.Group = this.CircleToolAction.Group;
            this.PenToolAction.ShortLabel = Mono.Unix.Catalog.GetString("P_en Tool");
            w1.Add(this.PenToolAction, null);
            this.debug1 = new Gtk.Action("debug1", null, Mono.Unix.Catalog.GetString("Debug"), "gtk-dialog-warning");
            w1.Add(this.debug1, null);
            this.FiguresAction = new Gtk.Action("FiguresAction", Mono.Unix.Catalog.GetString("Fi_gures"), null, null);
            this.FiguresAction.ShortLabel = Mono.Unix.Catalog.GetString("Fi_gure");
            w1.Add(this.FiguresAction, null);
            this.OrderAction = new Gtk.Action("OrderAction", Mono.Unix.Catalog.GetString("Order"), null, null);
            this.OrderAction.ShortLabel = Mono.Unix.Catalog.GetString("Order");
            w1.Add(this.OrderAction, null);
            this.AlignAction = new Gtk.Action("AlignAction", Mono.Unix.Catalog.GetString("_Align"), null, null);
            this.AlignAction.ShortLabel = Mono.Unix.Catalog.GetString("_Align");
            w1.Add(this.AlignAction, null);
            this.BringToFrontAction = new Gtk.Action("BringToFrontAction", Mono.Unix.Catalog.GetString("Bring to _Front"), null, "gtk-goto-top");
            this.BringToFrontAction.Sensitive = false;
            this.BringToFrontAction.ShortLabel = Mono.Unix.Catalog.GetString("Bring to _Front");
            w1.Add(this.BringToFrontAction, null);
            this.SendToBackAction = new Gtk.Action("SendToBackAction", Mono.Unix.Catalog.GetString("Send to _Back"), null, "gtk-goto-bottom");
            this.SendToBackAction.Sensitive = false;
            this.SendToBackAction.ShortLabel = Mono.Unix.Catalog.GetString("Send to _Back");
            w1.Add(this.SendToBackAction, null);
            this.BringForwardsAction = new Gtk.Action("BringForwardsAction", Mono.Unix.Catalog.GetString("Bring Forwards"), null, "gtk-go-up");
            this.BringForwardsAction.Sensitive = false;
            this.BringForwardsAction.ShortLabel = Mono.Unix.Catalog.GetString("Send Forwards");
            w1.Add(this.BringForwardsAction, null);
            this.SendBackwarsAction = new Gtk.Action("SendBackwarsAction", Mono.Unix.Catalog.GetString("Send Backwars"), null, "gtk-go-down");
            this.SendBackwarsAction.Sensitive = false;
            this.SendBackwarsAction.ShortLabel = Mono.Unix.Catalog.GetString("Bring Backwars");
            w1.Add(this.SendBackwarsAction, null);
            this.LeftAction = new Gtk.Action("LeftAction", Mono.Unix.Catalog.GetString("Left"), null, null);
            this.LeftAction.ShortLabel = Mono.Unix.Catalog.GetString("Left");
            w1.Add(this.LeftAction, null);
            this.HorizontalCenterAction = new Gtk.Action("HorizontalCenterAction", Mono.Unix.Catalog.GetString("Horizontal Center"), null, null);
            this.HorizontalCenterAction.ShortLabel = Mono.Unix.Catalog.GetString("Center");
            w1.Add(this.HorizontalCenterAction, null);
            this.RightAction = new Gtk.Action("RightAction", Mono.Unix.Catalog.GetString("Right"), null, null);
            this.RightAction.ShortLabel = Mono.Unix.Catalog.GetString("Right");
            w1.Add(this.RightAction, null);
            this.TopAction = new Gtk.Action("TopAction", Mono.Unix.Catalog.GetString("Top"), null, null);
            this.TopAction.ShortLabel = Mono.Unix.Catalog.GetString("Top");
            w1.Add(this.TopAction, null);
            this.VerticalCenterAction = new Gtk.Action("VerticalCenterAction", Mono.Unix.Catalog.GetString("Vertical Center"), null, null);
            this.VerticalCenterAction.ShortLabel = Mono.Unix.Catalog.GetString("Middle");
            w1.Add(this.VerticalCenterAction, null);
            this.BottomAction = new Gtk.Action("BottomAction", Mono.Unix.Catalog.GetString("Bottom"), null, null);
            this.BottomAction.ShortLabel = Mono.Unix.Catalog.GetString("Bottom");
            w1.Add(this.BottomAction, null);
            this.debug2 = new Gtk.Action("debug2", null, Mono.Unix.Catalog.GetString("Debug2"), "gtk-dialog-error");
            w1.Add(this.debug2, null);
            this.CloneAction = new Gtk.Action("CloneAction", Mono.Unix.Catalog.GetString("Cl_one"), null, null);
            this.CloneAction.ShortLabel = Mono.Unix.Catalog.GetString("Clone");
            w1.Add(this.CloneAction, null);
            this.SelectAllAction = new Gtk.Action("SelectAllAction", Mono.Unix.Catalog.GetString("_Select All"), null, null);
            this.SelectAllAction.ShortLabel = Mono.Unix.Catalog.GetString("Select All");
            w1.Add(this.SelectAllAction, "<Control><Mod2>a");
            this.ClearSelectionAction = new Gtk.Action("ClearSelectionAction", Mono.Unix.Catalog.GetString("Clear S_election"), null, null);
            this.ClearSelectionAction.ShortLabel = Mono.Unix.Catalog.GetString("Clear S_election");
            w1.Add(this.ClearSelectionAction, null);
            this.UIManager.InsertActionGroup(w1, 0);
            this.AddAccelGroup(this.UIManager.AccelGroup);
            this.Name = "LunarEclipse.View.MainWindow";
            this.Title = Mono.Unix.Catalog.GetString("Lunar Eclipse");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.DefaultWidth = 800;
            // Container child LunarEclipse.View.MainWindow.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.UIManager.AddUiFromString("<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'><menuitem name='NewAction' action='NewAction'/><menuitem name='OpenAction' action='OpenAction'/><separator/><menuitem name='SaveAction' action='SaveAction'/><menuitem name='SaveAsAction' action='SaveAsAction'/><separator/><menuitem name='QuitAction' action='QuitAction'/></menu><menu name='EditAction' action='EditAction'><menuitem name='UndoAction' action='UndoAction'/><menuitem name='RedoAction' action='RedoAction'/><separator/><menuitem name='CutAction' action='CutAction'/><menuitem name='CopiarAction' action='CopiarAction'/><menuitem name='PasteAction' action='PasteAction'/><menuitem name='CloneAction' action='CloneAction'/><menuitem name='DeleteAction' action='DeleteAction'/><separator/><menuitem name='SelectAllAction' action='SelectAllAction'/><menuitem name='ClearSelectionAction' action='ClearSelectionAction'/></menu><menu name='DrawingAction' action='DrawingAction'><menuitem name='CleanAction' action='CleanAction'/></menu><menu name='FiguresAction' action='FiguresAction'><menu name='OrderAction' action='OrderAction'><menuitem name='BringToFrontAction' action='BringToFrontAction'/><menuitem name='BringForwardsAction' action='BringForwardsAction'/><menuitem name='SendToBackAction' action='SendToBackAction'/><menuitem name='SendBackwarsAction' action='SendBackwarsAction'/></menu><menu name='AlignAction' action='AlignAction'><menuitem name='LeftAction' action='LeftAction'/><menuitem name='HorizontalCenterAction' action='HorizontalCenterAction'/><menuitem name='RightAction' action='RightAction'/><separator/><menuitem name='TopAction' action='TopAction'/><menuitem name='VerticalCenterAction' action='VerticalCenterAction'/><menuitem name='BottomAction' action='BottomAction'/></menu></menu><menu name='ToolsAction' action='ToolsAction'><menuitem name='SelectionToolAction' action='SelectionToolAction'/><menuitem name='RectangleToolAction' action='RectangleToolAction'/><menuitem name='SquareToolAction' action='SquareToolAction'/><menuitem name='EllipseToolAction' action='EllipseToolAction'/><menuitem name='CircleToolAction' action='CircleToolAction'/><menuitem name='LineToolAction' action='LineToolAction'/><menuitem name='PolylineToolAction' action='PolylineToolAction'/><menuitem name='PenToolAction' action='PenToolAction'/><menuitem name='PathToolAction' action='PathToolAction'/><menuitem name='TextToolAction' action='TextToolAction'/><menuitem name='ImageToolAction' action='ImageToolAction'/></menu><menu name='AnimationAction' action='AnimationAction'><menuitem name='StopAction' action='StopAction'/><menuitem name='ReproducirAction' action='ReproducirAction'/><menuitem name='RecordAction' action='RecordAction'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='AboutAction' action='AboutAction'/></menu></menubar></ui>");
            this.menubar1 = ((Gtk.MenuBar)(this.UIManager.GetWidget("/menubar1")));
            this.menubar1.Name = "menubar1";
            this.vbox1.Add(this.menubar1);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox1[this.menubar1]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.UIManager.AddUiFromString("<ui><toolbar name='toolbar1'><toolitem name='NewAction' action='NewAction'/><toolitem name='OpenAction' action='OpenAction'/><toolitem name='SaveAction' action='SaveAction'/><separator/><toolitem name='UndoAction' action='UndoAction'/><toolitem name='RedoAction' action='RedoAction'/><separator/><toolitem name='CutAction' action='CutAction'/><toolitem name='CopiarAction' action='CopiarAction'/><toolitem name='PasteAction' action='PasteAction'/></toolbar></ui>");
            this.toolbar1 = ((Gtk.Toolbar)(this.UIManager.GetWidget("/toolbar1")));
            this.toolbar1.Name = "toolbar1";
            this.toolbar1.ShowArrow = false;
            this.toolbar1.ToolbarStyle = ((Gtk.ToolbarStyle)(0));
            this.vbox1.Add(this.toolbar1);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox1[this.toolbar1]));
            w3.Position = 1;
            w3.Expand = false;
            w3.Fill = false;
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
            this.UIManager.AddUiFromString("<ui><toolbar name='toolbar2'><toolitem name='SelectionToolAction' action='SelectionToolAction'/><toolitem name='SquareToolAction' action='SquareToolAction'/><toolitem name='RectangleToolAction' action='RectangleToolAction'/><toolitem name='CircleToolAction' action='CircleToolAction'/><toolitem name='EllipseToolAction' action='EllipseToolAction'/><toolitem name='LineToolAction' action='LineToolAction'/><toolitem name='PolylineToolAction' action='PolylineToolAction'/><toolitem name='PathToolAction' action='PathToolAction'/><toolitem name='PenToolAction' action='PenToolAction'/><toolitem name='TextToolAction' action='TextToolAction'/><separator/><toolitem name='CleanAction' action='CleanAction'/><toolitem name='debug1' action='debug1'/><toolitem name='debug2' action='debug2'/></toolbar></ui>");
            this.toolbar2 = ((Gtk.Toolbar)(this.UIManager.GetWidget("/toolbar2")));
            this.toolbar2.Name = "toolbar2";
            this.toolbar2.Orientation = ((Gtk.Orientation)(1));
            this.toolbar2.ShowArrow = false;
            this.toolbar2.ToolbarStyle = ((Gtk.ToolbarStyle)(0));
            this.hbox2.Add(this.toolbar2);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox2[this.toolbar2]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hpaned1 = new Gtk.HPaned();
            this.hpaned1.CanFocus = true;
            this.hpaned1.Name = "hpaned1";
            this.hpaned1.Position = 842;
            // Container child hpaned1.Gtk.Paned+PanedChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.VscrollbarPolicy = ((Gtk.PolicyType)(0));
            this.scrolledwindow1.HscrollbarPolicy = ((Gtk.PolicyType)(0));
            // Container child scrolledwindow1.Gtk.Container+ContainerChild
            Gtk.Viewport w5 = new Gtk.Viewport();
            w5.ShadowType = ((Gtk.ShadowType)(0));
            // Container child GtkViewport.Gtk.Container+ContainerChild
            this.moonlightwidget = new LunarEclipse.View.MoonlightWidget();
            this.moonlightwidget.Events = ((Gdk.EventMask)(256));
            this.moonlightwidget.Name = "moonlightwidget";
            this.moonlightwidget.Height = 800;
            this.moonlightwidget.Width = 800;
            w5.Add(this.moonlightwidget);
            this.scrolledwindow1.Add(w5);
            this.vbox3.Add(this.scrolledwindow1);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.vbox3[this.scrolledwindow1]));
            w8.Position = 0;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Zoom:");
            this.hbox3.Add(this.label3);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.hbox3[this.label3]));
            w9.Position = 0;
            w9.Expand = false;
            w9.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.zoomScale = new Gtk.HScale(null);
            this.zoomScale.CanFocus = true;
            this.zoomScale.Name = "zoomScale";
            this.zoomScale.Adjustment.Upper = 500;
            this.zoomScale.Adjustment.PageIncrement = 10;
            this.zoomScale.Adjustment.StepIncrement = 1;
            this.zoomScale.Adjustment.Value = 100;
            this.zoomScale.DrawValue = true;
            this.zoomScale.Digits = 0;
            this.zoomScale.ValuePos = ((Gtk.PositionType)(2));
            this.hbox3.Add(this.zoomScale);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.hbox3[this.zoomScale]));
            w10.Position = 1;
            this.vbox3.Add(this.hbox3);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox3]));
            w11.Position = 1;
            w11.Expand = false;
            w11.Fill = false;
            this.hpaned1.Add(this.vbox3);
            Gtk.Paned.PanedChild w12 = ((Gtk.Paned.PanedChild)(this.hpaned1[this.vbox3]));
            w12.Resize = false;
            // Container child hpaned1.Gtk.Paned+PanedChild
            this.propertypanel = new LunarEclipse.View.PropertyPanel();
            this.propertypanel.Events = ((Gdk.EventMask)(256));
            this.propertypanel.Name = "propertypanel";
            this.hpaned1.Add(this.propertypanel);
            Gtk.Paned.PanedChild w13 = ((Gtk.Paned.PanedChild)(this.hpaned1[this.propertypanel]));
            w13.Resize = false;
            this.vbox2.Add(this.hpaned1);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.vbox2[this.hpaned1]));
            w14.Position = 0;
            this.hbox2.Add(this.vbox2);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox2]));
            w15.Position = 1;
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
            Gtk.Viewport w17 = new Gtk.Viewport();
            w17.ShadowType = ((Gtk.ShadowType)(0));
            // Container child GtkViewport2.Gtk.Container+ContainerChild
            this.xaml_textview = new Gtk.TextView();
            this.xaml_textview.CanFocus = true;
            this.xaml_textview.Name = "xaml_textview";
            w17.Add(this.xaml_textview);
            this.scrolledwindow2.Add(w17);
            this.notebook.Add(this.scrolledwindow2);
            Gtk.Notebook.NotebookChild w20 = ((Gtk.Notebook.NotebookChild)(this.notebook[this.scrolledwindow2]));
            w20.Position = 1;
            // Notebook tab
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Xaml Code");
            this.notebook.SetTabLabel(this.scrolledwindow2, this.label1);
            this.label1.ShowAll();
            this.vbox1.Add(this.notebook);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.vbox1[this.notebook]));
            w21.Position = 2;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultHeight = 729;
            this.Show();
            this.DeleteEvent += new Gtk.DeleteEventHandler(this.OnDeleteEvent);
            this.OpenAction.Activated += new System.EventHandler(this.OnOpenActionActivated);
            this.SaveAction.Activated += new System.EventHandler(this.OnSaveActionActivated);
            this.UndoAction.Activated += new System.EventHandler(this.OnUndoActionActivated);
            this.RedoAction.Activated += new System.EventHandler(this.OnRedoActionActivated);
            this.CutAction.Activated += new System.EventHandler(this.OnCutActionActivated);
            this.CopiarAction.Activated += new System.EventHandler(this.OnCopyActionActivated);
            this.PasteAction.Activated += new System.EventHandler(this.OnPasteActionActivated);
            this.DeleteAction.Activated += new System.EventHandler(this.OnDeleteActionActivated);
            this.AboutAction.Activated += new System.EventHandler(this.OnAboutActionActivated);
            this.SelectionToolAction.Activated += new System.EventHandler(this.OnSelectionToolActionActivated);
            this.RectangleToolAction.Activated += new System.EventHandler(this.OnRectangleToolActionActivated);
            this.SquareToolAction.Activated += new System.EventHandler(this.OnSquareToolActionActivated);
            this.EllipseToolAction.Activated += new System.EventHandler(this.OnEllipseToolActionActivated);
            this.CircleToolAction.Activated += new System.EventHandler(this.OnCircleToolActionActivated);
            this.PathToolAction.Activated += new System.EventHandler(this.OnPathToolActionActivated);
            this.TextToolAction.Activated += new System.EventHandler(this.OnTextToolActionActivated);
            this.ImageToolAction.Activated += new System.EventHandler(this.OnImageToolActionActivated);
            this.LineToolAction.Activated += new System.EventHandler(this.OnLineToolActionActivated);
            this.CleanAction.Activated += new System.EventHandler(this.OnLimpiarActionActivated);
            this.PolylineToolAction.Activated += new System.EventHandler(this.OnPolylineToolActionActivated);
            this.PenToolAction.Activated += new System.EventHandler(this.OnPenToolActionActivated);
            this.debug1.Activated += new System.EventHandler(this.OnDebug1Activated);
            this.BringToFrontAction.Activated += new System.EventHandler(this.OnBringToFrontActionActivated);
            this.SendToBackAction.Activated += new System.EventHandler(this.OnSendToBackActionActivated);
            this.BringForwardsAction.Activated += new System.EventHandler(this.OnBringForwardsActionActivated);
            this.SendBackwarsAction.Activated += new System.EventHandler(this.OnSendBackwarsActionActivated);
            this.LeftAction.Activated += new System.EventHandler(this.OnLeftActionActivated);
            this.HorizontalCenterAction.Activated += new System.EventHandler(this.OnHorizontalCenterActionActivated);
            this.RightAction.Activated += new System.EventHandler(this.OnRightActionActivated);
            this.TopAction.Activated += new System.EventHandler(this.OnTopActionActivated);
            this.VerticalCenterAction.Activated += new System.EventHandler(this.OnVerticalCenterActionActivated);
            this.BottomAction.Activated += new System.EventHandler(this.OnBottomActionActivated);
            this.debug2.Activated += new System.EventHandler(this.OnDebug2Activated);
            this.CloneAction.Activated += new System.EventHandler(this.OnCloneActionActivated);
            this.SelectAllAction.Activated += new System.EventHandler(this.OnSelectAllActionActivated);
            this.ClearSelectionAction.Activated += new System.EventHandler(this.OnClearSelectionActionActivated);
            this.notebook.SwitchPage += new Gtk.SwitchPageHandler(this.OnNotebookSwitchPage);
            this.zoomScale.ValueChanged += new System.EventHandler(this.OnZoomScaleValueChanged);
        }
    }
}
