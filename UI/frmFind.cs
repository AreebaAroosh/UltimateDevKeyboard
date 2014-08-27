using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.Diagnostics;
using DevExpress.CodeRush.Diagnostics.General;
using DevExpress.DXCore.Controls.XtraTreeList;
using DevExpress.DXCore.Controls.XtraTreeList.Nodes;
using DevExpress.DXCore.Controls.XtraTreeList.Columns;
using DevExpress.CodeRush.PlugInCore;
using DevExpress.DXCore.Controls.XtraTab;

namespace CR_XkeysEngine
{	
	public class frmFind: DXCoreForm
	{
		#region Private fields ...
		private TreeList _List;
		
		private CustomInputShortcut _CustomInputShortcut;
    private CustomInputShortcut _OldCustomInputShortcut;
		
		private string _OldFolderName;
		private string _OldCommandName;
		
		private int _FoldersActiveNumber;
		private int _CustomInputBindingsActiveNumber;
		private int _CommandActiveNumber;

		private ArrayList _Folders;
		private ArrayList _CustomInputBindings;
		private ArrayList _CommandBindings;
		#endregion

		#region private fields ...
		private XkeysEditControl edCustomInput;
		private XtraTabControl tabControl;
		private XtraTabPage tabFolder;
		private XtraTabPage tabCustomInputShortcut;
		private System.Windows.Forms.Label label1;
		private DevExpress.DXCore.Controls.XtraEditors.TextEdit edFolderName;
		private DevExpress.DXCore.Controls.XtraEditors.SimpleButton btnFindNext;
		private DevExpress.DXCore.Controls.XtraEditors.SimpleButton btnFind;
		private XtraTabPage tabCommand;
		private System.Windows.Forms.Label label2;
		private DevExpress.DXCore.Controls.XtraEditors.ComboBoxEdit cmbCommands;
		
		private System.ComponentModel.Container components = null;
		#endregion

		// constructors ...
		#region frmFind
		public frmFind()
		{		
			InitializeComponent();
			InitializeVariables();
		}
		#endregion

		//VS generated code ...
		#region Dispose
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				CleanUpVariables();
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.tabControl = new DevExpress.DXCore.Controls.XtraTab.XtraTabControl();
      this.tabCustomInputShortcut = new DevExpress.DXCore.Controls.XtraTab.XtraTabPage();
      this.edCustomInput = new CR_XkeysEngine.XkeysEditControl();
      this.tabFolder = new DevExpress.DXCore.Controls.XtraTab.XtraTabPage();
      this.edFolderName = new DevExpress.DXCore.Controls.XtraEditors.TextEdit();
      this.label1 = new System.Windows.Forms.Label();
      this.tabCommand = new DevExpress.DXCore.Controls.XtraTab.XtraTabPage();
      this.cmbCommands = new DevExpress.DXCore.Controls.XtraEditors.ComboBoxEdit();
      this.label2 = new System.Windows.Forms.Label();
      this.btnFindNext = new DevExpress.DXCore.Controls.XtraEditors.SimpleButton();
      this.btnFind = new DevExpress.DXCore.Controls.XtraEditors.SimpleButton();
      ((System.ComponentModel.ISupportInitialize)(this.Images16x16)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
      this.tabControl.SuspendLayout();
      this.tabCustomInputShortcut.SuspendLayout();
      this.tabFolder.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.edFolderName.Properties)).BeginInit();
      this.tabCommand.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.cmbCommands.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // tabControl
      // 
      this.tabControl.Location = new System.Drawing.Point(-4, 0);
      this.tabControl.Margin = new System.Windows.Forms.Padding(0);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedTabPage = this.tabCustomInputShortcut;
      this.tabControl.Size = new System.Drawing.Size(316, 217);
      this.tabControl.TabIndex = 0;
      this.tabControl.TabPages.AddRange(new DevExpress.DXCore.Controls.XtraTab.XtraTabPage[] {
            this.tabFolder,
            this.tabCustomInputShortcut,
            this.tabCommand});
      // 
      // tabCustomInputShortcut
      // 
      this.tabCustomInputShortcut.Controls.Add(this.edCustomInput);
      this.tabCustomInputShortcut.Name = "tabCustomInputShortcut";
      this.tabCustomInputShortcut.Size = new System.Drawing.Size(314, 192);
      this.tabCustomInputShortcut.Text = "X-keys";
      // 
      // edCustomInput
      // 
      this.edCustomInput.AltKeyDown = false;
      this.edCustomInput.AnyShiftModifier = false;
      this.edCustomInput.CtrlKeyDown = false;
      this.edCustomInput.Location = new System.Drawing.Point(8, 9);
      this.edCustomInput.Name = "edCustomInput";
      this.edCustomInput.ShiftKeyDown = false;
      this.edCustomInput.Size = new System.Drawing.Size(288, 180);
      this.edCustomInput.TabIndex = 17;
      this.edCustomInput.CustomInputChanged += new CR_XkeysEngine.CustomInputChangedEventHandler(this.edCustomInput_CustomInputChanged);
      // 
      // tabFolder
      // 
      this.tabFolder.Controls.Add(this.edFolderName);
      this.tabFolder.Controls.Add(this.label1);
      this.tabFolder.Margin = new System.Windows.Forms.Padding(0);
      this.tabFolder.Name = "tabFolder";
      this.tabFolder.Size = new System.Drawing.Size(314, 192);
      this.tabFolder.Text = "Folder";
      // 
      // edFolderName
      // 
      this.edFolderName.EditValue = "";
      this.edFolderName.Location = new System.Drawing.Point(84, 16);
      this.edFolderName.Name = "edFolderName";
      this.edFolderName.Size = new System.Drawing.Size(213, 20);
      this.edFolderName.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(8, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 25);
      this.label1.TabIndex = 0;
      this.label1.Text = "Folder name";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabCommand
      // 
      this.tabCommand.Controls.Add(this.cmbCommands);
      this.tabCommand.Controls.Add(this.label2);
      this.tabCommand.Name = "tabCommand";
      this.tabCommand.Size = new System.Drawing.Size(314, 192);
      this.tabCommand.Text = "Command";
      // 
      // cmbCommands
      // 
      this.cmbCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbCommands.EditValue = "";
      this.cmbCommands.Location = new System.Drawing.Point(76, 10);
      this.cmbCommands.Name = "cmbCommands";
      this.cmbCommands.Properties.Buttons.AddRange(new DevExpress.DXCore.Controls.XtraEditors.Controls.EditorButton[] {
            new DevExpress.DXCore.Controls.XtraEditors.Controls.EditorButton(DevExpress.DXCore.Controls.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.cmbCommands.Properties.CycleOnDblClick = false;
      this.cmbCommands.Properties.DropDownRows = 24;
      this.cmbCommands.Size = new System.Drawing.Size(228, 20);
      this.cmbCommands.TabIndex = 9;
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(8, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 24);
      this.label2.TabIndex = 2;
      this.label2.Text = "Command";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // btnFindNext
      // 
      this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFindNext.ImageIndex = 387;
      this.btnFindNext.ImageList = this.Images16x16;
      this.btnFindNext.Location = new System.Drawing.Point(211, 220);
      this.btnFindNext.Name = "btnFindNext";
      this.btnFindNext.Size = new System.Drawing.Size(88, 25);
      this.btnFindNext.TabIndex = 2;
      this.btnFindNext.Text = "&Find Next";
      this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
      // 
      // btnFind
      // 
      this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFind.ImageIndex = 388;
      this.btnFind.ImageList = this.Images16x16;
      this.btnFind.Location = new System.Drawing.Point(139, 220);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new System.Drawing.Size(68, 25);
      this.btnFind.TabIndex = 3;
      this.btnFind.Text = "&Find";
      this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
      // 
      // frmFind
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
      this.ClientSize = new System.Drawing.Size(306, 251);
      this.Controls.Add(this.btnFind);
      this.Controls.Add(this.btnFindNext);
      this.Controls.Add(this.tabControl);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmFind";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Find";
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmFind_KeyDown);
      ((System.ComponentModel.ISupportInitialize)(this.Images16x16)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
      this.tabControl.ResumeLayout(false);
      this.tabCustomInputShortcut.ResumeLayout(false);
      this.tabFolder.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.edFolderName.Properties)).EndInit();
      this.tabCommand.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.cmbCommands.Properties)).EndInit();
      this.ResumeLayout(false);

		}
		#endregion

		// private methods ..
		private void CleanUpVariables()
		{
			_List = null;

			_OldFolderName = "";
			_OldCustomInputShortcut = null;
			_OldCommandName = "";

			_CustomInputShortcut = null;			

			_FoldersActiveNumber = -1;
			_CustomInputBindingsActiveNumber = -1;
			_CommandActiveNumber = -1;

			_Folders = null;
			_CustomInputBindings = null;
			_CommandBindings = null;
		}
		#region InitializeVariables
		private void InitializeVariables()
		{
			CleanUpVariables();

			_Folders = new ArrayList();
			_CustomInputBindings = new ArrayList();
			_CommandBindings = new ArrayList();

			tabControl.SelectedTabPageIndex = 1;

			LoadCommands();
		}
		#endregion

		#region LoadCommands
		private void LoadCommands()
		{
			cmbCommands.Properties.Items.Clear();

			string[] lCommands = new string[CodeRush.Actions.Count];

			for(int i = 0; i < CodeRush.Actions.Count; i++)
				lCommands[i] = CodeRush.Actions[i].ActionName;

			Array.Sort(lCommands);
			
			for(int i = 0; i < lCommands.Length; i++)
				cmbCommands.Properties.Items.Add(lCommands[i]);
		}
		#endregion

		private void SetTargetTreeList(TreeList list)
		{
			_List = list;
		}

		#region ClearFolders
		private void ClearFolders()
		{
			if (_Folders == null)
				_Folders = new ArrayList();

			if (_Folders.Count > 0)
				_Folders.Clear();
		}
		#endregion
		
		#region ClearMouseBindings
		private void ClearCustomInputBindings()
		{
			if (_CustomInputBindings == null)
				_CustomInputBindings = new ArrayList();

			if (_CustomInputBindings.Count > 0)
				_CustomInputBindings.Clear();
		}
		#endregion
		#region ClearCommands
		private void ClearCommands()
		{
			if (_CommandBindings == null)
				_CommandBindings = new ArrayList();

			if (_CommandBindings.Count > 0)
				_CommandBindings.Clear();
		}
		#endregion

		#region FillFoldersRecursive
		private void FillFoldersRecursive(TreeListNodes nodes)
		{
			if (nodes == null || nodes.Count == 0)
				return;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (!(nodes[i].Tag is CommandKeyFolder))
					continue;
				CommandKeyFolder lFolder = nodes[i].Tag as CommandKeyFolder;
				if (FolderName.Length > 0 && lFolder.Name == FolderName)
					_Folders.Add(nodes[i]);

				FillFoldersRecursive(nodes[i].Nodes);
			}
		}
		#endregion
		
    #region FillCustomDataBindingsRecursive
    private void FillCustomDataBindingsRecursive(TreeListNodes nodes)
		{
			if (nodes == null || nodes.Count == 0 || _CustomInputShortcut == null)
				return;

			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i].Tag is CommandKeyFolder)
					FillCustomDataBindingsRecursive(nodes[i].Nodes);
				else 
				{
					CommandKeyBinding binding = nodes[i].Tag as CommandKeyBinding;
					if (binding == null)
						continue;

          if (binding.Matches(_CustomInputShortcut) == MatchQuality.FullMatch)
						_CustomInputBindings.Add(nodes[i]);
				}				
			}
		}
		#endregion
		#region FillCommandsRecursive
		private void FillCommandsRecursive(TreeListNodes nodes)
		{
			if (nodes == null || nodes.Count == 0)
				return;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i].Tag is CommandKeyFolder)
					FillCommandsRecursive(nodes[i].Nodes);
				else
				{
					CommandKeyBinding lBinding = nodes[i].Tag as CommandKeyBinding;
					if (lBinding == null)
						continue;
					if (lBinding.Command != null && lBinding.Command == CommandName)
						_CommandBindings.Add(nodes[i]);
				}
			}
		}
		#endregion

    #region CompareCustomInputShortcuts
    private bool CompareCustomInputShortcuts()
		{
			if (_OldCustomInputShortcut == null || _CustomInputShortcut == null)
				return false;

      if (_OldCustomInputShortcut.DisplayName != _CustomInputShortcut.DisplayName)
				return false;

			return true;
		}
		#endregion
		#region CompareCommands
		private bool CompareCommands()
		{
			if (_OldCommandName == null || CommandName == null)
				return false;

			return (_OldCommandName == CommandName);
		}
		#endregion

		#region RefreshFolders
		private void RefreshFolders()
		{
			if (_OldFolderName != FolderName)
				try
				{					
					ClearFolders();			
					FillFoldersRecursive(_List.Nodes);					
				}
				finally
				{
					_FoldersActiveNumber = -1;
					_OldFolderName = FolderName;
				}
		}
		#endregion

    #region RefreshCustomInputBindings
    private void RefreshCustomInputBindings()
		{
			if (!CompareCustomInputShortcuts())
				try
				{
					ClearCustomInputBindings();
					FillCustomDataBindingsRecursive(_List.Nodes);
				}
				finally
				{
					_CustomInputBindingsActiveNumber = -1;
					_OldCustomInputShortcut = _CustomInputShortcut;
				}
		}
		#endregion
		#region RefreshCommands
		private void RefreshCommands()
		{
			if (!CompareCommands())
				try
				{
					ClearCommands();
					FillCommandsRecursive(_List.Nodes);
				}
				finally
				{
					_CustomInputBindingsActiveNumber = -1;
					_OldCustomInputShortcut = _CustomInputShortcut;
				}
		}
		#endregion

		#region FindCustomNode
		private bool FindCustomNode(int folderNumber, ArrayList nodes)
		{
			if (nodes == null || nodes.Count == 0)
			{				
				if (_List.Nodes.Count > 0)
					_List.FocusedNode = _List.Nodes[0];

				MessageBox.Show("No items were found!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			try
			{	
				TreeListNode lNode = nodes[folderNumber] as TreeListNode;
				
				_List.FocusedNode = lNode;
				_List.MakeNodeVisible(lNode);
				
				lNode.Expanded = true;				
				_List.Refresh();
				return true;
			}
			catch
			{
				Log.SendError("Shortcuts : can not make the node visible");
				return false;
			}
		}
		#endregion

		#region FindFirstFolder
		private void FindFirstFolder()
		{
			RefreshFolders();
			_FoldersActiveNumber = 0;
			FindCustomNode(_FoldersActiveNumber, _Folders);
		}
		#endregion

    #region FindFirstCustomInputBinding
    private void FindFirstCustomInputBinding()
		{
			RefreshCustomInputBindings();
			_CustomInputBindingsActiveNumber = 0;
			FindCustomNode(_CustomInputBindingsActiveNumber, _CustomInputBindings);			
		}
		#endregion
		#region FindFirstCommand
		private void FindFirstCommand()
		{
			RefreshCommands();
			_CommandActiveNumber = 0;
			FindCustomNode(_CommandActiveNumber, _CommandBindings);			
		}
		#endregion

		#region FindNextFolder
		private void FindNextFolder()
		{						
			RefreshFolders();

			if (_Folders == null || _Folders.Count == 0)
				return;

			_FoldersActiveNumber++;
			if (_FoldersActiveNumber >= _Folders.Count)
				_FoldersActiveNumber = 0;

			FindCustomNode(_FoldersActiveNumber, _Folders);
		}
		#endregion
		
		#region FindNextMouseBinding
		private void FindNextCustomInputBinding()
		{
			RefreshCustomInputBindings();
			if (_CustomInputBindings == null || _CustomInputBindings.Count == 0)
				return;

			_CustomInputBindingsActiveNumber++;
			if (_CustomInputBindingsActiveNumber >= _CustomInputBindings.Count)
				_CustomInputBindingsActiveNumber = 0;

			FindCustomNode(_CustomInputBindingsActiveNumber, _CustomInputBindings);
		}
		#endregion
		#region FindNextCommand
		private void FindNextCommand()
		{
			RefreshCommands();
			if (_CommandBindings == null || _CommandBindings.Count == 0)
				return;

			_CommandActiveNumber++;
			if (_CommandActiveNumber >= _CommandBindings.Count)
				_CommandActiveNumber = 0;

			FindCustomNode(_CommandActiveNumber, _CommandBindings);
		}
		#endregion

		// event handlers
		#region btnFind_Click
		private void btnFind_Click(object sender, System.EventArgs e)
		{			
			switch (tabControl.SelectedTabPageIndex)
			{
				case 0:
					FindFirstFolder();
					break;
				case 1:
					FindFirstCustomInputBinding();
					break;
				case 2:
					FindFirstCommand();
					break;
			}			
		}
		#endregion
		#region btnFindNext_Click
		private void btnFindNext_Click(object sender, System.EventArgs e)
		{
			switch (tabControl.SelectedTabPageIndex)
			{
				case 0:
					FindNextFolder();
					break;
				case 1:
					FindNextCustomInputBinding();
					break;
				case 2:
					FindNextCommand();
					break;
			}
		}
		#endregion
		#region frmFind_KeyDown
		private void frmFind_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch (e.KeyData)
			{				
				case Keys.Escape:					
					DialogResult = DialogResult.Cancel;
					break;
			}
		}
		#endregion

		#region edKeyEdit_KeyChanged
		private void edCustomInput_CustomInputChanged(object sender, CR_XkeysEngine.CustomInputChangedEventArgs ea)
		{
      _CustomInputShortcut = ea.Shortcut;
		}
		#endregion
		
		
		// public methods ...
		#region Execute
		public static void Execute(TreeList list)
		{
			if (list == null)
				return;

			using (frmFind lForm = new frmFind())
			{
				lForm.SetTargetTreeList(list);
				lForm.ShowDialog();
			}
		}
		#endregion		

					
		// private properties ...
		#region FolderName
		private string FolderName
		{
			get
			{
				return edFolderName.Text;
			}
			set
			{
				edFolderName.Text = value;
			}
		}
		#endregion
		#region CommandsName
		private string CommandName
		{
			get
			{
				return cmbCommands.Text;
			}			
		}
		#endregion				
	}
}