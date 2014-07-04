using PIEHidDotNet;
using System;
using System.Collections.Generic;

namespace CR_XkeysEngine
{
  public class XkeysErrorHandler : PIEErrorHandler
  {
    public void SetActiveDevice(PIEDevice activeDevice)
    {
      activeDevice.SetErrorCallback(this);
    }

    public void HandlePIEHidError(int error, PIEDevice sourceDevices)
    {
      // Error 301 happens when the device is unplugged. We need a strategy for disconnecting and reconnecting.

      // Error 1001 occurs when an exception is thrown during the data changed handler (e.g., trying to access controls or control properties that otherwise must be accessed on the UI thread could throw this exception).

      //throw new NotImplementedException();
    }
  }
}
