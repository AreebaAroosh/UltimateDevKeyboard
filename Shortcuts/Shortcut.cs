using System;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
	/// <summary>
	/// Represents a keystroke in the IDE.
	/// </summary>
	public class Shortcut: ShortcutBase, IShortcut, ICloneable
	{
		#region private fields...
		private bool _RightSide;
		private bool _KeyDownIsRepeated;
		private int _OriginalKeyState;
		#endregion

		// constructors...
		#region Shortcut
		public Shortcut()
		{
		}
		#endregion
		#region Shortcut(KeyPressedEventArgs e)
		public Shortcut(KeyPressedEventArgs e)
		{
			Assign(e);
		}
		#endregion

    // public overridden methods...
		#region Equals
    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (obj.GetType() != typeof(Shortcut))
        return false;

      Shortcut lShortcut = (Shortcut)obj;

      bool lResult = false;

      lResult = (lShortcut._Alt == _Alt) &&
                (lShortcut._Ctrl == _Ctrl) && 
                (lShortcut._Shift == _Shift) && 
                (lShortcut._KeyCode == _KeyCode) &&
                (lShortcut._Extended == _Extended);

			if (OptXkeysShortcuts.SeparateAltKeys)
				lResult = lResult && (lShortcut._RightSide == _RightSide); 

      if (lShortcut._KeyDownIsRepeated)
        return lResult && (CompareKeyState(lShortcut._OriginalKeyState, _KeyState));

      return lResult && (CompareKeyState(lShortcut._KeyState, _KeyState));
    }
		#endregion
		#region GetHashCode
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion
		#region CompareKeyState(int first, int second)
		bool CompareKeyState(int first, int second)
		{
			//return first == second;
			uint mask = 0xE100FFF0;
			return (first & mask) == (second & mask);
		}
		#endregion
		
		// public methods...
    #region Assign(KeyPressedEventArgs e)
    public void Assign(KeyPressedEventArgs e)
    {
      _Alt = e.AltKeyDown;
      _Shift = e.ShiftKeyDown;
      _Ctrl = e.CtrlKeyDown;
      //_RightSide = (e.VirtualKey != e.VirtualKeyLeftRight);
			_RightSide = e.RAltPressed;
			_Extended = e.ExtendedKey;
      _KeyDownIsRepeated = e.KeyDownIsRepeated;
      _KeyState = e.KeyState;
      _OriginalKeyState = e.OriginalKeyState;
      _KeyCode = e.KeyCode;
    }
    #endregion
    #region Clear
		public void Clear()
    {
			_Alt = false;
			_Shift = false;
			_Ctrl = false;
			_RightSide = false;		// For right-alt combinations....
			_Extended = false;
			_KeyState = 0;
			_KeyCode = 0;
    }
		#endregion
    #region Clone
    public Shortcut Clone()
    {
      Shortcut lResult = new Shortcut();

      lResult._Alt = this._Alt;
      lResult._Shift = this._Shift;
      lResult._Ctrl = this._Ctrl;
      lResult._RightSide = this._RightSide;
      lResult._Extended = this._Extended;
      lResult._KeyState = this._KeyState;
      lResult._KeyCode = this._KeyCode;

      return lResult;
    }
    #endregion
    #region Matches
		/// <summary>
    /// Returns true if aShortcut matches this shortcut.
    /// </summary>
		public bool Matches(Shortcut sc)
    {
			bool lResult = false;

			lResult = (sc._KeyCode == _KeyCode) &&
								(sc._Alt == _Alt) &&
								(sc._Ctrl == _Ctrl) &&
								(sc._Shift == _Shift) &&
								(sc._Extended == _Extended) &&
								(sc._RightSide == _RightSide);

			if (sc._KeyDownIsRepeated)
				return lResult && (sc._OriginalKeyState == _KeyState);

			return lResult && (sc._KeyState == _KeyState);
    }
		#endregion

		#region Load
		public void Load(DecoupledStorage storage, string section, string prefix)
		{
			_KeyState = storage.ReadInt32(section, prefix + "KeyState");
			_KeyCode = storage.ReadInt32(section, prefix + "KeyCode");
			_Ctrl = storage.ReadBoolean(section, prefix + "Ctrl");
			_Shift = storage.ReadBoolean(section, prefix + "Shift");
			_Alt = storage.ReadBoolean(section, prefix + "Alt");
			_RightSide = storage.ReadBoolean(section, prefix + "RightSide");
			_Extended = storage.ReadBoolean(section, prefix + "Extended");
		}
		#endregion
		#region Save
		public void Save(DecoupledStorage storage, string section, string prefix)
		{
			storage.WriteInt32(section, prefix + "KeyState", _KeyState);
			storage.WriteInt32(section, prefix + "KeyCode", _KeyCode);
			storage.WriteBoolean(section, prefix + "Ctrl", _Ctrl);
			storage.WriteBoolean(section, prefix + "Shift", _Shift);
			storage.WriteBoolean(section, prefix + "Alt", _Alt);
			storage.WriteBoolean(section, prefix + "RightSide", _RightSide);
			storage.WriteBoolean(section, prefix + "Extended", _Extended);
		}
		#endregion

    // public properties...
    #region Assigned
    /// <summary>
    /// Returns true if this shortcut has been assigned a valid value.
    /// </summary>
    public bool Assigned
    {
      get
      {
        return (_KeyState != 0 || _KeyCode != 0);
      }
    }
    #endregion
    #region DisplayName
    public string DisplayName
    {
      get
      {
        string lResult = CodeRush.Key.GetName(_KeyState, _KeyCode, _Ctrl, _Shift, _Alt);

        if (_RightSide && _Alt)
          lResult = lResult + " (Right Alt)";

        return lResult;
      }
    }
    #endregion
    #region VisualStudioBindingName
    public string VisualStudioBindingName
    {
      get
      {
        string lResult = CodeRush.Key.GetVisualStudioBindingName(_KeyState, _KeyCode, _Ctrl, _Shift, _Alt);

        if (_RightSide && _Alt)
          lResult = lResult + " (Right Alt)";

        return lResult;
      }
    }
    #endregion
    #region RightSide
    public bool RightSide
    {
      get
      {
        return _RightSide;
      }
      set
      {
        if (_RightSide == value)
          return;

        _RightSide = value;
        OnChanged();
      }
    }
    #endregion
    #region KeyDownIsRepeated
    public bool KeyDownIsRepeated
    {
      get
      {
        return _KeyDownIsRepeated;
      }
      set
      {
        if (_KeyDownIsRepeated == value)
          return;

        _KeyDownIsRepeated = value;
        OnChanged();
      }
    }
    #endregion
    #region OriginalKeyState
    public int OriginalKeyState
    {
      get
      {
        return _OriginalKeyState;
      }
      set
      {
        if (_OriginalKeyState == value)
          return;

        _OriginalKeyState = value;
        OnChanged();
      }
    }
    #endregion

    // ICloneable methods...
		#region ICloneable.Clone
		object ICloneable.Clone()
		{
			return Clone();
		}
		#endregion
	}
}