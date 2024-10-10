using System.Diagnostics;

namespace Kotz.Tests.Extensions;

public sealed class NumberBaseExtTests
{
    [Theory]
    [InlineData(2146483647, 2147483647, 2)]
    [InlineData(2146483647, 2147483647, 8)]
    [InlineData(2146483647, 2147483647, 16)]
    [InlineData(0, 10000, 2)]
    [InlineData(0, 10000, 8)]
    [InlineData(0, 10000, 16)]
    [InlineData(-10000, 0, 2)]
    [InlineData(-10000, 0, 8)]
    [InlineData(-10000, 0, 16)]
    [InlineData(-2147483648, -2146483648, 2)]
    [InlineData(-2147483648, -2146483648, 8)]
    [InlineData(-2147483648, -2146483648, 16)]
    internal void FromDigitsTest(int min, int max, int @base)
    {
        while (min < max)
        {
            var number = Convert.ToString(min, @base);
            Assert.Equal($"{min}: {Convert.ToInt32(number, @base)}", $"{min}: {number.FromDigits<int>(@base)}");
            min++;
        }
    }

    [Theory]
    [InlineData(2146483647, 2147483647)]
    [InlineData(0, 10000)]
    [InlineData(-10000, 0)]
    [InlineData(-2147483648, -2146483648)]
    internal void ToBinaryTest(int min, int max)
    {
        while (min < max)
        {
            Assert.Equal($"{min}: {Convert.ToString(min, 2).ToUpperInvariant()}", $"{min}: {min.ToDigits(2)}");
            min++;
        }
    }

    [Theory]
    [InlineData(2146483647, 2147483647)]
    [InlineData(0, 10000)]
    [InlineData(-10000, 0)]
    [InlineData(-2147483648, -2146483648)]
    internal void ToOctalTest(int min, int max)
    {
        while (min < max)
        {
            Assert.Equal($"{min}: {Convert.ToString(min, 8).ToUpperInvariant()}", $"{min}: {min.ToDigits(8)}");
            min++;
        }
    }

    [Theory]
    [InlineData(2146483647, 2147483647)]
    [InlineData(0, 10000)]
    [InlineData(-10000, 0)]
    [InlineData(-2147483648, -2146483648)]
    internal void ToHexadecimalTest(int min, int max)
    {
        while (min < max)
        {
            Assert.Equal($"{min}: {Convert.ToString(min, 16).ToUpperInvariant()}", $"{min}: {min.ToDigits(16)}");
            min++;
        }
    }

    [Theory]
    [InlineData(2147482647, 2147483647)]
    [InlineData(0, 1000)]
    // [InlineData(-10000, 0)]
    // [InlineData(-2147483648, -2146483648)]
    internal void CrossBaseTest(int min, int max)
    {
        for (var @base = 2; @base <= 36; @base++)
        {
            if (int.IsPow2(@base))
                continue;

            for (var number = min; number < max; number++)
            {
                var convertedNumber = number.ToDigits(@base);
                Assert.Equal($"{number}({@base}): {convertedNumber}", $"{number}({@base}): {convertedNumber.FromDigits<int>(@base).ToDigits(@base)}");
            }
        }
    }
}