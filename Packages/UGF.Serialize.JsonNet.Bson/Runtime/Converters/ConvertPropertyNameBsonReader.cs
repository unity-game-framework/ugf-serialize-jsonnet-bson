using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace UGF.Serialize.JsonNet.Bson.Converters
{
    public class ConvertPropertyNameBsonReader : BsonDataReader
    {
        public IReadOnlyDictionary<string, string> Names { get; }

        public ConvertPropertyNameBsonReader(IReadOnlyDictionary<string, string> names, Stream stream) : base(stream)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
        }

        public ConvertPropertyNameBsonReader(IReadOnlyDictionary<string, string> names, BinaryReader reader) : base(reader)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
        }

        public override bool Read()
        {
            bool result = base.Read();

            if (result && TokenType == JsonToken.PropertyName && Value is string name)
            {
                if (Names.TryGetValue(name, out string value))
                {
                    SetToken(JsonToken.PropertyName, value);
                }
            }

            return result;
        }
    }
}
