using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DevExpress.CodeRush.PlugInCore;
using System.Collections.Generic;

namespace CR_XkeysEngine
{
  public class XkeysEditControl : DXCoreUserControl
  {
    #region private fields...
    bool _UpdatingInternally;
    private DevExpress.DXCore.Controls.XtraEditors.CheckButton btnShift;
    private DevExpress.DXCore.Controls.XtraEditors.CheckButton btnAlt;
    private DevExpress.DXCore.Controls.XtraEditors.CheckButton btnCtrl;
    private System.Windows.Forms.Label lblXKeysPlus;
    private System.Windows.Forms.Label lblShiftKey;
    private System.Windows.Forms.Label lblXKeys;
    private System.ComponentModel.Container components = null;
    
    string customData;
    KeyLayout xkeyLayout;
    XkeysEngine xkeysEngine;

    private FlickerFreeFocusPanel xkeyPreview;

    CustomInputShortcut _Shortcut;
    private DevExpress.DXCore.Controls.XtraEditors.CheckButton chkAny;

    XkeysPainter xkeysPainter = new XkeysPainter();
    #endregion

    void OnCustomInputChangedAndFocused()
    {
      if (!xkeyPreview.Focused)
        return;
      OnCustomInputChanged(ShortcutFromControls());

      xkeysPainter.ClearKeySelection();
      List<KeyBase> pressedKeys = xkeyLayout.GetPressedKeys(_Shortcut.CustomData);
      foreach (KeyBase pressedKey in pressedKeys)
        xkeysPainter.AddKeyToSelection(pressedKey);

      xkeyPreview.Invalidate();
    }

    void xkeysEngine_XkeyPressed(object sender, XkeyPressedEventArgs e)
    {
      if (!IsHandleCreated)
        return;
      
      if (e.KeysDown.Count == 0)
        return;   // No keys pressed.

      bool shiftDown = e.ShiftKeyDown;
      bool ctrlDown = e.CtrlKeyDown;
      bool altDown = e.AltKeyDown;
      customData = e.CustomData;
      this.Invoke((MethodInvoker)delegate
      {
        AltKeyDown = altDown;
        CtrlKeyDown = ctrlDown;
        ShiftKeyDown = shiftDown;
        OnCustomInputChangedAndFocused();
      });
    }

    
    #region XKeysEditControl
    public XkeysEditControl()
    {
      InitializeComponent();

      xkeysEngine = new XkeysEngine();
      xkeysEngine.XkeyPressed += xkeysEngine_XkeyPressed;

      _Shortcut = new CustomInputShortcut();
      XkeysRaw.Data.DataChanged += Data_DataChanged;
      xkeysPainter.SetKeySizeToFit(xkeyPreview.ClientRectangle.Size);
      xkeyLayout = xkeysPainter.GetLayout(XkeysRaw.Data.LastKeyCode, XkeysRaw.Data.BlockedKeysMask);
      xkeyLayout.Load();
    }
    #endregion

    void Data_DataChanged(object sender, EventArgs e)
    {
      xkeyPreview.Invalidate();
    }
    
    public void ClearData()
    {
      SetCustomData("");
      btnCtrl.Checked = false;
      btnAlt.Checked = false;
      btnShift.Checked = false;
    }

    #region Dispose
    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if (_Shortcut != null)
          _Shortcut = null;
        if(components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }
    #endregion

    #region Component Designer generated code
    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.btnShift = new DevExpress.DXCore.Controls.XtraEditors.CheckButton();
      this.btnAlt = new DevExpress.DXCore.Controls.XtraEditors.CheckButton();
      this.btnCtrl = new DevExpress.DXCore.Controls.XtraEditors.CheckButton();
      this.lblXKeysPlus = new System.Windows.Forms.Label();
      this.lblShiftKey = new System.Windows.Forms.Label();
      this.lblXKeys = new System.Windows.Forms.Label();
      this.xkeyPreview = new CR_XkeysEngine.FlickerFreeFocusPanel();
      this.chkAny = new DevExpress.DXCore.Controls.XtraEditors.CheckButton();
      this.SuspendLayout();
      // 
      // btnShift
      // 
      this.btnShift.Location = new System.Drawing.Point(152, 5);
      this.btnShift.Name = "btnShift";
      this.btnShift.Size = new System.Drawing.Size(48, 24);
      this.btnShift.TabIndex = 11;
      this.btnShift.Text = "Shift";
      this.btnShift.CheckedChanged += new System.EventHandler(this.BindingDataChanged);
      // 
      // btnAlt
      // 
      this.btnAlt.Location = new System.Drawing.Point(104, 5);
      this.btnAlt.Name = "btnAlt";
      this.btnAlt.Size = new System.Drawing.Size(48, 24);
      this.btnAlt.TabIndex = 10;
      this.btnAlt.Text = "Alt";
      this.btnAlt.CheckedChanged += new System.EventHandler(this.BindingDataChanged);
      // 
      // btnCtrl
      // 
      this.btnCtrl.Location = new System.Drawing.Point(56, 5);
      this.btnCtrl.Name = "btnCtrl";
      this.btnCtrl.Size = new System.Drawing.Size(48, 24);
      this.btnCtrl.TabIndex = 9;
      this.btnCtrl.Text = "Ctrl";
      this.btnCtrl.CheckedChanged += new System.EventHandler(this.BindingDataChanged);
      // 
      // lblXKeysPlus
      // 
      this.lblXKeysPlus.ForeColor = System.Drawing.SystemColors.GrayText;
      this.lblXKeysPlus.Location = new System.Drawing.Point(20, 23);
      this.lblXKeysPlus.Name = "lblXKeysPlus";
      this.lblXKeysPlus.Size = new System.Drawing.Size(32, 16);
      this.lblXKeysPlus.TabIndex = 12;
      this.lblXKeysPlus.Text = "+";
      // 
      // lblShiftKey
      // 
      this.lblShiftKey.Location = new System.Drawing.Point(2, 10);
      this.lblShiftKey.Name = "lblShiftKey";
      this.lblShiftKey.Size = new System.Drawing.Size(76, 16);
      this.lblShiftKey.TabIndex = 8;
      this.lblShiftKey.Text = "Shift Key:";
      // 
      // lblXKeys
      // 
      this.lblXKeys.Location = new System.Drawing.Point(8, 36);
      this.lblXKeys.Name = "lblXKeys";
      this.lblXKeys.Size = new System.Drawing.Size(76, 16);
      this.lblXKeys.TabIndex = 13;
      this.lblXKeys.Text = "X-keys:";
      // 
      // xkeyPreview
      // 
      this.xkeyPreview.Location = new System.Drawing.Point(54, 35);
      this.xkeyPreview.Name = "xkeyPreview";
      this.xkeyPreview.Size = new System.Drawing.Size(214, 138);
      this.xkeyPreview.TabIndex = 14;
      this.xkeyPreview.TabStop = true;
      this.xkeyPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.xkeyPreview_Paint);
      // 
      // chkAny
      // 
      this.chkAny.Location = new System.Drawing.Point(217, 5);
      this.chkAny.Name = "chkAny";
      this.chkAny.Size = new System.Drawing.Size(48, 24);
      this.chkAny.TabIndex = 15;
      this.chkAny.Text = "Any";
      this.chkAny.CheckedChanged += new System.EventHandler(this.chkAny_CheckedChanged);
      // 
      // XkeysEditControl
      // 
      this.Controls.Add(this.chkAny);
      this.Controls.Add(this.xkeyPreview);
      this.Controls.Add(this.btnShift);
      this.Controls.Add(this.btnAlt);
      this.Controls.Add(this.btnCtrl);
      this.Controls.Add(this.lblShiftKey);
      this.Controls.Add(this.lblXKeys);
      this.Controls.Add(this.lblXKeysPlus);
      this.Name = "XkeysEditControl";
      this.Size = new System.Drawing.Size(268, 173);
      this.ResumeLayout(false);

    }
    #endregion

    bool CustomDataIsInvalid()
    {
      // TODO: implement this:
      return false;
    }
    
    public string GetCustomData()
    {
      return customData;
    }

    public void SetCustomData(string customData)
    {
      this.customData = customData;
      xkeysPainter.ClearKeySelection();
      List<KeyBase> pressedKeys = xkeyLayout.GetPressedKeys(customData);
      foreach (KeyBase pressedKey in pressedKeys)
        xkeysPainter.AddKeyToSelection(pressedKey);
      xkeyPreview.Invalidate();
    }

    #region UpdateShortcut
    public void UpdateShortcut()
    {
      if (CustomDataIsInvalid())
        _Shortcut = null;
      else
      {
        if (_Shortcut == null)
          _Shortcut = new CustomInputShortcut();

        _Shortcut.CtrlKeyDown = btnCtrl.Checked;
        _Shortcut.AnyShiftModifier = chkAny.Checked;
        _Shortcut.AltKeyDown = btnAlt.Checked;
        _Shortcut.ShiftKeyDown = btnShift.Checked;
        _Shortcut.CustomData = GetCustomData();
      }			
    }
    #endregion
    #region ShortcutFromControls()
    private CustomInputShortcut ShortcutFromControls()
    {
      UpdateShortcut();
      return _Shortcut;
    }
    #endregion
    #region BindingDataChanged
    private void BindingDataChanged(object sender, System.EventArgs e)
    {
      if (_UpdatingInternally)
        return;
      if (sender == btnAlt || sender == btnCtrl || sender == btnShift)
        chkAny.Checked = false;
      CustomInputShortcut shortcut = ShortcutFromControls();
      OnCustomInputChanged(shortcut);
    }
    #endregion

    protected virtual void OnCustomInputChanged(CustomInputShortcut shortcut)
    {
      CustomInputChangedEventHandler handler = CustomInputChanged;
      if (handler == null)
        return;

      CustomInputChangedEventArgs args = new CustomInputChangedEventArgs();
      args.SetValues(shortcut);
      handler(this, args);
    }

    public event CustomInputChangedEventHandler CustomInputChanged;

    // public methods...
    #region FocusPrimaryEditControl
    public void FocusPrimaryEditControl()
    {
      xkeyPreview.Focus();
    }
    #endregion
    #region EnableControls
    public void EnableControls(bool enable)
    {
      lblXKeysPlus.Enabled = enable;
      btnCtrl.Enabled = enable;
      btnAlt.Enabled = enable;
      btnShift.Enabled = enable;
      chkAny.Enabled = enable;
      if (!enable)
        xkeysPainter.ClearKeySelection();
    }
    #endregion

    // public properties...
    #region AltKeyDown
    public bool AltKeyDown
    {
      get
      {
        return btnAlt.Checked;
      }
      set
      {
        btnAlt.Checked = value;
        UpdateShortcut();
      }
    }
    #endregion
    #region CtrlKeyDown
    public bool CtrlKeyDown
    {
      get
      {
        return btnCtrl.Checked;
      }
      set
      {
        btnCtrl.Checked = value;
        UpdateShortcut();
      }
    }
    #endregion

    #region AnyShiftModifier
    public bool AnyShiftModifier
    {
      get
      {
        return chkAny.Checked;
      }
      set
      {
        chkAny.Checked = value;
        UpdateShortcut();
      }
    }
    #endregion
    
    #region ShiftKeyDown
    public bool ShiftKeyDown
    {
      get
      {
        return btnShift.Checked;
      }
      set
      {
        btnShift.Checked = value;
        UpdateShortcut();
      }
    }
    #endregion

    private void xkeyPreview_Paint(object sender, PaintEventArgs e)
    {
      using (SolidBrush backBrush = new SolidBrush(Parent.BackColor))
        e.Graphics.FillRectangle(backBrush, ClientRectangle);

      xkeysPainter.Draw(e.Graphics, XkeysRaw.Data, xkeyLayout, xkeyPreview.Focused);
    }

    public string CustomData
    {
      get
      {
        return customData;
      }
    }
    
    public KeyLayout XkeyLayout
    {
      get
      {
        return xkeyLayout;
      }
    }

    private void chkAny_CheckedChanged(object sender, EventArgs e)
    {
      if (chkAny.Checked)
      {
        _UpdatingInternally = true;
        try
        {
          btnCtrl.Checked = false;
          btnAlt.Checked = false;
          btnShift.Checked = false;
        }
        finally
        {
          _UpdatingInternally = false;
        }
      }
      BindingDataChanged(sender, e);
    }
  }
}