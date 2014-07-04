using System;

namespace CR_XkeysEngine
{
  public class CustomInputShortcut : ICloneable
  {
    // constructor ...
    #region CustomInputShortcut
    public CustomInputShortcut()
    {
      CustomData = string.Empty;
    }
    #endregion

    // private methods ...
    #region CustomInputAsStr
    private string CustomInputAsStr()
    {
      // TODO: Get the key name from the X-keys layout.
      return CustomData;
    }
    #endregion

    // public properties ...
    #region DisplayName
    public string DisplayName
    {
      get
      {
        return DevExpress.CodeRush.Core.CodeRush.Key.GetName(0, 0, CtrlKeyDown, ShiftKeyDown, AltKeyDown) + CustomInputAsStr();
      }
    }
    #endregion

    public bool ShiftKeyDown { get; set; }
    public bool AltKeyDown { get; set; }
    public bool CtrlKeyDown { get; set; }
    public bool AnyShiftModifier { get; set; }
    public string CustomData { get; set; }

    #region ICloneable Members
    object ICloneable.Clone()
    {
      return Clone();
    }
    public CustomInputShortcut Clone()
    {
      CustomInputShortcut clone = new CustomInputShortcut();
      clone.AltKeyDown = AltKeyDown;
      clone.ShiftKeyDown = ShiftKeyDown;
      clone.CtrlKeyDown = CtrlKeyDown;
      clone.AnyShiftModifier = AnyShiftModifier;
      clone.CustomData = CustomData;
      return clone;
    }
    #endregion
  }
}
