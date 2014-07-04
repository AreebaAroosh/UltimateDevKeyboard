using System;
using System.Collections.Generic;
using System.Drawing;

namespace CR_XkeysEngine
{
  public class XkeysPainter
  {
    List<XkeyBase> selectedKeys = new List<XkeyBase>();

    static Font[] textFont = new Font[6];
    static XkeysPainter()
    {
      textFont[0] = new Font(FontFamily.GenericSansSerif, 6);
      textFont[1] = new Font(FontFamily.GenericSansSerif, 8);
      textFont[2] = new Font(FontFamily.GenericSansSerif, 10);
      textFont[3] = new Font(FontFamily.GenericSansSerif, 12);
      textFont[4] = new Font(FontFamily.GenericSansSerif, 14);
      textFont[5] = new Font(FontFamily.GenericSansSerif, 16);
    }

    int leftMargin;
    int topMargin;
    int keyWidth;
    int keyHeight;

    /// <summary>
    /// Initializes a new instance of the XkeysPainter class.
    /// </summary>
    /// <param name="keyWidth">The width of the key.</param>
    /// <param name="keyHeight">The height of the key.</param>
    public XkeysPainter(int keyWidth = 10, int keyHeight = -1)
    {
      this.keyWidth = keyWidth;
      if (keyHeight == -1)
        keyHeight = keyWidth;
      this.keyHeight = keyHeight;
    }

    public void SetKeySizeToFit(Size size)
    {
      SetKeySizeToFit(size.Width, size.Height);
    }

    public void SetKeySizeToFit(int width, int height)
    {
      const int internalMargin = 3;

      int availableWidth = width - internalMargin;
      int availableHeight = height - internalMargin;

      keyWidth = (int)Math.Round(availableWidth / Hardware.Keyboard.Width - 1);
      keyHeight = (int)Math.Round(availableHeight / Hardware.Keyboard.Height - 1);
      leftMargin = (int)Math.Round((availableWidth - keyWidth * Hardware.Keyboard.Width) / 2 + 1);
      topMargin = (int)Math.Floor((availableHeight - keyHeight * Hardware.Keyboard.Height) / 2.0) + 1;
    }

    Point GetTopLeft(int columnIndex, int rowIndex)
    {
      int x = columnIndex * keyWidth;
      int y = rowIndex * keyHeight;
      if (rowIndex > 1)
      {
        y += keyHeight / 2;
        if (columnIndex > 5)    // There's another half-KeyWidth margin after column 5  (zero-indexed).
          x += keyWidth;
        else if (columnIndex > 1)  // There's a half-KeyWidth margin after column 1 (zero-indexed).
          x += keyWidth / 2;
      }
      return new Point(x, y);
    }

    /// <summary>
    /// Converts the client x and y coordinates into corresponding row and column coordinates.
    /// </summary>
    public bool IsHit(int x, int y, out int column, out int row)
    {
      row = -1;
      column = -1;
      Point clientPt = SubtractMargin(new System.Drawing.Point(x, y));

      int clientX = clientPt.X;
      int clientY = clientPt.Y;

      if (clientX < 0)    // Too far to the left.
        return false;

      if (clientY < 0)    // Too far above.
        return false;

      if (clientY > keyHeight * 2)
      {
        if (clientY < keyHeight * 2 + keyHeight / 2)    // In the first horizontal separator margin
          return false;

        clientY -= keyHeight / 2;  // remove margin after row 1 (zero-indexed)

        if (clientY > Hardware.Keyboard.NumRows * keyHeight)    // Too far below.
          return false;

        int marginOneLeft = keyWidth * 2;
        if (clientX > marginOneLeft)
        {
          int halfKeyWidth = keyWidth / 2;
          int marginOneRight = marginOneLeft + halfKeyWidth;
          if (clientX < marginOneRight)   // Inside inner vertical margin one.
            return false;

          // Subtract inner vertical margin one's width
          clientX -= halfKeyWidth;

          int marginTwoLeft = keyWidth * 6;
          if (clientX > marginTwoLeft)
          {
            int marginTwoRight = marginTwoLeft + halfKeyWidth;
            if (clientX < marginTwoRight)   // Inside inner vertical margin two.
              return false;

            // Subtract inner vertical margin two's width:
            clientX -= halfKeyWidth;
          }
        }

        if (clientX > 8 * keyWidth)   // Too far to the right.
          return false;
      }
      else // In rows 0 or 1...
      {
        if (clientX > 9 * keyWidth)   // Too far to the right.
          return false;
      }

      column = clientX / keyWidth;
      row = clientY / keyHeight;
      return true;
    }


    Point AddMargin(Point point)
    {
      return new Point(point.X + leftMargin, point.Y + topMargin);
    }

    Point SubtractMargin(Point point)
    {
      return new Point(point.X - leftMargin, point.Y - topMargin);
    }

    Rectangle GetKeyRect(int columnIndex, int rowIndex, XKeysGroupType keyType)
    {
      Point point = AddMargin(GetTopLeft(columnIndex, rowIndex));
      switch (keyType)
      {
        case XKeysGroupType.NoGroup:
          return new Rectangle(point.X, point.Y, keyWidth, keyHeight);
        case XKeysGroupType.Tall:
          return new Rectangle(point.X, point.Y, keyWidth, keyHeight * 2);
        case XKeysGroupType.Wide:
          return new Rectangle(point.X, point.Y, keyWidth * 2, keyHeight);
        case XKeysGroupType.Square:
          return new Rectangle(point.X, point.Y, keyWidth * 2, keyHeight * 2);
        default:
          return new Rectangle(point.X, point.Y, keyWidth, keyHeight);
      }
    }

    static void DrawSelectionBorder(Graphics graphics, bool isSelected, Rectangle keyRect)
    {
      if (!isSelected)
        return;

      Rectangle selectionRect = new Rectangle(keyRect.Left + 2, keyRect.Top + 2, keyRect.Width - 3, keyRect.Height - 3);
      using (Pen borderPen = new Pen(Color.Blue, 2))
        graphics.DrawRectangle(borderPen, selectionRect);
    }

    void DrawBlockedKey(Graphics graphics, int columnIndex, int rowIndex, XKeysGroupType keyType, bool isSelected)
    {
      Rectangle keyRect = GetKeyRect(columnIndex, rowIndex, keyType);
      graphics.FillRectangle(Brushes.DimGray, keyRect);
      graphics.DrawRectangle(Pens.Gray, keyRect);
      DrawSelectionBorder(graphics, isSelected, keyRect);
    }

    int GetIdealFontIndex(Graphics graphics, string keyName, Rectangle keyRect)
    {
      int keyWidth = keyRect.Width;
      int keyHeight = keyRect.Height;

      using (StringFormat format = new StringFormat())
      {
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        bool hasSpaces = keyName.IndexOf(' ') >= 0;

        for (int i = textFont.Length - 1; i >= 0; i--)
        {
          SizeF strRect;
          if (hasSpaces)
            strRect = graphics.MeasureString(keyName, textFont[i], keyRect.Width, format);
          else
            strRect = graphics.MeasureString(keyName, textFont[i]);
          if (strRect.Width < keyWidth && strRect.Height < keyHeight)
          {
            bool eachWordFits = true;
            if (hasSpaces)
            {
              // Make sure every word fits.
              string[] lines = keyName.Split(' ');
              foreach (string word in lines)
              {
                SizeF wordRect = graphics.MeasureString(word, textFont[i]);
                if (wordRect.Width >= keyWidth)
                {
                  eachWordFits = false;
                  break;
                }
              }
            }
            if (eachWordFits)
              return i;
          }
        }
      }
      return 0;
    }

    void DrawKeyName(Graphics graphics, int columnIndex, int rowIndex, string keyName, Brush brush, Rectangle keyRect)
    {
      if (String.IsNullOrEmpty(keyName))
        return;

      int xOffset = 0;
      int yOffset = 0;
      if (rowIndex > 1)
      {
        yOffset += keyHeight / 2;

        if (columnIndex > 1)
          xOffset += keyWidth / 2;

        if (columnIndex > 5)
          xOffset += keyWidth / 2;
      }

      PointF topLeft = new PointF(leftMargin + columnIndex * keyWidth + 4 + xOffset, topMargin + rowIndex * keyHeight + 4 + yOffset);  // 4,4 is the inner text margin.

      using (StringFormat format = new StringFormat())
      {
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;
        int fontIndex = GetIdealFontIndex(graphics, keyName, keyRect);
        graphics.DrawString(keyName, textFont[fontIndex], brush, keyRect, format);
      }
    }

    void DrawFilledKey(Graphics graphics, int columnIndex, int rowIndex, XKeysGroupType keyType, bool isSelected, string keyName, Brush fillBrush)
    {
      Rectangle keyRect = GetKeyRect(columnIndex, rowIndex, keyType);
      graphics.FillRectangle(fillBrush, keyRect);
      DrawKeyName(graphics, columnIndex, rowIndex, keyName, Brushes.White, keyRect);
      DrawSelectionBorder(graphics, isSelected, keyRect);
    }

    void DrawKeyDown(Graphics graphics, int columnIndex, int rowIndex, XKeysGroupType keyType, bool isSelected, string keyName, bool isFocused)
    {
      Brush fillBrush;
      if (isFocused)
        fillBrush = Brushes.Red;
      else
        fillBrush = Brushes.Pink;
      
      DrawFilledKey(graphics, columnIndex, rowIndex, keyType, isSelected, keyName, fillBrush);
    }

    void DrawSelectedKey(Graphics graphics, int columnIndex, int rowIndex, XKeysGroupType keyType, bool isSelected, string keyName)
    {
      DrawFilledKey(graphics, columnIndex, rowIndex, keyType, isSelected, keyName, Brushes.MediumBlue);
    }

    void DrawKeyUp(Graphics graphics, int columnIndex, int rowIndex, XKeysGroupType keyType, bool isSelected, string keyName)
    {
      Rectangle keyRect = GetKeyRect(columnIndex, rowIndex, keyType);
      graphics.FillRectangle(Brushes.White, keyRect);
      graphics.DrawRectangle(Pens.LightGray, keyRect);
      DrawKeyName(graphics, columnIndex, rowIndex, keyName, Brushes.Gray, keyRect);
      DrawSelectionBorder(graphics, isSelected, keyRect);
    }

    public XkeyLayout GetLayout(Byte[] keyCode, Byte[] blockedKeys)
    {
      XkeyLayout result = new XkeyLayout();
      for (int columnIndex = 0; columnIndex < keyCode.Length; columnIndex++)
        for (int rowIndex = 0; rowIndex < Hardware.Keyboard.NumRows; rowIndex++)
        {
          int mask = (int)Math.Round(Math.Pow(2, rowIndex));
          if (!Hardware.Keyboard.IsValidKey(columnIndex, rowIndex))
            continue;
          Xkey xkey = new Xkey();
          xkey.SetPosition(columnIndex, rowIndex);

          int keyCodeMask = keyCode[columnIndex];
          if ((mask & keyCodeMask) == mask)
          {
            int blockedKeyMask = blockedKeys[columnIndex];
            if ((mask & blockedKeyMask) == mask)
              xkey.IsBlocked = true;
          }
          result.Keys.Add(xkey);
        }
      return result;
    }

    void DrawKey(Graphics graphics, int columnIndex, int rowIndex, byte keyCode, int blockedKeyMask, XkeyLayout xkeyLayout, XKeysGroupType keyType, bool isFocused)
    {
      int mask = (int)Math.Round(Math.Pow(2, rowIndex));
      if (Hardware.Keyboard.IsValidKey(columnIndex, rowIndex))
      {
        bool isSelected;
        string keyName = string.Empty;
        bool blockOverride = false;
        if (xkeyLayout != null)
        {
          isSelected = xkeyLayout.IsSelected(columnIndex, rowIndex);
          keyName = xkeyLayout.GetKeyName(columnIndex, rowIndex);
          Xkey key = xkeyLayout.GetKey(columnIndex, rowIndex) as Xkey;
          if (key != null)
            blockOverride = key.IsBlocked;
        }
        else
          isSelected = false;

        if ((mask & keyCode) == mask)
        {
          if (blockOverride || (mask & blockedKeyMask) == mask)
            DrawBlockedKey(graphics, columnIndex, rowIndex, keyType, isSelected);
          else
            DrawKeyDown(graphics, columnIndex, rowIndex, keyType, isSelected, keyName, isFocused);
        }
        else if (blockOverride)
          DrawBlockedKey(graphics, columnIndex, rowIndex, keyType, isSelected);
        else
        {
          foreach (XkeyBase selectedKey in selectedKeys)
            if (columnIndex == selectedKey.Column && rowIndex == selectedKey.Row)
            {
              DrawSelectedKey(graphics, columnIndex, rowIndex, keyType, isSelected, keyName);
              return;
            }
          DrawKeyUp(graphics, columnIndex, rowIndex, keyType, isSelected, keyName);
        }
      }
    }

    public void ClearKeySelection()
    {
      selectedKeys.Clear();
    }

    public void AddKeyToSelection(XkeyBase key)
    {
      selectedKeys.Add(key);
    }

    void DrawColumn(Graphics graphics, int columnIndex, byte keyCode, int blockedKeyMask, XkeyLayout xkeyLayout, bool isFocused)
    {
      for (int rowIndex = 0; rowIndex < Hardware.Keyboard.NumRows; rowIndex++)
        DrawKey(graphics, columnIndex, rowIndex, keyCode, blockedKeyMask, xkeyLayout, XKeysGroupType.NoGroup, isFocused);
    }

    public void Draw(Graphics graphics, XkeysDataHandler xkeysDataHandler, XkeyLayout xkeyLayout, bool isFocused)
    {
      if (xkeyLayout != null)
      {
        foreach (XkeyBase key in xkeyLayout.Keys)
        {
          int columnIndex = key.Column;
          byte keyCode = xkeysDataHandler.LastKeyCode[columnIndex];
          byte blockedKeyMask = xkeysDataHandler.BlockedKeysMask[columnIndex];
          XkeyGroup xkeyGroup = key as XkeyGroup;

          XKeysGroupType keyType;
          if (xkeyGroup != null)
            keyType = xkeyGroup.Type;
          else
            keyType = XKeysGroupType.NoGroup;

          DrawKey(graphics, columnIndex, key.Row, keyCode, blockedKeyMask, xkeyLayout, keyType, isFocused);
        }

      }
      else
        Draw(graphics, xkeysDataHandler.LastKeyCode, xkeysDataHandler.BlockedKeysMask, xkeyLayout, isFocused);
    }

    public void Draw(Graphics graphics, Byte[] keyCode, Byte[] blockedKeys, XkeyLayout xkeyLayout, bool isFocused)
    {
      for (int i = 0; i < keyCode.Length; i++)
        DrawColumn(graphics, i, keyCode[i], blockedKeys[i], xkeyLayout, isFocused);
    }

    public int KeyHeight
    {
      get
      {
        return keyHeight;
      }
    }

    public int KeyWidth
    {
      get
      {
        return keyWidth;
      }
    }
  }
}
