using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using DevExpress.CodeRush.Diagnostics.General;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.PlugInCore;
using System.Windows.Forms;
using DevExpress.CodeRush.Interop;
using DevExpress.DXCore.Win32;
using System.Collections.Generic;
using System.Threading;
using DevExpress.CodeRush.Win32;

namespace CR_XkeysEngine
{
	public class ShortcutsPlugIn: StandardPlugIn
	{
    const int INT_InitialDelay = 400;
    const int INT_RepeatInterval = 60;
		#region private fields...
    List<XkeyPressedEventArgs> keyBuffer = new List<XkeyPressedEventArgs>();
		CommandKeyFolder _RootFolder;
    SynchronizationContext _UIContext;
    string lastCustomData;
    private List<XkeyPressedEventArgs> repeatingKeys = new List<XkeyPressedEventArgs>();
    private System.Windows.Forms.Timer tmrRepeat;
    private XkeysEngine xkeysEngine;
    private DevExpress.CodeRush.Core.Action actSendKeys;
    private IContainer components;
    CommandKeyBindingCollection _Shortcuts;
		#endregion

		// constructors...
		#region ShortcutEnginePlugIn
		public ShortcutsPlugIn()
		{
      tmrRepeat = new System.Windows.Forms.Timer();
      tmrRepeat.Tick += tmrRepeat_Tick;
      InitializeComponent();
		}
		#endregion

		// initialization/finalization...
		#region InitializePlugIn
		public override void InitializePlugIn()
		{
      _UIContext = SynchronizationContext.Current;
			EnableActionExecuting = true;
			_Shortcuts = new CommandKeyBindingCollection();
			
      LoadShortcuts();

      Hardware.Keyboard.Connect();
      
      xkeysEngine = new XkeysEngine();
      xkeysEngine.XkeyPressed += xkeysEngine_XkeyPressed;
		}
		#endregion

		public override void FinalizePlugIn()
		{
		  //CodeRush.Interfaces.UnregisterShortcutEngine();
			base.FinalizePlugIn();
		}

		#region Component Designer generated code
		private void InitializeComponent()
		{
      this.components = new System.ComponentModel.Container();
      DevExpress.CodeRush.Core.Parameter parameter1 = new DevExpress.CodeRush.Core.Parameter(new DevExpress.CodeRush.Core.StringParameterType());
      this.actSendKeys = new DevExpress.CodeRush.Core.Action(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.actSendKeys)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      // 
      // actSendKeys
      // 
      this.actSendKeys.ActionName = "SendKeys";
      this.actSendKeys.CommonMenu = DevExpress.CodeRush.Menus.VsCommonBar.None;
      this.actSendKeys.Description = "Sends the specified keys to the IDE.";
      this.actSendKeys.ImageBackColor = System.Drawing.Color.Empty;
      parameter1.DefaultValue = "";
      parameter1.Description = "The keys to send. You can include special keys from System.Windows.Forms.Keys enu" +
    "m in the sequence if you embed them inside square brackets (e.g., [Up]).";
      parameter1.Name = "keys";
      parameter1.Optional = false;
      this.actSendKeys.Parameters.Add(parameter1);
      this.actSendKeys.ToolbarItem.ButtonIsPressed = false;
      this.actSendKeys.ToolbarItem.Caption = null;
      this.actSendKeys.ToolbarItem.Image = null;
      this.actSendKeys.ToolbarItem.ToolbarName = null;
      // 
      // ShortcutsPlugIn
      // 
      this.OptionsChanged += new DevExpress.CodeRush.Core.OptionsChangedEventHandler(this.ShortcutsPlugIn_OptionsChanged);
      ((System.ComponentModel.ISupportInitialize)(this.actSendKeys)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		#region ShortcutsPlugIn_OptionsChanged
		private void ShortcutsPlugIn_OptionsChanged(DevExpress.CodeRush.Core.OptionsChangedEventArgs e)
		{
			if (e.OptionsPages.Contains(typeof(OptXkeysShortcuts)))
			{
				try
				{
					LoadShortcuts();
				}
				catch (Exception ex)
				{
					Log.SendException(ex);
				}
			}
		}
		#endregion

    #region ExecuteBinding
    public void ExecuteBinding(CommandKeyBinding binding)
		{
      if (binding != null && binding.IsAvailable)
        binding.Execute();
		}
		#endregion

    // private methods...

		#region LoadShortcuts
		private void LoadShortcuts()
		{
			_RootFolder = new CommandKeyFolder();
			_RootFolder.Load(OptXkeysShortcuts.GetCategory(), OptXkeysShortcuts.GetPageName());
			_Shortcuts = _RootFolder.ToPlainCollection();
		}
		#endregion

		// public properties...
		public static bool EnableActionExecuting { get; set; }

    static bool OptionsDialogHasFocus()
    {
      Form optionsForm = CodeRush.Options.IntegratedOptionsDialog as Form;

      HWND foregroundWindowHandle = NativeMethods.GetForegroundWindow();

      return !(optionsForm == null || optionsForm.Handle != foregroundWindowHandle);
    }

    void HandleRepeatingKeys()
    {
      if (repeatingKeys.Count == 0)
        return;
      tmrRepeat.Interval = INT_InitialDelay;
      tmrRepeat.Enabled = true;
    }

    void tmrRepeat_Tick(object sender, EventArgs e)
    {
      FireRepeatingKeys();
      if (tmrRepeat.Enabled)
        tmrRepeat.Interval = INT_RepeatInterval;
      else
        tmrRepeat.Interval = INT_InitialDelay;
      tmrRepeat.Enabled = true;
    }

    void FireRepeatingKeys()
    {
      ExecuteShortcuts(repeatingKeys);
    }

    void CollectRepeatingKeys(List<XkeyPressedEventArgs> keyBuffer)
    {
      if (repeatingKeys.Count > 0)
        repeatingKeys.Clear();

      foreach (XkeyPressedEventArgs ea in keyBuffer)
        foreach (KeyBase xkeyBase in ea.KeysDown)
          if (xkeyBase.Repeating)
          {
            repeatingKeys.Add(ea);
            break;
          }
    }

    void ExecuteShortcuts(List<XkeyPressedEventArgs> keyBuffer)
    {
      List<CommandKeyBinding> shortcutsToExecute = new List<CommandKeyBinding>();
      foreach (XkeyPressedEventArgs ea in keyBuffer)
        foreach (CommandKeyBinding shortcut in _Shortcuts)
          if (shortcut.Enabled && shortcut.Matches(ea.CustomData) == MatchQuality.FullMatch)
            shortcutsToExecute.Add(shortcut);

      foreach (CommandKeyBinding shortcut in shortcutsToExecute)
        shortcut.Execute();
    }
    void ProcessShortcutsOnUIThread()
    {
      tmrRepeat.Enabled = false;

      if (OptionsDialogHasFocus())
      {
        lock (keyBuffer)
          keyBuffer.Clear();
        return;
      }

      List<XkeyPressedEventArgs> localKeyBuffer = new List<XkeyPressedEventArgs>();

      lock (keyBuffer)
      {
        localKeyBuffer.AddRange(keyBuffer);
        keyBuffer.Clear();
      }

      CollectRepeatingKeys(localKeyBuffer);
      ExecuteShortcuts(localKeyBuffer);
      HandleRepeatingKeys();
    }

    void xkeysEngine_XkeyPressed(object sender, XkeyPressedEventArgs e)
    {
      if (!CodeRush.IDE.IsActive)
        return;

      lock (keyBuffer)
      {
        lastCustomData = e.CustomData;
        keyBuffer.Add(e);
      }

      _UIContext.Post(s =>
      {
        ProcessShortcutsOnUIThread();
      }, null);
    }
	}
}