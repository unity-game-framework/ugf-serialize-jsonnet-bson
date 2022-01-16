using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Scopes;
using UGF.Serialize.JsonNet.Bson.Runtime;
using UnityEditor;

namespace UGF.Serialize.JsonNet.Bson.Editor
{
    [CustomEditor(typeof(SerializerJsonNetBsonAsset), true)]
    internal class SerializerJsonNetBsonAssetEditor : UnityEditor.Editor
    {
        private SerializedProperty m_propertySettings;
        private SerializerJsonNetBsonConvertNamesListDrawer m_listSerializeNames;
        private SerializerJsonNetBsonConvertNamesListDrawer m_listDeserializeNames;
        private SerializedProperty m_propertyAllowAllTypes;
        private ReorderableListDrawer m_listTypeProviders;
        private ReorderableListSelectionDrawerByElement m_listTypeProvidersSelection;

        private void OnEnable()
        {
            m_propertySettings = serializedObject.FindProperty("m_settings");
            m_listSerializeNames = new SerializerJsonNetBsonConvertNamesListDrawer(serializedObject.FindProperty("m_serializeNames"));
            m_listDeserializeNames = new SerializerJsonNetBsonConvertNamesListDrawer(serializedObject.FindProperty("m_deserializeNames"));
            m_propertyAllowAllTypes = serializedObject.FindProperty("m_allowAllTypes");
            m_listTypeProviders = new ReorderableListDrawer(serializedObject.FindProperty("m_typeProviders"));

            m_listTypeProvidersSelection = new ReorderableListSelectionDrawerByElement(m_listTypeProviders)
            {
                Drawer =
                {
                    DisplayTitlebar = true
                }
            };

            m_listTypeProviders.Enable();
            m_listTypeProvidersSelection.Enable();
        }

        private void OnDisable()
        {
            m_listTypeProviders.Disable();
            m_listTypeProvidersSelection.Disable();
        }

        public override void OnInspectorGUI()
        {
            using (new SerializedObjectUpdateScope(serializedObject))
            {
                EditorIMGUIUtility.DrawScriptProperty(serializedObject);

                EditorGUILayout.PropertyField(m_propertySettings);

                m_listSerializeNames.DrawGUILayout();
                m_listDeserializeNames.DrawGUILayout();

                EditorGUILayout.PropertyField(m_propertyAllowAllTypes);

                m_listTypeProviders.DrawGUILayout();
                m_listTypeProvidersSelection.DrawGUILayout();
            }
        }
    }
}
