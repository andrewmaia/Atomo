using System;

namespace Atomo.Utils
{
    public static class ConvertionFunctions
    {
        public static object Parse(object input, string type)
        {
            return Convert.ChangeType(input, Type.GetType(type));
        }

        public static object Parse(object input, Type type)
        {
            return Convert.ChangeType(input, type);
        }

        public static ReturnType Parse<ReturnType>(object input)
        {
            return (ReturnType)Convert.ChangeType(input, typeof(ReturnType));
        }
    }
}
