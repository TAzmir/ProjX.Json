using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjX.Json.Converters;

public class ISO8601DateOnlyConverter: JsonConverter<DateOnly>
{
    private const string DATE_FORMAT = "yyyy-MM-dd";
    private static readonly string _exampleDateOnly = DateOnly.FromDateTime(DateTime.UtcNow).ToString(DATE_FORMAT, CultureInfo.InvariantCulture);

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateOnlyValue = reader.GetString();

        if (DateOnly.TryParseExact(dateOnlyValue, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly result))
            return result;

        throw new JsonException($"Invalid DateOnly value '{dateOnlyValue}'. Expected ISO 8601 Date-Only format: '{DATE_FORMAT}', e.g. '{_exampleDateOnly}'.");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        var dateOnlyStr = value.ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
        writer.WriteStringValue(dateOnlyStr);
    }
}