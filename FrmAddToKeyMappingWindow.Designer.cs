namespace DX_ShortcutEngine
{
  partial class FrmAddToKeyMappingWindow
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
      this.cmbCommands = new DevExpress.DXCore.Controls.XtraEditors.ComboBoxEdit();
      this.lblCommand = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.cmbParentGroups = new DevExpress.DXCore.Controls.XtraEditors.ComboBoxEdit();
      this.label2 = new System.Windows.Forms.Label();
      this.tbxDescription = new System.Windows.Forms.TextBox();
      this.btnAdd = new DevExpress.DXCore.Controls.XtraEditors.SimpleButton();
      this.btnCancel = new DevExpress.DXCore.Controls.XtraEditors.SimpleButton();
      this.tbxShortcut = new System.Windows.Forms.TextBox();
      this.lblShortcut = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.cmbCommands.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cmbParentGroups.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // cmbCommands
      // 
      this.cmbCommands.EditValue = "";
      this.cmbCommands.Location = new System.Drawing.Point(91, 12);
      this.cmbCommands.Name = "cmbCommands";
      this.cmbCommands.Properties.Buttons.AddRange(new DevExpress.DXCore.Controls.XtraEditors.Controls.EditorButton[] {
            new DevExpress.DXCore.Controls.XtraEditors.Controls.EditorButton(DevExpress.DXCore.Controls.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.cmbCommands.Properties.CycleOnDblClick = false;
      this.cmbCommands.Properties.DropDownRows = 24;
      this.cmbCommands.Size = new System.Drawing.Size(282, 20);
      this.cmbCommands.TabIndex = 0;
      // 
      // lblCommand
      // 
      this.lblCommand.AutoSize = true;
      this.lblCommand.Location = new System.Drawing.Point(12, 15);
      this.lblCommand.Name = "lblCommand";
      this.lblCommand.Size = new System.Drawing.Size(57, 13);
      this.lblCommand.TabIndex = 10;
      this.lblCommand.Text = "Command:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 93);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(71, 13);
      this.label1.TabIndex = 12;
      this.label1.Text = "Parent group:";
      // 
      // cmbParentGroups
      // 
      this.cmbParentGroups.EditValue = "";
      this.cmbParentGroups.Location = new System.Drawing.Point(91, 90);
      this.cmbParentGroups.Name = "cmbParentGroups";
      this.cmbParentGroups.Properties.Buttons.AddRange(new DevExpress.DXCore.Controls.XtraEditors.Controls.EditorButton[] {
            new DevExpress.DXCore.Controls.XtraEditors.Controls.EditorButton(DevExpress.DXCore.Controls.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.cmbParentGroups.Properties.CycleOnDblClick = false;
      this.cmbParentGroups.Properties.DropDownRows = 24;
      this.cmbParentGroups.Size = new System.Drawing.Size(282, 20);
      this.cmbParentGroups.TabIndex = 2;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 38);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(63, 13);
      this.label2.TabIndex = 13;
      this.label2.Text = "Description:";
      // 
      // tbxDescription
      // 
      this.tbxDescription.Location = new System.Drawing.Point(91, 38);
      this.tbxDescription.Name = "tbxDescription";
      this.tbxDescription.Size = new System.Drawing.Size(282, 20);
      this.tbxDescription.TabIndex = 1;
      // 
      // btnAdd
      // 
      this.btnAdd.Location = new System.Drawing.Point(217, 125);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(75, 23);
      this.btnAdd.TabIndex = 3;
      this.btnAdd.Text = "&Add";
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(298, 125);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // tbxShortcut
      // 
      this.tbxShortcut.Enabled = false;
      this.tbxShortcut.Location = new System.Drawing.Point(91, 64);
      this.tbxShortcut.Name = "tbxShortcut";
      this.tbxShortcut.Size = new System.Drawing.Size(282, 20);
      this.tbxShortcut.TabIndex = 18;
      // 
      // lblShortcut
      // 
      this.lblShortcut.AutoSize = true;
      this.lblShortcut.Location = new System.Drawing.Point(12, 64);
      this.lblShortcut.Name = "lblShortcut";
      this.lblShortcut.Size = new System.Drawing.Size(50, 13);
      this.lblShortcut.TabIndex = 17;
      this.lblShortcut.Text = "Shortcut:";
      // 
      // FrmAddToKeyMappingWindow
      // 
      this.AcceptButton = this.btnAdd;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(382, 158);
      this.Controls.Add(this.tbxShortcut);
      this.Controls.Add(this.lblShortcut);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.tbxDescription);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cmbParentGroups);
      this.Controls.Add(this.lblCommand);
      this.Controls.Add(this.cmbCommands);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmAddToKeyMappingWindow";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Adding Shortcut to Key Mapping Window";
      this.TopMost = true;
      ((System.ComponentModel.ISupportInitialize)(this.cmbCommands.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cmbParentGroups.Properties)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private DevExpress.DXCore.Controls.XtraEditors.ComboBoxEdit cmbCommands;
    private System.Windows.Forms.Label lblCommand;
    private System.Windows.Forms.Label label1;
    private DevExpress.DXCore.Controls.XtraEditors.ComboBoxEdit cmbParentGroups;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox tbxDescription;
    private DevExpress.DXCore.Controls.XtraEditors.SimpleButton btnAdd;
    private DevExpress.DXCore.Controls.XtraEditors.SimpleButton btnCancel;
    private System.Windows.Forms.TextBox tbxShortcut;
    private System.Windows.Forms.Label lblShortcut;
  }
}