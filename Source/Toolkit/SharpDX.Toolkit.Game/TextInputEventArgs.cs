using System;

namespace SharpDX.Toolkit
{
    public class TextInputEventArgs
    {
        char character;
        public TextInputEventArgs(char character)
        {
            this.character = character;
        }
        public char Character
        {
            get
            {
                return character;
            }
        }
    }
}
