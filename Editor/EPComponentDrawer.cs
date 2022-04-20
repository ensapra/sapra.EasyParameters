#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using sapra.EasyParameters;

namespace sapra.EasyParameters.Editor
{
    [CustomPropertyDrawer(typeof(EPComponent))]
    public class EPComponentDrawer : EasyParameterDrawer
    {
        public override GenericMenu GenerateSelectionMenu(object component, string currentDirection, SerializedProperty property)
        {
            GenericMenu newMenu = new GenericMenu();
            Component currentComponent = (Component)component;
            Component[] arrayOfComponents;
            if(currentComponent != null)
                arrayOfComponents = currentComponent.GetComponents<Component>();
            else
                arrayOfComponents = new Component[0];

            newMenu.AddItem(new GUIContent("None"), currentDirection.Equals(""), 
            () =>
            {
                property.FindPropertyRelative("fieldName").stringValue = "";
                property.serializedObject.ApplyModifiedProperties();
            });

            foreach(Component componentFound in arrayOfComponents)
            {
                List<string> fieldsFound = new List<string>();
                GetFields(componentFound.GetType(), ref fieldsFound, componentFound.GetType().ToString());
                foreach(string field in fieldsFound)      
                {
                    string simpleDirection = getWithoutDot(field);
                    newMenu.AddItem(new GUIContent(simpleDirection), currentDirection.Equals(simpleDirection), 
                    () => {
                        Undo.RecordObject(property.serializedObject.targetObject, "Added a new animation to " + property.serializedObject.targetObject.name);
                        property.FindPropertyRelative("componentFound").objectReferenceValue = componentFound;
                        property.FindPropertyRelative("fieldName").stringValue = field;
                        property.serializedObject.ApplyModifiedProperties();
                    });          
                }           
            }
            return newMenu;
        }

        public override object GetComponentReference(SerializedProperty property)
        {
            return (Component)property.FindPropertyRelative("componentFound").objectReferenceValue;
        }

        public override void NoComponent(Rect buttonPosition)
        {
            EditorGUI.DropShadowLabel(buttonPosition, "Select a component first");
        }

        public override void ObjectField(SerializedProperty property, Rect ComponentPosition)
        {
            //Second line of editor
            EditorGUI.BeginChangeCheck();
            EditorGUI.ObjectField(ComponentPosition, property.FindPropertyRelative("componentFound"), GUIContent.none);
            if(EditorGUI.EndChangeCheck())     
                property.FindPropertyRelative("fieldName").stringValue = "";    
        }
    }
}
#endif