using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  public class KeyGroup : KeyBase
  {
    public KeyGroupType Type { get; set; }
    readonly List<KeyBase> keys = new List<KeyBase>();

    public override bool ClearSelection()
    {
      bool changed = base.ClearSelection();
      foreach (KeyBase key in keys)
        if (key.ClearSelection())
          changed = true;
      return changed;
    }

    public override void Select(int column, int row)
    {
      base.Select(column, row);
      foreach (KeyBase key in keys)
        key.Select(column, row);
    }

    public override void ToggleSelection(int column, int row)
    {
      base.ToggleSelection(column, row);
      foreach (KeyBase key in keys)
        key.ToggleSelection(column, row);
    }


    public override void SetName(string newName)
    {
      base.SetName(newName);
      foreach (KeyBase key in keys)
        key.SetName(newName);
    }

    public override bool IsAt(int column, int row)
    {
      if (base.IsAt(column, row))
        return true;
      foreach (KeyBase key in keys)
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

    public void Ungroup(KeyLayout layout)
    {
      layout.Remove(this);
      foreach (KeyBase key in keys)
        layout.Keys.Add(key);
    }

    public override void Load(DecoupledStorage storage, string section, int index)
    {
      base.Load(storage, section, index);
      Type = (KeyGroupType)storage.ReadEnum<KeyGroupType>(section, "Type" + index, KeyGroupType.Wide);

      int numElements = storage.ReadInt32(section, "GroupCount" + index, 0);
      for (int i = 0; i < numElements; i++)
        keys.Add(KeyBase.CreateAndLoad(storage, String.Format("{0}.Group{1}", section, index), i));
    }

    /// <summary>
    /// Returns the sum of all the row data values (for all keys of this group) if this key is down.
    /// </summary>
    public override byte GetRowDataValue()
    {
      // This appears to NEVER be called. Let's delete and decouple hardware dependencies from this class.
      byte result = 0;
      foreach (KeyBase key in keys)
      {
        // Add all row data values regardless of whether each sub-key is down. A group's row data value will always be the sum of all of its keys being down, even though it's physically possible to press only one key of the entire group - we need to be able to match that one key or any combination.
        result += key.GetRowDataValue();
        // TODO: Is this correct for wide keys? Reviewing the code it looks like the operator should be "|=". We apparently don't need to fix because this appears to never be called (we are individually accessing GetSingleKey when a group is involved).
      }
      return result;
    }

    public override bool IsDown
    {
      get
      {
        foreach (KeyBase key in keys)
          if (Hardware.Keyboard.IsDown(key.Column, key.Row))
            return true;
        return false;
      }
    }

    public List<KeyBase> Keys
    {
      get
      {
        return keys;
      }
    }
  }
}
