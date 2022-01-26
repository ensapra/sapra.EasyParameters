using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sapra.EasyParameters
{
    [System.Serializable]
    public class EPComponentList
    {
        public List<EPComponent> easyParameterList = new List<EPComponent>();
        public void ApplyParameters(Animator _animator)
        {
            foreach (EPComponent item in easyParameterList)
            {
                item.ProcessEasyParameter(_animator);
            }
        }
    }
}
