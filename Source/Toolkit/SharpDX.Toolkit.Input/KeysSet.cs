using System.Collections.Generic;

namespace SharpDX.Toolkit.Input
{
    internal struct KeysSet
    {
        // We cannot use fixed array on WinRT, so we are faking it here
        private Keys key0;
        private Keys key1;
        private Keys key2;
        private Keys key3;
        private Keys key4;
        private Keys key5;
        private Keys key6;
        private Keys key7;

        public bool HasKey(Keys key)
        {
            return key0 == key ||
                   key1 == key ||
                   key2 == key ||
                   key3 == key ||
                   key4 == key ||
                   key5 == key ||
                   key6 == key ||
                   key7 == key;
        }

        public unsafe void UnSet(Keys key)
        {
            fixed (void* keysPtr = &key0)
            {
                var keys = (Keys*)keysPtr;
                for (int i = 0; i < 8; i++)
                {
                    if (*keys == key)
                    {
                        *keys = Keys.None;
                        break;
                    }
                    keys++;
                }
            }
        }

        public unsafe void Set(Keys key)
        {
            fixed (void* keysPtr = &key0)
            {
                var keys = (Keys*)keysPtr;
                var firstNullKey = (Keys*)0;
                for (int i = 0; i < 8; i++)
                {
                    if (*keys == key)
                    {
                        return;
                    }

                    if (*keys == Keys.None && firstNullKey == (Keys*)0)
                    {
                        firstNullKey = keys;
                    } 
                    keys++;
                }

                if(firstNullKey != (Keys*)0)
                {
                    *firstNullKey = key;
                }
            }
        }

        public unsafe void ListKeys(List<Keys> keysList)
        {
            fixed (void* keysPtr = &key0)
            {
                var keys = (Keys*)keysPtr;
                for (int i = 0; i < 8; i++)
                {
                    if (*keys != Keys.None)
                    {
                        keysList.Add(*keys);
                    }
                    keys++;
                }
            }
        }
    }
}