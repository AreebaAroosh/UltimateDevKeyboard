using System;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  partial class OptXkeysLayout
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public OptXkeysLayout()
    {
      /// <summary>
      /// Required for Windows.Forms Class Composition Designer support
      /// </summary>
      InitializeComponent();
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptXkeysLayout));
      this.chkWide = new DevExpress.DXCore.Controls.XtraEditors.CheckButton();
      this.chkSquareButton = new DevExpress.DXCore.Controls.XtraEditors.CheckButton();
      this.chkTall = new DevExpress.DXCore.Controls.XtraEditors.CheckButton();
      this.pnlKeyPreviewContainer = new System.Windows.Forms.Panel();
      this.pnlKeyPreview = new CR_XkeysEngine.FlickerFreeFocusPanel();
      this.chkBlocker = new DevExpress.DXCore.Controls.XtraEditors.CheckButton();
      this.label1 = new System.Windows.Forms.Label();
      this.btnAutoDetectBlockers = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.txtKeyName = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.dxCoreEvents1 = new DevExpress.DXCore.PlugInCore.DXCoreEvents(this.components);
      this.pnlLayoutOptions = new System.Windows.Forms.Panel();
      this.chkKeyRepeatsifHeldDown = new System.Windows.Forms.CheckBox();
      this.linkShortcuts = new DevExpress.DXCore.Controls.XtraEditors.HyperLinkEdit();
      ((System.ComponentModel.ISupportInitialize)(this.Images16x16)).BeginInit();
      this.pnlKeyPreviewContainer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dxCoreEvents1)).BeginInit();
      this.pnlLayoutOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.linkShortcuts.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      this.SuspendLayout();
      // 
      // chkWide
      // 
      this.chkWide.Enabled = false;
      this.chkWide.Location = new System.Drawing.Point(3, 129);
      this.chkWide.Name = "chkWide";
      this.chkWide.Size = new System.Drawing.Size(100, 50);
      this.chkWide.TabIndex = 3;
      this.chkWide.Text = "&Wide";
      this.toolTip1.SetToolTip(this.chkWide, "Convert between two adjacent keys and a wide key.");
      this.chkWide.CheckedChanged += new System.EventHandler(this.chkWide_CheckedChanged);
      // 
      // chkSquareButton
      // 
      this.chkSquareButton.Enabled = false;
      this.chkSquareButton.Location = new System.Drawing.Point(62, 21);
      this.chkSquareButton.Name = "chkSquareButton";
      this.chkSquareButton.Size = new System.Drawing.Size(100, 100);
      this.chkSquareButton.TabIndex = 2;
      this.chkSquareButton.Text = "&Square";
      this.chkSquareButton.CheckedChanged += new System.EventHandler(this.chkSquareButton_CheckedChanged);
      // 
      // chkTall
      // 
      this.chkTall.Enabled = false;
      this.chkTall.Location = new System.Drawing.Point(3, 21);
      this.chkTall.Name = "chkTall";
      this.chkTall.Size = new System.Drawing.Size(50, 100);
      this.chkTall.TabIndex = 1;
      this.chkTall.Text = "&Tall";
      this.chkTall.ToolTip = "Convert between two stacked keys and a tall key.";
      this.chkTall.CheckedChanged += new System.EventHandler(this.chkTall_CheckedChanged);
      // 
      // pnlKeyPreviewContainer
      // 
      this.pnlKeyPreviewContainer.BackColor = System.Drawing.Color.DimGray;
      this.pnlKeyPreviewContainer.Controls.Add(this.pnlKeyPreview);
      this.pnlKeyPreviewContainer.Location = new System.Drawing.Point(3, 69);
      this.pnlKeyPreviewContainer.Name = "pnlKeyPreviewContainer";
      this.pnlKeyPreviewContainer.Size = new System.Drawing.Size(357, 296);
      this.pnlKeyPreviewContainer.TabIndex = 1;
      // 
      // pnlKeyPreview
      // 
      this.pnlKeyPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlKeyPreview.Location = new System.Drawing.Point(5, 5);
      this.pnlKeyPreview.Name = "pnlKeyPreview";
      this.pnlKeyPreview.Size = new System.Drawing.Size(346, 285);
      this.pnlKeyPreview.TabIndex = 0;
      this.pnlKeyPreview.TabStop = true;
      this.pnlKeyPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlKeyPreview_Paint);
      this.pnlKeyPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlKeyPreview_MouseDown);
      this.pnlKeyPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlKeyPreview_MouseMove);
      // 
      // chkBlocker
      // 
      this.chkBlocker.Enabled = false;
      this.chkBlocker.Location = new System.Drawing.Point(112, 129);
      this.chkBlocker.Name = "chkBlocker";
      this.chkBlocker.Size = new System.Drawing.Size(50, 50);
      this.chkBlocker.TabIndex = 4;
      this.chkBlocker.Text = "&Blocker";
      this.chkBlocker.CheckedChanged += new System.EventHandler(this.chkBlocker_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(346, 52);
      this.label1.TabIndex = 0;
      this.label1.Text = resources.GetString("label1.Text");
      // 
      // btnAutoDetectBlockers
      // 
      this.btnAutoDetectBlockers.Location = new System.Drawing.Point(40, 271);
      this.btnAutoDetectBlockers.Name = "btnAutoDetectBlockers";
      this.btnAutoDetectBlockers.Size = new System.Drawing.Size(122, 33);
      this.btnAutoDetectBlockers.TabIndex = 8;
      this.btnAutoDetectBlockers.Text = "&Auto-detect Blockers";
      this.btnAutoDetectBlockers.UseVisualStyleBackColor = true;
      this.btnAutoDetectBlockers.Click += new System.EventHandler(this.btnAutoDetectBlockers_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(40, 308);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(122, 65);
      this.label2.TabIndex = 9;
      this.label2.Text = "Auto-detect works by \r\ndetecting the keys that \r\nare currently down and \r\nmarking" +
    " them all as \r\nkey blockers.";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(0, 193);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(59, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Key &Name:";
      // 
      // txtKeyName
      // 
      this.txtKeyName.Enabled = false;
      this.txtKeyName.Location = new System.Drawing.Point(2, 209);
      this.txtKeyName.Name = "txtKeyName";
      this.txtKeyName.Size = new System.Drawing.Size(159, 21);
      this.txtKeyName.TabIndex = 6;
      this.toolTip1.SetToolTip(this.txtKeyName, "Enter the name of the selected key.");
      this.txtKeyName.TextChanged += new System.EventHandler(this.txtKeyName_TextChanged);
      this.txtKeyName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyName_KeyDown);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(0, 2);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(84, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Layout Options:";
      // 
      // toolTip1
      // 
      this.toolTip1.ShowAlways = true;
      // 
      // dxCoreEvents1
      // 
      this.dxCoreEvents1.OptionsChanged += new DevExpress.CodeRush.Core.OptionsChangedEventHandler(this.dxCoreEvents1_OptionsChanged);
      // 
      // pnlLayoutOptions
      // 
      this.pnlLayoutOptions.Controls.Add(this.chkKeyRepeatsifHeldDown);
      this.pnlLayoutOptions.Controls.Add(this.chkTall);
      this.pnlLayoutOptions.Controls.Add(this.label4);
      this.pnlLayoutOptions.Controls.Add(this.chkWide);
      this.pnlLayoutOptions.Controls.Add(this.txtKeyName);
      this.pnlLayoutOptions.Controls.Add(this.chkSquareButton);
      this.pnlLayoutOptions.Controls.Add(this.label3);
      this.pnlLayoutOptions.Controls.Add(this.chkBlocker);
      this.pnlLayoutOptions.Controls.Add(this.label2);
      this.pnlLayoutOptions.Controls.Add(this.btnAutoDetectBlockers);
      this.pnlLayoutOptions.Location = new System.Drawing.Point(366, 31);
      this.pnlLayoutOptions.Name = "pnlLayoutOptions";
      this.pnlLayoutOptions.Size = new System.Drawing.Size(168, 409);
      this.pnlLayoutOptions.TabIndex = 2;
      // 
      // chkKeyRepeatsifHeldDown
      // 
      this.chkKeyRepeatsifHeldDown.AutoSize = true;
      this.chkKeyRepeatsifHeldDown.Enabled = false;
      this.chkKeyRepeatsifHeldDown.Location = new System.Drawing.Point(4, 240);
      this.chkKeyRepeatsifHeldDown.Name = "chkKeyRepeatsifHeldDown";
      this.chkKeyRepeatsifHeldDown.Size = new System.Drawing.Size(148, 17);
      this.chkKeyRepeatsifHeldDown.TabIndex = 0;
      this.chkKeyRepeatsifHeldDown.Text = "Key &Repeats if held down";
      this.chkKeyRepeatsifHeldDown.UseVisualStyleBackColor = true;
      this.chkKeyRepeatsifHeldDown.CheckedChanged += new System.EventHandler(this.chkKeyRepeatsifHeldDown_CheckedChanged);
      // 
      // linkShortcuts
      // 
      this.linkShortcuts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.linkShortcuts.EditValue = "Shortcuts";
      this.linkShortcuts.Location = new System.Drawing.Point(478, 459);
      this.linkShortcuts.Name = "linkShortcuts";
      this.linkShortcuts.Properties.BorderStyle = DevExpress.DXCore.Controls.XtraEditors.Controls.BorderStyles.NoBorder;
      this.linkShortcuts.Size = new System.Drawing.Size(50, 18);
      this.linkShortcuts.TabIndex = 3;
      this.linkShortcuts.OpenLink += new DevExpress.DXCore.Controls.XtraEditors.Controls.OpenLinkEventHandler(this.linkShortcuts_OpenLink);
      // 
      // OptXkeysLayout
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.Controls.Add(this.linkShortcuts);
      this.Controls.Add(this.pnlLayoutOptions);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.pnlKeyPreviewContainer);
      this.Name = "OptXkeysLayout";
      this.CommitChanges += new DevExpress.CodeRush.Core.OptionsPage.CommitChangesEventHandler(this.OptXkeysLayout_CommitChanges);
      this.PreparePage += new DevExpress.CodeRush.Core.OptionsPage.PreparePageEventHandler(this.OptXkeysLayout_PreparePage);
      this.RestoreDefaults += new DevExpress.CodeRush.Core.OptionsPage.RestoreDefaultsEventHandler(this.OptXkeysLayout_RestoreDefaults);
      this.Resize += new System.EventHandler(this.OptXkeysLayout_Resize);
      ((System.ComponentModel.ISupportInitialize)(this.Images16x16)).EndInit();
      this.pnlKeyPreviewContainer.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dxCoreEvents1)).EndInit();
      this.pnlLayoutOptions.ResumeLayout(false);
      this.pnlLayoutOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.linkShortcuts.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    ///
    /// Gets a DecoupledStorage instance for this options page.
    ///
    public static DecoupledStorage Storage
    {
      get
      {
        return DevExpress.CodeRush.Core.CodeRush.Options.GetStorage(GetCategory(), GetPageName());
      }
    }
    ///
    /// Returns the category of this options page.
    ///
    public override string Category
    {
      get
      {
        return OptXkeysLayout.GetCategory();
      }
    }
    ///
    /// Returns the page name of this options page.
    ///
    public override string PageName
    {
      get
      {
        return OptXkeysLayout.GetPageName();
      }
    }
    ///
    /// Returns the full path (Category + PageName) of this options page.
    ///
    public static string FullPath
    {
      get
      {
        return GetCategory() + "\\" + GetPageName();
      }
    }

    ///
    /// Displays the DXCore options dialog and selects this page.
    ///
    public new static void Show()
    {
      DevExpress.CodeRush.Core.CodeRush.Command.Execute("Options", FullPath);
    }

    private FlickerFreeFocusPanel pnlKeyPreview;
    private DevExpress.DXCore.Controls.XtraEditors.CheckButton chkWide;
    private DevExpress.DXCore.Controls.XtraEditors.CheckButton chkSquareButton;
    private DevExpress.DXCore.Controls.XtraEditors.CheckButton chkTall;
    private System.Windows.Forms.Panel pnlKeyPreviewContainer;
    private DevExpress.DXCore.Controls.XtraEditors.CheckButton chkBlocker;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnAutoDetectBlockers;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtKeyName;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ToolTip toolTip1;
    private DevExpress.DXCore.PlugInCore.DXCoreEvents dxCoreEvents1;
    private System.Windows.Forms.Panel pnlLayoutOptions;
    private DevExpress.DXCore.Controls.XtraEditors.HyperLinkEdit linkShortcuts;
    private System.Windows.Forms.CheckBox chkKeyRepeatsifHeldDown;
  }
}