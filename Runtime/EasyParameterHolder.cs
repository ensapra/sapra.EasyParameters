using System.Collections.Generic;
using UnityEngine;

namespace sapra.EasyParameters
{
    public class EasyParameterHolder : MonoBehaviour
    {
        public Animator animator;
        public List<EasyParameter> parameters;

        // Update is called once per frame
        void Update()
        {
            foreach(EasyParameter parameter in parameters){
                parameter.Update(animator);
            }
        }
    }
}
