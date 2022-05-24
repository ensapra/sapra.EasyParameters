using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Reflection;
using sapra.EasyParameters;

namespace sapra.EasyParameters.Editor
{
    public abstract class EasyParameterDrawer<T> : PropertyDrawer where T : class
    {
        /// <summary>
        /// Draws the second line on the editor
        /// <summary/>
        protected abstract void ObjectField(SerializedProperty property, Rect ComponentPosition);

        /// <summary>
        /// Should return the list of objects to be analized
        /// <summary/>
        protected abstract object[] GetObjects(object component);
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Begin Property Drawer
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.BeginProperty(position, label, property);

            //Get currentValues
            object currentComponent = null;
            if(typeof(T).IsEquivalentTo(typeof(Component)))
                currentComponent = property.FindPropertyRelative("parentObject").objectReferenceValue;
            else
                currentComponent = property.FindPropertyRelative("parentObject").managedReferenceValue;
            
            string fieldValue = property.FindPropertyRelative("fieldName").stringValue;
            string currentDirection = "";
            if(fieldValue != "" && currentComponent != null)
                currentDirection = getWithoutDot(currentComponent.GetType() + "/" + fieldValue);

            //Declare dimentions of each button
            var ButtonPosition = new Rect(position.x, position.y, position.width-position.width/4, position.height/2-5);
            var LabelPosition = new Rect(position.x+(position.width-position.width/4)+3, position.y+2, position.width-position.width/4-3, position.height/2-5);
            var ComponentPosition = new Rect(position.x, position.y+position.height/2-2f, position.width-position.width/4, position.height/2-5);
            var TextPosition = new Rect(position.x+(position.width-position.width/4)+3, position.y+position.height/2-2f, position.width/4-3, position.height/2-5);

            //First line of editor
            EditorGUI.PrefixLabel(LabelPosition, new GUIContent("Parameter name"));

            string buttonText = currentDirection;
            if(currentDirection == "")
                buttonText = "Select a Field";        
            if(currentComponent == null)
                EditorGUI.DropShadowLabel(ButtonPosition, "An Object should be selected");
            else
            {       
                if(GUI.Button(ButtonPosition, buttonText))
                {
                    GenericMenu newMenu = new GenericMenu();
                    newMenu.AddItem(new GUIContent("None"), currentDirection.Equals(""), 
                    () =>
                    {
                        property.FindPropertyRelative("fieldName").stringValue = "";
                        property.serializedObject.ApplyModifiedProperties();
                    });
                    object[] objectsFound = GetObjects(currentComponent);
                    foreach(object objectFound in objectsFound)
                    {
                        List<string> fieldsFound = new List<string>();
                        GetFields(objectFound.GetType(), ref fieldsFound, objectFound.GetType().ToString());
                        foreach(string field in fieldsFound)      
                        {
                            string simpleDirection = getWithoutDot(field);
                            newMenu.AddItem(new GUIContent(simpleDirection), currentDirection.Equals(simpleDirection), 
                            () => {
                                Undo.RecordObject(property.serializedObject.targetObject, "Added a new parameters to " + property.serializedObject.targetObject.name);
                                if(typeof(T).IsEquivalentTo(typeof(Component)))
                                    property.FindPropertyRelative("parentObject").objectReferenceValue = objectFound as Component;
                                else
                                    property.FindPropertyRelative("parentObject").managedReferenceValue = (T)objectFound;
                                property.FindPropertyRelative("fieldName").stringValue = field;
                                property.serializedObject.ApplyModifiedProperties();
                            });          
                        }           
                    }
                    newMenu.ShowAsContext();
                }
            }

            ObjectField(property, ComponentPosition);
            property.FindPropertyRelative("nameOnAnimator").stringValue = EditorGUI.TextField(TextPosition, property.FindPropertyRelative("nameOnAnimator").stringValue, EditorStyles.textField);
        
            //End property Drawer
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //Rewrite Height
            float extraHeight = 30;
            return base.GetPropertyHeight(property, label)+extraHeight;
        }

        protected void GetFields(Type objectType, ref List<string> newFields, string baseDirection)
        {
            FieldInfo[] fieldsInfo = objectType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] propertyInfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SuppressChangeType);
            if(fieldsInfo.Length > 0)
                AddFieldInfo(fieldsInfo, newFields, baseDirection);
            if(propertyInfos.Length > 0)
                AddPropertyInfos(propertyInfos, newFields, baseDirection);

            if(fieldsInfo.Length <= 0 && propertyInfos.Length<= 0)
                newFields.Add(baseDirection);
        }
        void AddFieldInfo(FieldInfo[] fieldInfos, List<string> fields, string currentDirection)
        {
            string baseDirection = currentDirection;
            foreach(FieldInfo info in fieldInfos)
            {
                Type objectType = info.FieldType;
                baseDirection = currentDirection + "/" + AddItemToList(fields, objectType, info.Name);  
                GetFields(objectType, ref fields, baseDirection);
            }
        }
        void AddPropertyInfos(PropertyInfo[] propertyInfos, List<string> fields, string currentDirection)
        {
            string baseDirection = currentDirection;
            foreach(PropertyInfo info in propertyInfos)
            {
                try
                {
                    Type objectType = info.PropertyType;
                    string resultingName = AddItemToList(fields, objectType, info.Name); 
                    if(resultingName != info.Name)
                        fields.Add(currentDirection + "/" + resultingName);
                }
                catch (System.Exception)
                {    
                    continue;
                }                     
            }
        }
        string AddItemToList(List<string> fields, Type value, string fieldName)
        {
            string resultWord = fieldName;
            if(value == null)
                return resultWord;   
            
            if(value.IsEquivalentTo(typeof(float)))     
            { 
                resultWord = "float " + fieldName;
            }
            
            if(value.IsEquivalentTo(typeof(int)))     
            {
                resultWord = "int " + fieldName;
            }

            if(value.IsEquivalentTo(typeof(bool)))
            {
                resultWord = "bool " + fieldName;
            }
            return resultWord;
        }

        protected string getWithoutDot(string target)
        {
            string[] parts = target.Split('.');
            return parts[parts.Length-1];
        }
    }
}