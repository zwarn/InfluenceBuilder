using System;

namespace helper
{
    public static class EnumUtils
    {
        public static int GetLength<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length;
        }
    }
}