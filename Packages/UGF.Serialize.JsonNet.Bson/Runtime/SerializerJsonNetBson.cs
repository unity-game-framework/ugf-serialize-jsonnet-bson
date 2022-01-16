using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UGF.JsonNet.Runtime;
using UGF.RuntimeTools.Runtime.Contexts;
using UGF.Serialize.JsonNet.Bson.Converters;
using UGF.Serialize.Runtime;
using Unity.Profiling;

namespace UGF.Serialize.JsonNet.Bson
{
    public class SerializerJsonNetBson : SerializerAsync<byte[]>
    {
        public JsonSerializerSettings Settings { get; }
        public Dictionary<string, string> SerializeNames { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> DeserializeNames { get; } = new Dictionary<string, string>();

        private static ProfilerMarker m_markerSerialize;
        private static ProfilerMarker m_markerDeserialize;

#if ENABLE_PROFILER
        static SerializerJsonNetBson()
        {
            m_markerSerialize = new ProfilerMarker("SerializerJsonNetBson.Serialize");
            m_markerDeserialize = new ProfilerMarker("SerializerJsonNetBson.Deserialize");
        }
#endif

        public SerializerJsonNetBson() : this(JsonNetUtility.DefaultSettings)
        {
        }

        public SerializerJsonNetBson(JsonSerializerSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        protected override object OnSerialize(object target, IContext context)
        {
            return InternalSerialize(target);
        }

        protected override object OnDeserialize(Type targetType, byte[] data, IContext context)
        {
            return InternalDeserialize(targetType, data);
        }

        protected override Task<byte[]> OnSerializeAsync(object target, IContext context)
        {
            return Task.Run(() => InternalSerialize(target));
        }

        protected override Task<object> OnDeserializeAsync(Type targetType, byte[] data, IContext context)
        {
            return Task.Run(() => InternalDeserialize(targetType, data));
        }

        private byte[] InternalSerialize(object target)
        {
            m_markerSerialize.Begin();

            using var stream = new MemoryStream();
            using var writer = new ConvertPropertyNameBsonWriter(SerializeNames, stream);
            var serializer = JsonSerializer.Create(Settings);

            serializer.Serialize(writer, target, typeof(object));

            byte[] data = stream.ToArray();

            m_markerSerialize.End();

            return data;
        }

        private object InternalDeserialize(Type targetType, byte[] data)
        {
            m_markerDeserialize.Begin();

            using var stream = new MemoryStream(data);
            using var reader = new ConvertPropertyNameBsonReader(DeserializeNames, stream);
            var serializer = JsonSerializer.Create(Settings);

            object target = serializer.Deserialize(reader, targetType);

            m_markerDeserialize.End();

            return target;
        }
    }
}
