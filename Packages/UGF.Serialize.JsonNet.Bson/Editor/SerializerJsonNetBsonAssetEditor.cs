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
        private ReorderableListKeyAndValueDrawer m_listSerializeNames;
        private ReorderableListKeyAndValueDrawer m_listDeserializeNames;
        private SerializedProperty m_propertyAllowAllTypes;
        private ReorderableListDrawer m_listCollections;
        private ReorderableListSelectionDrawerByElement m_listCollectionsSelection;

        private void OnEnable()
        {
            m_propertySettings = serializedObject.FindProperty("m_settings");

            m_listSerializeNames = new ReorderableListKeyAndValueDrawer(serializedObject.FindProperty("m_serializeNames"), "m_from", "m_to")
            {
                DisplayLabels = true
            };

            m_listDeserializeNames = new ReorderableListKeyAndValueDrawer(serializedObject.FindProperty("m_deserializeNames"), "m_from", "m_to")
            {
                DisplayLabels = true
            };

            m_propertyAllowAllTypes = serializedObject.FindProperty("m_allowAllTypes");
            m_listCollections = new ReorderableListDrawer(serializedObject.FindProperty("m_collections"));

            m_listCollectionsSelection = new ReorderableListSelectionDrawerByElement(m_listCollections)
            {
                Drawer = { DisplayTitlebar = true }
            };

            m_listCollections.Enable();
            m_listCollectionsSelection.Enable();
        }

        private void OnDisable()
        {
            m_listCollections.Disable();
            m_listCollectionsSelection.Disable();
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

                m_listCollections.DrawGUILayout();
                m_listCollectionsSelection.DrawGUILayout();
            }
        }
    }
}
