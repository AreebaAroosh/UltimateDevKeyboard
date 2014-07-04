using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  public class XkeyGroup : XkeyBase
  {
    public XKeysGroupType Type { get; set; }
    readonly List<XkeyBase> keys = new List<XkeyBase>();

    //public override bool IsHit(int column, int row)
    //{
    //  foreach (XkeyBase key in keys)
    //    if (key.IsHit(column, row))
    //      return true;
    //  return false;
    //}

    public override bool ClearSelection()
    {
      bool changed = base.ClearSelection();
      foreach (XkeyBase key in keys)
        if (key.ClearSelection())
          changed = true;
      return changed;
    }

    public override void Select(int column, int row)
    {
      base.Select(column, row);
      foreach (XkeyBase key in keys)
        key.Select(column, row);
    }

    public override void ToggleSelection(int column, int row)
    {
      base.ToggleSelection(column, row);
      foreach (XkeyBase key in keys)
        key.ToggleSelection(column, row);
    }


    public override void SetName(string newName)
    {
      base.SetName(newName);
      foreach (XkeyBase key in keys)
        key.SetName(newName);
    }

    public override bool IsAt(int column, int row)
    {
      if (base.IsAt(column, row))
        return true;
      foreach (XkeyBase key in keys)
        if (key.IsAt(column, row))
          return true;
      return false;
    }

    public override void Save(DecoupledStorage storage, string section, int index)
    {
      base.Save(storage, section, index);
      
      storage.WriteBoolean(section, "IsGroup" + index, true);  // Use later to determine if this is a group on loading.
      storage.WriteInt32(section, "GroupCount" + index, keys.Count);  // Use later to determine number of elements in this group.
      
      storage.WriteEnum(section, "Type" + index, Type);
      for (int i = 0; i < keys.Count; i++)
        keys[i].Save(storage, String.Format("{0}.Group{1}", section, index), i);
    }

    public void Ungroup(XkeyLayout layout)
    {
      layout.Remove(this);
      foreach (XkeyBase key in keys)
        layout.Keys.Add(key);
    }

    public override void Load(DecoupledStorage storage, string section, int index)
    {
      base.Load(storage, section, index);
      Type = (XKeysGroupType)storage.ReadEnum<XKeysGroupType>(section, "Type" + index, XKeysGroupType.Wide);

      int numElements = storage.ReadInt32(section, "GroupCount" + index, 0);
      for (int i = 0; i < numElements; i++)
        keys.Add(XkeyBase.CreateAndLoad(storage, String.Format("{0}.Group{1}", section, index), i));
    }

    /// <summary>
    /// Returns the sum of all the row data values (for all keys of this group) if this key is down.
    /// </summary>
    public override byte GetRowDataValue()
    {
      byte result = 0;
      foreach (XkeyBase key in keys)
      {
        // Add all row data values regardless of whether each sub-key is down. A group's row data value will always be the sum of all of its keys being down, even though it's physically possible to press only one key of the entire group - we need to be able to match that one key or any combination.
        result += key.GetRowDataValue();
      }
      return result;
    }

    public override bool IsDown
    {
      get
      {
        foreach (XkeyBase key in keys)
          if (XkeysRaw.Data.IsKeyDown(key.Column, key.Row))
            return true;
        return false;
      }
    }

    public List<XkeyBase> Keys
    {
      get
      {
        return keys;
      }
    }
  }
}
