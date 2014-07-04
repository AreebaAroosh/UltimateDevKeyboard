using System;
using System.Collections;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
	public enum SortOrder
	{
		ByName,
		ByCommand,
		ByContext
	}
	
	#region ShortcutNameComparer
	public class ShortcutNameComparer: IComparer
	{
		public int Compare(object a, object b)
		{
			string astr = ((CommandKeyBinding)a).DisplayShortcut;
			string bstr = ((CommandKeyBinding)b).DisplayShortcut;
			return (astr).CompareTo(bstr);
		}
	}
	#endregion
	#region ShortcutCommandComparer
	public class ShortcutCommandComparer: IComparer
	{
		public int Compare(object a, object b)
		{
			string astr = ((CommandKeyBinding)a).DisplayCommand;
			string bstr = ((CommandKeyBinding)b).DisplayCommand;
			return (astr).CompareTo(bstr);
		}
	}
	#endregion
	#region ShortcutReadableContextComparer
	public class ShortcutReadableContextComparer: IComparer
	{
		public int Compare(object a, object b)
		{
			string astr = ((CommandKeyBinding)a).ReadableContext;
			string bstr = ((CommandKeyBinding)b).ReadableContext;
			return (astr).CompareTo(bstr);
		}
	}
	#endregion

	
	public class CommandKeyBindingCollection: CollectionBase, ICloneable
	{
		// public static fields
		public static ShortcutNameComparer SortByName = new ShortcutNameComparer();
		public static ShortcutCommandComparer SortByCommand = new ShortcutCommandComparer();
		public static ShortcutReadableContextComparer SortByContext = new ShortcutReadableContextComparer();

		// public methods...
		#region Add
		public void Add(CommandKeyBinding aCommandKeyBinding)
		{
			InnerList.Add(aCommandKeyBinding);
		}
		#endregion
		#region AddRange
		public void AddRange(ICollection collection)
		{
			InnerList.AddRange(collection);
		}
		#endregion
		#region Remove
		public void Remove(CommandKeyBinding aCommandKeyBinding)
		{
			InnerList.Remove(aCommandKeyBinding);
		}
		#endregion


    #region Find(EditorCustomInputEventArgs ea)
    public CommandKeyBinding Find(EditorCustomInputEventArgs ea)
    {
      MatchQuality matchQuality;
      foreach (CommandKeyBinding commandKeyBinding in this)
      {
        if (!commandKeyBinding.Enabled)
          continue;
        matchQuality = commandKeyBinding.Matches(ea.CustomData);
        if (matchQuality == MatchQuality.FullMatch)    // Found it
        {
          // TODO: Check to see if the command itself is enabled here. If not, continue to look.
          return commandKeyBinding;
        }
      }
      return null;    // Not found
    }
    #endregion

		#region Find(string name)
		public CommandKeyBinding Find(string name)
		{
			foreach (CommandKeyBinding lShortcut in this)
			{
				if (lShortcut.DisplayShortcut == name)
					return lShortcut;
			}
			return null;
		}
		#endregion
    #region FindByDisplayCommand
    public CommandKeyBinding FindByDisplayCommand(string diplayCommand)
		{
			foreach (CommandKeyBinding lShortcut in this)
			{
				if (lShortcut.DisplayCommand == diplayCommand)
					return lShortcut;
			}
			return null;
		}
		#endregion
		#region Save
		public void Save(DecoupledStorage aDecoupledStorage, CommandKeyBinding lastSelected)
		{
			aDecoupledStorage.WriteInt32("Header", "Count", Count);
			int lIndex = 0;
			foreach(CommandKeyBinding lCommandKeyBinding in this)
			{
				lCommandKeyBinding.Save(aDecoupledStorage, "Command" + lIndex.ToString(), lastSelected == lCommandKeyBinding);
				lIndex++;
			}
		}
		#endregion
		#region Load
		/// <summary>
		/// Loads the command key bindings from storage.
		/// </summary>
		/// <param name="aDecoupledStorage"></param>
		/// <returns>Returns the most recently selected command key binding.</returns>
		public CommandKeyBinding Load(DecoupledStorage aDecoupledStorage, CommandKeyFolder parentFolder)
		{
			Clear();
			CommandKeyBinding lLastSelected = null;
			CommandKeyBinding lCommandKeyBinding = null;
			int thisCount = aDecoupledStorage.ReadInt32("Header", "Count", Count);
			for (int i = 0; i < thisCount; i++)
			{
				lCommandKeyBinding = new CommandKeyBinding();
				lCommandKeyBinding.SetParentFolder(parentFolder);
				if (lCommandKeyBinding.Load(aDecoupledStorage, "Command" + i.ToString()))
					lLastSelected = lCommandKeyBinding;
				Add(lCommandKeyBinding);
			}
			return lLastSelected;
		}
		#endregion
		#region Load
		/// <summary>
		/// Loads the command key bindings from storage.
		/// </summary>
		/// <param name="aDecoupledStorage"></param>
		/// <returns>Returns the most recently selected command key binding.</returns>
		public CommandKeyBinding Load(DecoupledStorage aDecoupledStorage)
		{
			return Load(aDecoupledStorage, null);
		}
		#endregion
		#region Sort
		public void Sort(IComparer comparer)
		{
			InnerList.Sort(comparer);
		}
		#endregion
		#region GetComparer
		public static IComparer GetComparer(SortOrder order)
		{
			switch (order)
			{
				case SortOrder.ByCommand:
					return SortByCommand;
				case SortOrder.ByContext:
					return SortByContext;
				case SortOrder.ByName:
				default:
					return SortByName;
			}
		}
		#endregion

		#region ICloneable Members
		object ICloneable.Clone()
		{
			return Clone();
		}
		public CommandKeyBindingCollection Clone()
		{
			CommandKeyBindingCollection lNewCollection = new CommandKeyBindingCollection();
			for (int i = 0; i < this.Count; i++)
			{
				CommandKeyBinding lNewBinding = this[i].Clone();
				lNewCollection.Add(lNewBinding);
			}

			return lNewCollection;
		}
		#endregion

		public void SetParent(CommandKeyFolder parentFolder)
		{
			for (int i = 0; i < this.Count; i++)			
				this[i].SetParentFolder(parentFolder);
		}
		
		// public properties...
		#region (default indexer)
		public CommandKeyBinding this[int aIndex] 
		{
			get
			{
				return (CommandKeyBinding) InnerList[aIndex];
			}
		}
		#endregion
	}
}
