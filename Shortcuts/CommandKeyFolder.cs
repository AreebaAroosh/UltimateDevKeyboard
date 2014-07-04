using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
	public enum FolderEnabledState
	{
		Enabled,
		Disabled,
		Mixed
	}

	public enum FolderState
	{
		Expanded,
		Collapsed
	}

	public class CommandKeyFolder : ICloneable
	{
		#region private constants ...
		private const string STR_PathSeparator = "_";
		#endregion
		#region private fields ...
		private bool _Enabled = true;
		private FolderState _FolderState;
		private string _Name;
		private string _Description;
		private bool _IsDeleted;
		private CommandKeyFolder _ParentFolder;		
		private ArrayList _Folders;
		private CommandKeyBindingCollection _CommandBindings;		
		#endregion

		#region CommandKeyFolder
		public CommandKeyFolder()
		{
			_Enabled = true;
			_IsDeleted = false;
			_ParentFolder = null;			
			_Folders = new ArrayList();
			_CommandBindings = new CommandKeyBindingCollection();
		}
		#endregion
		#region CommandKeyFolder
		public CommandKeyFolder(CommandKeyFolder parentFolder)
			: this()
		{
			if (parentFolder != null)
			parentFolder.AddFolder(this);
		}
		#endregion

		// private methods...						
		#region AddFoldersToCollection
		private void AddFoldersToCollection(CommandKeyBindingCollection collection)
		{
			for (int i = 0; i < _Folders.Count; i++)
			{
				CommandKeyFolder lFolder = _Folders[i] as CommandKeyFolder;
				lFolder.ToPlainCollection(collection);
			}
		}
		#endregion
		#region AddBindingsToCollection
		private void AddBindingsToCollection(CommandKeyBindingCollection collection)
		{
			collection.AddRange(_CommandBindings);
		}
		#endregion
		#region ToPlainCollection
		private void ToPlainCollection(CommandKeyBindingCollection collection)
		{
			AddFoldersToCollection(collection);
			AddBindingsToCollection(collection);
		}
		#endregion
		
		#region SearchInSubFolders
		private CommandKeyFolder SearchInSubFolders(ArrayList folders, string path)
		{
			if (folders == null)
				return null;
			for (int i = 0; i < folders.Count; i++)
			{
				if (!(folders[i] is CommandKeyFolder))
					continue;
				CommandKeyFolder lFolder = (CommandKeyFolder)folders[i];
				CommandKeyFolder lFoundFolder = lFolder.FindFolderByPath(path);
				if (lFoundFolder != null)
					return lFoundFolder;
			}
			return null;
		}
		#endregion

		#region SplitPath
		private void SplitPath(string path, out string name, out string trailingPath)
		{
			name = String.Empty;
			trailingPath = String.Empty;
			string lWork = path;
			int lPos = lWork.IndexOf(STR_PathSeparator);
			name = CodeRush.StrUtil.DecodeName(lWork);
			if (lPos >= 0)
			{
				name = lWork.Substring(0, lPos);
				name = CodeRush.StrUtil.DecodeName(name);				
				if (lPos < lWork.Length - 1)
					trailingPath = lWork.Substring(lPos + 1);
			}
		}
		#endregion

		#region ChangeParent
		private void ChangeParent(CommandKeyFolder newParent)
		{		
			if (_ParentFolder != null || newParent != null)
			{	
				if (_ParentFolder == null)
					newParent.AddFolder(this);
				else if (newParent == null)			
					_ParentFolder.RemoveFolder(this);			
				else
				{
					CommandKeyFolder lNewFolder = this.Clone();
					lNewFolder._IsDeleted = true;
					_ParentFolder.AddFolder(lNewFolder);
					_ParentFolder.RemoveFolder(this);							
					newParent.AddFolder(this);
				}
			}
		}
		#endregion
		#region ChangeName
		private void ChangeName(string newName)
		{		
			if (newName.Length == 0 || newName == Name)
				return;

			if (_Name != null && _Name != string.Empty)
			{
				CommandKeyFolder lNewFolder = this.Clone();
				lNewFolder.IsDeleted = true;
				_ParentFolder.AddFolder(lNewFolder);
			}

			_Name = newName;
		}
		#endregion
		
		#region RemoveDeletedFolders
		private void RemoveDeletedFolders(DecoupledStorage storage)
		{
			if (storage == null)
				return;
			
			for (int i = 0; i < Folders.Count; i++)
			{
				CommandKeyFolder lFolder = (CommandKeyFolder)Folders[i];
				if (lFolder.IsDeleted)
					storage.DeleteFolder(lFolder.TreePath.Trim(STR_PathSeparator[0]));
				else
					lFolder.RemoveDeletedFolders(storage);
			}			
		}
		#endregion
				
		#region SaveSubFolders
		private void SaveSubFolders(DecoupledStorage storage, CommandKeyBinding lastSelected)
		{
			if (storage == null || _Folders == null)
				return;
			
			for (int i = 0; i < _Folders.Count; i++)
			{
				CommandKeyFolder lFolder = _Folders[i] as CommandKeyFolder;
				if (lFolder == null)
					continue;
				if (!lFolder.IsDeleted)
					lFolder.Save(storage, lastSelected);
			}
		}
		#endregion
		#region ProcessFolderName
		private string ProcessFolderName(string folderName)
		{
			if (folderName == null || folderName.Length == 0)
				return folderName;
			int lLastSeparatorIdx = folderName.LastIndexOf(STR_PathSeparator);
			if (lLastSeparatorIdx < 0)
				return folderName;
			string lResult = folderName.Substring(lLastSeparatorIdx);
			return lResult.Trim(STR_PathSeparator[0]);
		}
		#endregion

		#region Load
		private CommandKeyBinding Load(DecoupledStorage storage, string folderName)
		{
			if (storage == null)
				return null;			

			CommandKeyBinding lLastSelected = null;

			Hashtable lFolders = storage.GetSubFolders(folderName);
			foreach (DictionaryEntry lEntry in lFolders)
			{
				string lFolderName = (string)lEntry.Key;
				CommandKeyFolder lNewFolder = new CommandKeyFolder(this);
				CommandKeyBinding lLastSelectedInFolder = lNewFolder.Load(storage, lFolderName);
				if (lLastSelectedInFolder != null)
					lLastSelected = lLastSelectedInFolder;
			}

			_Name = ProcessFolderName(folderName);
			_Name = CodeRush.StrUtil.DecodeName(_Name);

			storage.FolderName = folderName;

			CommandKeyBinding lLastSelectedInBindings = LoadThisFolder(storage);
			if (lLastSelected == null)
				lLastSelected = lLastSelectedInBindings;
			return lLastSelected;
		}
		#endregion
		#region Save
		private void Save(DecoupledStorage storage, CommandKeyBinding lastSelected)
		{
			if (storage == null)
				return;

			string lFolderName = TreePath;
			storage.FolderName = lFolderName.Trim(STR_PathSeparator[0]);
      storage.Clear();
			//if (CommandBindings.Count != 0)
			SaveThisFolder(storage, lastSelected);
			SaveSubFolders(storage, lastSelected);
		}
		#endregion

		// protected methods...		
		#region SaveThisFolder(DecoupledStorage storage, CommandKeyBinding lastSelected)
		protected void SaveThisFolder(DecoupledStorage storage, CommandKeyBinding lastSelected)
		{
			if (storage == null)
				return;
						
			SaveFolderInfo(storage);
			SaveBindings(storage, lastSelected);			
		}
		#endregion
		#region SaveFolderInfo
		protected void SaveFolderInfo(DecoupledStorage storage)
		{
			if (storage == null)
				return;
			storage.WriteBoolean("FolderInfo", "Enabled", Enabled);
			//string lDescription = TextUtils.EncodeText(Description);
			string lDescription = CodeRush.StrUtil.EncodeText(Description);
			storage.WriteString("FolderInfo", "Description", lDescription);
			storage.WriteEnum("FolderInfo", "FolderState", FolderState);
		}
		#endregion
		#region SaveBindings
		protected void SaveBindings(DecoupledStorage storage, CommandKeyBinding lastSelected)
		{
			if (storage == null)
				return;
			_CommandBindings.Save(storage, lastSelected);
		}
		#endregion
				
		#region LoadThisFolder(DecoupledStorage storage)
		protected CommandKeyBinding LoadThisFolder(DecoupledStorage storage)
		{
			if (storage == null)
				return null;
			
			LoadFolderInfo(storage);
			return LoadBindings(storage);			
		}
		#endregion
		#region LoadFolderInfo
		protected void LoadFolderInfo(DecoupledStorage storage)
		{
			if (storage == null)
				return;
			Enabled = storage.ReadBoolean("FolderInfo", "Enabled", true);
			string lDescription = storage.ReadString("FolderInfo", "Description", "");
			//Description = TextUtils.DecodeText(lDescription);
			Description = CodeRush.StrUtil.DecodeText(lDescription);
			FolderState = (FolderState)storage.ReadEnum("FolderInfo", "FolderState", typeof(FolderState), FolderState.Collapsed);
		}
		#endregion
		#region LoadBindings
		protected CommandKeyBinding LoadBindings(DecoupledStorage storage)
		{
			if (storage == null)
				return null;
			return _CommandBindings.Load(storage, this);
		}
		#endregion

		// public methods...
		#region AddFolder
		public void AddFolder(CommandKeyFolder folder)
		{
			if (folder == null)
				return;
			folder._ParentFolder = this;
			_Folders.Add(folder);
		}
		#endregion
		#region RemoveFolder
		public void RemoveFolder(CommandKeyFolder folder)
		{
			if (folder == null)
				return;
			folder._ParentFolder = null;
			_Folders.Remove(folder);
		}
		#endregion
		#region AddBinding
		public void AddBinding(CommandKeyBinding binding)
		{
			if (binding == null)
				return;
			binding.SetParentFolder(this);
			_CommandBindings.Add(binding);
		}
		#endregion
		#region RemoveBinding
		public void RemoveBinding(CommandKeyBinding binding)
		{
			if (binding == null)
				return;
			binding.SetParentFolder(null);
			_CommandBindings.Remove(binding);
		}
		#endregion

		#region EnableFolder
		public void EnableFolder(bool enable)
		{
			if (Enabled == enable)
				return;
			_Enabled = enable;
			//EnableSubFolders(enable);
			//EnableCommandBindings(enable);
		}
		#endregion
		
		#region ContainsFolder
		public bool ContainsFolder(string name)
		{
			if (name == null || name == "")
				return false;
			for (int i = 0; i < Folders.Count; i ++)
			{
				CommandKeyFolder lFolder = Folders[i] as CommandKeyFolder;
				if (lFolder == null)
					continue;
				if (lFolder.Name.ToLower() == name.ToLower() && !lFolder.IsDeleted)
					return true;
			}
			return false;
		}
		#endregion
		#region CreateStorage
		public DecoupledStorage CreateStorage(string category, string name)
		{
			return CodeRush.Options.GetStorage(category, name);
		}
		#endregion
		
		#region Load
		public CommandKeyBinding Load(string category, string pageName)
		{
			CommandKeyBinding lResult = null;
			using (DecoupledStorage lStorage = new DecoupledStorage(category, pageName))
			{
				lStorage.LoadSubFolders();			
				lResult = Load(lStorage, "");
			}
			return lResult;
		}
		#endregion
		#region Save
		public void Save(string category, string pageName, CommandKeyBinding lastSelected)
		{			
			using (DecoupledStorage lStorage = new DecoupledStorage(category, pageName))
			{	
				lStorage.LoadSubFolders();
				RemoveDeletedFolders(lStorage);				
				lStorage.ClearAll();
				Save(lStorage, lastSelected);
			}						
		}
		#endregion
		
		#region ToPlainCollection
		public CommandKeyBindingCollection ToPlainCollection()
		{
			return ToPlainCollection(new ShortcutCommandComparer());
		}
		#endregion
		#region ToPlainCollection
		public CommandKeyBindingCollection ToPlainCollection(IComparer comparer)
		{
			CommandKeyBindingCollection lResult = new CommandKeyBindingCollection();
			ToPlainCollection(lResult);
			if (comparer != null)
				lResult.Sort(comparer);
			return lResult;
		}
		#endregion
		
		#region FindFolderByPath
		/// <summary>
		/// Finds folder by the given path inside subfolders of this folder.
		/// If folder is not found retuns null
		/// </summary>
		/// <param name="path">The path to use.</param>
		/// <returns></returns>
		public CommandKeyFolder FindFolderByPath(string path)
		{
			if (path == null || path == String.Empty)
				return null;
			
			string lName;
			string lTrailingPath;
			SplitPath(path, out lName, out lTrailingPath);
			if (lName == Name)
			{
				CommandKeyFolder lFolder = SearchInSubFolders(Folders, lTrailingPath);
				if (lFolder != null)
					return lFolder;
				return this;
			}
			return null;
		}
		#endregion

		
    #region FindInFolders(EditorCustomInputEventArgs ea)
    private CommandKeyBinding FindInFolders(EditorCustomInputEventArgs ea)
		{
			if (_Folders == null)
				return null;
			for (int i = 0; i < _Folders.Count; i++)
			{
				CommandKeyFolder folder = _Folders[i] as CommandKeyFolder;
				CommandKeyBinding lBinding = folder.Find(ea);
				if (lBinding != null)
					return lBinding;
			}
			return null;
		}
		#endregion
		#region FindInFolders(string name)
		private CommandKeyBinding FindInFolders(string name)
		{
			if (_Folders == null)
				return null;
			for (int i = 0; i < _Folders.Count; i++)
			{
				CommandKeyFolder lFolder = _Folders[i] as CommandKeyFolder;
				CommandKeyBinding lBinding = lFolder.Find(name);
				if (lBinding != null)
					return lBinding;
			}
			return null;
		}
		#endregion

		
    #region Find(EditorCustomInputEventArgs ea)
    public CommandKeyBinding Find(EditorCustomInputEventArgs ea)
		{
			CommandKeyBinding keyInBindings = _CommandBindings.Find(ea);
			if (keyInBindings != null)
				return keyInBindings;
			return FindInFolders(ea);
		}
		#endregion
		#region Find(string name)
		public CommandKeyBinding Find(string name)
		{
			CommandKeyBinding lKeyInBindings = _CommandBindings.Find(name);
			if (lKeyInBindings != null)
				return lKeyInBindings;
			return FindInFolders(name);
		}
		#endregion
		
		#region ICloneable Members
		object ICloneable.Clone()
		{
			return Clone();
		}
		private void CloneFolders(CommandKeyFolder newParent)
		{			
			for (int i = 0; i < Folders.Count; i++)
			{
				CommandKeyFolder lOldFolder = Folders[i] as CommandKeyFolder;
				if (lOldFolder == null)
					continue;

				CommandKeyFolder lNewFolder = lOldFolder.Clone();
				newParent.AddFolder(lNewFolder);				
			}			
		}
		public CommandKeyFolder Clone()
		{
			CommandKeyFolder lNewFolder = new CommandKeyFolder();
			lNewFolder._Name = this.Name;
			lNewFolder._CommandBindings = this.CommandBindings.Clone();
			lNewFolder._CommandBindings.SetParent(lNewFolder);			
			CloneFolders(lNewFolder);
			
			return lNewFolder;
		}
		#endregion

		#region GetTreePath
		public string GetTreePath()
		{
			return GetTreePath(true);
		}
		#endregion
		#region GetTreePath
		public string GetTreePath(bool encodeNames)
		{
			string lName = Name;
			if (encodeNames)
				lName = CodeRush.StrUtil.EncodeName(lName);
			if (_ParentFolder == null)
				return lName;
			return String.Format("{0}{1}{2}", _ParentFolder.GetTreePath(encodeNames), STR_PathSeparator, lName);
		}
		#endregion
		
		// public properties...
		#region Name
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if (_Name == value)
					return;

				ChangeName(value);
			}
		}
		#endregion
		#region Description
		public string Description
		{
			get
			{
				return _Description;
			}
			set
			{
				_Description = value;
			}
		}
		#endregion

		#region Folders
		public ArrayList Folders
		{
			get
			{
				return _Folders;
			}
			set
			{
				if (_Folders == value)
					return;
				_Folders = value;
			}
		}
		#endregion
		#region CommandBindings
		public CommandKeyBindingCollection CommandBindings
		{
			get
			{
				return _CommandBindings;
			}
			set
			{
				if (_CommandBindings == value)
				return;
				_CommandBindings = value;
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

				ChangeParent(value);
			}
		}
		#endregion
		#region TreePath
		public string TreePath
		{
			get
			{				
				return GetTreePath();
			}
		}
		#endregion
		#region IsDeleted
		public bool IsDeleted
		{
			get
			{
				return _IsDeleted;
			}
			set
			{
				_IsDeleted = value;
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
				if (_Enabled == value)
					return;
				EnableFolder(value);
			}
		}
		#endregion
		#region FolderState
		public FolderState FolderState
		{
			get
			{
				return _FolderState;
			}
			set
			{
				if (_FolderState == value)
					return;
				_FolderState = value;
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
					return true;

        // SEE B180873
        if (lParentFolder == this)
          return true;
        if (lParentFolder == lParentFolder.ParentFolder)
          return true;

				return _Enabled && lParentFolder.IsCompletelyEnabled;
			}
		}
		#endregion
		#region IsEmpty
		public bool IsEmpty
		{
			get
			{
				return CommandBindings.Count == 0 && Folders.Count == 0;
			}
		}
		#endregion
	}
}