using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sapra.EasyParameters
{
    [System.Serializable]
    public class EPComponent : EasyParameter
    {
        [SerializeField]
        public Component componentFound = null;
        public override object GetSelectedObject()
        {
            return componentFound;
        }
    }
}
