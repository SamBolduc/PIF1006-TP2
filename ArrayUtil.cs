using System;
using System.Collections.Generic;

namespace PIF1006_tp2
{
    public class ArrayUtil
    {
        public static T CreateJaggedArray<T>(params int[] lengths)
        {
            return (T)InitializeJaggedArray(typeof(T).GetElementType(), 0, lengths);
        }

        private static object InitializeJaggedArray(Type type, int index, IReadOnlyList<int> lengths)
        {
            var array = Array.CreateInstance(type, lengths[index]);
            var elementType = type.GetElementType();

            if (elementType == null) return array;
            for (var i = 0; i < lengths[index]; i++)
            {
                array.SetValue(InitializeJaggedArray(elementType, index + 1, lengths), i);
            }

            return array;
        }
    }
}