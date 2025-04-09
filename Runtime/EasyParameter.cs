using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace sapra.EasyParameters
{
    [System.Serializable]
    public class EasyParameter 
    {
        [SerializeField] private string fieldName = "";
        [SerializeField] private string nameOnAnimator = "";
        [SerializeField]private Component valueHolderComponent;
        [SerializeReference]private object valueHolderReference;
        [SerializeField] protected bool isReference = false;

        private FieldInfo fieldInfo;
        private PropertyInfo propertyInfo;
        public enum State{FieldFound, PropertyFound, None};
        private State currentState = State.None;
        public void SetParameter(string fieldName, string nameOnAnimator)
        {
            this.fieldName = fieldName;
            this.nameOnAnimator = nameOnAnimator;
        }
        public void SetParentObject(object parentObject)
        {
            if(parentObject is Component component)
                this.valueHolderComponent = component;            
            else
                this.valueHolderReference = parentObject;
            isReference = !(parentObject is Component);
        }
        public void Update(Animator _animator)
        {
            if(valueHolderComponent == null && valueHolderReference == null)
                return;
            if(_animator == null) return;

            object valueHolder = isReference ? valueHolderReference : valueHolderComponent;
            object valueFound = null;
            string[] parts = fieldName.Split(" ");
            var realFieldName = parts[parts.Length-1];
            switch(currentState){
                case State.FieldFound:
                    valueFound = fieldInfo.GetValue(valueHolder);      
                break;
                case State.PropertyFound:
                    valueFound = propertyInfo.GetValue(valueHolder);      
                break;
                case State.None:
                    fieldInfo = valueHolder.GetType().GetField(realFieldName);
                    if(fieldInfo != null){
                        currentState = State.FieldFound;
                        break;
                    }

                    propertyInfo = valueHolder.GetType().GetProperty(realFieldName);
                    if(propertyInfo != null){
                        currentState = State.PropertyFound;
                        break;
                    }     
                break;
            }
            SetAnimator(_animator, valueFound);
        }
        void SetAnimator(Animator _animator, object value)
        {
            if(value == null)
                return;

            if(value is float){
                _animator.SetFloat(nameOnAnimator, (float)value);
                return;
            }
            
            if(value is int){       
                _animator.SetInteger(nameOnAnimator, (int)value);
                return;
            }

            if(value is bool){     
                _animator.SetBool(nameOnAnimator, (bool)value);
                return;
            }
        }
    }
}