using PIEHidDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CR_XkeysEngine
{
  public class XkeysDataHandler : PIEDataHandler
  {
    #region private fields...
    readonly byte[] lastKeyCode = new byte[Hardware.Keyboard.NumColumns];
    readonly byte[] blockedKeysMask = new byte[Hardware.Keyboard.NumColumns];
    readonly byte[] simpleKeyCode = new byte[Hardware.Keyboard.NumColumns];    // With blocked keys removed.
    PIEDevice activeDevice;
    string lastKeyCodeStr;
    SwitchPosition switchPosition = SwitchPosition.Unknown;
    string unitID;
    #endregion

    // private methods...
    #region CheckSwitchPosition
    void CheckSwitchPosition(byte[] readData)
    {
      byte switchPos = (byte)(readData[11] & 16);
      if (switchPos == 0)
        switchPosition = SwitchPosition.Down;
      else
        switchPosition = SwitchPosition.Up;
    }
    #endregion
    #region GetKeyData
    void GetKeyData(byte[] rdata)
    {
      lastKeyCodeStr = String.Empty;
      for (int i = 1; i <= lastKeyCode.Length; i++)
      {
        lastKeyCode[i - 1] = rdata[i];
        simpleKeyCode[i - 1] = (byte)(rdata[i] & (byte)(255 - blockedKeysMask[i - 1]));
        lastKeyCodeStr += simpleKeyCode[i - 1] + ".";
      }
      lastKeyCodeStr = lastKeyCodeStr.TrimEnd('.');

      OnDataChanged();
    }
    #endregion

    #region IsKeyDown
    /// <summary>
    /// Returns true if the key at the specified position is down.
    /// </summary>
    public bool IsKeyDown(int column, int row)
    {
      if (column < 0 || column >= simpleKeyCode.Length)
        return false;
      byte lastKeyCode = simpleKeyCode[column];
      return ((byte)Math.Pow(2, row) & lastKeyCode) != 0;
    }
    #endregion

    #region ProcessData
    void ProcessData(byte[] readData)
    {
      if (readData[0] == 2)
      {
        CheckSwitchPosition(readData);
        ReadUnitID(readData);
      }

      GetKeyData(readData);
    }
    #endregion
    #region ReadUnitID
    void ReadUnitID(byte[] readData)
    {
      unitID = readData[10].ToString();
    }
    #endregion

    // protected virtual methods...
    #region OnDataChanged
    protected virtual void OnDataChanged()
    {
      EventHandler handler = DataChanged;
      if (handler != null)
        handler(this, EventArgs.Empty);
    }
    #endregion

    // public methods...
    #region ReadDataOnce
    public void ReadDataOnce()
    {
      byte[] readData = null;
      if (activeDevice.ReadData(ref readData) == 0)
        ProcessData(readData);
    }
    #endregion

    #region HandlePIEHidData
    public void HandlePIEHidData(byte[] data, PIEDevice sourceDevice)
    {
      if (sourceDevice != activeDevice)
        return;

      byte[] readData = null;
      while (sourceDevice.ReadData(ref readData) == 0) // Continue to read data as long as we have it.
        ProcessData(readData);
    }
    #endregion
    #region SetActiveDevice
    public void SetActiveDevice(PIEDevice activeDevice)
    {
      this.activeDevice = activeDevice;
      if (activeDevice == null)
        return;
      activeDevice.SetDataCallback(this, DataCallbackFilterType.callOnChangedData);
    }
    #endregion
    #region SetBlockedKeysMask
    /// <summary>
    /// Maps the current key-down state to a mask.
    /// </summary>
    public void SetBlockedKeysMask()
    {
      for (int i = 0; i < lastKeyCode.Length; i++)
        blockedKeysMask[i] = lastKeyCode[i];
      OnDataChanged();
    }
    #endregion

    // public events...
    public event EventHandler DataChanged;

    // public properties...
    #region ActiveDevice
    public PIEDevice ActiveDevice
    {
      get
      {
        return activeDevice;
      }
    }
    #endregion
    #region BlockedKeysMask
    public byte[] BlockedKeysMask
    {
      get
      {
        return blockedKeysMask;
      }
    }
    #endregion
    #region LastKeyCode
    public byte[] LastKeyCode
    {
      get
      {
        return lastKeyCode;
      }
    }
    #endregion
    #region LastKeyCodeStr
    public string LastKeyCodeStr
    {
      get
      {
        return lastKeyCodeStr;
      }
    }
    #endregion
    #region SwitchPosition
    public SwitchPosition SwitchPosition
    {
      get
      {
        return switchPosition;
      }
    }
    #endregion
    #region UnitID
    public string UnitID
    {
      get
      {
        return unitID;
      }
    }
    #endregion
  }
}
