using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sapra.EasyParameters
{
    public class ParameterController : MonoBehaviour
    {
        public EPComponentList easyParameter = new EPComponentList();
        public Animator _animator;
        void Update()
        {
            easyParameter.ApplyParameters(_animator);    
        }
    }
}