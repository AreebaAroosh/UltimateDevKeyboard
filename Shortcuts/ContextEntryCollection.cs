using System;
using System.Collections;

namespace CR_XkeysEngine
{
	public class ContextEntryCollection: CollectionBase
  {
  	// public methods...
  	#region Add
  	public void Add(ContextEntry aContextEntry)
  	{
  		InnerList.Add(aContextEntry);
  	}
  	#endregion
  	#region Remove
  	public void Remove(ContextEntry aContextEntry)
  	{
  		InnerList.Remove(aContextEntry);
  	}
  	#endregion
  	
  	// public properties...
		public ContextOption Choice 
		{
			set
			{
				foreach (ContextEntry lContextEntry in this)
					lContextEntry.Choice = value;
			}
		}
  	#region (default indexer)
  	public ContextEntry this[int aIndex] 
  	{
  		get
  		{
  			return (ContextEntry) InnerList[aIndex];
  		}
  	}
  	#endregion
  }
}
