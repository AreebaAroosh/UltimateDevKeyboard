using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  /* Need to figure out:
 *  Method implementation for grouping operations (Wide, Tall, Square)
 */

  
  public class Xkey : XkeyBase
  {
    public bool IsBlocked { get; set; }
    public Xkey()
    {

    }

    public override void Save(DecoupledStorage storage, string section, int index)
    {
      base.Save(storage, section, index);
      storage.WriteBoolean(section, "IsBlocked" + index, IsBlocked);
    }

    public override void Load(DecoupledStorage storage, string section, int index)
    {
      base.Load(storage, section, index);
      IsBlocked = storage.ReadBoolean(section, "IsBlocked" + index);
    }

  }
}
