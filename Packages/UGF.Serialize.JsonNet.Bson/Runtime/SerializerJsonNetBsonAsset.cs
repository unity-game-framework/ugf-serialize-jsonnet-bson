using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UGF.JsonNet.Runtime;
using UGF.JsonNet.Runtime.Converters;
using UGF.Serialize.JsonNet.Runtime;
using UGF.Serialize.JsonNet.Runtime.Binders;
using UGF.Serialize.Runtime;
using UnityEngine;

namespace UGF.Serialize.JsonNet.Bson.Runtime
{
    [CreateAssetMenu(menuName = "Unity Game Framework/Serialize/Serializer JsonNet Bson", order = 2000)]
    public class SerializerJsonNetBsonAsset : SerializerAsset<byte[]>
    {
        [SerializeField] private SerializerJsonNetSettings m_settings = new SerializerJsonNetSettings();
        [SerializeField] private List<ConvertNameData> m_serializeNames = new List<ConvertNameData>();
        [SerializeField] private List<ConvertNameData> m_deserializeNames = new List<ConvertNameData>();
        [SerializeField] private bool m_allowAllTypes = true;
        [SerializeField] private List<SerializeTypeCollectionAsset> m_collections = new List<SerializeTypeCollectionAsset>();

        public SerializerJsonNetSettings Settings { get { return m_settings; } }
        public List<ConvertNameData> SerializeNames { get { return m_serializeNames; } }
        public List<ConvertNameData> DeserializeNames { get { return m_deserializeNames; } }
        public bool AllowAllTypes { get { return m_allowAllTypes; } set { m_allowAllTypes = value; } }
        public List<SerializeTypeCollectionAsset> Collections { get { return m_collections; } }

        [Serializable]
        public struct ConvertNameData
        {
            [SerializeField] private string m_from;
            [SerializeField] private string m_to;

            public string From { get { return m_from; } set { m_from = value; } }
            public string To { get { return m_to; } set { m_to = value; } }

            public bool IsValid()
            {
                return !string.IsNullOrEmpty(m_from) && !string.IsNullOrEmpty(m_to);
            }
        }

        protected override ISerializer<byte[]> OnBuildTyped()
        {
            JsonSerializerSettings settings = OnCreateSettings();
            var serializer = new SerializerJsonNetBson(settings);

            settings.SerializationBinder = OnCreateTypesBinder();

            OnSetupNames(serializer);

            return serializer;
        }

        protected virtual JsonSerializerSettings OnCreateSettings()
        {
            JsonSerializerSettings settings = JsonNetUtility.CreateDefault();

            settings.ReferenceLoopHandling = m_settings.ReferenceLoopHandling;
            settings.MissingMemberHandling = m_settings.MissingMemberHandling;
            settings.ObjectCreationHandling = m_settings.ObjectCreationHandling;
            settings.NullValueHandling = m_settings.NullValueHandling;
            settings.DefaultValueHandling = m_settings.DefaultValueHandling;
            settings.PreserveReferencesHandling = m_settings.PreserveReferencesHandling;
            settings.TypeNameHandling = m_settings.TypeNameHandling;
            settings.MetadataPropertyHandling = m_settings.MetadataPropertyHandling;
            settings.TypeNameAssemblyFormatHandling = m_settings.TypeNameAssemblyFormatHandling;
            settings.ConstructorHandling = m_settings.ConstructorHandling;
            settings.Formatting = m_settings.Formatting;
            settings.DateFormatHandling = m_settings.DateFormatHandling;
            settings.DateTimeZoneHandling = m_settings.DateTimeZoneHandling;
            settings.DateParseHandling = m_settings.DateParseHandling;
            settings.FloatFormatHandling = m_settings.FloatFormatHandling;
            settings.FloatParseHandling = m_settings.FloatParseHandling;
            settings.StringEscapeHandling = m_settings.StringEscapeHandling;
            settings.DateFormatString = m_settings.DateFormatString;
            settings.CheckAdditionalContent = m_settings.CheckAdditionalContent;

            if (m_settings.MaxDepth > 0)
            {
                settings.MaxDepth = m_settings.MaxDepth;
            }

            return settings;
        }

        protected virtual void OnSetupNames(SerializerJsonNetBson serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            for (int i = 0; i < m_serializeNames.Count; i++)
            {
                ConvertNameData data = m_serializeNames[i];

                if (!data.IsValid()) throw new ArgumentException("Value should be valid.", nameof(data));

                serializer.SerializeNames.Add(data.From, data.To);
            }

            for (int i = 0; i < m_deserializeNames.Count; i++)
            {
                ConvertNameData data = m_deserializeNames[i];

                if (!data.IsValid()) throw new ArgumentException("Value should be valid.", nameof(data));

                serializer.DeserializeNames.Add(data.From, data.To);
            }
        }

        protected virtual ISerializationBinder OnCreateTypesBinder()
        {
            var binder = new ConvertTypeNameBinder(new ConvertTypeProvider(), m_allowAllTypes ? new DefaultSerializationBinder() : new SerializeJsonNetDisabledBinder());

            OnSetupTypes(binder.Provider);

            return binder;
        }

        protected virtual void OnSetupTypes(IConvertTypeProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var types = new Dictionary<object, Type>();

            for (int i = 0; i < m_collections.Count; i++)
            {
                SerializeTypeCollectionAsset collection = m_collections[i];

                collection.GetTypes(types);
            }

            foreach ((object id, Type type) in types)
            {
                provider.Add(type, id.ToString());
            }
        }
    }
}
