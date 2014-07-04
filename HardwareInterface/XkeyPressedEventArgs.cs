using System;
using System.Collections.Generic;
using System.Linq;

namespace CR_XkeysEngine
{
  public class XkeyPressedEventArgs : EventArgs
  {
    readonly List<XkeyBase> keysDown = new List<XkeyBase>();

    public XkeyPressedEventArgs(List<XkeyBase> keysDown, bool shiftKeyDown, bool ctrlKeyDown, bool altKeyDown, string customData)
    {
      CustomData = customData;
      KeysDown.AddRange(keysDown);
      ShiftKeyDown = shiftKeyDown;
      CtrlKeyDown = ctrlKeyDown;
      AltKeyDown = altKeyDown;
    }

    public XkeyPressedEventArgs()
    {
    }

    public List<XkeyBase> KeysDown
    {
      get
      {
        return keysDown;
      }
    }

    public string CustomData { get; private set; }
    public bool ShiftKeyDown { get; private set; }
    public bool CtrlKeyDown { get; private set; }
    public bool AltKeyDown { get; private set; }
  }
}
