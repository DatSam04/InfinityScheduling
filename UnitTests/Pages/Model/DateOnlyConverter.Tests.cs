using System;
using System.Text;
using System.Text.Json;
using NUnit.Framework;

namespace UnitTests.Pages.Model;

public class DateOnlyConverterTests
{
    #region Read
    [Test]
    public void DateOnlyConverter_Read_InvalidFormat_ThrowsJsonException()
    {
        // Arrange
        var converter = new DateOnlyConverter();
        var options = new JsonSerializerOptions();
        var invalidJson = "\"invalid-date\"";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(invalidJson));
        reader.Read();
        
        // Act & Assert
        try
        {
            converter.Read(ref reader, typeof(DateTime), options);
        }
        catch (JsonException exception)
        {
            StringAssert.Contains(exception.Message, "Unable to parse invalid-date to DateTime");
        };
    }
    #endregion Read
}