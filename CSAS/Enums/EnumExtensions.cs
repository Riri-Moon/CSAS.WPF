using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CSAS.Enums
{
    public static class EnumExtension
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }

        public static T Previous<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int index = Array.IndexOf<T>(Arr, src);

            /* if current item is first enum => no previous exists => return first item*/
            int j = (index == 0) ? 0 : index - 1;

            return (Arr.Length == 0) ? Arr[0] : Arr[j];
        }

        public static bool IsLastEnum<T>(this T value) where T : struct
        {
            T lastEnum = Enum.GetValues(typeof(T)).Cast<T>().Max();

            if (value.Equals(lastEnum))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Description<T>(this T value) where T : struct
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes?.Any() ?? false)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            //throw new ArgumentException("Not found.", "description");
            return default(T);
        }

        public static IEnumerable<T> Get<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static string GetDescription(dynamic enumInDynamic)
        {
            /*
			 Lokalize generic extension method in class. 
			 Create generic method with type of input parameter as parameter.
			 Pass input parameter to created method and invoke it.
			*/
            return typeof(EnumExtension).GetMethod("Description", BindingFlags.Static | BindingFlags.Public).
                MakeGenericMethod(enumInDynamic.GetType()).
                Invoke(null, new object[] { enumInDynamic });
        }

        public static string GetDescriptionFromEnumValue<T>(string value) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                return ((T)Enum.Parse(typeof(T), value)).Description(); ;
            }
        }
    }
}
