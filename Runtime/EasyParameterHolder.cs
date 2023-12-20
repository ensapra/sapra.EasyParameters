using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sapra.EasyParameters
{
    public class EasyParameterHolder : MonoBehaviour
    {
        public Animator animator;
        public List<EasyParameter> parameters;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            foreach(EasyParameter parameter in parameters){
                parameter.Update(animator);
            }
        }
    }
}
