using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YahiaDroubiTask
{

    public class BlendInfo : MonoBehaviour
    {
        public bool water = false;
        public bool starch = false;
        public bool IKI = false;




        public SkinnedMeshRenderer skMsRndr ;
        public Material material ;
        public Color materialblendColor;

        void Awake()
        {
            if(!skMsRndr)
                skMsRndr = transform.GetComponent<SkinnedMeshRenderer>();

            if(skMsRndr)
                material = skMsRndr.material;

            materialblendColor = material.color;



        }





    }

}