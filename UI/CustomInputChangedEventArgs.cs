using System;

namespace CR_XkeysEngine
{
  public class CustomInputChangedEventArgs : EventArgs
  {
    CustomInputShortcut _Shortcut;

    #region CustomInputChangedEventArgs
    public CustomInputChangedEventArgs()
    {
      _Shortcut = null;
    }
    #endregion

    #region SetValues
    protected internal void SetValues(CustomInputShortcut shortcut)
    {
      _Shortcut = shortcut;
    }
    #endregion

    #region Shortcut
    public CustomInputShortcut Shortcut
    {
      get
      {
        return _Shortcut;
      }
    }
    #endregion
  }
}
