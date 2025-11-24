using FluentAssertions;
using ProjX.Json.Converters;
using System.Globalization;
using System.Text.Json;

namespace ProjX.Json.Tests.Converters;

public class ISO8601DateOnlyConverterTests
{
    private const string DATE_FORMAT = "yyyy-MM-dd";
    private readonly JsonSerializerOptions _options;

    public ISO8601DateOnlyConverterTests()
    {
        _options = new JsonSerializerOptions
        {
            Converters = { new ISO8601DateOnlyConverter() }
        };
    }

    [Theory]
    [InlineData("2025-11-24")]
    public void WhenValidIso8601DateOnlyStr_ShouldSerialize(string dateOnlyStr)
    {
        // Arrange
        var inputDate = DateOnly.ParseExact(dateOnlyStr, DATE_FORMAT, CultureInfo.InvariantCulture);
        
        // Act
        var json = JsonSerializer.Serialize(inputDate, _options);

        // Assert
        json.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("2025-11-24")]
    public void WhenValidIso8601DateOnlyStr_ShouldDeserialize(string dateOnlyStr)
    {
        // Arrange
        var json = JsonSerializer.Serialize(dateOnlyStr, _options);

        // Act
        var deserializedDateOnly = JsonSerializer.Deserialize<DateOnly>(json, _options);

        // Assert
        deserializedDateOnly.Should().NotBe(DateOnly.MinValue);
    }

    [Theory]
    [InlineData("2025-11-24-")]
    [InlineData("11/24/2025-test")]
    [InlineData("24/11/2025-00")]
    [InlineData("")]
    [InlineData(null)]
    public void Deserialize_WhenInvalidFormats_ShouldThrowJsonException(string? inputDateString)
    {
        // Arrange
        var json = JsonSerializer.Serialize(inputDateString, _options);

        Action action = () =>
        {
            JsonSerializer.Deserialize<DateOnly>(json, _options);
        };

        //act
        action.Should().Throw<JsonException>();
    }

    [Fact]
    public void WhenSymmetrically_ShouldSerializeAndDeserialize()
    {
        // Arrange
        var original = new DateOnly(2025, 11, 24);

        // Act
        var json = JsonSerializer.Serialize(original, _options);
        var roundTrip = JsonSerializer.Deserialize<DateOnly>(json, _options);

        // Assert
        roundTrip.Should().Be(original);
    }
}