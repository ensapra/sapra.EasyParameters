using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace sapra.EasyParameters.Editor
{
    [CustomPropertyDrawer(typeof(EasyParameter))]
    public class EasyParameterDrawer : PropertyDrawer
    {
        private const string VALUE_HOLDER_UNITY = "valueHolderComponent";
        private const string VALUE_HOLDER_REF = "valueHolderReference";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Begin Property Drawer
            var indent = EditorGUI.indentLevel;
            position.height = EditorGUIUtility.singleLineHeight*2+EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.indentLevel = 0;
            EditorGUI.BeginProperty(position, label, property);

            GenerateTopLine(property, position);
            GenerateBottomLine(property, position);

            //End property Drawer
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
        private void GenerateBottomLine(SerializedProperty property, Rect position)
        {
            SerializedProperty componentProperty = property.FindPropertyRelative(VALUE_HOLDER_UNITY);
            SerializedProperty referenceProperty = property.FindPropertyRelative(VALUE_HOLDER_REF);

            SerializedProperty propertyName = property.FindPropertyRelative("fieldName");
            SerializedProperty isReference = property.FindPropertyRelative("isReference");
            object FieldsGetter = isReference.boolValue ? referenceProperty.managedReferenceValue : componentProperty.objectReferenceValue;
            var fieldName = propertyName.stringValue;

            var ButtonPosition = new Rect(position.x, position.y+EditorGUIUtility.singleLineHeight+EditorGUIUtility.standardVerticalSpacing, position.width-position.width/4,EditorGUIUtility.singleLineHeight);
            var ButtonText = fieldName.Equals("") ? "Select a Field" : fieldName;
            
            if(FieldsGetter == null)
                EditorGUI.DropShadowLabel(ButtonPosition, "An Object should be selected");
            else
            {
                if(GUI.Button(ButtonPosition, ButtonText))
                {
                    GenericMenu newMenu = new GenericMenu();
                    newMenu.AddItem(new GUIContent("None"), fieldName.Equals(""), 
                    () =>
                    {
                        propertyName.stringValue = "";
                        property.serializedObject.ApplyModifiedProperties();
                    });
                    object[] objectsFound = GetObjects(FieldsGetter);
                    foreach(object objectFound in objectsFound)
                    {
                        List<string> fieldsFound = new List<string>();
                        GetFields(objectFound.GetType(), ref fieldsFound, objectFound.GetType().Name);
                        foreach(string field in fieldsFound)      
                        {
                            newMenu.AddItem(new GUIContent(field), fieldName.Equals(field), 
                            () => {
                                Undo.RecordObject(property.serializedObject.targetObject, "Added a new parameters to " + property.serializedObject.targetObject.name);
                                if(isReference.boolValue)
                                    referenceProperty.managedReferenceValue = objectFound;
                                else
                                    componentProperty.objectReferenceValue = objectFound as Component;
                                
                                property.FindPropertyRelative("fieldName").stringValue = field;
                                property.serializedObject.ApplyModifiedProperties();
                            });          
                        }           
                    }
                    newMenu.ShowAsContext();
                }
            }
            var LabelPosition = new Rect(position.x+(position.width-position.width/4)+3, ButtonPosition.y, position.width-position.width/4-3, EditorGUIUtility.singleLineHeight);
            EditorGUI.PrefixLabel(LabelPosition, new GUIContent("Parameter name"));
        }
        private object[] GetObjects(object fromObject)
        {
            if(fromObject is Component component)
            {
                Component[] arrayOfComponents;
                if(component != null)
                    arrayOfComponents = component.GetComponents<Component>();
                else
                    arrayOfComponents = new Component[0];
                return arrayOfComponents;
            }
            else
                return new object[]{fromObject};
        }
        
        private void GetFields(Type objectType, ref List<string> newFields, string baseDirection)
        {
            FieldInfo[] fieldsInfo = objectType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] propertyInfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SuppressChangeType);
            if(fieldsInfo.Length > 0)
                AddFieldInfo(fieldsInfo, newFields, baseDirection);
            
            if(propertyInfos.Length > 0)
                AddPropertyInfos(propertyInfos, newFields, baseDirection);

            /* if(fieldsInfo.Length <= 0 && propertyInfos.Length<= 0)
                newFields.Add(baseDirection); */
        }

        void AddFieldInfo(FieldInfo[] fieldInfos, List<string> fields, string currentDirection)
        {
            foreach(FieldInfo info in fieldInfos)
            {
                Type objectType = info.FieldType;
                string fieldName = GetWithType(objectType, info.Name);
                if(!fieldName.Equals(""))
                    fields.Add(currentDirection + fieldName);
    /*             Type objectType = info.FieldType;
                baseDirection = currentDirection + GetWithType(objectType, info.Name);  
                GetFields(objectType, ref fields, baseDirection); */
            }
        }

        void AddPropertyInfos(PropertyInfo[] propertyInfos, List<string> fields, string currentDirection)
        {
            foreach(PropertyInfo info in propertyInfos)
            {
                try
                {
                    Type objectType = info.PropertyType;
                    string fieldName = GetWithType(objectType, info.Name); 
                    if(!fieldName.Equals(""))
                        fields.Add(currentDirection + fieldName);
                }
                catch (System.Exception)
                {    
                    continue;
                }                     
            }
        }
        string GetWithType(Type value, string fieldName)
        {
            if(value == null)
                return "";   
            
            if(value.IsEquivalentTo(typeof(float)))     
            { 
                return "/float " + fieldName;
            }
            
            if(value.IsEquivalentTo(typeof(int)))     
            {
                return "/int " + fieldName;
            }

            if(value.IsEquivalentTo(typeof(bool)))
            {
                return "/bool " + fieldName;
            }

            return "";
        }

        //Contains the fields options and the name on the animator
        private void GenerateTopLine(SerializedProperty property, Rect position)
        {
            var ComponentPosition = new Rect(position.x, position.y, position.width-position.width/4, EditorGUIUtility.singleLineHeight);
            var TextPosition = new Rect(position.x+(position.width-position.width/4)+3, position.y, position.width/4-3, EditorGUIUtility.singleLineHeight);
            ObjectHolderLine(property, ComponentPosition);
            SerializedProperty nameOnAnimator = property.FindPropertyRelative("nameOnAnimator");
            nameOnAnimator.stringValue = EditorGUI.TextField(TextPosition, nameOnAnimator.stringValue, EditorStyles.textField);
        }


        protected virtual void ObjectHolderLine(SerializedProperty property, Rect position)
        {
            EditorGUI.ObjectField(position, property.FindPropertyRelative(VALUE_HOLDER_UNITY), GUIContent.none);
            property.FindPropertyRelative("isReference").boolValue = false;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight*2 + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}