using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeeklyPlanner.Controllers
{
    public class DotvvmApiDateTimeConverter : JsonConverter<DateTime>
    {

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetDateTime();
            }
            else
            {
                throw new JsonException("The value specified in the JSON could not be converted to DateTime!");
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
        {
            var dateWithoutTimezone = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
            writer.WriteStringValue(dateWithoutTimezone);
        }
    }
}
