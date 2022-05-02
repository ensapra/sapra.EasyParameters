using UnityEngine;
using System.Reflection;

namespace sapra.EasyParameters
{
    public abstract class EasyParameter
    {
        public string fieldName = "";
        [SerializeField]
        public string nameOnAnimator = "";
        public object finalObject = null;
        public abstract object GetSelectedObject();
        public void SetParameter(string fieldName, string nameOnAnimator)
        {
            this.fieldName = fieldName;
            this.nameOnAnimator = nameOnAnimator;
        }
        public void ProcessEasyParameter(Animator _animator)
        {
            object originalComponent = GetSelectedObject();
            if(originalComponent == null)
                return;
            string[] fields = fieldName.Split('/');
            string variableName = fields[fields.Length-1].Split(' ')[1];
            object objectFound = null;
            for(int i = 1; i < fields.Length; i++)
            {
                if(objectFound == null)
                    objectFound = GetParameter(originalComponent, fields[i]);
                else
                    objectFound = GetParameter(objectFound, fields[i]);
            }
            this.finalObject = objectFound;
            SetAnimator(_animator);
        }
        object GetParameter(object component, string field)
        {
            string[] parts = field.Split(' ');
            field = parts[parts.Length-1];
            FieldInfo fieldInfo = component.GetType().GetField(field);
            if(fieldInfo != null)        
                return fieldInfo.GetValue(component);        

            PropertyInfo paramInfo = component.GetType().GetProperty(field);
            if(paramInfo != null)        
                return paramInfo.GetValue(component);
            
            return null;
        }
        void SetAnimator(Animator _animator)
        {
            if(finalObject == null)
                return;
            if(finalObject is float)
                _animator.SetFloat(nameOnAnimator, (float)finalObject);
            
            if(finalObject is int)        
                _animator.SetInteger(nameOnAnimator, (int)finalObject);
            
            if(finalObject is bool)        
                _animator.SetBool(nameOnAnimator, (bool)finalObject);
        }
    }
}