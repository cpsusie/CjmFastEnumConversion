using System;
using Cjm.Common.UtilLib.FastEnum;
using Xunit;

namespace FastEnumUnitTests
{
    internal class ExampleCode
    {
        public static void DemonstrateSuccessfulConversion()
        {
            //OK -- UShortBacked is backed by ushort
            UShortBacked enumValue = UShortBacked.MaxValueMinusOne;
            ushort convertedToBacker = EnumConverterUtil.ConvertEnumToUnderlier<UShortBacked, ushort>(enumValue);
            UShortBacked andBack =
                EnumConverterUtil.ConvertUnderlierToEnumValue<UShortBacked, ushort>(convertedToBacker);
            Assert.True(enumValue == andBack && convertedToBacker == (ushort)enumValue);

            //Note that no checking is done as to whether a (correctly typed) backer is actually a defined value of the type.
            ushort bla = 0xbabe;
            UShortBacked enumVal = EnumConverterUtil.ConvertUnderlierToEnumValue<UShortBacked, ushort>(bla);
            ushort roundTripped = EnumConverterUtil.ConvertEnumToUnderlier<UShortBacked, ushort>(enumVal);
            Assert.True(bla == roundTripped);
        }

        public static void DemonstrateExceptionOnMismatch()
        {
            UShortBacked enumValue = UShortBacked.DefaultPlusOne;
            short incorrectBacker = 1;
            //This will throw a type initialization exception because UShortBacked is backed by ushort, not short
            try
            {
                EnumConverterUtil.ConvertUnderlierToEnumValue<UShortBacked, short>(incorrectBacker);
            }
            catch (TypeInitializationException ex) when
                (ex.InnerException is EnumAndUnderlierMismatchException<UShortBacked, short>)
            {
                //CORRECT BEHAVIOR
            }
            catch (DidntThrowException)
            {
                //INCORRECT -- should have thrown
            }
            catch
            {
                Assert.False(true, "It threw the wrong thing!");
            }

            //And in reverse
            try
            {
                EnumConverterUtil.ConvertEnumToUnderlier<UShortBacked, short>(enumValue);
            }
            catch (TypeInitializationException ex) when
                (ex.InnerException is EnumAndUnderlierMismatchException<UShortBacked, short>)
            {
                //CORRECT BEHAVIOR
            }
            catch (DidntThrowException)
            {
                //INCORRECT -- should have thrown
            }
            catch
            {
                Assert.False(true, "It threw the wrong thing!");
            }
        }
    }

    sealed class DidntThrowException : ApplicationException
    {

    }
}
