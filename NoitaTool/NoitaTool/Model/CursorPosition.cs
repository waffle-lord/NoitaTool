using System;
using System.Collections.Generic;
using System.Text;

namespace NoitaTool.Model
{
    public class CursorPosition
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public CursorPosition(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
