using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YahiaDroubiTask
{
    public class SolutesContainers : MonoBehaviour, Interface_interactbles
    {
        public GameObject Blend;
        SkinnedMeshRenderer thisBottle_skdMshRndrer;

        public bool Solu_Interact(float AmountDrpNeeds, float amountDrpHave,BlendInfo anythingAcceptedHere, out bool isDropperAbsorb)
        {
            isDropperAbsorb = true;

            if (isDropperAbsorb)
            {
                return ReduceTheBlend(AmountDrpNeeds); // will try to absorb from bottle and return done?
            }else
            {
                return false; //but will never be here , we are controlling boolean 
            }
        }

        bool ReduceTheBlend(float perc)
        {
            if (!Blend || !thisBottle_skdMshRndrer)
            {
                //print(transform.name + ": Hello Yahia, plz attach the blend first");
                return false;
            }

            float newAmountAfterDecr = (thisBottle_skdMshRndrer.GetBlendShapeWeight(9) - perc);
            if (newAmountAfterDecr >= 0)
            {
                thisBottle_skdMshRndrer.SetBlendShapeWeight(9, newAmountAfterDecr);
                return true;//absorbing is done
            }
            else
            {
                //print(transform.name + ": Change the amount ur trying to absorb , I don't have that amount");
                return false;
            }
        }

        private void Awake()
        {
            if (!Blend)
                Blend = transform.Find("Blend").gameObject; //search for child with name Blend

            //caching components
            thisBottle_skdMshRndrer = Blend.GetComponent<SkinnedMeshRenderer>();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject GetBlend()
        {

            return Blend;
        }



        public float initialAmount = 100;



        public void ResetBottlesSitu()
        {
            thisBottle_skdMshRndrer.SetBlendShapeWeight(9, initialAmount);

        }

        public void OnEnable()
        {
            GeneralMethods.sing.ResetNowObserver += ResetBottlesSitu;
        }

        private void OnDisable()
        {

            GeneralMethods.sing.ResetNowObserver -= ResetBottlesSitu;


        }
    }
}