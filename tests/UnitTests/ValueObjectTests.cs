using Domain.ValueObject;
namespace UnitTests;

public class ValueObjectTests
{
    [Fact]
    public void Of_ValidAmountAndCurrency_ShouldCreateTotalAmount()
    {
        // Arrange
        decimal amount = 100.50m;
        string currency = "VND";
        // Act
        var money = Money.Of(amount, currency);
        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Money_ShouldBeImmutable()
    {
        var object1 = Money.Of(100, "VND");
        var object2 = object1 + Money.Of(99, "VND");

        // Assert
        Assert.Equal(100, object1.Amount);
        Assert.Equal(199, object2.Amount);
    }

    [Fact]
    public void Add_DifferentCurrency_ShouldThrowException()
    {
        var moneyVnd = Money.Of(100, "VND");
        var moneyUsd = Money.Of(10, "USD");

        Assert.Throws<InvalidOperationException>(() => 
        {
            var result = moneyVnd + moneyUsd;
        });
    }

    [Theory]
    [InlineData(-10, "VND")]
    [InlineData(100, "")]
    [InlineData(50, "")]
    public void Of_InvalidParameters_ShouldThrowArgumentException(decimal amount, string currency)
    {
        Assert.Throws<ArgumentException>(() => Money.Of(amount, currency));
    }
}
