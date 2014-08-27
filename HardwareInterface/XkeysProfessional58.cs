using System;
using System.Collections.Generic;
using System.Linq;

namespace CR_XkeysEngine
{
  public class XkeysProfessional58 : CustomKeyboard
  {
    void ClearBlockedKeysMask()
    {
      for (int column = 0; column < NumColumns; column++)
        XkeysRaw.Data.BlockedKeysMask[column] = 0;
    }

    static void TransferBlockSettings(Key xkey)
    {
      if (xkey == null || !xkey.IsBlocked)
        return;

      XkeysRaw.Data.BlockedKeysMask[xkey.Column] = (byte)(XkeysRaw.Data.BlockedKeysMask[xkey.Column] + (byte)Math.Pow(2, xkey.Row));
    }

    public override List<KeyBase> GetPressedKeys(string customData, IKeyGetter keyGetter)
    {
      List<KeyBase> keysPressed = new List<KeyBase>();
      string[] columns = customData.Split('.');
      for (int column = 0; column < columns.Length; column++)
      {
        int value;
        if (int.TryParse(columns[column], out value))
        {
          if (value == 0)
            continue;
          for (int row = 0; row < Hardware.Keyboard.NumRows; row++)
          {
            byte rowMask = (byte)(Math.Pow(2, row));
            if ((rowMask & value) == rowMask)
            {
              KeyBase keyPressed = keyGetter.GetKey(column, row);
              if (keysPressed.Contains(keyPressed))
                continue;
              keysPressed.Add(keyPressed);
            }
          }
        }
      }
      return keysPressed;
    }

    public override string GetCodeStr(IKeyGetter keyGetter)
    {
      string result = string.Empty;
      for (int column = 0; column < Hardware.Keyboard.NumColumns; column++)
      {
        byte thisValue = 0;
        for (int row = 0; row < Hardware.Keyboard.NumRows; row++)
          if (Hardware.Keyboard.IsValidKey(column, row))
          {
            KeyBase xkey = keyGetter.GetKey(column, row);
            if (!xkey.IsDown)
              continue;

            KeyBase singleXkey = keyGetter.GetSingleKey(column, row);
            if (singleXkey != null)
              thisValue += singleXkey.GetRowDataValue();
          }
        result += thisValue + ".";
      }
      return result.TrimEnd('.');
    }

    public override void BlockSettingsChanged(List<KeyBase> keys)
    {
      ClearBlockedKeysMask();

      foreach (KeyBase key in keys)
        TransferBlockSettings(key as Key);
    }

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

    public override void Connect()
    {
      XkeysRaw.Connect();
    }

    public override bool IsDown(int column, int row)
    {
      return XkeysRaw.Data.IsKeyDown(column, row);
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
