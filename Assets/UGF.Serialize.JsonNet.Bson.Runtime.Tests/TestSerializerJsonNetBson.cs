using System;
using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Contexts;
using UGF.Serialize.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

namespace UGF.Serialize.JsonNet.Bson.Runtime.Tests
{
    public class TestSerializerJsonNetBson
    {
        [Serializable]
        private class Target
        {
            [SerializeField] private bool m_boolValue = true;
            [SerializeField] private int m_intValue = 10;
            [SerializeField] private float m_floatValue = 10.5F;

            public bool BoolValue { get { return m_boolValue; } set { m_boolValue = value; } }
            public int IntValue { get { return m_intValue; } set { m_intValue = value; } }
            public float FloatValue { get { return m_floatValue; } set { m_floatValue = value; } }
        }

        [Test]
        public void Serialize()
        {
            var serialize = new SerializerJsonNetBson();
            var target = new Target();

            byte[] data = serialize.Serialize(target, new Context());

            Assert.NotNull(data);
            Assert.Greater(data.Length, 0);
        }

        [Test]
        public void Deserialize()
        {
            var serialize = new SerializerJsonNetBson();
            var target = new Target();

            byte[] data = serialize.Serialize(target, new Context());
            var target0 = serialize.Deserialize<Target>(data, new Context());

            Assert.AreEqual(target.BoolValue, target0.BoolValue);
            Assert.AreEqual(target.IntValue, target0.IntValue);
            Assert.AreEqual(target.FloatValue, target0.FloatValue);
        }

        [UnityTest]
        public IEnumerator SerializeAsync()
        {
            var serialize = new SerializerJsonNetBson();
            var target = new Target();

            Task<byte[]> task = serialize.SerializeAsync(target, new Context());

            while (!task.IsCompleted)
            {
                yield return null;
            }

            byte[] data = task.Result;

            Assert.NotNull(data);
            Assert.Greater(data.Length, 0);
        }

        [UnityTest]
        public IEnumerator DeserializeAsync()
        {
            var serialize = new SerializerJsonNetBson();
            var target = new Target();

            byte[] data = serialize.Serialize(target, new Context());

            Task<Target> task = serialize.DeserializeAsync<Target>(data, new Context());

            while (!task.IsCompleted)
            {
                yield return null;
            }

            Target target0 = task.Result;

            Assert.AreEqual(target.BoolValue, target0.BoolValue);
            Assert.AreEqual(target.IntValue, target0.IntValue);
            Assert.AreEqual(target.FloatValue, target0.FloatValue);
        }

        [Test]
        public void Copy()
        {
            var serializer = new SerializerJsonNetBson();
            var target = new Target();

            Target copy = SerializeUtility.Copy(target, serializer, new Context());

            Assert.NotNull(copy);
            Assert.AreNotEqual(copy, target);
            Assert.AreNotSame(copy, target);
            Assert.AreEqual(copy.BoolValue, target.BoolValue);
            Assert.AreEqual(copy.IntValue, target.IntValue);
            Assert.AreEqual(copy.FloatValue, target.FloatValue);
        }
    }
}
