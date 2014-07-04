using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.CodeRush.PlugInCore;

namespace CR_XkeysEngine
{
	public class NewFolderName : DXCoreForm
	{
		#region private consts
		private const string STR_ParentLblFormat = "(folder will be created inside \"{0}\")";
		private const string STR_NULLLENGTH_ERROR = "(Please type the name)";
		private const string STR_EXISTINGNAME_ERROR = "(This folder name already exists.)";		
		#endregion

		#region private Fields ...
		private StringCollection _ExistingFolders;
		private CommandKeyFolder _ParentFolder;
		private CommandKeyFolder _RootFolder;
		#endregion
		
		#region private fields ...
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label2;
		private DevExpress.DXCore.Controls.XtraEditors.SimpleButton btnApply;
		private DevExpress.DXCore.Controls.XtraEditors.SimpleButton btnCancel;
		private DevExpress.DXCore.Controls.XtraEditors.TextEdit edFolderName;
		private System.Windows.Forms.Label lblParent;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.CheckBox chkRoot;
		#endregion
		
		//---------------------------
		
		#region NewFolderName
		public NewFolderName()
		{
			InitializeComponent();
		}
		#endregion

		#region Dispose
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
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
      this.edFolderName = new DevExpress.DXCore.Controls.XtraEditors.TextEdit();
      this.label2 = new System.Windows.Forms.Label();
      this.btnApply = new DevExpress.DXCore.Controls.XtraEditors.SimpleButton();
      this.btnCancel = new DevExpress.DXCore.Controls.XtraEditors.SimpleButton();
      this.lblInfo = new System.Windows.Forms.Label();
      this.lblParent = new System.Windows.Forms.Label();
      this.chkRoot = new System.Windows.Forms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.Images16x16)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.edFolderName.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // edFolderName
      // 
      this.edFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.edFolderName.EditValue = "";
      this.edFolderName.Location = new System.Drawing.Point(84, 11);
      this.edFolderName.Name = "edFolderName";
      this.edFolderName.Size = new System.Drawing.Size(196, 20);
      this.edFolderName.TabIndex = 0;
      this.edFolderName.TextChanged += new System.EventHandler(this.edFolderName_TextChanged);
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(4, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(152, 24);
      this.label2.TabIndex = 4;
      this.label2.Text = "Folder name:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // btnApply
      // 
      this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnApply.Enabled = false;
      this.btnApply.Location = new System.Drawing.Point(124, 88);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new System.Drawing.Size(75, 25);
      this.btnApply.TabIndex = 1;
      this.btnApply.Text = "&OK";
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(204, 88);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 25);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      // 
      // lblInfo
      // 
      this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblInfo.Location = new System.Drawing.Point(4, 39);
      this.lblInfo.Name = "lblInfo";
      this.lblInfo.Size = new System.Drawing.Size(276, 21);
      this.lblInfo.TabIndex = 3;
      this.lblInfo.Text = "(Please type a name without \"_\")";
      this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.lblInfo.Visible = false;
      // 
      // lblParent
      // 
      this.lblParent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblParent.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblParent.Location = new System.Drawing.Point(4, 39);
      this.lblParent.Name = "lblParent";
      this.lblParent.Size = new System.Drawing.Size(276, 21);
      this.lblParent.TabIndex = 5;
      this.lblParent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // chkRoot
      // 
      this.chkRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.chkRoot.Location = new System.Drawing.Point(4, 65);
      this.chkRoot.Name = "chkRoot";
      this.chkRoot.Size = new System.Drawing.Size(276, 25);
      this.chkRoot.TabIndex = 6;
      this.chkRoot.Text = "Make this a top-level folder";
      this.chkRoot.CheckedChanged += new System.EventHandler(this.chkRoot_CheckedChanged);
      // 
      // NewFolderName
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
      this.ClientSize = new System.Drawing.Size(286, 117);
      this.Controls.Add(this.lblInfo);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnApply);
      this.Controls.Add(this.edFolderName);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.chkRoot);
      this.Controls.Add(this.lblParent);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "NewFolderName";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewFolderName_KeyDown);
      ((System.ComponentModel.ISupportInitialize)(this.Images16x16)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.edFolderName.Properties)).EndInit();
      this.ResumeLayout(false);

		}
		#endregion

		// private methods ...
		#region UpdateRootCheckedValue
		private void UpdateRootCheckedValue()
		{
			if (chkRoot.Checked)
				GetExistingFolders(RootFolder);
			else
				GetExistingFolders(ParentFolder);			

			btnApply.Enabled = GetNameAccuracy();
			ShowParentFolderInfo();			
		}
		#endregion
		#region ShowParentFolderInfo
		private void ShowParentFolderInfo()
		{					
			if (FolderName.Length == 0)
				lblParent.Text = STR_NULLLENGTH_ERROR;
			else if (_ExistingFolders != null && _ExistingFolders.Contains(FolderName))
				lblParent.Text = STR_EXISTINGNAME_ERROR;
			else if (chkRoot.Visible && !chkRoot.Checked && _ParentFolder != null)
				lblParent.Text = String.Format(STR_ParentLblFormat, _ParentFolder.Name);
			else
				lblParent.Text = "";				
		}
		#endregion		
		#region GetRootFolder
		private CommandKeyFolder GetRootFolder(CommandKeyFolder folder)
		{
			if (folder == null)
				return null;
			if (folder.ParentFolder == null)
				return folder;
			return GetRootFolder(folder.ParentFolder);
		}
		#endregion
		#region GetExistingFolders
		private void GetExistingFolders(CommandKeyFolder folder)
		{
			if (_ExistingFolders == null)
				_ExistingFolders = new StringCollection();
			else
				_ExistingFolders.Clear();

			if (folder == null || folder.Folders == null)
				return;

			for (int i = 0; i < folder.Folders.Count; i++)
			{
				CommandKeyFolder lFolder = folder.Folders[i] as CommandKeyFolder;
				if (lFolder == null)
					continue;

				if (!lFolder.IsDeleted)
					_ExistingFolders.Add(lFolder.Name);
			}
		}
		#endregion
		#region GetNameAccuracy
		private bool GetNameAccuracy()
		{
			if (FolderName.Length == 0)
				return false;
			if (_ExistingFolders == null)
				return true;			

			return !_ExistingFolders.Contains(FolderName);
		}
		#endregion

		// public methods ...
		#region CreateNewFolder
		public static bool CreateNewFolder(ref string value, ref bool inRoot)
		{
			return CreateNewFolder("Create Folder", null, ref value, ref inRoot);
		}
		#endregion
		#region CreateNewFolder
		public static bool CreateNewFolder(string caption, ref string value, ref bool inRoot)
		{
			return CreateNewFolder(caption, null, ref value, ref inRoot);
		}
		#endregion		
		#region CreateNewFolder
		public static bool CreateNewFolder(string caption, CommandKeyFolder parentFolder, ref string value, ref bool inRoot)
		{
			NewFolderName newFolderName = new NewFolderName();
			newFolderName.Text = caption;			
			newFolderName.RootFolder = newFolderName.GetRootFolder(parentFolder);
			newFolderName.ParentFolder = parentFolder;			
			newFolderName.CreateInRoot = (newFolderName.ParentFolder == null || newFolderName.ParentFolder == newFolderName.RootFolder);
			bool needToUpdateRootFlag = (newFolderName.chkRoot.Enabled == (newFolderName.ParentFolder != newFolderName.RootFolder));
			newFolderName.chkRoot.Enabled = (newFolderName.ParentFolder != newFolderName.RootFolder);
			
			if (needToUpdateRootFlag)
				newFolderName.UpdateRootCheckedValue();

			newFolderName.FolderName = value;

			if (newFolderName.ShowDialog() == DialogResult.OK)
			{
				value = newFolderName.FolderName;
				inRoot = newFolderName.CreateInRoot;
				return true;
			}
			else
				return false;
		}
		#endregion
		#region RenameFolder(string caption, ref string value)
		public static bool RenameFolder(string caption, ref string value)
		{
			return RenameFolder(caption, null, ref value);		
		}
		#endregion
		#region RenameFolder(string caption, CommandKeyFolder parentFolder, ref string value)
		public static bool RenameFolder(string caption, CommandKeyFolder parentFolder, ref string value)
		{
			NewFolderName newFolderName = new NewFolderName();
			newFolderName.Text = caption;			
			newFolderName.chkRoot.Visible = false;			
			newFolderName.RootFolder = null;			
			newFolderName.ParentFolder = parentFolder;
			newFolderName.GetExistingFolders(newFolderName.ParentFolder);
			newFolderName.FolderName = value;
			
			if (newFolderName.ShowDialog() == DialogResult.OK)
			{
				value = newFolderName.FolderName;
				return true;
			}
			else
				return false;
		}
		#endregion


		// event handlers ...
		#region edFolderName_TextChanged
		private void edFolderName_TextChanged(object sender, System.EventArgs e)
		{
			btnApply.Enabled = GetNameAccuracy();
			ShowParentFolderInfo();
		}
		#endregion
		#region NewFolderName_KeyDown
		private void NewFolderName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Enter:
					if (btnApply.Enabled)
						DialogResult = DialogResult.OK;
					break;
				case Keys.Escape:					
					DialogResult = DialogResult.Cancel;
					break;
			}
		}
		#endregion
		#region chkRoot_CheckedChanged
		private void chkRoot_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateRootCheckedValue();
		}
		#endregion
		
	
		// public properties ...
		#region FolderName
		/// <summary>
		/// Gets folder name.
		/// </summary>
		public string FolderName
		{
			get
			{
				return edFolderName.Text;
			}
			set
			{
				if (edFolderName.Text == value)
					return;
				edFolderName.Text = value;
			}
		}
		#endregion
		#region RootFolder
		public CommandKeyFolder RootFolder
		{
			get
			{
				return _RootFolder;
			}
			set
			{
				if (_RootFolder == value)
					return;
				_RootFolder = value;				
			}
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
				_ParentFolder = value;								
			}
		}
		#endregion
		#region CreateInRoot
		public bool CreateInRoot
		{
			get
			{
				return chkRoot.Checked;
			}
			set
			{
				chkRoot.Checked = value;				
			}
		}
		#endregion
	}
}
