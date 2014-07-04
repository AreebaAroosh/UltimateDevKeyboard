using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CR_XkeysEngine
{
  // TODO: When options page data changes, reload the xkeyLayout.
  public class XkeysEngine
  {
    string lastDataSent;
    DateTime lastTimeSent = DateTime.MinValue;
    XkeyLayout xkeyLayout;

    public XkeysEngine()
    {
      xkeyLayout = new XkeyLayout();
      xkeyLayout.Load();
      XkeysRaw.Data.DataChanged += Data_DataChanged;
    }

    void Data_DataChanged(object sender, EventArgs e)
    {

      List<XkeyBase> keysDown = new List<XkeyBase>();

      for (int column = 0; column < Hardware.Keyboard.NumColumns; column++)
      {
        byte blockedKeyMask = XkeysRaw.Data.BlockedKeysMask[column];
        byte keyCode = (byte)(XkeysRaw.Data.LastKeyCode[column] & (255 - blockedKeyMask));

        if (keyCode == 0)
          continue;

        for (int row = 0; row < Hardware.Keyboard.NumRows; row++)
          if ((keyCode & (byte)Math.Pow(2, row)) != 0)
          {
            XkeyBase key = xkeyLayout.GetKey(column, row);
            if (!keysDown.Contains(key))
              keysDown.Add(key);
          }
      }
      OnXkeyPressed(keysDown, xkeyLayout.GetCodeStr());
      /* 
       3. Set a timer for about 500ms and when it fires check the data again to make sure the key isn't "stuck".
     */
    }

    protected virtual void OnXkeyPressed(List<XkeyBase> keysDown, string customData)
    {
      if (lastDataSent == customData && DateTime.Now - lastTimeSent < TimeSpan.FromMilliseconds(250))
        return;
      lastDataSent = customData;
      XkeyPressedEventHandler handler = XkeyPressed;
      if (handler != null)
      {
        Keys modifierKeys = Control.ModifierKeys;
        bool shiftKeyDown = (modifierKeys & Keys.Shift) == Keys.Shift;
        bool ctrlKeyDown = (modifierKeys & Keys.Control) == Keys.Control;
        bool altKeyDown = (modifierKeys & Keys.Alt) == Keys.Alt;

        // Important: This next line (new XkeyPressedEventArgs(...)) is crucial for the current multi-threaded implementation. DO NOT CHANGE to a shared event args approach (considered more memory-efficient and performant).
        XkeyPressedEventArgs e = new XkeyPressedEventArgs(keysDown, shiftKeyDown, ctrlKeyDown, altKeyDown, customData);
        handler(this, e);
      }
      lastTimeSent = DateTime.Now;
    }

    public event XkeyPressedEventHandler XkeyPressed;
  }
}