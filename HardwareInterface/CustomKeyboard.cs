using System;
using System.Collections.Generic;
using System.Linq;

namespace CR_XkeysEngine
{
  public abstract class CustomKeyboard
  {
    public abstract bool IsValidKey(int column, int row);

    public abstract int NumColumns { get; }
    public abstract int NumRows { get; }

    /// <summary>
    /// The width of the keyboard, as measured in key widths.
    /// </summary>
    public abstract double Width { get; }

    /// <summary>
    /// The height of the keyboard, as measured in key heights.
    /// </summary>
    public abstract double Height { get; }
  }
}
