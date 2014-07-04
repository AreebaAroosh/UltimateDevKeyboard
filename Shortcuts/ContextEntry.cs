using System;

namespace CR_XkeysEngine
{
	public class ContextEntry
	{
		#region private fields...
    private ContextOption _Choice;
    private ContextEntry _Parent = null;
    private readonly ContextEntryCollection _SubContext;
		private readonly string _Name = String.Empty;
    private readonly string _Description = String.Empty;
    private readonly int _ImageIndex;
    #endregion

    // constructors...
    #region ContextEntry(string name, int imageIndex, string description)
    public ContextEntry(string name, int imageIndex, string description)
    {
			_Name = name;
			_Description = description;
			_ImageIndex = imageIndex;
    	_SubContext = new ContextEntryCollection();
    }
		#endregion

		// public methods...
		#region PropagateChoiceToParents
		public void PropagateChoiceToParents()
    {
			if (_Parent != null)
			{
				ContextOption lSaveParentChoice = _Parent.Choice;
      	ContextOption lNewParentChoice = _Parent.CalculateState();
				if (lNewParentChoice != lSaveParentChoice)
					_Parent.PropagateChoiceToParents();
      }
    }
		#endregion
		#region CalculateState
		public ContextOption CalculateState()
    {
			bool firstTime = true;
			_Choice = ContextOption.Ignored;
			foreach(ContextEntry contextEntry in _SubContext)
			{
      	if (firstTime)
				{
        	firstTime = false;
					_Choice = contextEntry.Choice;
        }
				else
					if (_Choice != contextEntry.Choice)
					{
          	_Choice = ContextOption.Mixed;
						break;
          }
      }
			return _Choice;
    }
		#endregion
		#region AddChild
		public void AddChild(ContextEntry contextEntry)
    {
			_SubContext.Add(contextEntry);
			contextEntry.Parent = this;
    }
		#endregion
		#region ContextDescription
		/// <summary>
    /// Returns the description of this ContextEntry (if it is selected). Otherwise returns 
    /// the description of the first child entry that is selected. Because this is recursive
    /// multiple descriptions can be returned, each separated by commas.
    /// </summary>
		public string ContextDescription
    {
    	get
      {
				// First check ourselves...
				if (Choice == ContextOption.Selected)
      		return Description;
				// Note: The check above is somewhat redundant in light of the recursive 
				// call below, but to remove it would place an additional burden on client 
				// code, requiring this check on the topmost node (e.g., the Global context) 
				// before accessing this property.

				string context = "";
				// Next check our immediate children (starting for the largest scope available)
				foreach(ContextEntry contextEntry in _SubContext)
					if (contextEntry.Choice == ContextOption.Selected)
					{
						// Once we find a context option that is selected, 
						// there is no need to drill further into it's 
						// children, since the most general context is 
						// already known (child contexts, which are more
						// specific will *all* be selected as well). 
						
						// For example, suppose we have a context for a 
						// command that works in the "Code Editor". More 
						// specific contexts "No Selection", "Any Selection", 
						// "Selection Fragment", "Whole Line Selection", and 
						// "Multiple Line Selection" will *all* be selected, 
						// so this entry that we've found (e.g., "Code Editor") 
						// will always be the most concise description of the 
						// context for this command.
						context += contextEntry.Description + ", ";
					}
					else 
					{
						// Recursive property reference -- looking for a more specific context, narrowing the scope.
						string childEntry = contextEntry.ContextDescription;
						if (childEntry != "")
							context += childEntry + ", ";
					}
				context = context.TrimEnd(' ', ',');
//				while ((lContext.Length > 0) && ((lContext[lContext.Length - 1] == ',') || (lContext[lContext.Length - 1] == ' ')))
//					lContext = lContext.Remove(lContext.Length - 1, 1);
				return context;
      }
    }
		#endregion

		#region ContextImageIndexes
		public string ContextImageIndexes
		{
			get
			{
				// First check ourselves...
				if (Choice == ContextOption.Selected)
					return ImageIndex.ToString();
					
				// Note: The check above is somewhat redundant in light of the recursive 
				// call below, but to remove it would place an additional burden on client 
				// code, requiring this check on the topmost node (e.g., the Global context) 
				// before accessing this property.

				string lContext = "";
				// Next check our immediate children (starting for the largest scope available)
				foreach(ContextEntry lContextEntry in _SubContext)
					if (lContextEntry.Choice == ContextOption.Selected)
					{
						// Once we find a context option that is selected, 
						// there is no need to drill further into it's 
						// children, since the most general context is 
						// already known (child contexts, which are more
						// specific will *all* be selected as well). 
						
						// For example, suppose we have a context for a 
						// command that works in the "Code Editor". More 
						// specific contexts "No Selection", "Any Selection", 
						// "Selection Fragment", "Whole Line Selection", and 
						// "Multiple Line Selection" will *all* be selected, 
						// so this entry that we've found (e.g., "Code Editor") 
						// will always be the most concise description of the 
						// context for this command.
						lContext += lContextEntry.ImageIndex.ToString() + ",";
					}
					else 
					{
						// Recursive property reference -- looking for a more specific context, narrowing the scope.
						string childEntry = lContextEntry.ContextImageIndexes;
						if (childEntry != "")
							lContext += childEntry + ",";
					}
				lContext = lContext.TrimEnd(',');
//				while ((lContext.Length > 0) && (lContext[lContext.Length - 1] == ','))
//					lContext = lContext.Remove(lContext.Length - 1, 1);
				return lContext;
			}
		}
		#endregion

		public int[] ContextImageIndexArray
		{
			get
      {
      	string lImageIndexes = ContextImageIndexes;
				if (lImageIndexes.Length == 0)
					return new int[0];

				int lNumElements = 1;
				for (int i = 0; i < lImageIndexes.Length; i++)
					if (lImageIndexes[i] == ',')
						lNumElements++;

				// Now we know how big the result will be:
				int[] lResult = new int[lNumElements];

				int lContextIndex = 0;
				int lThisIndex = 0;
				
				string lThisIndexAsStr = "";
				for (int i = 0; i < lImageIndexes.Length; i++)
					if (lImageIndexes[i] == ',')
					{
						if (lThisIndexAsStr != "")
						{
							try
              {
              	lThisIndex = Convert.ToInt32(lThisIndexAsStr);
              }
              catch
              {
              	lThisIndex = 0;
              }
							lResult[lContextIndex] = lThisIndex;
							lContextIndex++;
						}
						lThisIndexAsStr = "";
					}
					else	// Collect index...
						lThisIndexAsStr += lImageIndexes[i];

				if (lThisIndexAsStr != "")
				{
					try
					{
						lThisIndex = Convert.ToInt32(lThisIndexAsStr);
					}
					catch
					{
						lThisIndex = 0;
					}
					lResult[lContextIndex] = lThisIndex;
				}
				return lResult;
      }
		}
		
		
		// public properties...
		#region Name
		public string Name
    {
    	get
    	{
    		return _Name;
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
    }
		#endregion
		#region ImageIndex
		public int ImageIndex
		{
			get
			{
				return _ImageIndex;
			}
		}
		#endregion
		#region Parent
		public ContextEntry Parent
    {
    	get
    	{
    		return _Parent;
    	}
    
    	set
    	{
    		_Parent = value;
    	}
    }
		#endregion
		#region SubContext
		public ContextEntryCollection SubContext
    {
    	get
    	{
    		return _SubContext;
    	}
    }
		#endregion
		#region Choice
		public ContextOption Choice
    {
    	get
    	{
				if (_SubContext.Count == 0)
    		{
					// Validate option...
        	if (_Choice == ContextOption.Mixed)
						_Choice = ContextOption.Ignored;
        }
				return _Choice;
    	}
    
    	set
    	{
				if (_SubContext.Count == 0)		// We can only be mixed if we have children...
				{
					// Validate option...
        	if (value == ContextOption.Mixed)
						value = ContextOption.Ignored;
        }
				else if (value != ContextOption.Mixed)		// propagate choice to children.
					_SubContext.Choice = value;
				if (_Choice != value)
				{
        	_Choice = value;
					PropagateChoiceToParents();
        }
    	}
    }
		#endregion
	}
}
