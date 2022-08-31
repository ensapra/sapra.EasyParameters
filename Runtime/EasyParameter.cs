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

        public void SetParameter(string fieldName, string nameOnAnimator)
        {
            this.fieldName = fieldName;
            this.nameOnAnimator = nameOnAnimator;
        }
        public void SetParentObject(object parentObject)
        {
            if(parentObject is Component component)
            {
                this.valueHolderComponent = component;
                isReference = false;
            }
            else
            {
                this.valueHolderReference = parentObject;
                isReference = true;
            }
            
        }
        public void ProcessEasyParameter(Animator _animator)
        {
            if(valueHolderComponent == null && valueHolderReference == null)
                return;

            object valueHolder = isReference ? valueHolderReference : valueHolderComponent;
            object valueFound = null;
            string[] parts = fieldName.Split(" ");
            var realFieldName = parts[parts.Length-1];
            FieldInfo fieldInfo = valueHolder.GetType().GetField(realFieldName);
            if(fieldInfo != null)        
                valueFound = fieldInfo.GetValue(valueHolder);        
            else
            {
                PropertyInfo paramInfo = valueHolder.GetType().GetProperty(realFieldName);
                if(paramInfo != null)        
                    valueFound = paramInfo.GetValue(valueHolder);
            }
            
            SetAnimator(_animator, valueFound);
        }
        void SetAnimator(Animator _animator, object value)
        {
            if(value == null)
                return;
            
            if(value is float)
                _animator.SetFloat(nameOnAnimator, (float)value);
            
            if(value is int)        
                _animator.SetInteger(nameOnAnimator, (int)value);
            
            if(value is bool)        
                _animator.SetBool(nameOnAnimator, (bool)value);
        }
    }
}
