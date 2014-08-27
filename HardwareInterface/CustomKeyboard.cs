using System;
using System.Collections.Generic;
using System.Linq;

namespace CR_XkeysEngine
{
  public abstract class CustomKeyboard
  {
    /// <summary>
    /// Call to initiate a connection with the custom keyboard.
    /// </summary>
    public abstract void Connect();

    /// <summary>
    /// Gets a collection of pressed keys based on the coded data string customData.
    /// </summary>
    public abstract List<KeyBase> GetPressedKeys(string customData, IKeyGetter keyGetter);

    /// <summary>
    /// Returns a coded string based on the keys that are currently down. This same 
    /// coded string can be used later, passed to GetPressedKeys, above, to return 
    /// a collection of keys that were pressed.
    /// </summary>
    public abstract string GetCodeStr(IKeyGetter keyGetter);

    /// <summary>
    /// Call when layout settings for which keys are blocked changes.
    /// </summary>
    public abstract void BlockSettingsChanged(List<KeyBase> keys);

    /// <summary>
    /// Returns true when the specified column and row mark a valid key on the keyboard.
    /// </summary>
    public abstract bool IsValidKey(int column, int row);

    /// <summary>
    /// Returns true if the key at the specified column and row is down.
    /// </summary>
    public abstract bool IsDown(int column, int row);

    /// <summary>
    /// The maximum number of columns the keyboard supports.
    /// </summary>
    public abstract int NumColumns { get; }

    /// <summary>
    /// The maximum number of rows the keyboard supports.
    /// </summary>
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
