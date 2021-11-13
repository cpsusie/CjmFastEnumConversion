using System;

namespace Cjm.Common.UtilLib.FastEnum
{
    /// <summary>
    /// An exception thrown when an attempt is made to use the <see cref="EnumConverterUtil"/>
    /// to interconvert between a <typeparamref name="TEnum"/> value and any other type as <typeparamref name="TUnderlier"/>
    /// except for the <see langword="enum"/>'s actual underlying type.
    /// </summary>
    /// <typeparam name="TEnum">The mismatched enum type.</typeparam>
    /// <typeparam name="TUnderlier">The underlying type falsely assumed to be the underling type of <typeparamref name="TEnum"/></typeparam>
    public sealed class EnumAndUnderlierMismatchException<TEnum, TUnderlier> : ApplicationException where TEnum : unmanaged, Enum where TUnderlier : unmanaged
    {
        internal EnumAndUnderlierMismatchException() : base(CreateMessage(null), null) { }

        internal EnumAndUnderlierMismatchException(Exception? inner) : base(CreateMessage(inner), inner) {}

        private static string CreateMessage(Exception? inner) =>
            $"The {nameof(EnumConverterUtil)} cannot inter-convert values of type " +
            $"{typeof(TUnderlier).Name} with values of enumeration type {typeof(TEnum).Name}.  " +
            $"The proper underlier for {typeof(TEnum).Name} is {Enum.GetUnderlyingType(typeof(TEnum))}, " +
            $"not {typeof(TUnderlier).Name}." + (inner != null
                ? " Consult inner exception for details."
                : string.Empty);
    }
}