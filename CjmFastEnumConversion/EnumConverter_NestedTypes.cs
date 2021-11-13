using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cjm.Common.UtilLib.FastEnum
{
    partial class EnumConverterUtil
    {
        private static unsafe class ConversionHelper<TEnum, TUnderlier> where TEnum : unmanaged, Enum where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>
        {
            public static TEnum ConvertUnderToEnum(TUnderlier convertMe) => *((TEnum*)&convertMe);
            
            public static TUnderlier ConvertEnumToUnder(TEnum convertMe) => *((TUnderlier*)&convertMe);

            public static TEnum[] ConvertUnderToEnumArr(TUnderlier[] items) => Unsafe.As<TEnum[]>(items);

            public static TUnderlier[] ConvertEnumToUnderArr(TEnum[] items) => Unsafe.As<TUnderlier[]>(items);

            static ConversionHelper()
            {
                Type specifiedUnderlier = typeof(TUnderlier);
                Type actualUnderlier = Enum.GetUnderlyingType(typeof(TEnum));
                if (specifiedUnderlier != actualUnderlier)
                {
                    throw new EnumAndUnderlierMismatchException<TEnum, TUnderlier>();
                }
            }
        }
    }
}
