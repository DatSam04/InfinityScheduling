using System;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// This class is a custom json converter to control serialization
/// </summary>
public class DateOnlyConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd";

    //Override the serialization by reading and return only the Date (YYYY-MM-DD)
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (DateTime.TryParse(value, out var date))
        {
            return date.Date;
        }
        throw new JsonException($"Unable to parse {value} to DateTime");
    }

    //Return the date string
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
}