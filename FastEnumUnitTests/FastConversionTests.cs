using System;
using System.Collections.Immutable;
using System.Linq;
using Cjm.Common.UtilLib.FastEnum;
using Xunit;
using Xunit.Abstractions;

namespace FastEnumUnitTests
{
    public class FastConversionTests : FixtureAndHelperHavingTests<FastEnumTestFixture>, IClassFixture<FastEnumTestFixture>
    {
        /// <inheritdoc />
        public FastConversionTests(ITestOutputHelper helper, FastEnumTestFixture fixture) : base(helper, fixture)
        {
        }

        [Fact]
        public void TestByteConversionEnumsPassing()
        {
            RunPassingTest(Fixture.AllByteBacked);
            RunPassingTest(Fixture.AllSByteBacked);

            RunPassingTest(Fixture.AllUShortBacked);
            RunPassingTest(Fixture.AllShortBacked);

            RunPassingTest(Fixture.AllUIntBacked);
            RunPassingTest(Fixture.AllIntBacked);

            RunPassingTest(Fixture.AllULongBacked);
            RunPassingTest(Fixture.AllLongBacked);
        }

        [Fact]
        public void DoFailingByteTests()
        {
            RunFailingTest<ByteBacked, byte, sbyte>(Fixture.AllByteBacked);
            RunFailingTest<ByteBacked, byte, ushort>(Fixture.AllByteBacked);
            RunFailingTest<ByteBacked, byte, short>(Fixture.AllByteBacked);
            RunFailingTest<ByteBacked, byte, uint>(Fixture.AllByteBacked);
            RunFailingTest<ByteBacked, byte, int>(Fixture.AllByteBacked);
            RunFailingTest<ByteBacked, byte, ulong>(Fixture.AllByteBacked);
            RunFailingTest<ByteBacked, byte, long>(Fixture.AllByteBacked);
        }

        [Fact]
        public void DoFailingSByteTests()
        {
            RunFailingTest<SByteBacked, sbyte, byte>(Fixture.AllSByteBacked);
            RunFailingTest<SByteBacked, sbyte, ushort>(Fixture.AllSByteBacked);
            RunFailingTest<SByteBacked, sbyte, short>(Fixture.AllSByteBacked);
            RunFailingTest<SByteBacked, sbyte, uint>(Fixture.AllSByteBacked);
            RunFailingTest<SByteBacked, sbyte, int>(Fixture.AllSByteBacked);
            RunFailingTest<SByteBacked, sbyte, ulong>(Fixture.AllSByteBacked);
            RunFailingTest<SByteBacked, sbyte, long>(Fixture.AllSByteBacked);
        }

        [Fact]
        public void DoFailingUShortTests()
        {
            RunFailingTest<UShortBacked, ushort, byte>(Fixture.AllUShortBacked);
            RunFailingTest<UShortBacked, ushort, sbyte>(Fixture.AllUShortBacked);
            RunFailingTest<UShortBacked, ushort, short>(Fixture.AllUShortBacked);
            RunFailingTest<UShortBacked, ushort, uint>(Fixture.AllUShortBacked);
            RunFailingTest<UShortBacked, ushort, int>(Fixture.AllUShortBacked);
            RunFailingTest<UShortBacked, ushort, ulong>(Fixture.AllUShortBacked);
            RunFailingTest<UShortBacked, ushort, long>(Fixture.AllUShortBacked);
        } 

        [Fact]
        public void DoFailingShortTests()
        {
            RunFailingTest<ShortBacked, short, byte>(Fixture.AllShortBacked);
            RunFailingTest<ShortBacked, short, sbyte>(Fixture.AllShortBacked);
            RunFailingTest<ShortBacked, short, ushort>(Fixture.AllShortBacked);
            RunFailingTest<ShortBacked, short, uint>(Fixture.AllShortBacked);
            RunFailingTest<ShortBacked, short, int>(Fixture.AllShortBacked);
            RunFailingTest<ShortBacked, short, ulong>(Fixture.AllShortBacked);
            RunFailingTest<ShortBacked, short, long>(Fixture.AllShortBacked);
        }

        [Fact]
        public void DoFailingUIntTests()
        {
            RunFailingTest<UIntBacked, uint, byte>(Fixture.AllUIntBacked);
            RunFailingTest<UIntBacked, uint, sbyte>(Fixture.AllUIntBacked);
            RunFailingTest<UIntBacked, uint, ushort>(Fixture.AllUIntBacked);
            RunFailingTest<UIntBacked, uint, short>(Fixture.AllUIntBacked);
            RunFailingTest<UIntBacked, uint, int>(Fixture.AllUIntBacked);
            RunFailingTest<UIntBacked, uint, ulong>(Fixture.AllUIntBacked);
            RunFailingTest<UIntBacked, uint, long>(Fixture.AllUIntBacked);
        }

        [Fact]
        public void DoFailingIntTests()
        {
            RunFailingTest<IntBacked, int, byte>(Fixture.AllIntBacked);
            RunFailingTest<IntBacked, int, sbyte>(Fixture.AllIntBacked);
            RunFailingTest<IntBacked, int, ushort>(Fixture.AllIntBacked);
            RunFailingTest<IntBacked, int, short>(Fixture.AllIntBacked);
            RunFailingTest<IntBacked, int, uint>(Fixture.AllIntBacked);
            RunFailingTest<IntBacked, int, ulong>(Fixture.AllIntBacked);
            RunFailingTest<IntBacked, int, long>(Fixture.AllIntBacked);
        }

        [Fact] 
        public void DoFailingULongTests()
        {
            RunFailingTest<ULongBacked, ulong, byte>(Fixture.AllULongBacked);
            RunFailingTest<ULongBacked, ulong, sbyte>(Fixture.AllULongBacked);
            RunFailingTest<ULongBacked, ulong, ushort>(Fixture.AllULongBacked);
            RunFailingTest<ULongBacked, ulong, short>(Fixture.AllULongBacked);
            RunFailingTest<ULongBacked, ulong, uint>(Fixture.AllULongBacked);
            RunFailingTest<ULongBacked, ulong, int>(Fixture.AllULongBacked);
            RunFailingTest<ULongBacked, ulong, long>(Fixture.AllULongBacked);
        }

        [Fact]
        public void DoFailingLongTests()
        {
            RunFailingTest<LongBacked, long, byte>(Fixture.AllLongBacked);
            RunFailingTest<LongBacked, long, sbyte>(Fixture.AllLongBacked);
            RunFailingTest<LongBacked, long, ushort>(Fixture.AllLongBacked);
            RunFailingTest<LongBacked, long, short>(Fixture.AllLongBacked);
            RunFailingTest<LongBacked, long, uint>(Fixture.AllLongBacked);
            RunFailingTest<LongBacked, long, int>(Fixture.AllLongBacked);
            RunFailingTest<LongBacked, long, ulong>(Fixture.AllLongBacked);
        }

        [Fact]
        public void RunExampleCode()
        {
            ExampleCode.DemonstrateSuccessfulConversion();
            ExampleCode.DemonstrateExceptionOnMismatch();
        }

        private void RunFailingTest<TActualEnum, TCorrectBacker, TIncorrectBacker>(
            ImmutableArray<EnumBackerPair<TActualEnum, TCorrectBacker>> items) where TActualEnum : unmanaged, Enum
            where TCorrectBacker : unmanaged, IEquatable<TCorrectBacker>, IComparable<TCorrectBacker>, IConvertible
            where TIncorrectBacker : unmanaged, IEquatable<TIncorrectBacker>, IComparable<TIncorrectBacker>,
            IConvertible
        {
            ImmutableArray<TIncorrectBacker> badBackerValues =
                Fixture.FindAllValuesMatchingBacker<TIncorrectBacker>().ToImmutableArray();
            if (badBackerValues.Any())
            {
                
                foreach (TIncorrectBacker incorrect in badBackerValues)
                {
                    Assert.Throws<TypeInitializationException>(() =>
                    {
                        try
                        {
                            EnumConverterUtil.ConvertUnderlierToEnumValue<TActualEnum, TIncorrectBacker>(incorrect);
                        }
                        catch (TypeInitializationException ex)
                        {
                            Assert.True(
                                ex.InnerException is EnumAndUnderlierMismatchException<TActualEnum, TIncorrectBacker>);
                            throw;
                        }
                    });
                }

                foreach ((TActualEnum enumVal, _, _) in items)
                {
                    Assert.Throws<TypeInitializationException>(() =>
                    {
                        try
                        {
                            EnumConverterUtil.ConvertEnumToUnderlier<TActualEnum, TIncorrectBacker>(enumVal);
                        }
                        catch (TypeInitializationException ex)
                        {
                            Assert.True(ex.InnerException is EnumAndUnderlierMismatchException<TActualEnum, TIncorrectBacker>);
                            throw;
                        }
                    });
                }
            }
        }

        private void RunPassingTest<TEnum, TUnderling>(ImmutableArray<EnumBackerPair<TEnum, TUnderling>> toConvert)
            where TEnum : unmanaged, Enum
            where TUnderling : unmanaged, IEquatable<TUnderling>, IComparable<TUnderling>, IConvertible
        {
            for (int i = 0; i < toConvert.Length; i++)
            {
                ref readonly var item = ref toConvert.ItemRef(i);
                Assert.Equal(EnumConverterUtil.ConvertEnumToUnderlier<TEnum, TUnderling>(item.EnumValue), item.BackingValue);
                Assert.Equal(EnumConverterUtil.ConvertUnderlierToEnumValue<TEnum, TUnderling>(item.BackingValue), item.EnumValue);
            }
        }
    }
}
