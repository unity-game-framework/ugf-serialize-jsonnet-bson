using System.Collections.Generic;
using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Contexts;
using UGF.Serialize.Runtime;
using UnityEngine;

namespace UGF.Serialize.JsonNet.Bson.Runtime.Tests
{
    public class TestSerializerJsonNetBsonAsset
    {
        private class Target
        {
            public List<object> Targets { get; set; } = new List<object>();
        }

        private class Target1
        {
            public int IntValue { get; set; } = 10;
            public float FloatValue { get; set; } = 10.5F;
        }

        private class Target2
        {
            public bool BoolValue { get; set; } = true;
            public int IntValue { get; set; } = 10;
        }

        [Test]
        public void SerializeAndDeserialize()
        {
            var builder = Resources.Load<SerializerAsset>("SerializerJsonNetBson");
            var serializer = builder.Build<ISerializer<byte[]>>();

            var target = new Target()
            {
                Targets =
                {
                    new Target1(),
                    new Target2()
                }
            };

            byte[] result = serializer.Serialize(target, new Context());

            var result2 = serializer.Deserialize<Target>(result, new Context());

            Assert.NotNull(result2);
            Assert.IsNotEmpty(result2.Targets);
            Assert.AreEqual(2, result2.Targets.Count);
            Assert.IsInstanceOf<Target1>(result2.Targets[0]);
            Assert.IsInstanceOf<Target2>(result2.Targets[1]);
        }
    }
}
