using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CR_XkeysEngine
{
  public class EditorCustomInputEventArgs : EventArgs
  {
    public string CustomData { get; set; }

    public EditorCustomInputEventArgs(string customData)
    {
      CustomData = customData;
    }
    public EditorCustomInputEventArgs()
    {
      
    }
     
  }
}
