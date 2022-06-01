using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YahiaDroubiTask
{
    public class TorchManager : Movables
    {                           //inheritance




        protected override void Awake()
        {
            base.Awake();


        }


        //another way of detection 2.5D transform-transform position dis
        private void OnTriggerEnter(Collider other)
        {   //must a rigid body be on one of the objects 


            if (other.gameObject.layer != LayerMask.NameToLayer("Flammable")) //using layers
                return;

            if (other.gameObject.tag.Equals("FlameTransform") )
            {
                //( Way1:SendMessage ) don't need to specific tells where function is at any class but its (not recommended for performance optmization)
                //other.transform.root.SendMessage("FireTheFlame");


                //careful of Trig-colliders on another object since any of them will return the required valid result
                var thegasFlameTrans = other.GetComponentInParent<FireBunsenHandler>();
                if (thegasFlameTrans != null)
                    thegasFlameTrans.FireTheFlame();


            }
        }




        public override void MakeAnotherSeperatedAction()
        {
            //from movable class
        }


    }
}