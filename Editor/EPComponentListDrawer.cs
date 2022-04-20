#if UNITY_EDITOR
using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using sapra.EasyParameters;

namespace sapra.EasyParameters.Editor
{
    [CustomPropertyDrawer(typeof(EPComponentList))]
    public class EPComponentListDrawer : PropertyDrawer
    {
        ReorderableList currentList;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            indent = 0;
            EditorGUI.BeginChangeCheck();
            if(currentList == null)
                MakeList(property);
            currentList.DoList(position);
            if (EditorGUI.EndChangeCheck()) 
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
        void MakeList(SerializedProperty property)
        {
            var list = property.FindPropertyRelative("easyParameterList");
            currentList = new ReorderableList(property.serializedObject, list, false, true, true, true);
            currentList.drawHeaderCallback = (Rect rect) => {EditorGUI.LabelField(rect, "Animator parameters");};
            currentList.elementHeightCallback = (int index) => {return 2.6f*EditorGUIUtility.singleLineHeight;};
            currentList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                rect = new Rect(rect.x, rect.y+4, rect.width, rect.height);
                EditorGUI.PropertyField(rect, list.GetArrayElementAtIndex(index));}
                ;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(currentList != null)
                return currentList.GetHeight();
            else
                return base.GetPropertyHeight(property, label);
        }
    }
}
#endif