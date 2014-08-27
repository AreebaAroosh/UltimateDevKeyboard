using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  public class Key : KeyBase
  {
    public bool IsBlocked { get; set; }
    public Key()
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
