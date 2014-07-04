using PIEHidDotNet;
using System;
using System.Collections.Generic;

namespace CR_XkeysEngine
{
  /// <summary>
  /// Low-level Xkeys connection class.
  /// </summary>
  public static class XkeysRaw
  {
    const int INT_GreenLight = 64;
    const int INT_RedLight = 128;
    const int INT_XKeysProfessionalID = 679;
    const int INT_MaxDevices = 128;

    readonly static XkeysErrorHandler xkeysErrorHandler = new XkeysErrorHandler();
    readonly static XkeysDataHandler xkeysDataHandler = new XkeysDataHandler();
    static PIEDevice activeDevice;
    static PIEDevice[] foundDevices;
    static int[] deviceIndices;
    static int selectedDeviceIndex;
    static int validDevicesFound = -1;
    static Byte[] writeDataBuffer;

    static XkeysRaw()
    {
      DevExpress.CodeRush.Core.EventNexus.BeginShutdown += EventNexus_BeginShutdown;
    }

    // event handlers...
    #region EventNexus_BeginShutdown
    static void EventNexus_BeginShutdown()
    {
      DevExpress.CodeRush.Core.EventNexus.BeginShutdown -= EventNexus_BeginShutdown;
      CloseAllInterfaces();
    }
    #endregion


    #region ActivateDevice
    /// <summary>
    /// Activates the device (among all the devices discovered by a previous 
    /// call to EnumerateDevices) based on the specified index.
    /// </summary>
    /// <param name="deviceIndex">The index of the device to select.</param>
    public static void ActivateDevice(int deviceIndex)
    {
      if (deviceIndex < 0 || deviceIndex >= foundDevices.Length)
        throw new ArgumentException(String.Format("deviceIndex ({0}) is out of bounds", deviceIndex));

      selectedDeviceIndex = deviceIndex;
      activeDevice = foundDevices[deviceIndex];
      writeDataBuffer = new byte[activeDevice.WriteLength]; // initialize the write data buffer for this device

      xkeysErrorHandler.SetActiveDevice(activeDevice);
      xkeysDataHandler.SetActiveDevice(activeDevice);
    }
    #endregion
    #region Connect
    /// <summary>
    /// Enumerates through all connected Xkeys devices and 
    /// connects with the first one found. 
    /// </summary>
    public static void Connect()
    {
      if (validDevicesFound == -1)
        EnumerateDevices();

      if (validDevicesFound > 0)
        ActivateDevice(deviceIndices[0]);   // Select the first device found.
    }
    #endregion
    #region CloseAllInterfaces
    /// <summary>
    /// Closes all previously-opened interfaces. This method is called 
    /// automatically when Visual Studio shuts down.
    /// </summary>
    public static void CloseAllInterfaces()
    {
      for (int i = 0; i < validDevicesFound; i++)
      {
        int deviceIndex = deviceIndices[i];
        foundDevices[deviceIndex].CloseInterface();
      }
    }
    #endregion
    #region EnumerateDevices
    /// <summary>
    /// Enumerates all Xkeys devices currently connected.
    /// </summary>
    /// <returns>Returns the number of devices found.</returns>
    public static int EnumerateDevices()
    {
      deviceIndices = new int[INT_MaxDevices];

      foundDevices = PIEDevice.EnumeratePIE();
      if (foundDevices.Length == 0)
        return 0;

      validDevicesFound = 0;
      for (int i = 0; i < foundDevices.Length; i++)
        if (foundDevices[i].HidUsagePage == 0xc)
          switch (foundDevices[i].Pid)
          {
            case INT_XKeysProfessionalID:
              deviceIndices[validDevicesFound] = i;
              validDevicesFound++;
              foundDevices[i].SetupInterface();
              break;
            default:
              // Unknown Device...
              break;
          }

      return validDevicesFound;
    }
    #endregion
    #region LightOn
    /// <summary>
    /// Turns on the specified light on the Xkeys Professional.
    /// </summary>
    /// <param name="xkeysLight">The light to turn on.</param>
    /// <returns>Returns true if the operation was successful.</returns>
    public static bool LightOn(XkeysLight xkeysLight)
    {
      ClearWriteBuffer();

      writeDataBuffer[0] = 2;
      writeDataBuffer[1] = 186;

      int light;
      if (xkeysLight == XkeysLight.Green)
        light = INT_GreenLight;
      else
        light = INT_RedLight;

      writeDataBuffer[7] = (byte)(writeDataBuffer[7] | light);
      return foundDevices[selectedDeviceIndex].WriteData(writeDataBuffer) == 0;
    }
    #endregion
    #region TurnOffLights
    /// <summary>
    /// Turns off both the red and green lights on the Xkeys Professional.
    /// </summary>
    /// <returns>Returns true if the operation was successful.</returns>
    public static bool TurnOffLights()
    {
      ClearWriteBuffer();

      writeDataBuffer[0] = 2;
      writeDataBuffer[1] = 186;
      writeDataBuffer[7] = 0;
      return foundDevices[selectedDeviceIndex].WriteData(writeDataBuffer) == 0;
    }
    #endregion


    // private static methods...
    #region ClearWriteBuffer
    /// <summary>
    /// Clears the write buffer for the active device.
    /// </summary>
    static void ClearWriteBuffer()
    {
      for (int i = 0; i < foundDevices[selectedDeviceIndex].WriteLength; i++)
        writeDataBuffer[i] = 0;
    }
    #endregion

    // public properties...
    #region ActiveDevice
    public static PIEDevice ActiveDevice
    {
      get
      {
        return activeDevice;
      }
    }
    #endregion
    #region Data
    public static XkeysDataHandler Data
    {
      get
      {
        return xkeysDataHandler;
      }
    }
    #endregion
    #region FoundDevices
    public static PIEDevice[] FoundDevices
    {
      get
      {
        return foundDevices;
      }
    }
    #endregion
  }
}
