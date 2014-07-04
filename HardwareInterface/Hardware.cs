using System;
using System.Collections.Generic;
using System.Linq;

namespace CR_XkeysEngine
{
  public class Hardware
  {
    public static CustomKeyboard Keyboard { get; set; }

    static Hardware()
    {
      Keyboard = new XkeysProfessional58();
    }
  }
}
