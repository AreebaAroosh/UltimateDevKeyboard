using System;
using System.Collections.Generic;

namespace CR_XkeysEngine
{
  public static class XkeysUtils
  {
    public static string GetKeyName(bool anyKey, bool ctrlKeyDown, bool shiftKeyDown, bool altKeyDown, List<XkeyBase> keysDown)
    {
      string keyName;
      if (anyKey)
        keyName = "(any)+";
      else 
        keyName = DevExpress.CodeRush.Core.CodeRush.Key.GetName(0, 0, ctrlKeyDown, shiftKeyDown, altKeyDown);
      foreach (XkeyBase xkeyBase in keysDown)
        keyName += xkeyBase.Name + "+";
      if (keyName.EndsWith("+"))
        keyName = keyName.Substring(0, keyName.Length - 1);
      return keyName;
    }
  }
}
