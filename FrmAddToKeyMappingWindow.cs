using System;
using System.Windows.Forms;
using System.Collections.Generic;
using DevExpress.CodeRush.Core;
using System.Collections;

namespace DX_ShortcutEngine
{
  public partial class FrmAddToKeyMappingWindow : Form
  {
    const string STR_GetKeyMappingWindowGroups = "Get Key Mapping Window Groups";
    
    public FrmAddToKeyMappingWindow()
    {
      InitializeComponent();

      Action actGetKeyMappingWindowGroups = CodeRush.Actions[STR_GetKeyMappingWindowGroups];
      object aVarIn = null;
      object aVarOut = null;
      if(actGetKeyMappingWindowGroups != null)
      {
        actGetKeyMappingWindowGroups.DoExecute(ref aVarIn, ref aVarOut);
        string[] groups = aVarOut as string[];
        if (groups != null && groups.Length > 0)
          cmbParentGroups.Properties.Items.AddRange(groups);
      }
      LoadCommands();
      DialogResult = System.Windows.Forms.DialogResult.Cancel;
    }
    private void LoadCommands()
    {
      cmbCommands.Properties.Items.Clear();

      ActionCollection actions = CodeRush.Actions;

      ArrayList actionNames = new ArrayList(actions.Count);
      foreach (Action action in actions)
      {
        if (!action.VisibleToUsers)
          continue;

        actionNames.Add(action.ActionName);
      }

      actionNames.Sort();

      cmbCommands.Properties.Items.AddRange((string[])actionNames.ToArray(typeof(string)));
    }

    // public properties...
    public string Description
    {
      get
      {
        return tbxDescription.Text;
      }
      set
      {
        tbxDescription.Text = value;
      }
    }
    public string Command
    {
      get
      {
        return cmbCommands.Text;
      }
      set
      {
        cmbCommands.Text = value;
      }
    }
    public string Shortcut
    {
      get
      {
        return tbxShortcut.Text;
      }
      set
      {
        tbxShortcut.Text = value;
      }
    }
    public string ParentGroup
    {
      get
      {
        return cmbParentGroups.Text;
      }
      set
      {
        cmbParentGroups.Text = value;
      }
    }

    // event handlers...
    private void btnAdd_Click(object sender, EventArgs e)
    {
      DialogResult = System.Windows.Forms.DialogResult.OK;
      Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {      
      Close();
    }    
  }
}
