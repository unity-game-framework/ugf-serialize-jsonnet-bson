using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;

namespace UGF.Serialize.JsonNet.Bson.Runtime.Converters
{
    public class ConvertPropertyNameBsonWriter : BsonDataWriter
    {
        public IReadOnlyDictionary<string, string> Names { get; }

        public ConvertPropertyNameBsonWriter(IReadOnlyDictionary<string, string> names, Stream stream) : base(stream)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
        }

        public ConvertPropertyNameBsonWriter(IReadOnlyDictionary<string, string> names, BinaryWriter writer) : base(writer)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
        }

        public override Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            name = Convert(name);

            return base.WritePropertyNameAsync(name, cancellationToken);
        }

        public override Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = new CancellationToken())
        {
            name = Convert(name);

            return base.WritePropertyNameAsync(name, escape, cancellationToken);
        }

        public override void WritePropertyName(string name)
        {
            name = Convert(name);

            base.WritePropertyName(name);
        }

        public override void WritePropertyName(string name, bool escape)
        {
            name = Convert(name);

            base.WritePropertyName(name, escape);
        }

        private string Convert(string name)
        {
            return Names.TryGetValue(name, out string value) ? value : name;
        }
    }
}
