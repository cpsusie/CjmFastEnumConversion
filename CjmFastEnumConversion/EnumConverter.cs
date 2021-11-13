using System;
using System.Threading.Tasks;


namespace Cjm.Common.UtilLib.FastEnum
{
    /// <summary>
    /// Use to perform fast interconversions between enums and their underlying types
    /// (especially useful in some generic contexts).  This utility will check once per each
    /// combination of the enum type and underlying type for which conversion is attempted.
    /// Once a combination -- for example <see cref="TaskStatus"/>  for TEnum and <see cref="int"/>
    /// for TUnderlying -- is checked (and passes) the check will not be repeated for future conversions using
    /// that combination.
    /// </summary>
    /// <remarks>
    /// Safety checks are made by
    /// an internal static constructor ONCE (assuming first is successful) for each
    /// distinct combination of enums and underlying types.
    /// After that, "unsafe" pointer conversions are used but these are provably safe by grace of the static ctor
    /// ensuring that these unsafe pointer conversions can only be used with enums and their actual underlying type.
    /// </remarks>
    public static partial class EnumConverterUtil
    {
        /// <summary>
        /// Convert <paramref name="convertMe"/> from the underlying tpe <typeparamref name="TUnderlier"/>
        /// to the matching enum type <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to interconvert.</typeparam>
        /// <typeparam name="TUnderlier">The underlying type to interconvert.</typeparam>
        /// <param name="convertMe">the value to interconvert</param>
        /// <returns>The converted value</returns>
        /// <exception cref="EnumAndUnderlierMismatchException{TEnum,TUnderlier}">
        /// Thrown if <typeparamref name="TUnderlier"/> is not the actual exact underlying type of the <typeparamref name="TEnum"/> enumeration.
        /// For example if "AnimalType" <see langword="enum"/> was backed by a <see cref="UInt64"/> yet you used this function to convert a <see cref="System.Byte"/> to AnimalType,
        /// the exception will be thrown.
        /// </exception>
        /// <remarks>The match check between <typeparamref name="TEnum"/> and <typeparamref name="TUnderlier"/> will happen
        /// ONCE for each <typeparamref name="TEnum"/> and <typeparamref name="TUnderlier"/> combination tried (per process).
        /// Only use this function to convert a <typeparamref name="TUnderlier"/> that
        /// is EXACTLY the underling type of the <typeparamref name="TEnum"/> type to which you wish to convert.
        /// </remarks>
        public static TEnum ConvertUnderlierToEnumValue<TEnum, TUnderlier>(TUnderlier convertMe)
            where TEnum : unmanaged, Enum where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>
            => ConversionHelper<TEnum, TUnderlier>.ConvertUnderToEnum(convertMe);

        /// <summary>
        /// Convert the <typeparamref name="TEnum"/> value specified by <paramref name="convertMe"/> to its underlying type
        /// <typeparamref name="TUnderlier"/>.  
        /// </summary>
        /// <param name="convertMe">The <see langword="enum"/> value to convert.</param>
        /// <typeparam name="TEnum"><see langword="enum"/> type.</typeparam>
        /// <typeparam name="TUnderlier"><see langword="enum"/>'s underlying type</typeparam>
        /// <returns>The converted value</returns>
        /// <exception cref="EnumAndUnderlierMismatchException{TEnum,TUnderlier}">
        /// Thrown if <typeparamref name="TUnderlier"/> is not the actual exact underlying type of the <typeparamref name="TEnum"/> enumeration.
        /// For example if "AnimalType" <see langword="enum"/> was backed by a <see cref="UInt64"/> yet you used this function to convert a <see cref="System.Byte"/> to AnimalType,
        /// the exception will be thrown.
        /// </exception>
        /// <remarks>The match check between <typeparamref name="TEnum"/> and <typeparamref name="TUnderlier"/> will happen
        /// ONCE for each <typeparamref name="TEnum"/> and <typeparamref name="TUnderlier"/> combination tried (per process).
        /// Only use this function to convert a <typeparamref name="TUnderlier"/> that
        /// is EXACTLY the underling type of the <typeparamref name="TEnum"/> type to which you wish to convert.
        /// </remarks>
        public static TUnderlier ConvertEnumToUnderlier<TEnum, TUnderlier>(TEnum convertMe)
            where TEnum : unmanaged, Enum where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>
                => ConversionHelper<TEnum, TUnderlier>.ConvertEnumToUnder(convertMe);

    }


}
