using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CR_XkeysEngine
{
  public class FlickerFreeFocusPanel : Panel
  {
    // constructors...
    #region FlickerFreeFocusPanel
    public FlickerFreeFocusPanel()
    {
      SetStyle(ControlStyles.Selectable | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.DoubleBuffer, true);
      TabStop = true;
    }
    #endregion

    // protected method overrides...
    #region IsInputKey
    protected override bool IsInputKey(Keys keyData)
    {
      if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
        return true;
      return base.IsInputKey(keyData);
    }
    #endregion
    #region OnEnter
    protected override void OnEnter(EventArgs ea)
    {
      Invalidate();
      base.OnEnter(ea);
    }
    #endregion
    #region OnLeave
    protected override void OnLeave(EventArgs ea)
    {
      Invalidate();
      base.OnLeave(ea);
    }
    #endregion
    #region OnMouseDown
    protected override void OnMouseDown(MouseEventArgs ea)
    {
      Focus();
      base.OnMouseDown(ea);
    }
    #endregion
    #region OnPaint
    protected override void OnPaint(PaintEventArgs ea)
    {
      base.OnPaint(ea);
      if (!Focused)
        return;

      var focusRect = ClientRectangle;
      focusRect.Inflate(-2, -2);
      ControlPaint.DrawFocusRectangle(ea.Graphics, focusRect);
    }
    #endregion
  }
}
