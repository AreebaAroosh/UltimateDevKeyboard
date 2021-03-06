using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  public class XkeySelection
  {
    List<KeyBase> selectedKeys = new List<KeyBase>();
    public XkeySelection()
    {

    }

    public bool IsBlocked()
    {
      int blockedKeys = 0;
      foreach (KeyBase key in selectedKeys)
      {
        Key xkey = key as Key;
        if (xkey != null)
          if (xkey.IsBlocked)
            blockedKeys++;
          else 
            return false;
      }
      return blockedKeys > 0;
    }

    public bool AllKeysRepeatIfHeldDown()
    {
      foreach (KeyBase key in selectedKeys)
        if (!key.Repeating)
          return false;
      return true;
    }

    public bool HasOnlySingleKeys()
    {
      foreach (KeyBase key in selectedKeys)
        if (key is KeyGroup)
          return false;
      return true;
    }

    public bool CanBeTall()
    {
      if (Count != 2)
        return false;

      // Two keys selected....

      if (GetGroupType() != KeyGroupType.NoGroup)
        return false;

      // Both keys can be grouped...

      KeyBase key1 = selectedKeys[0];
      KeyBase key2 = selectedKeys[1];

      if (key1.Column != key2.Column)
        return false;

      // Same column...

      if (Math.Abs(key1.Row - key2.Row) != 1)
        return false;

      // Adjacent rows...

      int topRow = Math.Min(key1.Row, key2.Row);

      if (topRow == 1 || topRow > 5)    // Tall keys cannot start on row 1 (zero indexed) or on or after row 6.
        return false;

      return true;
    }

    public bool CanBeWide()
    {
      if (Count != 2)
        return false;

      // Two keys selected....

      if (GetGroupType() != KeyGroupType.NoGroup)
        return false;

      // Both keys can be grouped...

      KeyBase key1 = selectedKeys[0];
      KeyBase key2 = selectedKeys[1];

      if (key1.Row != key2.Row)
        return false;

      // Same row...
      
      int row = key1.Row;

      if (Math.Abs(key1.Column - key2.Column) != 1)
        return false;

      // Adjacent columns...

      int leftColumn = Math.Min(key1.Column, key2.Column);

      if (row <= 1)  // top two rows have 9 keys...
      {
        if (leftColumn > 7)    // Wide keys cannot start on or after column 7 (zero indexed).
          return false;
      }
      else  // bottom section
      {
        if (leftColumn == 1 || leftColumn == 5 || leftColumn > 6)    // Wide keys in the bottom section cannot start on column 1, 5, 7 or greater (zero indexed).
          return false;
      }

      return true;
    }

    private int GetSmallestValueKeyIndex(KeyBase[] keys, Func<int, int> getValue)
    {
      int index = -1;
      int lowestValue = -1;
      for (int i = 0; i < keys.Length; i++)
      {
        int value = getValue(i);

        if (lowestValue == -1 || value < lowestValue)
        {
          lowestValue = value;
          index = i;
        }
      }
      return index;
    }

   
    int GetTopKeyIndex(KeyBase[] keys)
    {
      return GetSmallestValueKeyIndex(keys, i => keys[i].Row);
    }

    int GetLeftKeyIndex(KeyBase[] keys)
    {
      return GetSmallestValueKeyIndex(keys, i => keys[i].Column);
    }

    bool HasKeyAt(KeyBase[] keys, int column, int row)
    {
      foreach (KeyBase key in keys)
        if (key.Column == column && key.Row == row)
          return true;
      return false;
    }

    KeyBase GetTopLeftKey(KeyBase[] keys)
    {
      int topKeyIndex = GetTopKeyIndex(keys);
      int leftKeyIndex = GetLeftKeyIndex(keys);

      KeyBase topLeftKey = null;

      int leftColumn = keys[leftKeyIndex].Column;
      int topRow = keys[topKeyIndex].Row;

      for (int i = 0; i < keys.Length; i++)
        if (keys[i].IsAt(leftColumn, topRow))
          topLeftKey = keys[i];
      return topLeftKey;
    }
    
    public bool CanBeSquare()
    {
      if (Count != 4)
        return false;

      // Four keys selected....

      if (GetGroupType() != KeyGroupType.NoGroup)
        return false;

      // All four keys can be grouped...

      KeyBase[] keys = new KeyBase[4];
      keys[0] = selectedKeys[0];
      keys[1] = selectedKeys[1];
      keys[2] = selectedKeys[2];
      keys[3] = selectedKeys[3];

      KeyBase topLeftKey = GetTopLeftKey(keys);

      if (topLeftKey == null)
        return false;

      // OK, we have the top left key. Now let's see if the other three keys are in the positions we're expecting...

      if (!(HasKeyAt(keys, topLeftKey.Column + 1, topLeftKey.Row) && HasKeyAt(keys, topLeftKey.Column, topLeftKey.Row + 1) && HasKeyAt(keys, topLeftKey.Column + 1, topLeftKey.Row + 1)))
        return false;

      // OK, we now know the four keys are adjacent. Finally, let's make sure the four keys are in a legal position on the x-keys...

      if (topLeftKey.Row == 1 || topLeftKey.Row > 5)
        return false;

      if (topLeftKey.Row == 0 && topLeftKey.Column > 7)
        return false;

      if (topLeftKey.Row > 1)
        if (topLeftKey.Column == 1 || topLeftKey.Column == 5 || topLeftKey.Column > 6)
          return false;

      return true;
    }

    public string GetName()
    {
      if (Count == 0)
        return String.Empty;

      string compareName = selectedKeys[0].Name;
      for (int i = 1; i < Count; i++)
        if (selectedKeys[i].Name != compareName)
          return String.Empty;

      return compareName;
    }

    public KeyGroupType GetGroupType()
    {
      if (Count != 1)
        return KeyGroupType.NoGroup;

      KeyGroup xkeyGroup = selectedKeys[0] as KeyGroup;
      if (xkeyGroup != null)
        return xkeyGroup.Type;

      return KeyGroupType.NoGroup;
    }

    public void AddKey(KeyBase key)
    {
      selectedKeys.Add(key);
    }

    public int Count
    {
      get
      {
        return selectedKeys.Count;
      }
    }

    public List<KeyBase> SelectedKeys
    {
      get
      {
        return selectedKeys;
      }
    }
  }
}
