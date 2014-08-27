using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  public class KeyBase
  {
    int column;
    int row;
    public string Name { get; set; }

    public virtual bool ClearSelection()
    {
      if (Selected)  // Selection changed.
      {
        Selected = false;
        return true;
      }

      return false;
    }

    public static KeyBase CreateAndLoad(DecoupledStorage storage, string section, int index)
    {
      KeyBase key = null;
      if (storage.ReadBoolean(section, "IsGroup" + index, false))
        key = new KeyGroup();
      else
        key = new Key();
      key.Load(storage, section, index);
      return key;
    }

    // TODO: Move GetRowDataValue out to the hardware interface so the files in the Model folder are hardware-independent.
    /// <summary>
    /// Returns the column value to match the key row data value if this key were down.
    /// </summary>
    public virtual byte GetRowDataValue()
    {
      return (byte)Math.Pow(2, Row);
    }

    public virtual void Save(DecoupledStorage storage, string section, int index)
    {
      if (Name != null)
        storage.WriteString(section, "Name" + index, Name);
      else
        storage.WriteString(section, "Name" + index, string.Empty);
      storage.WriteInt32(section, "Column" + index, column);
      storage.WriteInt32(section, "Row" + index, row);
      storage.WriteBoolean(section, "Repeating" + index, Repeating);
    }

    public virtual void Load(DecoupledStorage storage, string section, int index)
    {
      Name = storage.ReadString(section, "Name" + index);
      column = storage.ReadInt32(section, "Column" + index);
      row = storage.ReadInt32(section, "Row" + index);
      Repeating = storage.ReadBoolean(section, "Repeating" + index);
    }

    public bool IsSelected(int column, int row)
    {
      return IsAt(column, row) && Selected;
    }

    public virtual void Select(int column, int row)
    {
      if (IsAt(column, row))
        Selected = true;
    }

    public void SetRepeat(bool shouldRepeat)
    {
      Repeating = shouldRepeat;      
    }

    public virtual void ToggleSelection(int column, int row)
    {
      if (IsAt(column, row))
        Selected = !Selected;
    }

    public virtual bool IsAt(int column, int row)
    {
      return row == this.row && column == this.column;
    }

    public virtual void SetName(string newName)
    {
      Name = newName;
    }

    public void SetPosition(int column, int row)
    {
      this.row = row;
      this.column = column;      
    }

    // public properties...
    public int Column
    {
      get
      {
        return column;
      }
    }

    public virtual bool IsDown
    {
      get
      {
        return Hardware.Keyboard.IsDown(column, row);
      }
    }

    public int Row
    {
      get
      {
        return row;
      }
    }
    
    public bool Selected { get; set; }
    public bool Repeating { get; set; }
  }
}
