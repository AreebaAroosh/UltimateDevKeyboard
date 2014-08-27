using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  [UserLevel(UserLevel.NewUser)]
  public partial class OptXkeysLayout : OptionsPage
  {
    KeyBase lastHoverKey;
    private double previewAspectRatio;
    bool updatingInternally;
    KeyLayout xkeyLayout = new KeyLayout();
    readonly XkeysPainter xkeysPainter = new XkeysPainter();

    // DXCore-generated code...
    #region Initialize
    protected override void Initialize()
    {
      base.Initialize();

      xkeysPainter.SetKeySizeToFit(pnlKeyPreview.ClientRectangle.Size);
      RestoreDefaultLayout();
      xkeyLayout.SelectionChanged += xkeyLayout_SelectionChanged;
      previewAspectRatio = pnlKeyPreviewContainer.Width / pnlKeyPreviewContainer.Height;
      XkeysRaw.Data.DataChanged += Data_DataChanged;
      ResizePreviewPanel();
    }
    #endregion

    void Data_DataChanged(object sender, EventArgs e)
    {
      pnlKeyPreview.Invalidate();
    }

    #region GetCategory
    public static string GetCategory()
    {
      return @"X-keys";
    }
    #endregion
    #region GetPageName
    public static string GetPageName()
    {
      return @"Layout";
    }
    #endregion

    void RestoreDefaultLayout()
    {
      if (xkeyLayout != null)
        xkeyLayout.SelectionChanged -= xkeyLayout_SelectionChanged;
      xkeyLayout = xkeysPainter.GetLayout(XkeysRaw.Data.LastKeyCode, XkeysRaw.Data.BlockedKeysMask);
      xkeyLayout.SelectionChanged += xkeyLayout_SelectionChanged;
    }

    private void OptXkeysLayout_PreparePage(object sender, OptionsPageStorageEventArgs ea)
    {
      xkeyLayout.Load();
      if (xkeyLayout.Keys.Count == 0)
        RestoreDefaultLayout();
    }

    private void pnlKeyPreview_Paint(object sender, PaintEventArgs e)
    {
      using (SolidBrush backBrush = new SolidBrush(Color.FromArgb(0x37, 0x37, 0x37)))
        e.Graphics.FillRectangle(backBrush, pnlKeyPreview.ClientRectangle);

      xkeysPainter.Draw(e.Graphics, XkeysRaw.Data, xkeyLayout, false);
    }

    void UpdateControlsBasedOnSelection()
    {
      updatingInternally = true;
      try
      {
        XkeySelection selection = xkeyLayout.GetSelection();
        bool isTall = selection.GetGroupType() == KeyGroupType.Tall;
        bool isWide = selection.GetGroupType() == KeyGroupType.Wide;
        bool isSquare = selection.GetGroupType() == KeyGroupType.Square;

        chkTall.Checked = isTall;
        chkWide.Checked = isWide;
        chkSquareButton.Checked = isSquare;
        chkKeyRepeatsifHeldDown.Checked = selection.AllKeysRepeatIfHeldDown();
        chkBlocker.Enabled = selection.HasOnlySingleKeys();
        txtKeyName.Enabled = selection.Count > 0;
        if (chkBlocker.Enabled)
          chkBlocker.Checked = selection.IsBlocked();
        chkTall.Enabled = isTall || selection.CanBeTall();
        chkWide.Enabled = isWide || selection.CanBeWide();
        chkKeyRepeatsifHeldDown.Enabled = selection.Count > 0;
        chkSquareButton.Enabled = isSquare || selection.CanBeSquare();
        txtKeyName.Text = selection.GetName();
      }
      finally
      {
        updatingInternally = false;
      }
    }

    private void btnAutoDetectBlockers_Click(object sender, EventArgs e)
    {
      foreach (KeyBase key in xkeyLayout.Keys)
      {
        Key xkey = key as Key;
        if (xkey != null)
          xkey.IsBlocked = XkeysRaw.Data.IsKeyDown(key.Column, key.Row);
      }
      xkeyLayout.BlockSettingsChanged();
      RefreshKeyLayout();
      txtKeyName.Focus();
    }

    private void chkTall_CheckedChanged(object sender, EventArgs e)
    {
      if (updatingInternally)
        return;
      XkeySelection selection = xkeyLayout.GetSelection();
      if (selection.GetGroupType() == KeyGroupType.Tall)
        xkeyLayout.UngroupSelection();
      else if (selection.Count == 2)
        xkeyLayout.GroupSelection(KeyGroupType.Tall);
      txtKeyName.Focus();
    }

    private void chkSquareButton_CheckedChanged(object sender, EventArgs e)
    {
      if (updatingInternally)
        return;
      XkeySelection selection = xkeyLayout.GetSelection();
      if (selection.GetGroupType() == KeyGroupType.Square)
        xkeyLayout.UngroupSelection();
      else if (selection.Count == 4)
        xkeyLayout.GroupSelection(KeyGroupType.Square);
      txtKeyName.Focus();
    }

    private void chkWide_CheckedChanged(object sender, EventArgs e)
    {
      if (updatingInternally)
        return;
      XkeySelection selection = xkeyLayout.GetSelection();
      if (selection.GetGroupType() == KeyGroupType.Wide)
        xkeyLayout.UngroupSelection();
      else if (selection.Count == 2)
        xkeyLayout.GroupSelection(KeyGroupType.Wide);
      txtKeyName.Focus();
    }

    private void chkBlocker_CheckedChanged(object sender, EventArgs e)
    {
      if (updatingInternally)
        return;
      XkeySelection selection = xkeyLayout.GetSelection();
      if (selection.GetGroupType() != KeyGroupType.NoGroup)
        return;

      bool allAreBlocked = true;
      foreach (KeyBase selectedKey in selection.SelectedKeys)
      {
        Key xkey = selectedKey as Key;
        if (xkey != null)
          if (!xkey.IsBlocked)
            allAreBlocked = false;
      }

      bool newBlockSetting;
      if (allAreBlocked)
        newBlockSetting = false;
      else
        newBlockSetting = true;
      foreach (KeyBase selectedKey in selection.SelectedKeys)
      {
        Key xkey = selectedKey as Key;
        if (xkey != null)
          xkey.IsBlocked = newBlockSetting;
      }
      xkeyLayout.BlockSettingsChanged();
      RefreshKeyLayout();
      txtKeyName.Focus();
    }

    private void txtKeyName_TextChanged(object sender, EventArgs e)
    {
      if (updatingInternally)
        return;
      xkeyLayout.SetName(txtKeyName.Text);
      pnlKeyPreview.Invalidate();
    }

    private void OptXkeysLayout_CommitChanges(object sender, CommitChangesEventArgs ea)
    {
      xkeyLayout.Save();
    }

    void RefreshKeyLayout()
    {
      pnlKeyPreview.Invalidate();
    }

    private void OptXkeysLayout_RestoreDefaults(object sender, OptionsPageEventArgs ea)
    {
      RestoreDefaultLayout();
      RefreshKeyLayout();
    }

    void xkeyLayout_SelectionChanged(object sender, EventArgs e)
    {
      RefreshKeyLayout();
    }

    void ResizePreviewPanel()
    {
      int spacing = chkSquareButton.Left - chkTall.Right;
      int rightSide = Width - pnlLayoutOptions.Width - spacing;

      int newWidth = rightSide - pnlKeyPreviewContainer.Left;
      int newHeight = (int)Math.Round(newWidth / previewAspectRatio);

      int availableHeight = Height - pnlKeyPreviewContainer.Top;

      if (newHeight > availableHeight)
      {
        newHeight = availableHeight;
        newWidth = (int)Math.Round(newHeight * previewAspectRatio);
      }

      Size newSize = new Size(newWidth, newHeight);

      pnlKeyPreviewContainer.Size = newSize;
      pnlLayoutOptions.Left = newWidth + spacing;
      xkeysPainter.SetKeySizeToFit(pnlKeyPreview.ClientRectangle.Size);
    }

    private void OptXkeysLayout_Resize(object sender, EventArgs e)
    {
      ResizePreviewPanel();
      pnlKeyPreview.Invalidate();
    }

    private void pnlKeyPreview_MouseDown(object sender, MouseEventArgs e)
    {
      int row;
      int column;
      if (xkeysPainter.IsHit(e.X, e.Y, out column, out row))
      {
        bool shiftKeyDown = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
        bool ctrlKeyDown = (Control.ModifierKeys & Keys.Control) == Keys.Control;
        if (xkeyLayout.Select(column, row, shiftKeyDown, ctrlKeyDown))
        {
          pnlKeyPreview.Invalidate();
          UpdateControlsBasedOnSelection();
          txtKeyName.Focus();
        }
      }
    }

    private void pnlKeyPreview_MouseMove(object sender, MouseEventArgs e)
    {
      // TODO: Fix this so tool tips over the layout keys actually work.
      int row;
      int column;
      if (xkeysPainter.IsHit(e.X, e.Y, out column, out row))
      {
        KeyBase key = xkeyLayout.GetKey(column, row);
        if (key != lastHoverKey)
        {
          lastHoverKey = key;
          if (key != null)
            toolTip1.SetToolTip(pnlKeyPreview, key.Name);
        }
      }
      else
        toolTip1.SetToolTip(pnlKeyPreview, null);
    }

    private void dxCoreEvents1_OptionsChanged(OptionsChangedEventArgs ea)
    {
      if (ea.OptionsPages.Contains(typeof(OptXkeysLayout)))
      {
        xkeyLayout.Load();
        pnlKeyPreview.Invalidate();
      }
    }

    private void linkShortcuts_OpenLink(object sender, DevExpress.DXCore.Controls.XtraEditors.Controls.OpenLinkEventArgs e)
    {
      CodeRush.Options.Show(typeof(OptXkeysShortcuts));
    }

    void MoveSelection(int deltaX, int deltaY, bool addToSelection)
    {
      xkeyLayout.MoveSelection(deltaX, deltaY, addToSelection);
      UpdateControlsBasedOnSelection();
      RefreshKeyLayout();
    }
    void MoveSelectionLeft(bool addToSelection)
    {
      MoveSelection(-1, 0, addToSelection);
    }

    void MoveSelectionRight(bool addToSelection)
    {
      MoveSelection(1, 0, addToSelection);
    }

    void MoveSelectionUp(bool addToSelection)
    {
      MoveSelection(0, -1, addToSelection);
    }

    void MoveSelectionDown(bool addToSelection)
    {
      MoveSelection(0, 1, addToSelection);
    }

    private void txtKeyName_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyData == (Keys.Alt | Keys.Up))
        MoveSelectionUp(false);
      else if (e.KeyData == (Keys.Alt | Keys.Down))
        MoveSelectionDown(false);
      else if (e.KeyData == (Keys.Alt | Keys.Left))
        MoveSelectionLeft(false);
      else if (e.KeyData == (Keys.Alt | Keys.Right))
        MoveSelectionRight(false);
      else if (e.KeyData == (Keys.Alt | Keys.Shift | Keys.Up))
        MoveSelectionUp(true);
      else if (e.KeyData == (Keys.Alt | Keys.Shift | Keys.Down))
        MoveSelectionDown(true);
      else if (e.KeyData == (Keys.Alt | Keys.Shift | Keys.Left))
        MoveSelectionLeft(true);
      else if (e.KeyData == (Keys.Alt | Keys.Shift | Keys.Right))
        MoveSelectionRight(true);
    }

    private void chkKeyRepeatsifHeldDown_CheckedChanged(object sender, EventArgs e)
    {
      if (updatingInternally)
        return;
      xkeyLayout.SetRepeat(chkKeyRepeatsifHeldDown.Checked);
      pnlKeyPreview.Invalidate();
      txtKeyName.Focus();
    }

  }
}