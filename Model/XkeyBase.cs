using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  public class XkeyBase
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

    public static XkeyBase CreateAndLoad(DecoupledStorage storage, string section, int index)
    {
      XkeyBase key = null;
      if (storage.ReadBoolean(section, "IsGroup" + index, false))
        key = new XkeyGroup();
      else
        key = new Xkey();
      key.Load(storage, section, index);
      return key;
    }

    /// <summary>
    /// Returns the column value to match the Xkeys row data value if this key were down.
    /// </summary>
    public virtual byte GetRowDataValue()
    {
      return (byte)Math.Pow(2, Row);
    }

    public virtual void Save(DecoupledStorage storage, string section, int index)
    {
      if (Name != null)
        storage.WriteString(section, "Name" + index, Name);
      storage.WriteInt32(section, "Column" + index, column);
      storage.WriteInt32(section, "Row" + index, row);      
    }

    public virtual void Load(DecoupledStorage storage, string section, int index)
    {
      Name = storage.ReadString(section, "Name" + index);
      column = storage.ReadInt32(section, "Column" + index);
      row = storage.ReadInt32(section, "Row" + index);
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
        return XkeysRaw.Data.IsKeyDown(column, row);
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
  }
}
