using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  public class XkeyLayout
  {
    List<XkeyBase> keys = new List<XkeyBase>();

    public XkeyLayout()
    {

    }

    public List<XkeyBase> GetPressedKeys(string customData)
    {
      List<XkeyBase> keysPressed = new List<XkeyBase>();
      string[] columns = customData.Split('.');
      for (int column = 0; column < columns.Length; column++)
      {
        int value;
        if (int.TryParse(columns[column], out value))
        {
          if (value == 0)
            continue;
          for (int row = 0; row < Hardware.Keyboard.NumRows; row++)
          {
            byte rowMask = (byte)(Math.Pow(2, row));
            if ((rowMask & value) == rowMask)
            {
              XkeyBase keyPressed = GetKey(column, row);
              if (keysPressed.Contains(keyPressed))
                continue;
              keysPressed.Add(keyPressed);
            }
          }
        }
      }
      return keysPressed;
    }

    public string GetBindingName(bool anyShiftModifier, bool ctrlKeyDown, bool shiftKeyDown, bool altKeyDown, string customData)
    {
      return XkeysUtils.GetKeyName(anyShiftModifier, ctrlKeyDown, shiftKeyDown, altKeyDown, GetPressedKeys(customData));
    }

    public string GetCodeStr()
    {
      string result = string.Empty;
      for (int column = 0; column < Hardware.Keyboard.NumColumns; column++)
      {
        byte thisValue = 0;
        for (int row = 0; row < Hardware.Keyboard.NumRows; row++)
          if (Hardware.Keyboard.IsValidKey(column, row))
          {
            XkeyBase xkey = GetKey(column, row);
            if (!xkey.IsDown)
              continue;

            XkeyBase singleXkey = GetSingleKey(column, row);
            if (singleXkey != null)
              thisValue += singleXkey.GetRowDataValue();
          }
        result += thisValue + ".";
      }
      return result.TrimEnd('.');
    }

    public void Remove(XkeyBase key)
    {
      keys.Remove(key);
    }

    public void UngroupSelection()
    {
      XkeySelection selection = GetSelection();
      foreach (XkeyBase selectedKey in selection.SelectedKeys)
      {
        XkeyGroup xkeyGroup = selectedKey as XkeyGroup;
        if (xkeyGroup != null)
          xkeyGroup.Ungroup(this);
      }
      OnSelectionChanged();
    }

    void RemoveSelectedKeys()
    {
      XkeySelection selection = GetSelection();
      foreach (XkeyBase selectedKey in selection.SelectedKeys)
        keys.Remove(selectedKey);
    }

    public void GroupSelection(XKeysGroupType xKeysGroupType)
    {
      XkeySelection selection = GetSelection();
      RemoveSelectedKeys();
      XkeyGroup xkeyGroup = new XkeyGroup();
      xkeyGroup.Type = xKeysGroupType;
      xkeyGroup.Name = selection.GetName();
      int topmostRow = -1;
      int leftmostColumn = -1;
      foreach (XkeyBase selectedKey in selection.SelectedKeys)
      {
        selectedKey.ClearSelection();
        xkeyGroup.Keys.Add(selectedKey);
        if (topmostRow == -1 || selectedKey.Row < topmostRow)
          topmostRow = selectedKey.Row;
        if (leftmostColumn == -1 || selectedKey.Column < leftmostColumn)
          leftmostColumn = selectedKey.Column;
      }
      xkeyGroup.Selected = true;
      xkeyGroup.SetPosition(leftmostColumn, topmostRow);
      keys.Add(xkeyGroup);
      OnSelectionChanged();
    }

    protected virtual void OnSelectionChanged()
    {
      EventHandler handler = SelectionChanged;
      if (handler != null)
        handler(this, EventArgs.Empty);
    }

    public event EventHandler SelectionChanged;

    public XkeySelection GetSelection()
    {
      XkeySelection selection = new XkeySelection();
      for (int i = 0; i < Keys.Count; i++)
        if (Keys[i].Selected)
          selection.AddKey(Keys[i]);
      return selection;
    }

    public void SetName(string newName)
    {
      for (int i = 0; i < Keys.Count; i++)
        if (Keys[i].Selected)
          Keys[i].SetName(newName);
    }

    public void ClearSelection()
    {
      bool selectionChanged = false;
      for (int i = 0; i < Keys.Count; i++)
        if (Keys[i].ClearSelection())
          selectionChanged = true;
      if (selectionChanged)
        OnSelectionChanged();
    }

    public bool Select(int column, int row, bool addToSelection, bool toggleSelection)
    {
      if (IsSelected(column, row) && !toggleSelection)
      {
        XkeySelection selection = GetSelection();
        if (selection.SelectedKeys.Count == 1)    // Already selected. No changes.
          return false;
      }

      if (!addToSelection && !toggleSelection)
        ClearSelection();

      for (int i = 0; i < Keys.Count; i++)
        if (toggleSelection)
          Keys[i].ToggleSelection(column, row);
        else
          Keys[i].Select(column, row);
      return true;
    }

    public XkeyBase GetKey(int column, int row)
    {
      for (int i = 0; i < Keys.Count; i++)
        if (Keys[i].IsAt(column, row))
          return Keys[i];
      return null;
    }

    /// <summary>
    /// Gets the single key at the specified column and row. If a group key 
    /// (tall, wide, or square) is at the specified location, then that group
    /// is drilled into to find and return the actual subkey at the column and 
    /// row.
    /// </summary>
    XkeyBase GetSingleKey(int column, int row)
    {
      XkeyBase key = GetKey(column, row);
      XkeyGroup keyGroup = key as XkeyGroup;
      if (keyGroup == null)
        return key;

      foreach (XkeyBase subkey in keyGroup.Keys)
        if (subkey.IsAt(column, row))
          return subkey;

      return null;
    }

    public string GetKeyName(int column, int row)
    {
      XkeyBase key = GetKey(column, row);
      if (key != null)
        return key.Name;
      else
        return null;
    }

    public bool IsSelected(int column, int row)
    {
      foreach (XkeyBase key in Keys)
        if (key.IsSelected(column, row))
          return true;
      return false;
    }

    public List<XkeyBase> Keys
    {
      get
      {
        return keys;
      }
    }

    public void Save()
    {
      DecoupledStorage storage = OptXkeysLayout.Storage;
      storage.WriteInt32("Layout", "Count", keys.Count);
      for (int i = 0; i < Keys.Count; i++)
        Keys[i].Save(storage, "Layout", i);
    }

    static void ClearBlockedKeysMask()
    {
      for (int column = 0; column < Hardware.Keyboard.NumColumns; column++)
        XkeysRaw.Data.BlockedKeysMask[column] = 0;
    }

    void SetBlockedKeys()
    {
      ClearBlockedKeysMask();
      
      foreach (XkeyBase key in Keys)
      {
        Xkey xkey = key as Xkey;
        if (xkey != null)
          if (xkey.IsBlocked)
            XkeysRaw.Data.BlockedKeysMask[xkey.Column] = (byte)(XkeysRaw.Data.BlockedKeysMask[xkey.Column] + (byte)Math.Pow(2, xkey.Row));
      }
    }
    public void Load()
    {
      Keys.Clear();
      DecoupledStorage storage = OptXkeysLayout.Storage;
      int numKeys = storage.ReadInt32("Layout", "Count");
      for (int i = 0; i < numKeys; i++)
        Keys.Add(XkeyBase.CreateAndLoad(storage, "Layout", i));
      SetBlockedKeys();
    }
  }
}
