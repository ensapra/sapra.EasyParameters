using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sapra.EasyParameters
{
    [RequireComponent(typeof(Animator))]
    public class ParameterController : MonoBehaviour
    {
        public EPComponentList easyParameter = new EPComponentList();
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        void Update()
        {
            easyParameter.ApplyParameters(_animator);    
        }
    }
}