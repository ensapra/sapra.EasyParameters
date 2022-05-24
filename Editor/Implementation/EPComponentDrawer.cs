using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using sapra.EasyParameters;

namespace sapra.EasyParameters.Editor
{
    [CustomPropertyDrawer(typeof(EPComponent))]
    public class EPComponentDrawer : EasyParameterDrawer<Component>
    {
        protected override object[] GetObjects(object component)
        {
            Component currentComponent = (Component)component;
            Component[] arrayOfComponents;
            if(currentComponent != null)
                arrayOfComponents = currentComponent.GetComponents<Component>();
            else
                arrayOfComponents = new Component[0];
            return arrayOfComponents;
        }

        protected override void ObjectField(SerializedProperty property, Rect ComponentPosition)
        {
            //Second line of editor
            EditorGUI.BeginChangeCheck();
            EditorGUI.ObjectField(ComponentPosition, property.FindPropertyRelative("parentObject"), GUIContent.none);
            if(EditorGUI.EndChangeCheck())     
                property.FindPropertyRelative("fieldName").stringValue = "";    
        }
    }
}