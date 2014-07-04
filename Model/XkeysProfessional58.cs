using System;
using System.Collections.Generic;
using System.Linq;

namespace CR_XkeysEngine
{
  public class XkeysProfessional58 : CustomKeyboard
  {
    public override bool IsValidKey(int column, int row)
    {
      if (row < 0 || row >= NumRows)
        return false;
      if (column < 0 || column >= NumColumns)
        return false;
      if (row > 1 && column > 7)    // After row 1 there are only eight columns instead of nine.
        return false;
      return true;
    }

    public override int NumColumns
    {
      get { return 9; }
    }

    public override int NumRows
    {
      get { return 7; }
    }

    public override double Width
    {
      get { return 9; }
    }

    public override double Height
    {
      get { return 7.5; }
    }
  }
}
