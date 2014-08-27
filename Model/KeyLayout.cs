using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  public class KeyLayout : IKeyGetter
  {
    List<KeyBase> keys = new List<KeyBase>();

    public KeyLayout()
    {

    }

    public List<KeyBase> GetPressedKeys(string customData)
    {
      return Hardware.Keyboard.GetPressedKeys(customData, this as IKeyGetter);
    }

    public string GetBindingName(bool anyShiftModifier, bool ctrlKeyDown, bool shiftKeyDown, bool altKeyDown, string customData)
    {
      return XkeysUtils.GetKeyName(anyShiftModifier, ctrlKeyDown, shiftKeyDown, altKeyDown, GetPressedKeys(customData));
    }

    public string GetCodeStr()
    {
      return Hardware.Keyboard.GetCodeStr(this as IKeyGetter);
    }

    public void Remove(KeyBase key)
    {
      keys.Remove(key);
    }

    public void UngroupSelection()
    {
      XkeySelection selection = GetSelection();
      foreach (KeyBase selectedKey in selection.SelectedKeys)
      {
        KeyGroup keyGroup = selectedKey as KeyGroup;
        if (keyGroup != null)
          keyGroup.Ungroup(this);
      }
      OnSelectionChanged();
    }

    void RemoveSelectedKeys()
    {
      XkeySelection selection = GetSelection();
      foreach (KeyBase selectedKey in selection.SelectedKeys)
        keys.Remove(selectedKey);
    }

    public void GroupSelection(KeyGroupType keysGroupType)
    {
      XkeySelection selection = GetSelection();
      RemoveSelectedKeys();
      KeyGroup keyGroup = new KeyGroup() { Type = keysGroupType, Name = selection.GetName() };
      int topmostRow = -1;
      int leftmostColumn = -1;
      foreach (KeyBase selectedKey in selection.SelectedKeys)
      {
        selectedKey.ClearSelection();
        keyGroup.Keys.Add(selectedKey);
        if (topmostRow == -1 || selectedKey.Row < topmostRow)
          topmostRow = selectedKey.Row;
        if (leftmostColumn == -1 || selectedKey.Column < leftmostColumn)
          leftmostColumn = selectedKey.Column;
      }
      keyGroup.Selected = true;
      keyGroup.SetPosition(leftmostColumn, topmostRow);
      keys.Add(keyGroup);
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

    public void MoveSelection(int deltaX, int deltaY, bool addToSelection)
    {
      int selectionColumn = -1;
      int keyWidth = 1;
      int keyHeight = 1;
      int selectionRow = -1;
      for (int i = 0; i < Keys.Count; i++)
        if (Keys[i].Selected)
        {
          KeyGroup keyGroup = Keys[i] as KeyGroup;
          if (keyGroup != null)
          {
            if (deltaX > 0)
              if (keyGroup.Type == KeyGroupType.Wide || keyGroup.Type == KeyGroupType.Square)
                keyWidth = 2;
            if (deltaY > 0)
              if (keyGroup.Type == KeyGroupType.Tall || keyGroup.Type == KeyGroupType.Square)
                keyHeight = 2;
          }
          selectionColumn = Keys[i].Column;
          selectionRow = Keys[i].Row;
          break;
        }
      if (selectionColumn == -1)
        return;

      
      int newColumn = selectionColumn + deltaX * keyWidth;
      int newRow = selectionRow + deltaY * keyHeight;
      if (Hardware.Keyboard.IsValidKey(newColumn, newRow))
        Select(newColumn, newRow, addToSelection, false);
    }

    public void SetRepeat(bool shouldRepeat)
    {
      for (int i = 0; i < Keys.Count; i++)
        if (Keys[i].Selected)
          Keys[i].SetRepeat(shouldRepeat);
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

    public KeyBase GetKey(int column, int row)
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
    public KeyBase GetSingleKey(int column, int row)
    {
      KeyBase key = GetKey(column, row);
      KeyGroup keyGroup = key as KeyGroup;
      if (keyGroup == null)
        return key;

      foreach (KeyBase subkey in keyGroup.Keys)
        if (subkey.IsAt(column, row))
          return subkey;

      return null;
    }

    public string GetKeyName(int column, int row)
    {
      KeyBase key = GetKey(column, row);
      if (key != null)
        return key.Name;
      else
        return null;
    }

    public bool IsSelected(int column, int row)
    {
      foreach (KeyBase key in Keys)
        if (key.IsSelected(column, row))
          return true;
      return false;
    }

    public List<KeyBase> Keys
    {
      get
      {
        return keys;
      }
    }

    public void Save()
    {
      DecoupledStorage storage = OptXkeysLayout.Storage;
      storage.ClearAll();
      storage.WriteInt32("Layout", "Count", keys.Count);
      for (int i = 0; i < Keys.Count; i++)
        Keys[i].Save(storage, "Layout", i);
    }

    public void BlockSettingsChanged()
    {
      Hardware.Keyboard.BlockSettingsChanged(Keys);
    }

    public void Load()
    {
      Keys.Clear();
      DecoupledStorage storage = OptXkeysLayout.Storage;
      int numKeys = storage.ReadInt32("Layout", "Count");
      for (int i = 0; i < numKeys; i++)
        Keys.Add(KeyBase.CreateAndLoad(storage, "Layout", i));
      BlockSettingsChanged();
    }
  }
}
