using System;
using System.Collections;
using System.Collections.Specialized;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Diagnostics.General;
using System.Windows.Forms;

namespace CR_XkeysEngine
{
	public enum MatchQuality {NoMatch, FullMatch}

	/// <summary>
	/// Binds a command with a keystroke and a context.
	/// </summary>
  public class CommandKeyBinding : ICloneable 
	{
		#region private fields...
		private CommandKeyFolder _ParentFolder;

		private bool _Enabled = true;
		
		private bool _CtrlKeyDown;
    private bool _AnyShiftModifier;
		private bool _AltKeyDown;
		private bool _ShiftKeyDown;

		private string _Command = String.Empty;
		private string _Parameters = String.Empty;
		private string _CustomData = String.Empty;
		private string _Comments = String.Empty;
		private ContextPickerData _Context = new ContextPickerData();
		private ContextNode _ContextRoot;
		#endregion

		// constructors...
		#region CommandKeyBinding
		public CommandKeyBinding()
		{
		}
		#endregion

		// public properties...
		#region Command
		public string Command
		{
			get
			{
				return _Command;
			}
			set
			{
				_Command = value;
			}
		}
		#endregion
		#region Parameters
		public string Parameters
		{
			get
			{
				return _Parameters;
			}

			set
			{
				_Parameters = value;
			}
		}
		#endregion
		#region Comments
		public string Comments
		{
			get
			{
				return _Comments;
			}

			set
			{
				_Comments = value;
			}
		}
		#endregion

		string GetShiftKeysAsStr()
		{
      if (_AnyShiftModifier)
        return "";
			return DevExpress.CodeRush.Core.CodeRush.Key.GetName(0, 0, _CtrlKeyDown, _ShiftKeyDown, _AltKeyDown);
		}

		public string CustomBindingName
		{
			get
			{
				return String.Format("{0}{1}", GetShiftKeysAsStr(), CustomData);
			}
		}
		
		#region ShiftKeyDown
		public bool ShiftKeyDown
		{
			get
			{
				return _ShiftKeyDown;
			}
		
			set
			{
				_ShiftKeyDown = value;
			}
		}
		#endregion
		#region AltKeyDown
		public bool AltKeyDown
		{
			get
			{
				return _AltKeyDown;
			}
		
			set
			{
				_AltKeyDown = value;
			}
		}
		#endregion
		#region CtrlKeyDown
		public bool CtrlKeyDown
		{
			get
			{
				return _CtrlKeyDown;
			}
		
			set
			{
				_CtrlKeyDown = value;
			}
		}
		#endregion

    #region AnyShiftModifier
    public bool AnyShiftModifier
    {
      get
      {
        return _AnyShiftModifier;
      }

      set
      {
        _AnyShiftModifier = value;
      }
    }
    #endregion

    
    public string GetDisplayShortcut(KeyLayout xkeyLayout = null)
    {
      if (xkeyLayout == null)
        return CustomBindingName;
      return xkeyLayout.GetBindingName(_AnyShiftModifier, _CtrlKeyDown, _ShiftKeyDown, _AltKeyDown, CustomData);
    }

    #region DisplayShortcut
		public string DisplayShortcut
		{
			get
			{
        return GetDisplayShortcut();
			}
		}
		#endregion
		#region DisplayCommand
		public string DisplayCommand
		{
			get
			{
				if (Parameters != "")
				{
					return String.Format("{0}({1})", Command, Parameters);
				}
				else 
				{
					return Command;
				}
			}
		}
		#endregion

		/// <summary>
		/// Returns true if current binding is available for executing.
		/// </summary>
		public bool IsAvailable
		{
			get
			{
				try
				{
					string parameters = _Parameters;

					DevExpress.CodeRush.Core.Action action = GetAction();
					if (action != null)
						return action.GetAvailability(parameters);

					string fullName = GetFullCommandName();

					// here we can not tell if command is available or not.
					return CodeRush.Command.Exists(fullName);
				}
				catch (Exception ex)
				{
					Log.SendException(ex);
					return false;
				}
			}
		}

		// private methods...
		string GetFullCommandName()
		{
			string commandName = _Command;
			if (commandName.IndexOf(".") < 0)
				commandName = "CodeRush." + commandName; // This code must be changed if we ever change the add-in name to "DXCore".
			return commandName;
		}

    public MatchQuality Matches(CustomInputShortcut customInputShortcut)
    {
      if (customInputShortcut.CustomData == CustomData)
        return MatchQuality.FullMatch;
      else
        return MatchQuality.NoMatch;
    }
		// public properties 
		#region Context
		public ContextPickerData Context
		{
			get
			{
				return _Context;
			}
		}
		#endregion
		#region ReadableContext
		public string ReadableContext
		{
			get
			{
				return GetReadableContext(new ContextNameDecorator(DefaultNameDecorator));
			}   
		}
		#endregion
		#region SetContext
		public void SetContext(ContextPickerData aContext)
		{
			if (aContext == null)				
			{
				_Context = aContext;
				_ContextRoot = null;
			}			
			else
			{
				_Context = aContext.Clone() as ContextPickerData;
				_ContextRoot = new ContextNode(aContext.Selected);
			}
		}
		#endregion

		#region Enabled
		public bool Enabled
		{
			get
			{
				return _Enabled;
			}
			set
			{
				_Enabled = value;
			}
		}
		#endregion

		// public methods
		public DevExpress.CodeRush.Core.Action GetAction()
		{
			DevExpress.CodeRush.Core.Action action = CodeRush.Actions.Get(_Command);
			if (action != null)
				return action;

			string parameters;
			string isolatedAction = _Command;
			CodeRush.StrUtil.SeparateParams(ref isolatedAction, out parameters);
			if (isolatedAction != _Command)
				return CodeRush.Actions.Get(isolatedAction);

			return null;
		}

		#region Execute
		public bool Execute()
		{
			string lParams = _Parameters;

			DevExpress.CodeRush.Core.Action codeRushCommand = GetAction();
			if (codeRushCommand != null)
			{
				bool handled = true;
				codeRushCommand.DoExecute(lParams, ref handled);
				return handled;
			}

			string commandName = GetFullCommandName();

			if (CodeRush.Command.Exists(commandName))
				return CodeRush.Command.Execute(commandName, _Parameters);
			else
				return false;
		}
		#endregion
		
		#region KeyStateMatches
		/// <summary>
		/// Returns true if the key state matches the expected state.
		/// </summary>
		/// <param name="keyToMatch">The key to match: one of Keys.Control, Keys.Shift, or Keys.Alt.</param>
		/// <param name="expectedState">true == down, false == up</param>
		private static bool KeyStateMatches(Keys keyToMatch, bool expectedState)
		{
			if ((Control.ModifierKeys & keyToMatch) == keyToMatch)		// Key is down.
				return expectedState;
			else  // Key is up (not pressed)
				return !expectedState;		// Return true if we were expecting the key to be up.
		}
		#endregion
		#region ShiftKeysMatch
    private bool ShiftKeysMatch()
		{
      if (AnyShiftModifier)
        return true;
			return KeyStateMatches(Keys.Control, CtrlKeyDown) && 
						 KeyStateMatches(Keys.Shift, ShiftKeyDown) && 
						 KeyStateMatches(Keys.Alt, AltKeyDown);
		}
		#endregion

		public string CustomData
		{
			get
			{
				return _CustomData;
			}
			set
			{
				_CustomData = value;
			}
		}

    #region Matches(string customData)
    public MatchQuality Matches(string customData)
    {
      if (!ShiftKeysMatch())
        return MatchQuality.NoMatch;

      if (CustomData == customData)
        if (ContextMatches())
          return MatchQuality.FullMatch;

      return MatchQuality.NoMatch;
    }
    #endregion
    
 
		internal bool ShouldLogDataForBinding()
		{ 
			return DevExpress.CodeRush.Core.KeyboardServices.LogShortcutChecks && !String.IsNullOrEmpty(DisplayCommand) && DisplayCommand.StartsWith("Embed");
		}
		#region AffirmativeContextMatches
		private bool AffirmativeContextMatches()
		{
			bool rootIsNull = _ContextRoot == null;
			if (rootIsNull)
			{
				if (ShouldLogDataForBinding())
					DevExpress.CodeRush.Core.KeyboardServices.LogShortcutMessage("AffirmativeContextMatches - root is null");
				return true;
			}

			bool contextSatisfied = _ContextRoot.IsSatisfied(ShouldLogDataForBinding());
			if (ShouldLogDataForBinding())
				DevExpress.CodeRush.Core.KeyboardServices.LogShortcutMessage("AffirmativeContextMatches {0} _ContextRoot {1} ", contextSatisfied, _ContextRoot.Description);
			return  contextSatisfied;
		}
		#endregion
		#region NegativeContextMatches
		private bool NegativeContextMatches()
		{
			bool result = CodeRush.Context.Satisfied(_Context.Excluded, false);
			if (ShouldLogDataForBinding())
				DevExpress.CodeRush.Core.KeyboardServices.LogShortcutMessage("NegativeContextMatches {0} _Context.Excluded {1} ", result, _Context.Excluded.ToString());
			return result;
		}
		#endregion

		// TODO: Move to CodeRush.State
		public ContextProviderBase[] GetStateContextProviders()
		{
			StateProviderBase[] lProviders = CodeRush.States.Providers;
			if (lProviders == null)
				return new ContextProviderBase[0];
			ContextProviderBase[] lContextProviders = new ContextProviderBase[lProviders.Length];
			for (int i = 0; i < lProviders.Length; i++)
				lContextProviders[i] = lProviders[i].StateContext;
			return lContextProviders;
		}
		private ContextProviderBase[] FilterSatisfiedProviders(ContextProviderBase[] providers)
		{
			if (providers == null || providers.Length == 0)
				return new ContextProviderBase[0];
			ArrayList lSatisfied = new ArrayList();
			for (int i = 0; i < providers.Length; i++)
			{
				ContextProviderBase lProvider = providers[i];
				if (CodeRush.Context.Satisfied(lProvider.ProviderName) == ContextResult.Satisfied)
					lSatisfied.Add(lProvider);
			}
			ContextProviderBase[] lResult = new ContextProviderBase[lSatisfied.Count];
			lSatisfied.CopyTo(lResult);
			return lResult;
		}

		#region StateContextMatches
		private bool StateContextMatches()
		{
			ContextProviderBase[] lStateProviders = GetStateContextProviders();
			ContextProviderBase[] lSatisfiedProviders = FilterSatisfiedProviders(lStateProviders);
			bool lIsAnyStateActive = lSatisfiedProviders.Length > 0;
			if (lIsAnyStateActive)
			{
				for (int i = 0; i < lSatisfiedProviders.Length; i++)
				{
					ContextProviderBase lProvider = lSatisfiedProviders[i];
					if (_Context.Selected.Contains(lProvider.ProviderName))
					{
						DevExpress.CodeRush.Core.KeyboardServices.LogShortcutMessage("StateContextMatches inside loop");
						return true;
					}
				}
				DevExpress.CodeRush.Core.KeyboardServices.LogShortcutMessage("!StateContextMatches");
				return false;
			}
			DevExpress.CodeRush.Core.KeyboardServices.LogShortcutMessage("StateContextMatches");
			return true;
		}
		#endregion
		#region ContextMatches
		internal bool ContextMatches()
		{
			return NegativeContextMatches() && AffirmativeContextMatches() && StateContextMatches();
		}
		#endregion

		#region ChangeParent
		private void ChangeParent(CommandKeyFolder newParent)
		{
			if (_ParentFolder != null)
				_ParentFolder.RemoveBinding(this);
			if (newParent != null)
				newParent.AddBinding(this);
		}
		#endregion
		#region DeleteBinding
		protected internal void DeleteBinding()
		{
			if (ParentFolder != null)
				ParentFolder = null;
		}
		#endregion

		#region Save
		public void Save(DecoupledStorage decoupledStorage, string section, bool isSelected)
		{
      decoupledStorage.WriteString(section, "CustomData", _CustomData);
      decoupledStorage.WriteString(section, "Command", _Command);
			decoupledStorage.WriteString(section, "Comments", _Comments);
			decoupledStorage.WriteString(section, "Parameters", _Parameters);
			decoupledStorage.WriteBoolean(section, "ShiftKeyDown", _ShiftKeyDown);
			decoupledStorage.WriteBoolean(section, "CtrlKeyDown", _CtrlKeyDown);
      decoupledStorage.WriteBoolean(section, "AnyShiftModifier", _AnyShiftModifier);
			decoupledStorage.WriteBoolean(section, "AltKeyDown", _AltKeyDown);
			decoupledStorage.WriteBoolean(section, "Enabled", _Enabled);
			_Context.Save(decoupledStorage,section,"Context");
			decoupledStorage.WriteBoolean(section, "Selected", isSelected);
		}
		#endregion
		#region Load
		/// <summary>
		/// Loads the command binding from the specified section.
		/// </summary>
		/// <param name="decoupledStorage"></param>
		/// <param name="section"></param>
		/// <returns>Returns true if this command was most recently selected.</returns>
		public bool Load(DecoupledStorage decoupledStorage, string section)
		{
      _CustomData = decoupledStorage.ReadString(section, "CustomData", "");
      _Command = decoupledStorage.ReadString(section, "Command", "");
			_Comments = decoupledStorage.ReadString(section, "Comments", "");
			_Parameters = decoupledStorage.ReadString(section, "Parameters", "");
			_CtrlKeyDown = decoupledStorage.ReadBoolean(section, "CtrlKeyDown", false);
      _AnyShiftModifier = decoupledStorage.ReadBoolean(section, "AnyShiftModifier", false);
			_AltKeyDown = decoupledStorage.ReadBoolean(section, "AltKeyDown", false);
			_ShiftKeyDown = decoupledStorage.ReadBoolean(section, "ShiftKeyDown", false);
			_Enabled = decoupledStorage.ReadBoolean(section, "Enabled", true);
			_Context.Load(decoupledStorage,section, "Context");
			_ContextRoot = new ContextNode(_Context.Selected);
			return decoupledStorage.ReadBoolean(section, "Selected", false);
		}
		#endregion

		#region ICloneable Members
		public CommandKeyBinding Clone()
		{
			CommandKeyBinding result = new CommandKeyBinding();

			result.Command = Command;
			result.Comments = Comments;
			result.CustomData= CustomData;
			result.Parameters = Parameters;
			result.SetContext(Context);
			result.AltKeyDown = AltKeyDown;
			result.CtrlKeyDown = CtrlKeyDown;
      result.AnyShiftModifier = AnyShiftModifier;
			result.ShiftKeyDown = ShiftKeyDown;
									
			return result;
		}

		object System.ICloneable.Clone()
		{
			return Clone();
		}

		#endregion
		#region DefaultNameDecorator
		private string DefaultNameDecorator(string name, string path)
		{
			return name;
		}
		#endregion
		#region GetReadableContext
		public string GetReadableContext(ContextNameDecorator decorator)
		{
			string lResult = "";
			if (_ContextRoot == null)
			{
				if (_Context == null)
					lResult = "";
				_ContextRoot = new ContextNode(_Context.Selected);
			}
			lResult = _ContextRoot.GetReadableContext(_Context, decorator);
			return lResult;
		}
		#endregion

		#region SetParentFolder
		protected internal void SetParentFolder(CommandKeyFolder parentFolder)
		{
			_ParentFolder = parentFolder;
		}
		#endregion

		#region ParentFolder
		public CommandKeyFolder ParentFolder
		{
			get
			{
				return _ParentFolder;
			}
			set
			{
				if (_ParentFolder == value)
					return;
				ChangeParent(value);
			}
		}
		#endregion
		#region IsCompletelyEnabled
		public bool IsCompletelyEnabled
		{
			get
			{
				CommandKeyFolder lParentFolder = ParentFolder;
				if (lParentFolder == null)
					return Enabled;
				return lParentFolder.IsCompletelyEnabled && Enabled;
			}
		}
		#endregion

		public override string ToString()
		{
			return string.Format("{0} '{1}'", DisplayCommand, DisplayShortcut);
		}
  }
}
