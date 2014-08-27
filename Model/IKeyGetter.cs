using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeRush.Core;

namespace CR_XkeysEngine
{
  public interface IKeyGetter
  {
    KeyBase GetKey(int column, int row);
    KeyBase GetSingleKey(int column, int row);
  }
}
