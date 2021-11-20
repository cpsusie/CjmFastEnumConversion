using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xunit;

namespace FastEnumUnitTests
{
    public sealed class FastEnumTestFixture
    {
        public IEnumerable<TBacker> FindAllValuesMatchingBacker<TBacker>(TBacker foo = default)
            where TBacker : unmanaged, IEquatable<TBacker>, IComparable<TBacker>, IConvertible
        {
            return foo switch
            {
                byte _ => Enumerate(AllTheByteBacked as ImmutableArray<EnumBackerPair<ByteBacked, TBacker>>? ??
                                    throw new InvalidCastException("Can't convert it")),
                sbyte _ => Enumerate(AllTheSByteBacked as ImmutableArray<EnumBackerPair<SByteBacked, TBacker>>? ??
                                     throw new InvalidCastException("Can't convert it")),

                ushort _ => Enumerate(AllTheUShortBacked as ImmutableArray<EnumBackerPair<UShortBacked, TBacker>>? ??
                                      throw new InvalidCastException("Can't convert it")),
                short _ => Enumerate(AllTheShortBacked as ImmutableArray<EnumBackerPair<ShortBacked, TBacker>>? ??
                                     throw new InvalidCastException("Can't convert it")),

                uint _ => Enumerate(AllTheUIntBacked as ImmutableArray<EnumBackerPair<UIntBacked, TBacker>>? ??
                                    throw new InvalidCastException("Can't convert it")),
                int _ => Enumerate(AllTheIntBacked as ImmutableArray<EnumBackerPair<IntBacked, TBacker>>? ??
                                   throw new InvalidCastException("Can't convert it")),

                ulong _ => Enumerate(AllTheULongBacked as ImmutableArray<EnumBackerPair<ULongBacked, TBacker>>? ??
                                     throw new InvalidCastException("Can't convert it")),
                long _ => Enumerate(AllTheLongBacked as ImmutableArray<EnumBackerPair<LongBacked, TBacker>>? ??
                                    throw new InvalidCastException("Can't convert it")),
                _ => throw new InvalidOperationException("Bad backer.")
            };
        }

        public ImmutableArray<EnumBackerPair<ByteBacked, byte>> AllByteBacked => AllTheByteBacked;
        public ImmutableArray<EnumBackerPair<SByteBacked, sbyte>> AllSByteBacked => AllTheSByteBacked;
        public ImmutableArray<EnumBackerPair<UShortBacked, ushort>> AllUShortBacked => AllTheUShortBacked;
        public ImmutableArray<EnumBackerPair<ShortBacked, short>> AllShortBacked => AllTheShortBacked;
        public ImmutableArray<EnumBackerPair<UIntBacked, uint>> AllUIntBacked => AllTheUIntBacked;
        public ImmutableArray<EnumBackerPair<IntBacked, int>> AllIntBacked => AllTheIntBacked;
        public ImmutableArray<EnumBackerPair<ULongBacked, ulong>> AllULongBacked => AllTheULongBacked;
        public ImmutableArray<EnumBackerPair<LongBacked, long>> AllLongBacked => AllTheLongBacked;
        public ImmutableSortedSet<EnumConversionTestValue> AllTestValues => AllTheTestValue;

        public (TEnum[] SortedAsEnum, TUnderlier[] SortedAsUnderlier) SortInverted<TEnum, TUnderlier>(ImmutableArray<EnumBackerPair<TEnum, TUnderlier>> sortInvert)
            where TEnum : unmanaged, Enum
            where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>, IConvertible
        {
            var byEnumVal = SortByEnum(sortInvert, true);
            var byUnderlier = SortByUnderlier(sortInvert, true);
            return (byEnumVal.Select(itm => itm.EnumValue).ToArray(),
                byUnderlier.Select(itm => itm.BackingValue).ToArray());
        }

        public ImmutableArray<EnumBackerPair<TEnum, TUnderlier>> SortByEnum<TEnum, TUnderlier>(
            ImmutableArray<EnumBackerPair<TEnum,
                TUnderlier>> sortUs, bool inverted = false) where TEnum : unmanaged, Enum
            where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>, IConvertible =>
            sortUs.Sort(ByEnumValComparer<TEnum, TUnderlier>.CreateByEnumComparer(inverted));

        public ImmutableArray<EnumBackerPair<TEnum, TUnderlier>> SortByUnderlier<TEnum, TUnderlier>(
            ImmutableArray<EnumBackerPair<TEnum,
                TUnderlier>> sortUs, bool inverted = false) where TEnum : unmanaged, Enum
            where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>, IConvertible =>
                sortUs.Sort(ByUnderlierValComparer<TEnum, TUnderlier>.CreateByUnderlierComparer(inverted));
        
        static FastEnumTestFixture()
        {
            AllTheByteBacked = InitAllDefinedValue<ByteBacked, byte>();
            AllTheSByteBacked = InitAllDefinedValue<SByteBacked, sbyte>();

            AllTheUShortBacked = InitAllDefinedValue<UShortBacked, ushort>();
            AllTheShortBacked = InitAllDefinedValue<ShortBacked, short>();

            AllTheUIntBacked = InitAllDefinedValue<UIntBacked, uint>();
            AllTheIntBacked = InitAllDefinedValue<IntBacked, int>();

            AllTheULongBacked = InitAllDefinedValue<ULongBacked, ulong>();
            AllTheLongBacked = InitAllDefinedValue<LongBacked, long>();

            int expectedTotal = AllTheByteBacked.Length + AllTheSByteBacked.Length + AllTheUShortBacked.Length +
                                AllTheShortBacked.Length + AllTheUIntBacked.Length + AllTheIntBacked.Length +
                                AllTheULongBacked.Length + AllTheLongBacked.Length;

            ImmutableSortedSet<EnumConversionTestValue>.Builder bldr 
                = ImmutableSortedSet.CreateBuilder<EnumConversionTestValue>();
            Merge(bldr, AllTheByteBacked);
            Merge(bldr, AllTheSByteBacked);

            Merge(bldr, AllTheUShortBacked);
            Merge(bldr, AllTheShortBacked);

            Merge(bldr, AllTheUIntBacked);
            Merge(bldr, AllTheIntBacked);

            Merge(bldr, AllTheULongBacked);
            Merge(bldr, AllTheLongBacked);

            Assert.Equal(expectedTotal, bldr.Count);

            AllTheTestValue = bldr.ToImmutable();

        }

        private static void Merge<TEnum, TBacker>(ImmutableSortedSet<EnumConversionTestValue>.Builder addToMe,
            ImmutableArray<EnumBackerPair<TEnum, TBacker>> addUs) where TEnum : unmanaged, Enum
            where TBacker : unmanaged, IEquatable<TBacker>, IComparable<TBacker>
        {
            for (int i = 0; i < addUs.Length; i++)
            {
                ref readonly EnumBackerPair<TEnum, TBacker> item = ref addUs.ItemRef(i);
                var addMe = EnumConversionTestValue.CreateConversionTestValue(item.EnumValue, item.BackingValue,
                    item.BackingValueAsULong);
                bool added = addToMe.Add(addMe);
                if (!added)
                {
                    Assert.False(true, "Duplicate item detected.");
                }
            }
        }

        private static ImmutableArray<EnumBackerPair<TEnum, TUnderlier>> InitAllDefinedValue<TEnum, TUnderlier>()
            where TEnum : unmanaged, Enum where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>, IConvertible
        {
            Type backingType = Enum.GetUnderlyingType(typeof(TEnum));
            if (typeof(TUnderlier) != backingType) throw new ArgumentException("Wrong backing type.");
            TEnum[] arr = Enum.GetValues<TEnum>();
            var bldr = ImmutableArray.CreateBuilder<EnumBackerPair<TEnum, TUnderlier>>(arr.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                TEnum enumVal = arr[i];
                (TUnderlier backingVal, ulong asUlong) = ConvertToBackingValueThenToUlong<TEnum, TUnderlier>(enumVal);
                bldr.Add((enumVal, backingVal, asUlong));
            }
            Debug.Assert(bldr.Count == bldr.Capacity);
            return bldr.MoveToImmutable();
        }

        private static (TUnderlier BackingValue, ulong ValAsULong) ConvertToBackingValueThenToUlong<TEnum, TUnderlier>(TEnum val) where TEnum : unmanaged, Enum where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>, IConvertible
        {
            Type backer = Enum.GetUnderlyingType(typeof(TEnum));
            object boxed = val;
            TUnderlier backerVal = (TUnderlier)boxed;
            return (backerVal, ConvertToULong(backerVal));
        }
        static ulong ConvertToULong<TBacker>(TBacker backerVal) where TBacker : unmanaged, IEquatable<TBacker>, IComparable<TBacker>, IConvertible => backerVal switch
        {
            byte bt => unchecked((ulong)bt),
            sbyte sbt => unchecked((ulong)sbt),
            ushort us => unchecked((ulong)us),
            short sh => unchecked((ulong)sh),
            uint ui => unchecked((ulong)ui),
            int i => unchecked((ulong)i),
            ulong ul => ul,
            long l => unchecked((ulong)l),
            _ => throw new InvalidOperationException("Backing value didn't match.")
        };

        static IEnumerable<TBacker> Enumerate<TEnum, TBacker>(ImmutableArray<EnumBackerPair<TEnum, TBacker>> enumerateUs)
            where TEnum : unmanaged, Enum where TBacker : unmanaged, IEquatable<TBacker>, IComparable<TBacker>, IConvertible
        {
            return enumerateUs.Select(itm => itm.BackingValue);
        }

        private static readonly ImmutableArray<EnumBackerPair<ByteBacked, byte>> AllTheByteBacked;
        private static readonly ImmutableArray<EnumBackerPair<SByteBacked, sbyte>> AllTheSByteBacked;
        private static readonly ImmutableArray<EnumBackerPair<UShortBacked, ushort>> AllTheUShortBacked;
        private static readonly ImmutableArray<EnumBackerPair<ShortBacked, short>> AllTheShortBacked;
        private static readonly ImmutableArray<EnumBackerPair<UIntBacked, uint>> AllTheUIntBacked;
        private static readonly ImmutableArray<EnumBackerPair<IntBacked, int>> AllTheIntBacked;
        private static readonly ImmutableArray<EnumBackerPair<ULongBacked, ulong>> AllTheULongBacked;
        private static readonly ImmutableArray<EnumBackerPair<LongBacked, long>> AllTheLongBacked;
        private static readonly ImmutableSortedSet<EnumConversionTestValue> AllTheTestValue;

        private sealed class ByEnumValComparer<TEnum, TUnderlier> : Comparer<EnumBackerPair<TEnum, TUnderlier>>
            where TEnum : unmanaged, Enum
            where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>, IConvertible
        {
            internal static ByEnumValComparer<TEnum, TUnderlier> CreateByEnumComparer() => CreateByEnumComparer(false);
            internal static ByEnumValComparer<TEnum, TUnderlier> CreateByEnumComparer(bool invert) => new(invert);
            /// <inheritdoc />
            public override int Compare(EnumBackerPair<TEnum, TUnderlier> x, EnumBackerPair<TEnum, TUnderlier> y)
            {
                var temp =  (x.EnumValue.CompareTo(y.EnumValue));
                return _inverted ? -temp : temp;
            }

            private ByEnumValComparer(bool inverted) => _inverted = inverted;

            private readonly bool _inverted;
        }

        private sealed class ByUnderlierValComparer<TEnum, TUnderlier> : Comparer<EnumBackerPair<TEnum, TUnderlier>>
            where TEnum : unmanaged, Enum
            where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>, IConvertible
        {
            internal static ByUnderlierValComparer<TEnum, TUnderlier> CreateByUnderlierComparer() =>
                CreateByUnderlierComparer(false);
            internal static ByUnderlierValComparer<TEnum, TUnderlier> CreateByUnderlierComparer(bool invert) => new(invert);
            /// <inheritdoc />
            public override int Compare(EnumBackerPair<TEnum, TUnderlier> x, EnumBackerPair<TEnum, TUnderlier> y)
            {
                var temp = (x.BackingValue.CompareTo(y.BackingValue));
                return _inverted ? -temp : temp;
            }

            private ByUnderlierValComparer(bool inverted) => _inverted = inverted;

            private readonly bool _inverted;
        }
    }

    public readonly record struct EnumBackerPair<TEnum, TUnderlier>(TEnum EnumValue, TUnderlier BackingValue, ulong BackingValueAsULong)
        where TEnum : unmanaged, Enum where TUnderlier : unmanaged, IEquatable<TUnderlier>, IComparable<TUnderlier>
    {
        public static implicit operator EnumBackerPair<TEnum, TUnderlier>(ValueTuple<TEnum, TUnderlier, ulong> pair) =>
            new(pair.Item1, pair.Item2, pair.Item3);
    }

    public readonly struct EnumConversionTestValue : IEquatable<EnumConversionTestValue>, IComparable<EnumConversionTestValue>
    {
        public static EnumConversionTestValue CreateConversionTestValue<TEnum, TBacking>(TEnum value, TBacking backing, ulong valAsULong) 
            where TEnum : unmanaged, Enum
        where TBacking : unmanaged, IEquatable<TBacking>, IComparable<TBacking>
        {

            Type backingType = Enum.GetUnderlyingType(typeof(TEnum));
            if (typeof(TBacking) != backingType) throw new ArgumentException("Wrong backing type.");
            return new EnumConversionTestValue(valAsULong, typeof(TBacking), typeof(TEnum), value.ToString());
        }

        public bool IsInvalidDefault => !_initialized;
        public ulong ValueAsULong { get; }
        public Type BackingType { get; }
        public Type EnumType { get; }
        public string EnumValueText { get; }

        private EnumConversionTestValue(ulong valueAsUlong, Type backingType, Type enumType, string enumText)
        {
            ValueAsULong = valueAsUlong;
            BackingType = backingType ?? throw new ArgumentNullException(nameof(backingType));
            EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
            string enumValText = (enumText ?? throw new ArgumentNullException(nameof(enumText))).Trim();
            EnumValueText = string.IsNullOrWhiteSpace(enumValText)
                ? throw new ArgumentException("An empty or whitespace-only string is prohibited.", nameof(enumText))
                : enumValText;
            _initialized = true;
        }

        public static int Compare(in EnumConversionTestValue lhs, in EnumConversionTestValue rhs)
        {
            if (lhs._initialized == rhs._initialized && !lhs._initialized) return 0;
            if (lhs._initialized != rhs._initialized) return lhs._initialized ? 1 : -1;

            int ret;
            int backingTypeComp = CompareTypes(lhs.BackingType, rhs.BackingType);
            if (backingTypeComp == 0)
            {
                int valueComp = lhs.ValueAsULong.CompareTo(rhs.ValueAsULong);
                if (valueComp == 0)
                {
                    int enumTypeComp = CompareTypes(lhs.EnumType, rhs.EnumType);
                    ret = enumTypeComp == 0
                        ? TheEnumTextStringComparer.Compare(lhs.EnumValueText, rhs.EnumValueText)
                        : enumTypeComp;
                }
                else
                {
                    ret = valueComp;
                }
            }
            else
            {
                ret = backingTypeComp;
            }
            return ret;
        }

        public override string ToString() => IsInvalidDefault
            ? "INVALID AND UNINITIALIZED"
            : $"[{nameof(EnumConversionTestValue)}] -- Backing type: [{BackingType.Name}], Backing value as ulong: [0x{ValueAsULong:x8}]; EnumVal: {EnumType.Name}.{EnumValueText}";

        public override int GetHashCode()
        {
            int hash;
            if (_initialized)
            {
                hash = ValueAsULong.GetHashCode();
                unchecked
                {
                    hash = (hash * 397) ^ BackingType.GetHashCode();
                    hash = (hash * 397) ^ EnumType.GetHashCode();
                    hash = (hash * 397) ^ TheEnumTextStringComparer.GetHashCode(EnumValueText);
                }
            }
            else
            {
                hash = int.MinValue;
            }
            return hash;
        }

        public static bool operator ==(in EnumConversionTestValue lhs, in EnumConversionTestValue rhs)
        {
            if (lhs._initialized == rhs._initialized && !lhs._initialized) return false;
            return lhs._initialized == rhs._initialized && lhs.EnumType == rhs.EnumType &&
                   lhs.BackingType == rhs.BackingType && TheEnumTextStringComparer.Equals(lhs.EnumValueText, rhs.EnumValueText);
        }

        public static bool operator !=(in EnumConversionTestValue lhs, in EnumConversionTestValue rhs) =>
            !(lhs == rhs);
        public static bool operator >(in EnumConversionTestValue lhs, in EnumConversionTestValue rhs) 
            => Compare(in lhs, in rhs) > 0;
        public static bool operator <(in EnumConversionTestValue lhs, in EnumConversionTestValue rhs)
            => Compare(in lhs, in rhs) < 0;
        public static bool operator >=(in EnumConversionTestValue lhs, in EnumConversionTestValue rhs)
            => !(lhs < rhs);
        public static bool operator <=(in EnumConversionTestValue lhs, in EnumConversionTestValue rhs)
            => !(lhs > rhs);
        public bool Equals(EnumConversionTestValue other) => other == this;
        public override bool Equals(object? obj) => obj is EnumConversionTestValue ectv && ectv == this;
        public int CompareTo(EnumConversionTestValue ectv) => Compare(in this, in ectv);

        private static int CompareTypes(Type l, Type r)
        {
            string lName = l.Name;
            string rName = r.Name;
            return TheTypeNameTextComparer.Compare(lName, rName);
        }

        private readonly bool _initialized;
        private static readonly StringComparer TheEnumTextStringComparer = StringComparer.OrdinalIgnoreCase;
        private static readonly StringComparer TheTypeNameTextComparer = StringComparer.Ordinal;
    }

    public enum ByteBacked : byte
    {
        DefaultValue = default,
        DefaultPlusOne = default(byte) + 1,
        MaxValueMinusOne = byte.MaxValue - 1,
        MaxValue = byte.MaxValue,
    }
    public enum UShortBacked : ushort
    {
        DefaultValue = default,
        DefaultPlusOne = default(ushort) + 1,
        MaxValueMinusOne = ushort.MaxValue - 1,
        MaxValue = ushort.MaxValue,
    }

    public enum UIntBacked : uint
    {
        DefaultValue = default,
        DefaultPlusOne = default(uint) + 1,
        MaxValueMinusOne = uint.MaxValue - 1,
        MaxValue = uint.MaxValue,
    }

    public enum ULongBacked : ulong
    {
        DefaultValue = default,
        DefaultPlusOne = default(ulong) + 1,
        MaxValueMinusOne = ulong.MaxValue - 1,
        MaxValue = ulong.MaxValue,
    }



    public enum SByteBacked : sbyte
    {
        MinValue = sbyte.MinValue,
        MinValuePlusOne = sbyte.MinValue + 1,
        DefaultValueMinusOne = default(sbyte) - 1,
        DefaultValue = default,
        DefaultPlusOne = default(sbyte) + 1,
        MaxValueMinusOne = sbyte.MaxValue - 1,
        MaxValue = sbyte.MaxValue,
    }
    public enum ShortBacked : short
    {
        MinValue = short.MinValue,
        MinValuePlusOne = short.MinValue + 1,
        DefaultValue = default,
        DefaultPlusOne = default(short) + 1,
        MaxValueMinusOne = short.MaxValue - 1,
        MaxValue = short.MaxValue,
    }

    public enum IntBacked : int
    {
        MinValue = int.MinValue,
        MinValuePlusOne = int.MinValue + 1,
        DefaultValue = default,
        DefaultPlusOne = default(int) + 1,
        MaxValueMinusOne = int.MaxValue - 1,
        MaxValue = int.MaxValue,
    }

    public enum LongBacked : long
    {
        MinValue = long.MinValue,
        MinValuePlusOne = long.MinValue + 1,
        DefaultValue = default,
        DefaultPlusOne = default(long) + 1,
        MaxValueMinusOne = long.MaxValue - 1,
        MaxValue = long.MaxValue,
    }

    public static class ArrayShuffleExtensions
    {
        public static ImmutableArray<T> CopyThenShuffleCopy<T>(this ImmutableArray<T> copyMeThenShuffle) where T : unmanaged
        {
            const int stackAllocCutoff = 5;
            Span<T> copy = copyMeThenShuffle switch
            {
                { IsDefault: true } => throw new ArgumentException("Parameter is invalid and uninitialized.",
                    nameof(copyMeThenShuffle)),
                { } buff when buff.Length == 0 => Span<T>.Empty,
                { } buff when buff.Length > 0 && buff.Length <= stackAllocCutoff => stackalloc T[copyMeThenShuffle
                    .Length],
                _ => new T[copyMeThenShuffle.Length]
            };
            for (int i = 0; i < copyMeThenShuffle.Length && i < copy.Length; ++i)
            {
                copy[i] = copyMeThenShuffle[i];
            }
            copy.Shuffle();
            return copy.ToImmutableArray();
        }

        public static void Shuffle<T>(this T[] array) 
            => Shuffle(array.AsSpan());
        
        public static void Shuffle<T>(this Span<T> array)
        {
            // https://stackoverflow.com/a/110570
            Random rgen = RGen;
            int n = array.Length;
            while (n > 1)
            {
                int k = rgen.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }

        public static ImmutableArray<T> ToImmutableArray<T>(this ReadOnlySpan<T> items)
        {
            ImmutableArray<T> ret = ImmutableArray<T>.Empty;
            if (items.Length > 0)
            {
                var bldr = ImmutableArray.CreateBuilder<T>(items.Length);
                for (int i = 0; i < items.Length; ++i)
                {
                    bldr.Add(items[i]);
                }
                ret = bldr.MoveToImmutable();
            }
            return ret;
        }

        public static ImmutableArray<T> ToImmutableArray<T>(this Span<T> items) =>
            ToImmutableArray((ReadOnlySpan<T>)items);
        

        //tl initialized with value factory, will not be null unless set to it explicitly.
        private static Random RGen => TheRng.Value!;
        private static readonly ThreadLocal<Random> TheRng = new(() => new(), false);
    }
}
