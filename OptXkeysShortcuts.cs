using System;
using System.Collections;
using DevExpress.DXCore.Controls.XtraBars;
using DevExpress.DXCore.Controls.XtraTreeList;
using DevExpress.DXCore.Controls.XtraTreeList.Nodes;
using DevExpress.DXCore.Controls.XtraEditors;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Diagnostics.General;
using System.Drawing;
using System.Windows.Forms;
using WinForms = System.Windows.Forms;
using System.Text;
using DevExpress.DXCore.Controls.LookAndFeel;
using System.Collections.Generic;

namespace CR_XkeysEngine
{
  /// <summary>
  /// Summary description for OptXKeysShortcuts.cs.
  /// </summary>
  public class OptXkeysShortcuts : OptionsPage
  {
    // const...
    #region const...
    private const string STR_FolderAlreadyExistTryDifferentName = "The folder {0} already exists! Try a different name.";
    private const string STR_RootFileName = "Shortcuts.ini";

    private const int INT_ContextIgnored = 0;
    private const int INT_ContextSelected = 1;
    private const int INT_ContextMixed = 2;

    private const int IMG_FolderClose = 0;
    private const int IMG_FolderOpen = 1;
    private const int IMG_FolderCloseHover = 2;
    private const int IMG_FolderOpenHover = 3;

    private const int IMG_KeyLightLetterA = 4;
    private const int IMG_KeyboardIcon = IMG_KeyLightLetterA;		// Translate: (choose a different image if letter "A" key is not appropriate in some locales)
    #endregion

    // private fields...
    #region components...
    private LabelControl lblCommand;
    private DevExpress.DXCore.Controls.XtraEditors.TextEdit txtParameters;
    private LabelControl lblParameters;
    private System.ComponentModel.IContainer components;
    private TreeList trlsShortcuts;
    private LabelControl lblParametersAreOptional;
    private System.Windows.Forms.ToolTip toolTip1;
    private DevExpress.DXCore.Controls.XtraTreeList.Columns.TreeListColumn colShortcut;
    private DevExpress.DXCore.Controls.XtraTreeList.Columns.TreeListColumn colCommand;
    private DevExpress.DXCore.Controls.XtraTreeList.Columns.TreeListColumn colContext;
    private DevExpress.CodeRush.Core.BubbleHints.BubbleHintController BubbleHints;
    private DevExpress.CodeRush.UserControls.ContextPicker ContextPicker1;
    private LabelControl lblUseIn1;
    private DevExpress.DXCore.Controls.XtraEditors.TextEdit lblUseIn;
    private DevExpress.DXCore.Controls.XtraEditors.ComboBoxEdit cmbCommands;
    private PanelControl pnlBindings;
    private DevExpress.DXCore.PlugInCore.DXCoreEvents CodeRushEvents;
    private DevExpress.DXCore.Controls.XtraBars.BarManager barManager;
    private DevExpress.DXCore.Controls.XtraBars.BarDockControl barDockControlTop;
    private DevExpress.DXCore.Controls.XtraBars.BarDockControl barDockControlBottom;
    private DevExpress.DXCore.Controls.XtraBars.BarDockControl barDockControlLeft;
    private DevExpress.DXCore.Controls.XtraBars.BarDockControl barDockControlRight;
    private DevExpress.DXCore.Controls.XtraBars.Bar barTools;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnNewFolder;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnNewKeyboardShortcut;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnDelete;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnRenameFolder;
    private System.Windows.Forms.ImageList imgTreeListIcons;
    private DevExpress.DXCore.Controls.XtraEditors.CheckEdit chkBindingEnabled;
    private PanelControl pnlFolders;
    private DevExpress.DXCore.Controls.XtraEditors.CheckEdit chkFolderEnabled;
    private DevExpress.DXCore.Controls.XtraEditors.MemoEdit memoFolderComment;
    private LabelControl label2;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnToggleFolders;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnDuplicate;
    private DevExpress.DXCore.Controls.XtraBars.PopupMenu pmTreeList;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnCopyBindingSummaryfortechsupport;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnCopyBindingLinkfordocumentation;
    private PanelControl pnlControls;
    private PanelControl pnlTreeList;
    private SplitterControl split;
    #endregion
    #region private fields...
    private DisplayMode _DisplayMode = DisplayMode.Tree;
    private bool _UpdatingInternally;
    private TreeListNode _LastHighlightedFolder = null;
    //private CommandKeyBindingCollection _ShortcutCollection;
    private CommandKeyFolder _RootFolder;
    private CommandKeyBinding _CurrentBinding;
    private CommandKeyFolder _CurrentFolder;
    private DevExpress.CodeRush.PlugInCore.BigHint bigHint;
    private CR_XkeysEngine.BindingEditControl edBinding;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnFind;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem barButtonItem1;
    private DevExpress.DXCore.Controls.XtraBars.BarSubItem barSubItem1;
    private DevExpress.DXCore.Controls.XtraBars.BarSubItem barSubItem2;
    private DevExpress.DXCore.Controls.XtraBars.BarToolbarsListItem barToolbarsListItem1;
    private DevExpress.DXCore.Controls.XtraBars.BarLargeButtonItem barLargeButtonItem1;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnCopyContext;
    private DevExpress.DXCore.Controls.XtraBars.BarButtonItem btnPasteContext;
    private BarCheckItem chkbSeparateAltKeys;
    bool _Loading;
    static bool _SeparateAltKeys;
    private BarButtonItem btnCollapseFolders;
    private BarButtonItem barButtonItem2;
    private HyperLinkEdit linkLayout;
    private Label lblKeyName;
    private Label label1;
    static bool _SeparateAltKeysLoaded = false;
    #endregion

    // constructors...
    #region OptXKeysShortcuts
    public OptXkeysShortcuts(): base()
    {
      InitializeComponent();

      Hardware.Keyboard.Connect();

      InitializeVariables();
    }
    #endregion

    #region CodeRush required methods
    public static string GetCategory()
    {
      return @"X-keys";
    }

    public static string GetPageName()
    {
      return "Shortcuts";
    }

    public static DecoupledStorage Storage
    {
      get
      {
        return CodeRush.Options.GetStorage(GetCategory(), GetPageName());
      }
    }
    public override string Category
    {
      get
      {
        return OptXkeysShortcuts.GetCategory();
      }
    }
    public override string PageName
    {
      get
      {
        return OptXkeysShortcuts.GetPageName();
      }
    }
    #endregion
    #region Component Designer generated code
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo1 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo2 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo3 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo4 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo5 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo22 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo15 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo18 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo17 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo19 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo16 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo14 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo20 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo21 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo10 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo11 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo12 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo13 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo24 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo25 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo26 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo27 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo9 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo7 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo8 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptXkeysShortcuts));
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo23 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo bubbleHintInfo6 = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintInfo();
      this.CodeRushEvents = new DevExpress.DXCore.PlugInCore.DXCoreEvents(this.components);
      this.trlsShortcuts = new DevExpress.DXCore.Controls.XtraTreeList.TreeList();
      this.colShortcut = new DevExpress.DXCore.Controls.XtraTreeList.Columns.TreeListColumn();
      this.colCommand = new DevExpress.DXCore.Controls.XtraTreeList.Columns.TreeListColumn();
      this.colContext = new DevExpress.DXCore.Controls.XtraTreeList.Columns.TreeListColumn();
      this.barManager = new DevExpress.DXCore.Controls.XtraBars.BarManager(this.components);
      this.barTools = new DevExpress.DXCore.Controls.XtraBars.Bar();
      this.btnNewKeyboardShortcut = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.btnDuplicate = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.btnNewFolder = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.btnRenameFolder = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.btnDelete = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.btnToggleFolders = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.btnFind = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.chkbSeparateAltKeys = new DevExpress.DXCore.Controls.XtraBars.BarCheckItem();
      this.barDockControlTop = new DevExpress.DXCore.Controls.XtraBars.BarDockControl();
      this.barDockControlBottom = new DevExpress.DXCore.Controls.XtraBars.BarDockControl();
      this.barDockControlLeft = new DevExpress.DXCore.Controls.XtraBars.BarDockControl();
      this.barDockControlRight = new DevExpress.DXCore.Controls.XtraBars.BarDockControl();
      this.btnCopyBindingSummaryfortechsupport = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.btnCopyBindingLinkfordocumentation = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.barButtonItem1 = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.barSubItem1 = new DevExpress.DXCore.Controls.XtraBars.BarSubItem();
      this.barSubItem2 = new DevExpress.DXCore.Controls.XtraBars.BarSubItem();
      this.barToolbarsListItem1 = new DevExpress.DXCore.Controls.XtraBars.BarToolbarsListItem();
      this.barLargeButtonItem1 = new DevExpress.DXCore.Controls.XtraBars.BarLargeButtonItem();
      this.btnCopyContext = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.btnPasteContext = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.btnCollapseFolders = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.barButtonItem2 = new DevExpress.DXCore.Controls.XtraBars.BarButtonItem();
      this.pmTreeList = new DevExpress.DXCore.Controls.XtraBars.PopupMenu(this.components);
      this.lblCommand = new DevExpress.DXCore.Controls.XtraEditors.LabelControl();
      this.txtParameters = new DevExpress.DXCore.Controls.XtraEditors.TextEdit();
      this.lblParameters = new DevExpress.DXCore.Controls.XtraEditors.LabelControl();
      this.lblParametersAreOptional = new DevExpress.DXCore.Controls.XtraEditors.LabelControl();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.BubbleHints = new DevExpress.CodeRush.Core.BubbleHints.BubbleHintController(this.components);
      this.ContextPicker1 = new DevExpress.CodeRush.UserControls.ContextPicker();
      this.lblUseIn1 = new DevExpress.DXCore.Controls.XtraEditors.LabelControl();
      this.lblUseIn = new DevExpress.DXCore.Controls.XtraEditors.TextEdit();
      this.chkBindingEnabled = new DevExpress.DXCore.Controls.XtraEditors.CheckEdit();
      this.cmbCommands = new DevExpress.DXCore.Controls.XtraEditors.ComboBoxEdit();
      this.pnlBindings = new DevExpress.DXCore.Controls.XtraEditors.PanelControl();
      this.lblKeyName = new System.Windows.Forms.Label();
      this.linkLayout = new DevExpress.DXCore.Controls.XtraEditors.HyperLinkEdit();
      this.edBinding = new CR_XkeysEngine.BindingEditControl();
      this.pnlFolders = new DevExpress.DXCore.Controls.XtraEditors.PanelControl();
      this.memoFolderComment = new DevExpress.DXCore.Controls.XtraEditors.MemoEdit();
      this.label2 = new DevExpress.DXCore.Controls.XtraEditors.LabelControl();
      this.chkFolderEnabled = new DevExpress.DXCore.Controls.XtraEditors.CheckEdit();
      this.pnlControls = new DevExpress.DXCore.Controls.XtraEditors.PanelControl();
      this.pnlTreeList = new DevExpress.DXCore.Controls.XtraEditors.PanelControl();
      this.split = new DevExpress.DXCore.Controls.XtraEditors.SplitterControl();
      this.imgTreeListIcons = new System.Windows.Forms.ImageList(this.components);
      this.bigHint = new DevExpress.CodeRush.PlugInCore.BigHint(this.components);
      this.label1 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.CodeRushEvents)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trlsShortcuts)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Images16x16)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pmTreeList)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.txtParameters.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.BubbleHints)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lblUseIn.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.chkBindingEnabled.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cmbCommands.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pnlBindings)).BeginInit();
      this.pnlBindings.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.linkLayout.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pnlFolders)).BeginInit();
      this.pnlFolders.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.memoFolderComment.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.chkFolderEnabled.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).BeginInit();
      this.pnlControls.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pnlTreeList)).BeginInit();
      this.pnlTreeList.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bigHint)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      this.SuspendLayout();
      // 
      // CodeRushEvents
      // 
      this.CodeRushEvents.OptionsChanged += new DevExpress.CodeRush.Core.OptionsChangedEventHandler(this.CodeRushEvents_OptionsChanged);
      // 
      // trlsShortcuts
      // 
      bubbleHintInfo1.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.trlsShortcuts, bubbleHintInfo1);
      this.trlsShortcuts.Columns.AddRange(new DevExpress.DXCore.Controls.XtraTreeList.Columns.TreeListColumn[] {
            this.colShortcut,
            this.colCommand,
            this.colContext});
      this.trlsShortcuts.Dock = System.Windows.Forms.DockStyle.Fill;
      this.trlsShortcuts.Location = new System.Drawing.Point(4, 4);
      this.trlsShortcuts.MenuManager = this.barManager;
      this.trlsShortcuts.Name = "trlsShortcuts";
      this.trlsShortcuts.OptionsBehavior.DragNodes = true;
      this.trlsShortcuts.OptionsBehavior.Editable = false;
      this.trlsShortcuts.OptionsBehavior.KeepSelectedOnClick = false;
      this.trlsShortcuts.OptionsBehavior.ShowToolTips = false;
      this.trlsShortcuts.OptionsBehavior.SmartMouseHover = false;
      this.trlsShortcuts.OptionsSelection.EnableAppearanceFocusedCell = false;
      this.trlsShortcuts.OptionsView.ShowButtons = false;
      this.trlsShortcuts.OptionsView.ShowFocusedFrame = false;
      this.trlsShortcuts.OptionsView.ShowIndicator = false;
      this.trlsShortcuts.OptionsView.ShowRoot = false;
      this.trlsShortcuts.OptionsView.ShowVertLines = false;
      this.trlsShortcuts.Size = new System.Drawing.Size(244, 625);
      this.trlsShortcuts.TabIndex = 0;
      this.trlsShortcuts.AfterExpand += new DevExpress.DXCore.Controls.XtraTreeList.NodeEventHandler(this.trlsShortcuts_AfterExpand);
      this.trlsShortcuts.AfterCollapse += new DevExpress.DXCore.Controls.XtraTreeList.NodeEventHandler(this.trlsShortcuts_AfterCollapse);
      this.trlsShortcuts.AfterFocusNode += new DevExpress.DXCore.Controls.XtraTreeList.NodeEventHandler(this.trlsShortcuts_AfterFocusNode);
      this.trlsShortcuts.FocusedNodeChanged += new DevExpress.DXCore.Controls.XtraTreeList.FocusedNodeChangedEventHandler(this.trlsShortcuts_FocusedNodeChanged);
      this.trlsShortcuts.CompareNodeValues += new DevExpress.DXCore.Controls.XtraTreeList.CompareNodeValuesEventHandler(this.trlsShortcuts_CompareNodeValues);
      this.trlsShortcuts.CustomDrawNodeCell += new DevExpress.DXCore.Controls.XtraTreeList.CustomDrawNodeCellEventHandler(this.trlsShortcuts_CustomDrawNodeCell);
      this.trlsShortcuts.DragDrop += new System.Windows.Forms.DragEventHandler(this.trlsShortcuts_DragDrop);
      this.trlsShortcuts.DragOver += new System.Windows.Forms.DragEventHandler(this.trlsShortcuts_DragOver);
      this.trlsShortcuts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trlsShortcuts_KeyDown);
      this.trlsShortcuts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trlsShortcuts_MouseDown);
      this.trlsShortcuts.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trlsShortcuts_MouseMove);
      this.trlsShortcuts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trlsShortcuts_MouseUp);
      // 
      // colShortcut
      // 
      this.colShortcut.Caption = "Shortcut";
      this.colShortcut.FieldName = "Shortcut";
      this.colShortcut.Name = "colShortcut";
      this.colShortcut.Visible = true;
      this.colShortcut.VisibleIndex = 0;
      // 
      // colCommand
      // 
      this.colCommand.Caption = "Command";
      this.colCommand.FieldName = "Command";
      this.colCommand.Name = "colCommand";
      this.colCommand.Visible = true;
      this.colCommand.VisibleIndex = 1;
      // 
      // colContext
      // 
      this.colContext.Caption = "Context";
      this.colContext.FieldName = "Context";
      this.colContext.Name = "colContext";
      this.colContext.Visible = true;
      this.colContext.VisibleIndex = 2;
      // 
      // barManager
      // 
      this.barManager.AllowCustomization = false;
      this.barManager.Bars.AddRange(new DevExpress.DXCore.Controls.XtraBars.Bar[] {
            this.barTools});
      this.barManager.Categories.AddRange(new DevExpress.DXCore.Controls.XtraBars.BarManagerCategory[] {
            new DevExpress.DXCore.Controls.XtraBars.BarManagerCategory("pmTreeList", new System.Guid("2552e158-e356-4b32-9618-c71cc9932967"))});
      this.barManager.DockControls.Add(this.barDockControlTop);
      this.barManager.DockControls.Add(this.barDockControlBottom);
      this.barManager.DockControls.Add(this.barDockControlLeft);
      this.barManager.DockControls.Add(this.barDockControlRight);
      this.barManager.Form = this;
      this.barManager.Images = this.Images16x16;
      this.barManager.Items.AddRange(new DevExpress.DXCore.Controls.XtraBars.BarItem[] {
            this.btnNewFolder,
            this.btnNewKeyboardShortcut,
            this.btnDelete,
            this.btnToggleFolders,
            this.btnRenameFolder,
            this.btnDuplicate,
            this.btnCopyBindingSummaryfortechsupport,
            this.btnCopyBindingLinkfordocumentation,
            this.btnFind,
            this.barButtonItem1,
            this.barSubItem1,
            this.barSubItem2,
            this.barToolbarsListItem1,
            this.barLargeButtonItem1,
            this.btnCopyContext,
            this.btnPasteContext,
            this.chkbSeparateAltKeys,
            this.btnCollapseFolders,
            this.barButtonItem2});
      this.barManager.MaxItemId = 33;
      // 
      // barTools
      // 
      this.barTools.BarName = "ToolBar";
      this.barTools.DockCol = 0;
      this.barTools.DockRow = 0;
      this.barTools.DockStyle = DevExpress.DXCore.Controls.XtraBars.BarDockStyle.Top;
      this.barTools.LinksPersistInfo.AddRange(new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo[] {
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnNewKeyboardShortcut),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnDuplicate),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnNewFolder, true),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnRenameFolder),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnDelete, true),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnToggleFolders, true),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnFind),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.chkbSeparateAltKeys, true)});
      this.barTools.OptionsBar.AllowQuickCustomization = false;
      this.barTools.OptionsBar.DisableClose = true;
      this.barTools.OptionsBar.DisableCustomization = true;
      this.barTools.OptionsBar.DrawDragBorder = false;
      this.barTools.OptionsBar.RotateWhenVertical = false;
      this.barTools.Text = "ToolBar";
      // 
      // btnNewKeyboardShortcut
      // 
      this.btnNewKeyboardShortcut.Caption = "New Keyboard Shortcut";
      this.btnNewKeyboardShortcut.Hint = "New Keyboard Shortcut";
      this.btnNewKeyboardShortcut.Id = 1;
      this.btnNewKeyboardShortcut.ImageIndex = 75;
      this.btnNewKeyboardShortcut.Name = "btnNewKeyboardShortcut";
      this.btnNewKeyboardShortcut.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnNewKeyboardShortcut_ItemClick);
      // 
      // btnDuplicate
      // 
      this.btnDuplicate.Caption = "Duplicate Shortcut";
      this.btnDuplicate.Hint = "Duplicate Shortcut";
      this.btnDuplicate.Id = 6;
      this.btnDuplicate.ImageIndex = 328;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnDuplicate_ItemClick);
      // 
      // btnNewFolder
      // 
      this.btnNewFolder.Caption = "New Folder";
      this.btnNewFolder.Hint = "New Folder";
      this.btnNewFolder.Id = 0;
      this.btnNewFolder.ImageIndex = 393;
      this.btnNewFolder.Name = "btnNewFolder";
      this.btnNewFolder.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnNewFolder_ItemClick);
      // 
      // btnRenameFolder
      // 
      this.btnRenameFolder.Caption = "Rename Folder";
      this.btnRenameFolder.Hint = "Rename Folder";
      this.btnRenameFolder.Id = 5;
      this.btnRenameFolder.ImageIndex = 50;
      this.btnRenameFolder.Name = "btnRenameFolder";
      this.btnRenameFolder.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnRenameFolder_ItemClick);
      // 
      // btnDelete
      // 
      this.btnDelete.Caption = "Delete";
      this.btnDelete.Hint = "Delete";
      this.btnDelete.Id = 3;
      this.btnDelete.ImageIndex = 244;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
      // 
      // btnToggleFolders
      // 
      this.btnToggleFolders.ButtonStyle = DevExpress.DXCore.Controls.XtraBars.BarButtonStyle.Check;
      this.btnToggleFolders.Caption = "Hide Shortcut Folders";
      this.btnToggleFolders.Hint = "Hide shortcut folders";
      this.btnToggleFolders.Id = 4;
      this.btnToggleFolders.ImageIndex = 318;
      this.btnToggleFolders.Name = "btnToggleFolders";
      this.btnToggleFolders.DownChanged += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnToggleFolders_DownChanged);
      // 
      // btnFind
      // 
      this.btnFind.Caption = "Find";
      this.btnFind.Id = 13;
      this.btnFind.ImageIndex = 388;
      this.btnFind.Name = "btnFind";
      this.btnFind.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnFind_ItemClick);
      // 
      // chkbSeparateAltKeys
      // 
      this.chkbSeparateAltKeys.Caption = "Separate ALT keys";
      this.chkbSeparateAltKeys.Checked = true;
      this.chkbSeparateAltKeys.Hint = "Distinguish between left and right \"Alt\" keys (only for keyboard shortcuts)";
      this.chkbSeparateAltKeys.Id = 25;
      this.chkbSeparateAltKeys.ImageIndex = 319;
      this.chkbSeparateAltKeys.Name = "chkbSeparateAltKeys";
      // 
      // barDockControlTop
      // 
      bubbleHintInfo2.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.barDockControlTop, bubbleHintInfo2);
      this.barDockControlTop.CausesValidation = false;
      this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
      this.barDockControlTop.Size = new System.Drawing.Size(556, 31);
      // 
      // barDockControlBottom
      // 
      bubbleHintInfo3.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.barDockControlBottom, bubbleHintInfo3);
      this.barDockControlBottom.CausesValidation = false;
      this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.barDockControlBottom.Location = new System.Drawing.Point(0, 664);
      this.barDockControlBottom.Size = new System.Drawing.Size(556, 0);
      // 
      // barDockControlLeft
      // 
      bubbleHintInfo4.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.barDockControlLeft, bubbleHintInfo4);
      this.barDockControlLeft.CausesValidation = false;
      this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
      this.barDockControlLeft.Size = new System.Drawing.Size(0, 633);
      // 
      // barDockControlRight
      // 
      bubbleHintInfo5.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.barDockControlRight, bubbleHintInfo5);
      this.barDockControlRight.CausesValidation = false;
      this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.barDockControlRight.Location = new System.Drawing.Point(556, 31);
      this.barDockControlRight.Size = new System.Drawing.Size(0, 633);
      // 
      // btnCopyBindingSummaryfortechsupport
      // 
      this.btnCopyBindingSummaryfortechsupport.Caption = "Copy Binding Summary (for tech support)";
      this.btnCopyBindingSummaryfortechsupport.Id = 11;
      this.btnCopyBindingSummaryfortechsupport.Name = "btnCopyBindingSummaryfortechsupport";
      this.btnCopyBindingSummaryfortechsupport.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnCopyBindingSummaryfortechsupport_ItemClick);
      // 
      // btnCopyBindingLinkfordocumentation
      // 
      this.btnCopyBindingLinkfordocumentation.Caption = "Copy Binding Link (for documentation)";
      this.btnCopyBindingLinkfordocumentation.Id = 12;
      this.btnCopyBindingLinkfordocumentation.Name = "btnCopyBindingLinkfordocumentation";
      // 
      // barButtonItem1
      // 
      this.barButtonItem1.Caption = "-";
      this.barButtonItem1.Id = 14;
      this.barButtonItem1.Name = "barButtonItem1";
      // 
      // barSubItem1
      // 
      this.barSubItem1.Caption = "barSubItem1";
      this.barSubItem1.Id = 15;
      this.barSubItem1.Name = "barSubItem1";
      // 
      // barSubItem2
      // 
      this.barSubItem2.Caption = "-";
      this.barSubItem2.Id = 16;
      this.barSubItem2.Name = "barSubItem2";
      // 
      // barToolbarsListItem1
      // 
      this.barToolbarsListItem1.Caption = "barToolbarsListItem1";
      this.barToolbarsListItem1.Id = 17;
      this.barToolbarsListItem1.Name = "barToolbarsListItem1";
      // 
      // barLargeButtonItem1
      // 
      this.barLargeButtonItem1.Caption = "barLargeButtonItem1";
      this.barLargeButtonItem1.Id = 18;
      this.barLargeButtonItem1.Name = "barLargeButtonItem1";
      // 
      // btnCopyContext
      // 
      this.btnCopyContext.Caption = "Copy Context";
      this.btnCopyContext.Id = 19;
      this.btnCopyContext.Name = "btnCopyContext";
      this.btnCopyContext.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnCopyContext_ItemClick);
      // 
      // btnPasteContext
      // 
      this.btnPasteContext.Caption = "Paste Context";
      this.btnPasteContext.Id = 20;
      this.btnPasteContext.Name = "btnPasteContext";
      this.btnPasteContext.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnPasteContext_ItemClick);
      // 
      // btnCollapseFolders
      // 
      this.btnCollapseFolders.Caption = "Collapse Folders";
      this.btnCollapseFolders.Hint = "Collapse Folders";
      this.btnCollapseFolders.Id = 29;
      this.btnCollapseFolders.Name = "btnCollapseFolders";
      this.btnCollapseFolders.ItemClick += new DevExpress.DXCore.Controls.XtraBars.ItemClickEventHandler(this.btnCollapseFolders_ItemClick);
      // 
      // barButtonItem2
      // 
      this.barButtonItem2.Id = 31;
      this.barButtonItem2.Name = "barButtonItem2";
      // 
      // pmTreeList
      // 
      this.pmTreeList.LinksPersistInfo.AddRange(new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo[] {
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnNewKeyboardShortcut, true),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnDuplicate),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnNewFolder, true),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnRenameFolder),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnDelete, true),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnToggleFolders, true),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnCollapseFolders),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnCopyBindingSummaryfortechsupport, true),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnCopyContext, true),
            new DevExpress.DXCore.Controls.XtraBars.LinkPersistInfo(this.btnPasteContext)});
      this.pmTreeList.Manager = this.barManager;
      this.pmTreeList.Name = "pmTreeList";
      this.pmTreeList.Popup += new System.EventHandler(this.pmTreeList_Popup);
      // 
      // lblCommand
      // 
      bubbleHintInfo22.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.lblCommand, bubbleHintInfo22);
      this.lblCommand.Location = new System.Drawing.Point(6, 221);
      this.lblCommand.Name = "lblCommand";
      this.lblCommand.Size = new System.Drawing.Size(51, 13);
      this.lblCommand.TabIndex = 7;
      this.lblCommand.Text = "&Command:";
      // 
      // txtParameters
      // 
      this.txtParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      bubbleHintInfo15.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.txtParameters, bubbleHintInfo15);
      this.txtParameters.Location = new System.Drawing.Point(6, 285);
      this.txtParameters.Name = "txtParameters";
      this.txtParameters.Size = new System.Drawing.Size(276, 20);
      this.txtParameters.TabIndex = 10;
      this.txtParameters.TextChanged += new System.EventHandler(this.BindingDataChanged);
      // 
      // lblParameters
      // 
      bubbleHintInfo18.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.lblParameters, bubbleHintInfo18);
      this.lblParameters.Location = new System.Drawing.Point(6, 269);
      this.lblParameters.Name = "lblParameters";
      this.lblParameters.Size = new System.Drawing.Size(59, 13);
      this.lblParameters.TabIndex = 9;
      this.lblParameters.Text = "&Parameters:";
      // 
      // lblParametersAreOptional
      // 
      this.lblParametersAreOptional.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblParametersAreOptional.Appearance.ForeColor = System.Drawing.SystemColors.GrayText;
      this.lblParametersAreOptional.Appearance.TextOptions.HAlignment = DevExpress.DXCore.Controls.Utils.HorzAlignment.Far;
      this.lblParametersAreOptional.Appearance.TextOptions.VAlignment = DevExpress.DXCore.Controls.Utils.VertAlignment.Top;
      bubbleHintInfo17.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.lblParametersAreOptional, bubbleHintInfo17);
      this.lblParametersAreOptional.Location = new System.Drawing.Point(170, 269);
      this.lblParametersAreOptional.Name = "lblParametersAreOptional";
      this.lblParametersAreOptional.Size = new System.Drawing.Size(46, 13);
      this.lblParametersAreOptional.TabIndex = 11;
      this.lblParametersAreOptional.Text = "(optional)";
      // 
      // BubbleHints
      // 
      this.BubbleHints.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      // 
      // ContextPicker1
      // 
      this.ContextPicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      bubbleHintInfo19.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.ContextPicker1, bubbleHintInfo19);
      this.ContextPicker1.HintDisplayTime = 6000;
      this.ContextPicker1.HintWindowWidth = 200;
      this.ContextPicker1.LegendBackground = System.Drawing.SystemColors.Control;
      this.ContextPicker1.LegendFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ContextPicker1.Location = new System.Drawing.Point(6, 330);
      this.ContextPicker1.Name = "ContextPicker1";
      this.ContextPicker1.RootContext = "";
      this.ContextPicker1.ShowHint = true;
      this.ContextPicker1.ShowLegend = true;
      this.ContextPicker1.Size = new System.Drawing.Size(276, 297);
      this.ContextPicker1.TabIndex = 15;
      this.ContextPicker1.CheckStateChanged += new System.EventHandler(this.BindingDataChanged);
      // 
      // lblUseIn1
      // 
      bubbleHintInfo16.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.lblUseIn1, bubbleHintInfo16);
      this.lblUseIn1.Location = new System.Drawing.Point(6, 311);
      this.lblUseIn1.Name = "lblUseIn1";
      this.lblUseIn1.Size = new System.Drawing.Size(22, 13);
      this.lblUseIn1.TabIndex = 13;
      this.lblUseIn1.Text = "Use:";
      // 
      // lblUseIn
      // 
      this.lblUseIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      bubbleHintInfo14.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.lblUseIn, bubbleHintInfo14);
      this.lblUseIn.EditValue = "lblUseIn";
      this.lblUseIn.Location = new System.Drawing.Point(32, 309);
      this.lblUseIn.Name = "lblUseIn";
      this.lblUseIn.Properties.BorderStyle = DevExpress.DXCore.Controls.XtraEditors.Controls.BorderStyles.NoBorder;
      this.lblUseIn.Properties.ReadOnly = true;
      this.lblUseIn.Properties.UseParentBackground = true;
      this.lblUseIn.Size = new System.Drawing.Size(235, 18);
      this.lblUseIn.TabIndex = 14;
      // 
      // chkBindingEnabled
      // 
      bubbleHintInfo20.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.chkBindingEnabled, bubbleHintInfo20);
      this.chkBindingEnabled.Location = new System.Drawing.Point(6, 197);
      this.chkBindingEnabled.Name = "chkBindingEnabled";
      this.chkBindingEnabled.Properties.Caption = "&Enabled";
      this.chkBindingEnabled.Size = new System.Drawing.Size(76, 19);
      this.chkBindingEnabled.TabIndex = 6;
      this.chkBindingEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_Click);
      // 
      // cmbCommands
      // 
      this.cmbCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      bubbleHintInfo21.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.cmbCommands, bubbleHintInfo21);
      this.cmbCommands.EditValue = "";
      this.cmbCommands.Location = new System.Drawing.Point(6, 237);
      this.cmbCommands.Name = "cmbCommands";
      this.cmbCommands.Properties.Buttons.AddRange(new DevExpress.DXCore.Controls.XtraEditors.Controls.EditorButton[] {
            new DevExpress.DXCore.Controls.XtraEditors.Controls.EditorButton(DevExpress.DXCore.Controls.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.cmbCommands.Properties.CycleOnDblClick = false;
      this.cmbCommands.Properties.DropDownRows = 24;
      this.cmbCommands.Size = new System.Drawing.Size(276, 20);
      this.cmbCommands.TabIndex = 8;
      this.cmbCommands.TextChanged += new System.EventHandler(this.BindingDataChanged);
      // 
      // pnlBindings
      // 
      bubbleHintInfo10.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.pnlBindings, bubbleHintInfo10);
      this.pnlBindings.Controls.Add(this.lblKeyName);
      this.pnlBindings.Controls.Add(this.linkLayout);
      this.pnlBindings.Controls.Add(this.edBinding);
      this.pnlBindings.Controls.Add(this.lblUseIn);
      this.pnlBindings.Controls.Add(this.txtParameters);
      this.pnlBindings.Controls.Add(this.lblUseIn1);
      this.pnlBindings.Controls.Add(this.lblParametersAreOptional);
      this.pnlBindings.Controls.Add(this.lblParameters);
      this.pnlBindings.Controls.Add(this.ContextPicker1);
      this.pnlBindings.Controls.Add(this.chkBindingEnabled);
      this.pnlBindings.Controls.Add(this.cmbCommands);
      this.pnlBindings.Controls.Add(this.lblCommand);
      this.pnlBindings.Controls.Add(this.label1);
      this.pnlBindings.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlBindings.Location = new System.Drawing.Point(2, 2);
      this.pnlBindings.Name = "pnlBindings";
      this.pnlBindings.Size = new System.Drawing.Size(288, 629);
      this.pnlBindings.TabIndex = 19;
      // 
      // lblKeyName
      // 
      this.lblKeyName.AutoEllipsis = true;
      bubbleHintInfo11.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.lblKeyName, bubbleHintInfo11);
      this.lblKeyName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
      this.lblKeyName.Location = new System.Drawing.Point(92, 182);
      this.lblKeyName.Name = "lblKeyName";
      this.lblKeyName.Size = new System.Drawing.Size(184, 17);
      this.lblKeyName.TabIndex = 18;
      this.lblKeyName.Text = "(none)";
      // 
      // linkLayout
      // 
      bubbleHintInfo12.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.linkLayout, bubbleHintInfo12);
      this.linkLayout.EditValue = "Layout";
      this.linkLayout.Location = new System.Drawing.Point(28, 160);
      this.linkLayout.MenuManager = this.barManager;
      this.linkLayout.Name = "linkLayout";
      this.linkLayout.Properties.BorderStyle = DevExpress.DXCore.Controls.XtraEditors.Controls.BorderStyles.NoBorder;
      this.linkLayout.Size = new System.Drawing.Size(38, 18);
      this.linkLayout.TabIndex = 17;
      this.linkLayout.OpenLink += new DevExpress.DXCore.Controls.XtraEditors.Controls.OpenLinkEventHandler(this.linkLayout_OpenLink);
      // 
      // edBinding
      // 
      this.edBinding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      bubbleHintInfo13.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.edBinding, bubbleHintInfo13);
      this.edBinding.Location = new System.Drawing.Point(12, 8);
      this.edBinding.Name = "edBinding";
      this.edBinding.Size = new System.Drawing.Size(268, 173);
      this.edBinding.TabIndex = 1;
      this.edBinding.CustomInputChanged += new CR_XkeysEngine.CustomInputChangedEventHandler(this.edBinding_CustomInputChanged);
      // 
      // pnlFolders
      // 
      bubbleHintInfo24.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.pnlFolders, bubbleHintInfo24);
      this.pnlFolders.Controls.Add(this.memoFolderComment);
      this.pnlFolders.Controls.Add(this.label2);
      this.pnlFolders.Controls.Add(this.chkFolderEnabled);
      this.pnlFolders.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlFolders.Location = new System.Drawing.Point(2, 2);
      this.pnlFolders.Name = "pnlFolders";
      this.pnlFolders.Size = new System.Drawing.Size(288, 629);
      this.pnlFolders.TabIndex = 20;
      // 
      // memoFolderComment
      // 
      this.memoFolderComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      bubbleHintInfo25.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.memoFolderComment, bubbleHintInfo25);
      this.memoFolderComment.Location = new System.Drawing.Point(4, 52);
      this.memoFolderComment.Name = "memoFolderComment";
      this.memoFolderComment.Size = new System.Drawing.Size(280, 104);
      this.memoFolderComment.TabIndex = 9;
      // 
      // label2
      // 
      bubbleHintInfo26.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.label2, bubbleHintInfo26);
      this.label2.Location = new System.Drawing.Point(8, 32);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(49, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "&Comment:";
      // 
      // chkFolderEnabled
      // 
      bubbleHintInfo27.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.chkFolderEnabled, bubbleHintInfo27);
      this.chkFolderEnabled.Location = new System.Drawing.Point(8, 8);
      this.chkFolderEnabled.Name = "chkFolderEnabled";
      this.chkFolderEnabled.Properties.Caption = "&Enabled";
      this.chkFolderEnabled.Size = new System.Drawing.Size(104, 19);
      this.chkFolderEnabled.TabIndex = 7;
      this.chkFolderEnabled.CheckedChanged += new System.EventHandler(this.chkFolderEnabled_Click);
      // 
      // pnlControls
      // 
      bubbleHintInfo9.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.pnlControls, bubbleHintInfo9);
      this.pnlControls.Controls.Add(this.pnlBindings);
      this.pnlControls.Controls.Add(this.pnlFolders);
      this.pnlControls.Dock = System.Windows.Forms.DockStyle.Right;
      this.pnlControls.Location = new System.Drawing.Point(264, 31);
      this.pnlControls.Name = "pnlControls";
      this.pnlControls.Size = new System.Drawing.Size(292, 633);
      this.pnlControls.TabIndex = 21;
      // 
      // pnlTreeList
      // 
      bubbleHintInfo7.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.pnlTreeList, bubbleHintInfo7);
      this.pnlTreeList.Controls.Add(this.trlsShortcuts);
      this.pnlTreeList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlTreeList.Location = new System.Drawing.Point(0, 31);
      this.pnlTreeList.Name = "pnlTreeList";
      this.pnlTreeList.Padding = new System.Windows.Forms.Padding(2);
      this.pnlTreeList.Size = new System.Drawing.Size(252, 633);
      this.pnlTreeList.TabIndex = 22;
      // 
      // split
      // 
      bubbleHintInfo8.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.split, bubbleHintInfo8);
      this.split.Dock = System.Windows.Forms.DockStyle.Right;
      this.split.Location = new System.Drawing.Point(252, 31);
      this.split.Name = "split";
      this.split.Size = new System.Drawing.Size(12, 633);
      this.split.TabIndex = 23;
      this.split.TabStop = false;
      // 
      // imgTreeListIcons
      // 
      this.imgTreeListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTreeListIcons.ImageStream")));
      this.imgTreeListIcons.TransparentColor = System.Drawing.Color.Lime;
      this.imgTreeListIcons.Images.SetKeyName(0, "");
      this.imgTreeListIcons.Images.SetKeyName(1, "");
      this.imgTreeListIcons.Images.SetKeyName(2, "");
      this.imgTreeListIcons.Images.SetKeyName(3, "");
      this.imgTreeListIcons.Images.SetKeyName(4, "");
      this.imgTreeListIcons.Images.SetKeyName(5, "");
      // 
      // bigHint
      // 
      this.bigHint.Color = System.Drawing.Color.DarkGray;
      this.bigHint.Feature = "Shortcuts Option Page";
      this.bigHint.IsSingleton = true;
      this.bigHint.OptionsPath = null;
      this.bigHint.ResetDisplayCountOnStartup = false;
      this.bigHint.Text = "There are no shortcuts inside this folder. If you would like to create a new bind" +
    "ing, click the New Keyboard Shortcut button above.\r\n";
      this.bigHint.Title = "Empty Folder";
      this.bigHint.UserGuidePage = null;
      // 
      // label1
      // 
      this.label1.AutoEllipsis = true;
      bubbleHintInfo23.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this.label1, bubbleHintInfo23);
      this.label1.Location = new System.Drawing.Point(61, 182);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(36, 17);
      this.label1.TabIndex = 19;
      this.label1.Text = "Key: ";
      // 
      // OptXkeysShortcuts
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      bubbleHintInfo6.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
      this.BubbleHints.SetBubbleHint(this, bubbleHintInfo6);
      this.Controls.Add(this.pnlTreeList);
      this.Controls.Add(this.split);
      this.Controls.Add(this.pnlControls);
      this.Controls.Add(this.barDockControlLeft);
      this.Controls.Add(this.barDockControlRight);
      this.Controls.Add(this.barDockControlBottom);
      this.Controls.Add(this.barDockControlTop);
      this.Name = "OptXkeysShortcuts";
      this.Size = new System.Drawing.Size(556, 664);
      this.CommitChanges += new DevExpress.CodeRush.Core.OptionsPage.CommitChangesEventHandler(this.OptShortcuts_CommitChanges);
      this.PreparePage += new DevExpress.CodeRush.Core.OptionsPage.PreparePageEventHandler(this.OptShortcuts_PreparePage);
      this.CancelChanges += new DevExpress.CodeRush.Core.OptionsPage.CancelChangesEventHandler(this.OptShortcuts_CancelChanges);
      this.CustomInitialization += new DevExpress.CodeRush.Core.OptionsPage.CustomInitializationEventHandler(this.OptShortcuts_CustomInitialization);
      ((System.ComponentModel.ISupportInitialize)(this.CodeRushEvents)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trlsShortcuts)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Images16x16)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pmTreeList)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.txtParameters.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.BubbleHints)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lblUseIn.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.chkBindingEnabled.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cmbCommands.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pnlBindings)).EndInit();
      this.pnlBindings.ResumeLayout(false);
      this.pnlBindings.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.linkLayout.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pnlFolders)).EndInit();
      this.pnlFolders.ResumeLayout(false);
      this.pnlFolders.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.memoFolderComment.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.chkFolderEnabled.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).EndInit();
      this.pnlControls.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pnlTreeList)).EndInit();
      this.pnlTreeList.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.bigHint)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
      this.ResumeLayout(false);

    }
    #endregion

    // private methods...
    #region InitializeVariables()
    private void InitializeVariables()
    {
    }
    #endregion
    #region LoadCommands
    private void LoadCommands()
    {
      cmbCommands.Properties.Items.Clear();

      ActionCollection actions = CodeRush.Actions;

      ArrayList actionNames = new ArrayList(actions.Count);
      foreach (DevExpress.CodeRush.Core.Action action in actions)
      {
        if (!action.VisibleToUsers)
          continue;

        actionNames.Add(action.ActionName);
      }

      actionNames.Sort();

      cmbCommands.Properties.Items.AddRange((string[])actionNames.ToArray(typeof(string)));
    }
    #endregion

    #region commented code (ShiftPrefix) ...
    //	private string ShiftPrefix(KeyPressedEventArgs ea)
    //	{
    //  	string lResult = "";
    //		if (ea.CtrlKeyDown)
    //			lResult = lResult + "Ctrl + ";
    //		if (ea.ShiftKeyDown)
    //			lResult = lResult + "Shift + ";
    //		if (ea.AltKeyDown)
    //			lResult = lResult + "Alt + ";
    //		return lResult;
    //  }
    #endregion

    // Transfer operations
    #region ClearBindingControls
    private void ClearBindingControls()
    {
      _UpdatingInternally = true;
      try
      {
        edBinding.CustomInputControl.ClearData();

        chkBindingEnabled.Checked = true;
        txtParameters.Text = "";
        cmbCommands.Text = "";
      }
      finally
      {
        _UpdatingInternally = false;
      }
    }
    #endregion
    #region ClearFolderControls
    private void ClearFolderControls()
    {
      _UpdatingInternally = true;
      try
      {
        chkFolderEnabled.Checked = true;
        memoFolderComment.Text = "";
      }
      finally
      {
        _UpdatingInternally = false;
      }
    }
    #endregion
    #region GetContextDisplayText
    private string GetContextDisplayText(CommandKeyBinding aCommandKeyBinding)
    {
      string lResult;
      if (aCommandKeyBinding != null)
        lResult = aCommandKeyBinding.ReadableContext /*Context.DisplayText*/;
      else
        lResult = "";
      if (lResult == "")
        lResult = "none";		// Translate
      return lResult;
    }
    #endregion
    #region SetUseIn
    private void SetUseIn(string aText)
    {
      lblUseIn.Text = aText;
      lblUseIn.SelectionStart = 0;
      lblUseIn.SelectionLength = 0;
      lblUseIn.ScrollToCaret();
      lblUseIn.ToolTip = aText;
    }
    #endregion
    #region UpdateContextPreview
    private void UpdateContextPreview()
    {
      if (_CurrentBinding != null)
      {
        SetUseIn(_CurrentBinding.ReadableContext);
      }
      else
      {
        SetUseIn("");
      }
    }
    #endregion
    #region TogglePanels
    private void TogglePanels()
    {
      if (CurrentFolder != null)
      {
        pnlBindings.Visible = false;
        pnlFolders.Visible = true;
      }
      else if (CurrentBinding != null)
      {
        pnlBindings.Visible = true;
        pnlFolders.Visible = false;
      }
    }
    #endregion

    #region EnableBindingControls
    private void EnableBindingControls(bool shouldEnable)
    {
      lblCommand.Enabled = shouldEnable;
      txtParameters.Enabled = shouldEnable;
      lblParameters.Enabled = shouldEnable;
      cmbCommands.Enabled = shouldEnable;
      lblParametersAreOptional.Enabled = shouldEnable;
      ContextPicker1.Enabled = shouldEnable;
      lblUseIn.Enabled = shouldEnable;
      lblUseIn1.Enabled = shouldEnable;

      btnCopyContext.Enabled = shouldEnable;
      btnPasteContext.Enabled = shouldEnable;
    }
    #endregion
    #region EnableFolderControls
    private void EnableFolderControls(bool aShouldEnable)
    {
      btnRenameFolder.Enabled = aShouldEnable;
      memoFolderComment.Enabled = aShouldEnable;
      TogglePanels();
    }
    #endregion
    #region EnableAllFolderControls
    private void EnableAllFolderControls(bool aShouldEnable)
    {
      chkFolderEnabled.Enabled = aShouldEnable;
      EnableFolderControls(aShouldEnable);
    }
    #endregion
    #region EnableAllBindingControls
    private void EnableAllBindingControls(bool shouldEnable)
    {
      btnDuplicate.Enabled = shouldEnable;
      btnCopyBindingLinkfordocumentation.Enabled = shouldEnable;
      btnCopyBindingSummaryfortechsupport.Enabled = shouldEnable;

      btnDelete.Enabled = shouldEnable || CurrentFolder != null;
      btnRenameFolder.Enabled = shouldEnable && CurrentFolder == null;

      chkBindingEnabled.Enabled = shouldEnable;

      edBinding.CustomInputControl.EnableControls(shouldEnable);

      EnableBindingControls(shouldEnable && chkBindingEnabled.Checked);

      TogglePanels();
    }
    #endregion

    #region TransferFoldersToTreeList
    private TreeListNode TransferFoldersToTreeList(TreeListNode parent, ArrayList folders, CommandKeyBinding lastSelected)
    {
      TreeListNode lFocusedNode = null;
      if (folders == null)
        return lFocusedNode;
      for (int i = 0; i < folders.Count; i++)
      {
        CommandKeyFolder lFolder = (CommandKeyFolder)folders[i];
        TreeListNode lCurrentFocusedNode = TransferFolderToTreeList(parent, lFolder, lastSelected);
        if (lCurrentFocusedNode != null)
          lFocusedNode = lCurrentFocusedNode;
      }
      return lFocusedNode;
    }
    #endregion
    #region TransferBindingsToTreeList
    private TreeListNode TransferBindingsToTreeList(TreeListNode parent, CommandKeyBindingCollection bindings, CommandKeyBinding lastSelected)
    {
      TreeListNode lFocusedNode = null;
      if (bindings == null)
        return lFocusedNode;
      TreeListNode lCommandNode;
      foreach (CommandKeyBinding lCommandKeyBinding in bindings)
      {
        lCommandNode = trlsShortcuts.AppendNode(new object[] { lCommandKeyBinding.DisplayShortcut, lCommandKeyBinding.DisplayCommand, lCommandKeyBinding.ReadableContext /*Context.DisplayText*/}, parent);
        lCommandNode.Tag = lCommandKeyBinding;
        if (lastSelected == lCommandKeyBinding)
          lFocusedNode = lCommandNode;
      }
      return lFocusedNode;
    }
    #endregion
    #region TransferFolderToTreeList(CommandKeyFolder folder, CommandKeyBinding lastSelected)
    private void TransferFolderToTreeList(CommandKeyFolder folder, CommandKeyBinding lastSelected)
    {
      trlsShortcuts.ClearNodes();
      trlsShortcuts.FocusedNode = TransferRootFolderToTreeList(folder, lastSelected);
    }
    #endregion
    #region TransferFolderToTreeList(TreeListNode parentNode, CommandKeyFolder folder, CommandKeyBinding lastSelected, bool isRoot)
    private TreeListNode TransferFolderToTreeList(TreeListNode parentNode, CommandKeyFolder folder, CommandKeyBinding lastSelected, bool isRoot)
    {
      if (folder == null || folder.IsDeleted)
        return null;

      TreeListNode lNodeToFocus = null;
      TreeListNode lFolderNode = null;

      if (!isRoot && _DisplayMode == DisplayMode.Tree)
      {
        lFolderNode = trlsShortcuts.AppendNode(new object[] { folder.Name }, parentNode);
        lFolderNode.Tag = folder;
      }

      TreeListNode lNodeToFocusInFolders = TransferFoldersToTreeList(lFolderNode, folder.Folders, lastSelected);
      TreeListNode lNodeToFocusInBindings = TransferBindingsToTreeList(lFolderNode, folder.CommandBindings, lastSelected);
      if (lNodeToFocusInFolders != null)
        lNodeToFocus = lNodeToFocusInFolders;
      else
        lNodeToFocus = lNodeToFocusInBindings;
      return lNodeToFocus;
    }
    #endregion
    #region TransferRootFolderToTreeList(CommandKeyFolder folder, CommandKeyBinding lastSelected)
    private TreeListNode TransferRootFolderToTreeList(CommandKeyFolder folder, CommandKeyBinding lastSelected)
    {
      return TransferFolderToTreeList(null, folder, lastSelected, true);
    }
    #endregion
    #region TransferFolderToTreeList(TreeListNode parentNode, CommandKeyFolder folder, CommandKeyBinding lastSelected)
    private TreeListNode TransferFolderToTreeList(TreeListNode parentNode, CommandKeyFolder folder, CommandKeyBinding lastSelected)
    {
      return TransferFolderToTreeList(parentNode, folder, lastSelected, false);
    }
    #endregion

    #region TransferControlsToCurrentFolder
    private void TransferControlsToCurrentFolder()
    {
      if (_CurrentFolder == null)
        return;
      _CurrentFolder.EnableFolder(chkFolderEnabled.Checked);
      _CurrentFolder.Description = memoFolderComment.Text;
      trlsShortcuts.Refresh();
    }
    #endregion
    #region TransferControlsToCurrentBinding
    private void TransferControlsToCurrentBinding()
    {
      if (_UpdatingInternally)
        return;
      if (_CurrentBinding == null)
        return;

      _CurrentBinding.Enabled = chkBindingEnabled.Checked;
      _CurrentBinding.AltKeyDown = edBinding.CustomInputControl.AltKeyDown;
      _CurrentBinding.CtrlKeyDown = edBinding.CustomInputControl.CtrlKeyDown;
      _CurrentBinding.ShiftKeyDown = edBinding.CustomInputControl.ShiftKeyDown;
      _CurrentBinding.AnyShiftModifier = edBinding.CustomInputControl.AnyShiftModifier;
      _CurrentBinding.CustomData = edBinding.CustomInputControl.GetCustomData();

      _CurrentBinding.Command = cmbCommands.Text;
      _CurrentBinding.Parameters = txtParameters.Text;
      _CurrentBinding.SetContext(ContextPicker1.GetData());
      UpdateContextPreview();

      // It would be nice to have a more immediate redraw of only the changed (focused) node, instead of refreshing the entire control...
      trlsShortcuts.Refresh();
    }
    #endregion

    #region TransferControlsToFolder
    private void TransferControlsToFolder(CommandKeyFolder folder)
    {
      if (folder == null)
        return;

      folder.Enabled = chkFolderEnabled.Checked;
      folder.Description = memoFolderComment.Text;
    }
    #endregion
    #region TransferFolderToControls
    private void TransferFolderToControls(CommandKeyFolder folder)
    {
      if (folder == null)
        return;

      memoFolderComment.Text = folder.Description;
      memoFolderComment.Enabled = folder.Enabled;
      chkFolderEnabled.Checked = folder.Enabled;
    }
    #endregion
    #region TransferCurrentFolderToControls
    private void TransferCurrentFolderToControls()
    {
      if (_CurrentBinding == null)
      {
        ClearBindingControls();
        EnableAllBindingControls(false);
      }
      if (_CurrentFolder == null)
      {
        ClearFolderControls();
        EnableAllFolderControls(false);
        return;
      }
      EnableAllFolderControls(true);
      TransferFolderToControls(_CurrentFolder);
    }
    #endregion
    #region TransferCurrentBindingToControls
    private void TransferCurrentBindingToControls()
    {
      if (_CurrentFolder == null)
      {
        ClearFolderControls();
        EnableAllFolderControls(false);
      }
      if (_CurrentBinding == null)
      {
        ClearBindingControls();
        EnableAllBindingControls(false);
        return;
      }

      _UpdatingInternally = true;
      try
      {
        chkBindingEnabled.Checked = _CurrentBinding.Enabled;

        // SEE S135000
        string enabledCheckBoxText;
        bool parentFolderIsDisabled = IsParentFolderDisabled(_CurrentBinding.ParentFolder);
        if (parentFolderIsDisabled)
          enabledCheckBoxText = "&Enabled (NOTE: disabled by the parent folder)";
        else
          enabledCheckBoxText = "&Enabled";
        chkBindingEnabled.Text = enabledCheckBoxText;

        cmbCommands.Text = _CurrentBinding.Command;
        txtParameters.Text = _CurrentBinding.Parameters;
        ContextPicker1.SetData(_CurrentBinding.Context);

        UpdateContextPreview();

        edBinding.CustomInputControl.AltKeyDown = _CurrentBinding.AltKeyDown;
        edBinding.CustomInputControl.CtrlKeyDown = _CurrentBinding.CtrlKeyDown;
        edBinding.CustomInputControl.AnyShiftModifier = _CurrentBinding.AnyShiftModifier;
        edBinding.CustomInputControl.ShiftKeyDown = _CurrentBinding.ShiftKeyDown;
        edBinding.CustomInputControl.SetCustomData(_CurrentBinding.CustomData);
        lblKeyName.Text = "Key: " + _CurrentBinding.GetDisplayShortcut(edBinding.CustomInputControl.XkeyLayout);
      }
      finally
      {
        _UpdatingInternally = false;
      }
      EnableAllBindingControls(true);
    }
    #endregion
    #region TransferControlsToNode
    private void TransferControlsToNode(TreeListNode node)
    {
      if (node != null && (node.Tag is CommandKeyBinding || node.Tag is CommandKeyFolder))
      {
        if (node.Tag is CommandKeyFolder)
          TransferControlsToFolder(node.Tag as CommandKeyFolder);
      }
    }
    #endregion

    #region EnableDragNodes
    private void EnableDragNodes(bool enable)
    {
      trlsShortcuts.OptionsBehavior.DragNodes = enable;
    }
    #endregion
    #region RefreshDisplayMode
    private void RefreshDisplayMode(DisplayMode displayMode)
    {
      if (_DisplayMode == displayMode)
        return;

      _DisplayMode = displayMode;
      bool lEnable = _DisplayMode == DisplayMode.Tree;

      EnableDragNodes(lEnable);
      string lText = "";
      if (lEnable)
        lText = "Hide Shortcut Folders";
      else
        lText = "Show Shortcut Folders";

      btnToggleFolders.Caption = lText;
      btnToggleFolders.Hint = lText;
      btnNewFolder.Enabled = lEnable;
      btnRenameFolder.Enabled = lEnable;

      FillTreeList(CurrentBinding);
    }
    #endregion
    #region FillTreeList
    private void FillTreeList(CommandKeyBinding lastBindingSelected)
    {
      try
      {
        trlsShortcuts.BeginUnboundLoad();
        TransferFolderToTreeList(_RootFolder, lastBindingSelected);
        ExpandFolders();
        //trlsShortcuts.FullExpand();
      }
      finally
      {
        trlsShortcuts.EndUnboundLoad();
        colShortcut.SortOrder = WinForms.SortOrder.Ascending;
      }

      //CommandKeyBinding lastBindingSelected = _ShortcutCollection.Load(e.Storage);
      //TransferShortcutCollectionToTreeList(lastBindingSelected);

      if (lastBindingSelected == null && _RootFolder.CommandBindings.Count > 0)
      {
        CurrentBinding = _RootFolder.CommandBindings[0];
        CurrentFolder = null;
      }
      else
      {
        CurrentBinding = lastBindingSelected;
        CurrentFolder = null;
      }

      if (trlsShortcuts.FocusedNode != null && trlsShortcuts.FocusedNode.Tag is CommandKeyFolder)
      {
        CurrentBinding = null;
        CurrentFolder = trlsShortcuts.FocusedNode.Tag as CommandKeyFolder;
      }
    }
    #endregion

    // Operations with Folders and Bindings
    #region AddCommand
    private void AddCommand(CommandKeyBinding binding)
    {
      TreeListNode lTreeListNode;

      CommandKeyFolder lParentFolder = null;
      if (trlsShortcuts.FocusedNode != null)
      {
        if (trlsShortcuts.FocusedNode.Tag is CommandKeyFolder)
        {
          lTreeListNode = trlsShortcuts.AppendNode(null, trlsShortcuts.FocusedNode);
          lParentFolder = (trlsShortcuts.FocusedNode.Tag as CommandKeyFolder);
        }
        else
        {
          lTreeListNode = trlsShortcuts.AppendNode(null, trlsShortcuts.FocusedNode.ParentNode);
          CommandKeyBinding lCurrentBinding = trlsShortcuts.FocusedNode.Tag as CommandKeyBinding;
          lParentFolder = lCurrentBinding.ParentFolder;
        }
      }
      else
      {
        lParentFolder = _RootFolder;
        lTreeListNode = trlsShortcuts.AppendNode(null, null);
      }

      binding.ParentFolder = lParentFolder;
      lTreeListNode.Tag = binding;

      CurrentBinding = binding;

      trlsShortcuts.FocusedNode = lTreeListNode;
      edBinding.CustomInputControl.FocusPrimaryEditControl();
    }
    #endregion
    #region CreateFolder
    private void CreateFolder()
    {
      string caption = "New Folder"; // Translate
      string folderName = "";
      string parentFolderName = "";

      TreeListNode treeListNode;
      CommandKeyFolder parentFolder = null;
      TreeListNode parentNode = null;
      if (trlsShortcuts.FocusedNode != null)
      {
        if (trlsShortcuts.FocusedNode.Tag is CommandKeyFolder)
        {
          parentFolder = (trlsShortcuts.FocusedNode.Tag as CommandKeyFolder);
          parentNode = trlsShortcuts.FocusedNode;
        }
        if (parentFolder != null && parentFolder != _RootFolder)
          parentFolderName = parentFolder.Name;
      }

      bool inRoot = true;
      if (!NewFolderName.CreateNewFolder(caption, parentFolder, ref folderName, ref inRoot))
        return;

      CommandKeyFolder folder = new CommandKeyFolder();
      folder.Name = folderName;

      if (trlsShortcuts.FocusedNode != null && !inRoot)
      {
        if (parentFolder == null)
          return;
        if (parentFolder.ContainsFolder(folderName))
        {
          MessageBox.Show(String.Format(STR_FolderAlreadyExistTryDifferentName, folderName), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); // Translate
          return;
        }
        treeListNode = trlsShortcuts.AppendNode(null, parentNode);
        folder.ParentFolder = parentFolder;
      }
      else
      {
        if (_RootFolder == null)
          return;
        if (_RootFolder.ContainsFolder(folderName))
        {
          MessageBox.Show(String.Format(STR_FolderAlreadyExistTryDifferentName, folderName), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); // Translate
          return;
        }

        folder.ParentFolder = _RootFolder;
        treeListNode = trlsShortcuts.AppendNode(null, null);
      }

      treeListNode.Tag = folder;

      CurrentFolder = folder;

      ResortNodes();
      trlsShortcuts.FocusedNode = treeListNode;
    }
    #endregion
    #region RenameFolder
    private void RenameFolder()
    {
      if (trlsShortcuts.FocusedNode == null || trlsShortcuts.FocusedNode.Tag == null || !(trlsShortcuts.FocusedNode.Tag is CommandKeyFolder))
        return;

      CommandKeyFolder lCurrentFolder = trlsShortcuts.FocusedNode.Tag as CommandKeyFolder;

      string lCaption = "Rename Folder"; // Translate
      string lFolderName = lCurrentFolder.Name;

      CommandKeyFolder lParentFolder = (trlsShortcuts.FocusedNode.Tag as CommandKeyFolder).ParentFolder;
      if (!NewFolderName.RenameFolder(lCaption, lParentFolder, ref lFolderName))
        return;

      if (lParentFolder == null)
        return;
      if (lParentFolder.ContainsFolder(lFolderName))
      {
        MessageBox.Show(String.Format(STR_FolderAlreadyExistTryDifferentName, lFolderName), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); // Translate
        return;
      }

      lCurrentFolder.Name = lFolderName;
      trlsShortcuts.InvalidateNode(trlsShortcuts.FocusedNode);
    }
    #endregion
    #region Delete
    private void Delete()
    {
      TreeListNode selectedNode = trlsShortcuts.FocusedNode;
      if (selectedNode == null)
        return;
      if (selectedNode.Tag is CommandKeyBinding)
        DeleteBinding(selectedNode);
      else if (selectedNode.Tag is CommandKeyFolder)
        DeleteFolder(selectedNode);
    }
    #endregion
    #region CreateShortcut
    private void CreateShortcut()
    {
      AddCommand(new CommandKeyBinding());
    }
    #endregion
    #region CreateShortcut
    private void CreateShortcut(CommandKeyBinding cloneFrom)
    {
      if (cloneFrom == null)
        AddCommand(new CommandKeyBinding());
      else
      {
        CommandKeyBinding commandKeyBinding = cloneFrom.Clone();
        commandKeyBinding.ParentFolder = cloneFrom.ParentFolder;
        AddCommand(commandKeyBinding);
      }
    }
    #endregion

    #region CreateShortcutForCommand
    private void CreateShortcutForCommand(string commandName)
    {
      CommandKeyBinding binding = new CommandKeyBinding();
      binding.Command = commandName;
      AddCommand(binding);
    }
    #endregion
    #region CreateShortcutForCommand
    private void CreateShortcutForCommand(string commandName, string parameters)
    {
      CommandKeyBinding binding = new CommandKeyBinding();
      binding.Command = commandName;
      binding.Parameters = parameters;
      AddCommand(binding);
    }
    #endregion

    #region ResortNodes
    private void ResortNodes()
    {
      trlsShortcuts.BeginSort();

      WinForms.SortOrder lOldShortCutSortOrder = colShortcut.SortOrder;
      WinForms.SortOrder lOldCommandSortOrder = colCommand.SortOrder;
      WinForms.SortOrder lOldContextSortOrder = colContext.SortOrder;

      colShortcut.SortOrder = WinForms.SortOrder.None;
      colCommand.SortOrder = WinForms.SortOrder.None;
      colContext.SortOrder = WinForms.SortOrder.None;

      colShortcut.SortOrder = lOldShortCutSortOrder;
      colCommand.SortOrder = lOldCommandSortOrder;
      colContext.SortOrder = lOldContextSortOrder;

      trlsShortcuts.EndSort();
    }
    #endregion

    #region IsParentFolderDisabled
    private bool IsParentFolderDisabled(CommandKeyFolder folder)
    {
      if (folder == null)
        return false;

      if (!folder.Enabled)
        return true;

      CommandKeyFolder parentFolder = folder.ParentFolder;
      while (parentFolder != null)
      {
        if (!parentFolder.Enabled)
          return true;

        parentFolder = parentFolder.ParentFolder;
      }
      return false;
    }
    #endregion

    #region MoveCommandKeyFolder
    private void MoveCommandKeyFolder(CommandKeyFolder sourceFolder, CommandKeyFolder targetFolder)
    {
      if (sourceFolder == null || targetFolder == null)
        return;

      sourceFolder.ParentFolder = targetFolder;
    }
    #endregion
    #region MoveKommandKeyBinding
    private void MoveKommandKeyBinding(CommandKeyBinding sourceBinding, CommandKeyFolder targetFolder)
    {
      if (sourceBinding == null || targetFolder == null)
        return;

      sourceBinding.ParentFolder = targetFolder;
    }
    #endregion

    #region DeleteBinding
    private void DeleteBinding(TreeListNode node)
    {
      CommandKeyBinding lCommandKeyBinding = (CommandKeyBinding)node.Tag;
      DialogResult lResult = MessageBox.Show(
        String.Format("Delete the selected shortcut ({0})?", lCommandKeyBinding.DisplayShortcut), "Confirm",			// Translate
        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button1);

      /* Delete the selected node. */
      if (lResult == DialogResult.Yes)
      {
        try
        {
          trlsShortcuts.DeleteNode(node);
          lCommandKeyBinding.DeleteBinding();
        }
        catch (Exception)
        {
          // Exception is thrown when trying to delete shortcut when there is some sorting order set.
          //Log.SendException(ex);
        }
      }
    }
    #endregion
    #region DeleteFolder
    private void DeleteFolder(TreeListNode node)
    {
      CommandKeyFolder lCommandKeyFolder = (CommandKeyFolder)node.Tag;
      DialogResult lResult = MessageBox.Show(
        String.Format("Delete the selected folder ({0})? \r\n This action will delete all shortcuts in the folder", lCommandKeyFolder.Name), "Confirm",			// Translate
        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button1);

      /* Delete the selected node. */
      if (lResult == DialogResult.Yes)
      {
        try
        {
          if (node == _LastHighlightedFolder)
            _LastHighlightedFolder = null;
          trlsShortcuts.DeleteNode(node);
          //lCommandKeyFolder.DeleteFolder();
          lCommandKeyFolder.IsDeleted = true;
        }
        catch (Exception)
        {
          // Exception is thrown when trying to delete shortcut when there is some sorting order set.
          //Log.SendException(ex);
        }
      }
    }
    #endregion

    // Operations with context
    #region GetContextNodeContext
    private ContextOption GetContextNodeContext(TreeListNode aTreeListNode)
    {
      if (aTreeListNode.StateImageIndex == INT_ContextIgnored)
        return ContextOption.Ignored;
      else if (aTreeListNode.StateImageIndex == INT_ContextSelected)
        return ContextOption.Selected;
      else
        return ContextOption.Mixed;
    }
    #endregion
    #region SetContextNodeContext
    private void SetContextNodeContext(TreeListNode aTreeListNode, ContextOption aContextOption)
    {
      if (aContextOption == ContextOption.Ignored)
        aTreeListNode.StateImageIndex = INT_ContextIgnored;
      else if (aContextOption == ContextOption.Selected)
        aTreeListNode.StateImageIndex = INT_ContextSelected;
      else
        aTreeListNode.StateImageIndex = INT_ContextMixed;
    }
    #endregion

    #region CopyContext
    private void CopyContext()
    {
      ContextPicker1.CopyContext();
    }
    #endregion
    #region PasteContext
    private void PasteContext()
    {
      ContextPicker1.PasteContext();
    }
    #endregion


    #region CompareShortcuts
    private void CompareShortcuts(CompareNodeValuesEventArgs e, CommandKeyBinding first, CommandKeyBinding second)
    {
      if (e.Column == colContext)
      {
        e.Result = String.Compare(first.ReadableContext /*Context.DisplayText*/, second.ReadableContext /*Context.DisplayText*/);
      }
      else if (e.Column == colCommand)
        e.Result = String.Compare(first.Command + "(" + first.Parameters,
          second.Command + "(" + second.Parameters);
      else if (e.Column == colShortcut)
        e.Result = String.Compare(first.CustomData, second.CustomData);
    }
    #endregion
    #region CompareFolders
    private static void CompareFolders(CompareNodeValuesEventArgs e, CommandKeyFolder first, CommandKeyFolder second)
    {
      if (first == null && second == null)
        e.Result = 0;
      else if (first == null)
        e.Result = -1;
      else if (second == null)
        e.Result = 1;
      else
        e.Result = String.Compare(first.Name, second.Name);
    }
    #endregion
    #region SetCurrentShortcuts
    private void SetCurrentShortcuts(TreeListNode node)
    {
      if (node != null && (node.Tag is CommandKeyBinding || node.Tag is CommandKeyFolder))
      {
        if (node.Tag is CommandKeyBinding)
        {
          CurrentBinding = (CommandKeyBinding)node.Tag;
          CurrentFolder = null;
        }
        else if (node.Tag is CommandKeyFolder)
        {
          CurrentBinding = null;
          CurrentFolder = (CommandKeyFolder)node.Tag;
        }
      }
      else
      {
        CurrentBinding = null;
        CurrentFolder = null;
      }
    }
    #endregion

    // Drawing operations
    #region DrawContext
    private static Rectangle DrawContext(CommandKeyBinding commandKeyBinding, Graphics graphics, Rectangle boundingRect, string description, Font font, Brush brush, StringFormat strFormat)
    {
      if (commandKeyBinding == null)
        return boundingRect;

      int yOffset = (boundingRect.Height / 2) - 9;

      boundingRect.X += 2;

      if ((description != "") && (graphics != null))
      {
        graphics.DrawString(description, font, brush,
          boundingRect, strFormat);		// Translate
      }
      return boundingRect;
    }
    #endregion
    #region DrawContext
    private Rectangle DrawContext(TreeListNode node, Graphics graphics, Rectangle boundingRect, string description, Font font, Brush brush, StringFormat strFormat)
    {
      if (node.Tag is CommandKeyBinding)
      {
        CommandKeyBinding commandKeyBinding = (CommandKeyBinding)node.Tag;
        return DrawContext(commandKeyBinding, graphics, boundingRect, description, font, brush, strFormat);
      }
      return boundingRect;
    }
    #endregion

    #region DrawShortcut
    private void DrawShortcut(CustomDrawNodeCellEventArgs e)
    {
      if (e.Node == null)
        return;

      CommandKeyBinding commandKeyBinding = (CommandKeyBinding)e.Node.Tag;

      Brush foreBrush = e.Appearance.GetForeBrush(e.Cache);

      if (commandKeyBinding != null && !commandKeyBinding.IsCompletelyEnabled)
        foreBrush = new SolidBrush(LookAndFeelHelper.GetSystemColor(UserLookAndFeel.Default.ActiveLookAndFeel, SystemColors.GrayText));
      // obtaining brushes for cells of focused and unfocused nodes
      Rectangle rectangle = e.Bounds;
      e.Appearance.DrawBackground(e.Cache, e.Bounds);

      string cellText;

      if (e.Column == colContext)
      {
        rectangle.Y++;
        rectangle.Height--;
        cellText = GetContextDisplayText(commandKeyBinding);
        DrawContext(e.Node, e.Graphics, rectangle, cellText, e.Appearance.GetFont(), foreBrush, e.Appearance.GetStringFormat());
      }
      else if (e.Column == colShortcut)
      {
        int imageIndex;

        cellText = commandKeyBinding.GetDisplayShortcut(edBinding.CustomInputControl.XkeyLayout);
        imageIndex = IMG_KeyboardIcon;
        int iconHeight = Math.Min(imgTreeListIcons.ImageSize.Height, rectangle.Height);
        int iconWidth = imgTreeListIcons.ImageSize.Width;
        int yOffset = (rectangle.Height - iconHeight) / 2;

        rectangle.X += 3;
        imgTreeListIcons.Draw(e.Graphics, rectangle.X, rectangle.Y + yOffset, iconWidth, iconHeight, imageIndex);
        rectangle.X += iconWidth + 2;
        e.Appearance.DrawString(e.Cache, cellText, rectangle, foreBrush);
      }
      else		// Command...
      {
        cellText = commandKeyBinding.DisplayCommand;
        e.Appearance.DrawString(e.Cache, cellText, rectangle, foreBrush);
      }
    }
    #endregion
    #region DrawFolder
    private void DrawFolder(CustomDrawNodeCellEventArgs e)
    {
      CommandKeyFolder commandKeyFolder = (CommandKeyFolder)e.Node.Tag;

      Brush foreBrush = e.Appearance.GetForeBrush(e.Cache);

      if (commandKeyFolder != null && !commandKeyFolder.IsCompletelyEnabled)
        foreBrush = new SolidBrush(LookAndFeelHelper.GetSystemColor(UserLookAndFeel.Default.ActiveLookAndFeel, SystemColors.GrayText));

      Rectangle rectangle = e.Bounds;

      // drawing icon and text
      string cellText;

      if (e.Column.VisibleIndex == 0)
      {
        RectangleF clipRect = e.Graphics.ClipBounds;
        int totalWidth = trlsShortcuts.ViewInfo.ViewRects.ColumnTotalWidth;
        clipRect.Width = totalWidth;
        e.Graphics.Clip = new Region(clipRect);
        rectangle.Width = totalWidth;

        int imageIndex;
        cellText = commandKeyFolder.Name;

        if (e.Node == _LastHighlightedFolder)
          imageIndex = (e.Node.Expanded && e.Node.HasChildren) ? IMG_FolderOpenHover : IMG_FolderCloseHover;
        else
          imageIndex = (e.Node.Expanded && e.Node.HasChildren) ? IMG_FolderOpen : IMG_FolderClose;

        // Drawing folder image
        int iconHeight = Math.Min(imgTreeListIcons.ImageSize.Height, rectangle.Height);
        int iconWidth = imgTreeListIcons.ImageSize.Width;
        int yOffset = (rectangle.Height - iconHeight) / 2;

        // filling the background
        e.Appearance.DrawBackground(e.Cache, rectangle);

        imgTreeListIcons.Draw(e.Graphics, rectangle.X + 3, rectangle.Y + yOffset, iconWidth, iconHeight, imageIndex);
        Rectangle stringRect = new Rectangle(rectangle.X + iconWidth + 5, rectangle.Y, rectangle.Width, rectangle.Height);
        e.Appearance.DrawString(e.Cache, cellText, stringRect, foreBrush);
      }
    }
    #endregion

    // Drag and drop operations
    #region HasParent
    private bool HasParent(TreeListNode node, TreeListNode parentNode)
    {
      if (node == null || node.ParentNode == null || parentNode == null)
        return false;

      TreeListNode lParentNode = node.ParentNode;
      if (lParentNode == parentNode)
        return true;

      return HasParent(lParentNode, parentNode);
    }
    #endregion
    #region GetDragNode
    private TreeListNode GetDragNode(IDataObject data)
    {
      return data.GetData(typeof(TreeListNode)) as TreeListNode;
    }
    #endregion
    #region GetTargetNode
    private TreeListNode GetTargetNode(TreeList tl, Point mousePosition)
    {
      if (tl == null)
        return null;

      Point clientMouse = tl.PointToClient(mousePosition);
      
      return tl.CalcHitInfo(clientMouse).Node;
    }
    #endregion
    #region GetTargetFolder
    private TreeListNode GetTargetFolder(TreeListNode node, int keyState)
    {
      if (node == null)
        return null;
      if ((keyState & 4) == 0)
      {
        if (node.Tag is CommandKeyFolder)
          return node;
        else if (node.Tag is CommandKeyBinding)
          return node.ParentNode;
      }
      else
        return node.ParentNode;
      return null;
    }
    #endregion
    #region GetDragDropEffect
    private DragDropEffects GetDragDropEffect(TreeList tl, Point mousePosition, int keyState, TreeListNode dragNode)
    {
      if (tl == null)
        return DragDropEffects.None;

      TreeListNode targetNode = GetTargetNode(tl, mousePosition);
      if (dragNode == null || dragNode == targetNode || dragNode.Tag == null || HasParent(targetNode, dragNode))
        return DragDropEffects.None;

      CommandKeyFolder dragFolder = dragNode.Tag as CommandKeyFolder;
      TreeListNode targetFolderNode = GetTargetFolder(targetNode, keyState);
      CommandKeyFolder targetFolder = null;

      if (targetFolderNode == null)
        targetFolder = _RootFolder;
      else
        targetFolder = targetFolderNode.Tag as CommandKeyFolder;

      if (targetFolder == null || (dragFolder != null && targetFolder.ContainsFolder(dragFolder.Name)))
        return DragDropEffects.None;

      return DragDropEffects.Move;
    }
    #endregion

    #region CheckInIcon
    private bool CheckInIcon(int dx, int dy, int shiftX, int shiftY, int width, int height)
    {
      return (dx >= shiftX && dx <= shiftX + width && dy >= shiftY && dy <= shiftY + height);
    }
    #endregion
    #region GetDelta
    private bool GetDelta(TreeList tl, out int dx, out int dy, out TreeListHitInfo hitInfo)
    {
      dx = 0;
      dy = 0;
      hitInfo = null;
      if (tl == null)
        return false;

      Point lCursor = tl.PointToClient(Cursor.Position);
      hitInfo = tl.CalcHitInfo(lCursor);
      if (hitInfo.Column != colShortcut)
        return false;

      dx = lCursor.X - hitInfo.Bounds.Left;
      dy = lCursor.Y - hitInfo.Bounds.Top;
      return true;
    }
    #endregion

    #region CursorInFolderIcon
    private bool CursorInFolderIcon(TreeList tl)
    {
      int lDx;
      int lDy;
      TreeListHitInfo lHitInfo;
      if (!GetDelta(tl, out lDx, out lDy, out lHitInfo))
        return false;

      int lWidth = 16;
      int lHeight = 16;

      int lShiftY = (lHitInfo.Bounds.Height - lHeight) / 2;
      int lShiftX = 3;

      return CheckInIcon(lDx, lDy, lShiftX, lShiftY, lWidth, lHeight);
    }
    #endregion

    #region ExpandFolders(TreeListNodes nodes)
    private void ExpandFolders(TreeListNodes nodes)
    {
      if (nodes == null || nodes.Count == 0)
        return;
      for (int i = 0; i < nodes.Count; i++)
      {
        TreeListNode lNode = nodes[i];
        if (lNode.Tag is CommandKeyFolder && lNode.HasChildren)
        {
          CommandKeyFolder lFolder = (CommandKeyFolder)lNode.Tag;
          lNode.Expanded = lFolder.FolderState == FolderState.Expanded;
          ExpandFolders(lNode.Nodes);
        }
      }
    }
    #endregion
    #region ExpandFolders
    private void ExpandFolders()
    {
      ExpandFolders(trlsShortcuts.Nodes);
    }
    #endregion

    #region ShowBigHintIfNeeded
    private void ShowBigHintIfNeeded(TreeListNode node)
    {
      if (node == null)
        return;
      if (node.Tag is CommandKeyFolder)
      {
        if (!node.HasChildren)
        {
          Rectangle lBounds = trlsShortcuts.ViewInfo.RowsInfo[node].Bounds;
          Point lPoint = new Point(lBounds.X + 20, lBounds.Bottom);
          lPoint = trlsShortcuts.PointToScreen(lPoint);
          MessageHintBase lHint = bigHint.ShowAt(lPoint);
          if (lHint != null)
            lHint.CloseByTimer(5000);
        }
      }
    }
    #endregion

    Hashtable _ChangedShortcuts = new Hashtable();
    void ExcludeChangedShortcutsFromFeatures()
    {
      if (_ChangedShortcuts == null || _ChangedShortcuts.Count == 0)
        return;
      foreach (string shortcut in _ChangedShortcuts.Keys)
        CodeRush.Feature.RemoveFeature(shortcut);
    }

    // for menu operations 
    // event handlers ...
    // TreeList
    #region trlsShortcuts_AfterFocusNode
    private void trlsShortcuts_AfterFocusNode(object sender, NodeEventArgs e)
    {
      try
      {
        ShowBigHintIfNeeded(e.Node);
        //TransferControlsToNode(e.Node);
        //SetCurrentShortcuts(e.Node);
      }
      catch (Exception ex)
      {
        Log.SendException(ex);
      }
    }
    #endregion
    #region trlsShortcuts_FocusedNodeChanged
    private void trlsShortcuts_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
    {
      try
      {
        if (_Loading)
          return;
        TransferControlsToNode(e.OldNode);
        SetCurrentShortcuts(e.Node);
      }
      catch (Exception ex)
      {
        Log.SendException(ex);
      }
    }
    #endregion
    #region trlsShortcuts_CustomDrawNodeCell
    private void trlsShortcuts_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
    {
      if (e.Node.Tag is CommandKeyBinding)
      {
        DrawShortcut(e);
        // prohibiting default painting
        e.Handled = true;
      }
      if (e.Node.Tag is CommandKeyFolder)
      {
        DrawFolder(e);
        // prohibiting default painting
        e.Handled = true;
      }

    }
    #endregion
    #region trlsShortcuts_CompareNodeValues
    private void trlsShortcuts_CompareNodeValues(object sender, CompareNodeValuesEventArgs e)
    {
      e.Result = 0;
      CommandKeyBinding lCommandKeyBinding1;
      CommandKeyBinding lCommandKeyBinding2;

      if (e.Node1.Tag is CommandKeyBinding && e.Node2.Tag is CommandKeyBinding)
      {
        lCommandKeyBinding1 = (CommandKeyBinding)e.Node1.Tag;
        lCommandKeyBinding2 = (CommandKeyBinding)e.Node2.Tag;
        CompareShortcuts(e, lCommandKeyBinding1, lCommandKeyBinding2);
      }
      else
      {
        if (e.Node1.Tag is CommandKeyFolder && e.Node2.Tag is CommandKeyBinding)
          //e.Result = e.SortOrder == WinForms.SortOrder.Ascending && e.Column != colShortcut ? -1 : 1;
          e.Result = e.SortOrder == WinForms.SortOrder.Ascending ? -1 : 1;
        else if (e.Node1.Tag is CommandKeyBinding && e.Node2.Tag is CommandKeyFolder)
          //e.Result = e.SortOrder == WinForms.SortOrder.Ascending && e.Column != colShortcut ? 1 : -1;
          e.Result = e.SortOrder == WinForms.SortOrder.Ascending ? 1 : -1;
        else if (e.Node1.Tag is CommandKeyFolder && e.Node2.Tag is CommandKeyFolder)
        {
          CommandKeyFolder lCommandKeyFolder1 = (CommandKeyFolder)e.Node1.Tag;
          CommandKeyFolder lCommandKeyFolder2 = (CommandKeyFolder)e.Node2.Tag;
          CompareFolders(e, lCommandKeyFolder1, lCommandKeyFolder2);
        }
      }
    }
    #endregion
    #region trlsShortcuts_DragOver
    private void trlsShortcuts_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
    {
      Point lMousePosition = new Point(e.X, e.Y);
      e.Effect = GetDragDropEffect(sender as TreeList, lMousePosition, e.KeyState, GetDragNode(e.Data));
    }
    #endregion
    #region trlsShortcuts_DragDrop
    private void trlsShortcuts_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
    {
      Point mousePosition = new Point(e.X, e.Y);
      TreeListNode dragNode = GetDragNode(e.Data);
      TreeListNode targetNode = GetTargetNode(sender as TreeList, mousePosition);
      TreeListNode targetFolderNode = GetTargetFolder(targetNode, e.KeyState);

      CommandKeyFolder targetFolder = null;
      if (targetFolderNode == null)
        targetFolder = _RootFolder;
      else
        targetFolder = targetFolderNode.Tag as CommandKeyFolder;

      if (dragNode != null && targetFolder != null && dragNode.TreeList == trlsShortcuts)
      {
        if (dragNode.Tag is CommandKeyFolder) // We are dragging folder.
          MoveCommandKeyFolder(dragNode.Tag as CommandKeyFolder, targetFolder);
        else if (dragNode.Tag is CommandKeyBinding) // We are dragging a binding.
          MoveKommandKeyBinding(dragNode.Tag as CommandKeyBinding, targetFolder);
        trlsShortcuts.MoveNode(dragNode, targetFolderNode);
      }
      e.Effect = DragDropEffects.None;
    }
    #endregion
    #region trlsShortcuts_MouseMove
    private void trlsShortcuts_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      TreeList treeList = sender as TreeList;
      TreeListNode newHighlightedFolder = null;

      TreeListHitInfo hitInfo = treeList.CalcHitInfo(new Point(e.X, e.Y));
      TreeListNode nodeUnderMouse = hitInfo.Node;

      if (nodeUnderMouse != null && nodeUnderMouse.Tag != null && nodeUnderMouse.Tag is CommandKeyFolder && CursorInFolderIcon(treeList))
        newHighlightedFolder = nodeUnderMouse;

      if (_LastHighlightedFolder != newHighlightedFolder)
      {
        _LastHighlightedFolder = newHighlightedFolder;
        treeList.Refresh();
      }
    }
    #endregion

    private TreeListNode GetNodeAtPoint(Point point)
    {
      return trlsShortcuts.CalcHitInfo(point).Node;
    }
    private bool NodeIsFolder(TreeListNode node)
    {
      if (node == null)
        return false;

      return (node.Tag is CommandKeyFolder);
    }

    bool m_RightButtonDown;
    
    #region trlsShortcuts_MouseDown
    private void trlsShortcuts_MouseDown(object sender, MouseEventArgs e)
    {
      TreeListNode nodeUnderMouse = GetNodeAtPoint(new Point(e.X, e.Y));
      if (nodeUnderMouse != null && NodeIsFolder(nodeUnderMouse) && CursorInFolderIcon(trlsShortcuts))
        nodeUnderMouse.Expanded = !nodeUnderMouse.Expanded; // toggle

      m_RightButtonDown = (e.Button == MouseButtons.Right);

      // OLD CODE:
      //			TreeList lTreeList = sender as TreeList;
      //			TreeListHitInfo lHitInfo = lTreeList.CalcHitInfo(new Point(e.X, e.Y));
      //			TreeListNode lNodeUnderMouse = lHitInfo.Node;
      //			if (lNodeUnderMouse != null && lNodeUnderMouse.Tag != null && lNodeUnderMouse.Tag is CommandKeyFolder)
      //			{
      //				if (CursorInFolderIcon(lTreeList))
      //				{
      //					if (!lNodeUnderMouse.Expanded)
      //						lNodeUnderMouse.Expanded = true;					
      //					else
      //						lNodeUnderMouse.Expanded = false;
      //				}	
      //			}
    }
    #endregion
    private void trlsShortcuts_MouseUp(object sender, MouseEventArgs e)
    {
      if (m_RightButtonDown)
      {
        TreeListNode nodeUnderMouse = GetNodeAtPoint(new Point(e.X, e.Y));
        if (nodeUnderMouse != null)
        {
          if (nodeUnderMouse.Tag is CommandKeyFolder)
            btnCollapseFolders.Visibility = BarItemVisibility.Always;
          else
            btnCollapseFolders.Visibility = BarItemVisibility.Never;

          trlsShortcuts.FocusedNode = nodeUnderMouse;
          pmTreeList.ShowPopup(trlsShortcuts.PointToScreen(new Point(e.X, e.Y)));
        }

        m_RightButtonDown = false;
      }
    }
    #region trlsShortcuts_AfterExpand
    private void trlsShortcuts_AfterExpand(object sender, NodeEventArgs e)
    {
      TreeListNode lNode = e.Node;
      if (lNode == null)
        return;
      if (lNode.Tag is CommandKeyFolder)
      {
        CommandKeyFolder lFolder = (CommandKeyFolder)lNode.Tag;
        lFolder.FolderState = FolderState.Expanded;
      }
    }
    #endregion
    #region trlsShortcuts_AfterCollapse
    private void trlsShortcuts_AfterCollapse(object sender, NodeEventArgs e)
    {
      TreeListNode lNode = e.Node;
      if (lNode == null)
        return;
      if (lNode.Tag is CommandKeyFolder)
      {
        CommandKeyFolder lFolder = (CommandKeyFolder)lNode.Tag;
        lFolder.FolderState = FolderState.Collapsed;
      }
    }
    #endregion
    #region trlsShortcuts_KeyDown
    private void trlsShortcuts_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete)
        Delete();
    }
    #endregion

    #region BindingDataChanged
    private void BindingDataChanged(object sender, System.EventArgs e)
    {
      TransferControlsToCurrentBinding();
    }
    #endregion

    #region pmTreeList_Popup
    private void pmTreeList_Popup(object sender, System.EventArgs e)
    {
      using (DecoupledStorage lStorage = CodeRush.Options.GetStorage("Tool Windows", "Tutorial"))
      {
        bool lVisible = lStorage.ReadBoolean("Config", "ShowEditMenus", true);
        if (lVisible)
          btnCopyBindingLinkfordocumentation.Visibility = BarItemVisibility.Always;
        else
          btnCopyBindingLinkfordocumentation.Visibility = BarItemVisibility.Never;
      }
    }
    #endregion

    // buttons
    #region btnNewFolder_Click
    private void btnNewFolder_ItemClick(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      try
      {
        CreateFolder();
      }
      catch (Exception ex)
      {
        Log.SendException(ex);
      }
    }
    #endregion
    #region btnRenameFolder_ItemClick
    private void btnRenameFolder_ItemClick(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      RenameFolder();
    }
    #endregion
    #region btnNewKeyboardShortcut_ItemClick
    private void btnNewKeyboardShortcut_ItemClick(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      CreateShortcut();
    }
    #endregion

    #region btnDelete_ItemClick
    private void btnDelete_ItemClick(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      Delete();
    }
    #endregion
    #region btnToggleFolders_DownChanged
    private void btnToggleFolders_DownChanged(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      if (btnToggleFolders.Down)
        RefreshDisplayMode(DisplayMode.Plain);
      else
        RefreshDisplayMode(DisplayMode.Tree);
    }
    #endregion
    private void btnCollapseFolders_ItemClick(object sender, ItemClickEventArgs e)
    {
      trlsShortcuts.CollapseAll();
    }
    #region btnDuplicate_ItemClick
    private void btnDuplicate_ItemClick(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      CreateShortcut(_CurrentBinding);
    }
    #endregion
    #region btnCopyBindingSummaryfortechsupport_ItemClick
    private void btnCopyBindingSummaryfortechsupport_ItemClick(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      if (CurrentBinding == null)
        return;

      StringBuilder summary = new StringBuilder();

      summary.AppendFormat("Binding: {0}\r\n", CurrentBinding.DisplayShortcut);
      summary.AppendFormat("Command: {0}\r\n", CurrentBinding.Command);
      summary.AppendFormat("Parameters: {0}\r\n", CurrentBinding.Parameters);
      summary.AppendFormat("Comments: {0}\r\n", CurrentBinding.Comments);
      summary.AppendFormat("Context: {0}\r\n", CurrentBinding.ReadableContext);
      summary.AppendFormat("Enabled: {0}\r\n", CurrentBinding.Enabled);

      string summaryStr = summary.ToString();
      Clipboard.SetDataObject(summaryStr, true);
    }
    #endregion
    private void btnFind_ItemClick(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      ShortcutsPlugIn.EnableActionExecuting = false;
      try
      {
        frmFind.Execute(trlsShortcuts);
      }
      finally
      {
        ShortcutsPlugIn.EnableActionExecuting = true;
      }
    }
    private void btnCopyContext_ItemClick(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      CopyContext();
    }
    private void btnPasteContext_ItemClick(object sender, DevExpress.DXCore.Controls.XtraBars.ItemClickEventArgs e)
    {
      PasteContext();
    }

    #region chkEnabled_Click
    private void chkEnabled_Click(object sender, System.EventArgs e)
    {
      TransferControlsToCurrentBinding();
      EnableBindingControls(chkBindingEnabled.Checked);
    }
    #endregion
    #region chkFolderEnabled_Click
    private void chkFolderEnabled_Click(object sender, System.EventArgs e)
    {
      TransferControlsToCurrentFolder();
      EnableFolderControls(chkFolderEnabled.Checked);
    }
    #endregion

    // Option Page
    private TreeListNode FindTreeListNodeToFocus(TreeListNodes nodes, string commandToMatch, string customDataToMatch)
    {
      if (nodes == null)
        return null;
      foreach (TreeListNode node in nodes)
      {
        if (node.Tag is CommandKeyBinding)
        {
          CommandKeyBinding commandKeyBinding = (CommandKeyBinding)node.Tag;
          if (commandKeyBinding.Command == commandToMatch && commandKeyBinding.Parameters == "")
          {
            if (string.IsNullOrEmpty(customDataToMatch) || commandKeyBinding.CustomData == customDataToMatch)
              return node;
          }
          else if (commandKeyBinding.Command + "(" + commandKeyBinding.Parameters + ")" == commandToMatch)
            return node;
        }
        else if (node.Tag is CommandKeyFolder)
        {
          if ((node.Tag as CommandKeyFolder).Name == commandToMatch)
            return node;
          TreeListNode lTreeListNodeToFocus = FindTreeListNodeToFocus(node.Nodes, commandToMatch, customDataToMatch);
          if (lTreeListNodeToFocus != null)
            return lTreeListNodeToFocus;
        }
      }
      return null;
    }
    private TreeListNode FindTreeListNodeToFocus(string searchString)
    {
      string[] parameters = searchString.Split(';');
      string firstKeyName = string.Empty;
      if (parameters.Length > 1)
      {
        searchString = parameters[0];
        firstKeyName = parameters[1];
      }

      return FindTreeListNodeToFocus(trlsShortcuts.Nodes, searchString, firstKeyName);
    }
    #region OptShortcuts_CustomInitialization
    private void OptShortcuts_CustomInitialization(object sender, CustomInitializationEventArgs ea)
    {
      //MessageBox.Show(ea.InitStr, "ea.InitStr");

      TreeListNode lTreeListNodeToFocus = FindTreeListNodeToFocus(ea.InitStr);

      if (lTreeListNodeToFocus != null)
      {
        trlsShortcuts.FocusedNode = lTreeListNodeToFocus;
        if (lTreeListNodeToFocus.Tag is CommandKeyBinding)
        {
          edBinding.CustomInputControl.FocusPrimaryEditControl();
        }
      }
      else
      {
        string initStr = ea.InitStr;
        if (!CodeRush.StrUtil.IsNullOrEmpty(initStr))
        {
          string title = "Confirm";		// Translate
          string message = String.Format("A shortcut for the \"{0}\" command does not yet exist. Would you like to create a new shortcut for this command?", initStr);		// Translate
          if (MessageBox.Show(CodeRush.IDE, message, title, MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            int openParen = initStr.IndexOf("(");
            int closeParen = initStr.IndexOf(")");
            if (openParen >= 0 && closeParen >= 0 && closeParen > openParen)
            {
              string command = initStr.Substring(0, openParen);
              string parameters = initStr.Substring(openParen + 1, closeParen - openParen - 1);
              CreateShortcutForCommand(command, parameters);
            }
            else
              CreateShortcutForCommand(initStr);
          }
        }
      }
    }
    #endregion
    #region OptShortcuts_PreparePage
    private void OptShortcuts_PreparePage(object sender, OptionsPageStorageEventArgs e)
    {
      Reload(e.Storage);
    }
    #endregion
    #region OptShortcuts_CommitChanges
    private void OptShortcuts_CommitChanges(object sender, OptionsPageStorageEventArgs e)
    {
      TransferControlsToCurrentBinding();
      TransferControlsToCurrentFolder();

      _RootFolder.Save(Category, PageName, CurrentBinding);

      using (DecoupledStorage storage = Storage)
        storage.WriteBoolean("Global", "SeparateAltKeys", chkbSeparateAltKeys.Checked);

      _SeparateAltKeys = chkbSeparateAltKeys.Checked;

      ExcludeChangedShortcutsFromFeatures();
      _ChangedShortcuts.Clear();
    }
    #endregion

    private void OptShortcuts_CancelChanges(object sender, OptionsPageStorageEventArgs ea)
    {
      _ChangedShortcuts.Clear();
    }

    private void edBinding_CustomInputChanged(object sender, CR_XkeysEngine.CustomInputChangedEventArgs ea)
    {
      if (_UpdatingInternally)
        return;
      if (_CurrentBinding == null)
        return;

      _CurrentBinding.AltKeyDown = ea.Shortcut.AltKeyDown;
      _CurrentBinding.CtrlKeyDown = ea.Shortcut.CtrlKeyDown;
      _CurrentBinding.AnyShiftModifier = ea.Shortcut.AnyShiftModifier;
      _CurrentBinding.ShiftKeyDown = ea.Shortcut.ShiftKeyDown;
      _CurrentBinding.CustomData = ea.Shortcut.CustomData;
      trlsShortcuts.Invalidate();
      lblKeyName.Invalidate();
    }
    protected override void HandleOptionsChanged(OptionsPageStorageEventArgs ea)
    {
      try
      {
        Reload(ea.Storage);
      }
      catch (Exception ex)
      {
        Log.SendException(ex);
      }
    }

    private void Reload(DecoupledStorage decoupledStorage)
    {
      _Loading = true;
      try
      {
        _ChangedShortcuts.Clear();

        using (DecoupledStorage storage = decoupledStorage)
          chkbSeparateAltKeys.Checked = storage.ReadBoolean("Global", "SeparateAltKeys", false);

        CodeRush.Context.PopulateContextPicker(ContextPicker1, this.LanguageID);

        _RootFolder = new CommandKeyFolder();
        CommandKeyBinding lastBindingSelected = _RootFolder.Load(Category, PageName);

        FillTreeList(lastBindingSelected);

        LoadCommands();
      }
      finally
      {
        _Loading = false;
      }
    }

    // private properties ...

    #region CurrentBinding
    private CommandKeyBinding CurrentBinding
    {
      get
      {
        return _CurrentBinding;
      }

      set
      {
        _CurrentBinding = value;
        TransferCurrentBindingToControls();
      }
    }
    #endregion
    #region CurrentFolder
    private CommandKeyFolder CurrentFolder
    {
      get
      {
        return _CurrentFolder;
      }

      set
      {
        _CurrentFolder = value;
        TransferCurrentFolderToControls();
      }
    }
    #endregion

    // public properties ...
    #region RootFileName
    public static string RootFileName
    {
      get
      {
        return STR_RootFileName;
      }
    }
    #endregion
    #region SeparateAltKeys
    public static bool SeparateAltKeys
    {
      get
      {
        if (!_SeparateAltKeysLoaded)
        {
          using (DecoupledStorage storage = Storage)
            _SeparateAltKeys = storage.ReadBoolean("Global", "SeparateAltKeys", false);
          _SeparateAltKeysLoaded = true;
        }

        return _SeparateAltKeys;
      }
    }
    #endregion

    private void linkLayout_OpenLink(object sender, DevExpress.DXCore.Controls.XtraEditors.Controls.OpenLinkEventArgs e)
    {
      CodeRush.Options.Show(typeof(OptXkeysLayout));
    }

    private void CodeRushEvents_OptionsChanged(OptionsChangedEventArgs ea)
    {
      if (ea.OptionsPages.Contains(typeof(OptXkeysLayout)))
        edBinding.CustomInputControl.XkeyLayout.Load();
    }

  }
}