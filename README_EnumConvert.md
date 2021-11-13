# CjmFastEnumConversion (Copyright © 2021, CJM Screws, LLC)

## Summary

This very simple utility is used for fast conversions between *enums* and their underlying types and is useful in the generic context.  Safety checks are made once per process per distinct pair of *enum* type and Underlying type.  Once the safety check passes for a given *enum* and type Underlying type, no further safety checks will be performed for that combination.  This utility can only be used to convert values back and forth between their enum form and the form of their **exact** underlying type.  

## Compatible Platforms
* Net Framework 4.8
* Net Standard 2.0
* Net Standard 2.1
* Net 5.0
* Net 6.0

## Usage Examples

### Correct Usage Example

This example comes from the example code portion of this repository's unit tests. 

```csharp
public static void DemonstrateSuccessfulConversion()
{
    //OK -- UShortBacked is backed by ushort
    UShortBacked enumValue = UShortBacked.MaxValueMinusOne;
    ushort convertedToBacker = EnumConverterUtil.ConvertEnumToUnderlier<UShortBacked, ushort>(enumValue);
    UShortBacked andBack =
        EnumConverterUtil.ConvertUnderlierToEnumValue<UShortBacked, ushort>(convertedToBacker);
    Assert.True(enumValue == andBack && convertedToBacker == (ushort)enumValue);

    //Note that no checking is done as to whether a (correctly typed) backer is actually a defined value of the type.
    ushort bla = 0xbabe; //random ushort
    UShortBacked enumVal = EnumConverterUtil.ConvertUnderlierToEnumValue<UShortBacked, ushort>(bla);
    ushort roundTripped = EnumConverterUtil.ConvertEnumToUnderlier<UShortBacked, ushort>(enumVal);
    Assert.True(bla == roundTripped); //works just fine even though val not defined
}
```

The above amply demonstrates correct usage.  

### Incorrect Usage Example

This utility is only usable to convert enum values to their exact underlying type and back.  Attempts to convert an enum to a type other than their exact backer or vice versa will result in a `TypeInitializationException` being thrown.  The inner exception of the `TypeInitializationException` will be of type `EnumAndUnderlierMismatchException<TEnum, TIncorrectBacker>`.  The following snippet demonstrates incorrect usage that will throw exceptions:

```csharp
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
```

The foregoing should be sufficient to demonstrate the usage of this (very) limited-purpose library.