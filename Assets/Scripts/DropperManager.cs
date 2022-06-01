using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YahiaDroubiTask
{
    public class DropperManager : Movables
    {
        public void ResetDroppersSitu()
        {
/*            if(thisDropperBlClone_skMeshRderer)
                thisDropperBlClone_skMeshRderer.SetBlendShapeWeight(9, 0);
*/
            prevAmountOfSolinthisDrpr = 0f;
            AmountDrpNeeds = 0f;

            Destroy(oldBlendClone); //empty at first

        }

        public void OnEnable()
        {
            GeneralMethods.sing.ResetNowObserver += ResetDroppersSitu;
        }

        private void OnDisable()
        {

            GeneralMethods.sing.ResetNowObserver -= ResetDroppersSitu;


        }

        public static float ChoosenAmountOfSolute = 50f; //any choosen value is global for all droppers so its static

        //[SerializeField] GameObject Blend;
        public Transform BlendTPos;

        SkinnedMeshRenderer thisDropperBlClone_skMeshRderer;

        [SerializeField] bool isEmpty = true;

        Quaternion initialRotation, inActionRotation = new Quaternion(0, 90, 90, 0);
        public override void MakeAnotherSeperatedAction()
        {
            RotateTheDropper();
        }


        public void RotateTheDropper()
        {
            //thisTransform.Rotate(thisTransform.position, degree);
            thisTransform.localRotation = inActionRotation;
        }



        private void FixedUpdate()
        {

        }

        protected override void Awake()
        {
            base.Awake();
            initialRotation = thisTransform.localRotation;


            /*            if (!Blend)
                            Blend = thisTransform.GetChild(0).gameObject;

                        thisDropperBlClone_skMeshRderer = Blend.GetComponent<SkinnedMeshRenderer>();*/
        }

        protected override void OnMouseUp()
        {
            base.OnMouseUp(); // a must in c#

            thisTransform.localRotation = initialRotation;
        }



        //disabled in child to run parent one
        /* void Update()
         {
             //inherited implementation of parent functions won't be active unless its not declared in child (like this one)
             //or declared with base.funname() imple..

         }*/







        bool thisDropperIsAbsorbinSol;// ( out ) refrence , getting with the solutuion_interact method
        private float prevAmountOfSolinthisDrpr = 0f;
        float AmountDrpNeeds;



        private void OnTriggerEnter(Collider other)
        {   //must a rigid body be on one of the objects 

            if (!inAction) // since we are using auto lerping , we must not allow the trigger to always interact with solution 
                return;


            if (TasksManager.MissionID <3)
            {
                //print("other: " + other.name);
                UIObjsHandler.instance.AlertString = " Check Safety Rules and Fire the Bunsen first ";
                return;
            }

            if (prevAmountOfSolinthisDrpr + ChoosenAmountOfSolute <= 100)
            {
                AmountDrpNeeds = ChoosenAmountOfSolute;
            }
            else
            {
                AmountDrpNeeds = 0f; //it will absorb from bottle but ( 0 amount ) so no bad effect ( on bottle ),, //  the dropper itself have a condition to not fill itself if nearfull
            }


            //( Solu_Interact ) handles visuals and action in another side like cylinders or soultions bottles
            var SolutionInteract = other.GetComponentInParent<Interface_interactbles>();

            if (SolutionInteract != null)
            {
                try
                {
                    //bool done = SolutionInteract.Solu_Interact(AmountDrpNeeds, prevAmountOfSolinthisDrpr, oldBlendClone.GetComponent<BlendInfo>().materialblendColor, oldBlendClone.GetComponent<BlendInfo>().thisBlendType, out thisDropperIsAbsorbinSol);
                    bool done = SolutionInteract.Solu_Interact(AmountDrpNeeds, prevAmountOfSolinthisDrpr, oldBlendClone.GetComponent<BlendInfo>(), out thisDropperIsAbsorbinSol);
                    if (!done)
                        return;
                }
                catch
                {

                    //the interface method is handling 2 types of solution: (1 method 2 types) giver and taker ,,, SO some parameters are for one and some for the other
                    // in this case (Giver:Bottles) don't care about dropper blend color and type they just give,, so any value is accepted with no effect 
                    //but oldBlendClone is null first time, so try and catch (but taker cares ofcourse) 

                    bool done = SolutionInteract.Solu_Interact(AmountDrpNeeds, prevAmountOfSolinthisDrpr, new BlendInfo(), out thisDropperIsAbsorbinSol);
                    if (!done)
                        return;




                }
            }
            else return;



            HandlingDropperVisualsAndAction(SolutionInteract.GetBlend()); //must be downhere cuz of guard-clauses



        }

        GameObject oldBlendClone;
        void HandlingDropperVisualsAndAction(GameObject interactBlend)
        {



            if (thisDropperIsAbsorbinSol)
            {
                float TotalEndInDropper = (prevAmountOfSolinthisDrpr + ChoosenAmountOfSolute);
                //print("dropper is absorbing from lotion bottle ");
                if (TotalEndInDropper <= 100)
                {
                    BlendInfo oldBlendCloneBInfo;
                    if (!oldBlendClone)
                    { //first time absorbing

                        oldBlendClone = GameObject.Instantiate(interactBlend, BlendTPos.position, BlendTPos.rotation);
                        oldBlendClone.transform.localScale = BlendTPos.localScale * 2.5f;
                        oldBlendClone.transform.SetParent(thisTransform);


                        oldBlendCloneBInfo = oldBlendClone.GetComponent<BlendInfo>();

                    }
                    else
                    {
                        oldBlendCloneBInfo = oldBlendClone.GetComponent<BlendInfo>();

                        if (prevAmountOfSolinthisDrpr > 0)
                        {
                            
                            //getting the info of blend from giver(bottles) to dropper is by (Hit object of ontrigger) (but from dropper to taker -cylinder with The Interact Method) 


                            //mix color of solution in dropper (taking percentage of each color in calculations)
                            oldBlendCloneBInfo.material.color = ((oldBlendCloneBInfo.materialblendColor * (prevAmountOfSolinthisDrpr / TotalEndInDropper)) + (interactBlend.GetComponent<BlendInfo>().materialblendColor * (ChoosenAmountOfSolute / TotalEndInDropper)));
                            oldBlendCloneBInfo.materialblendColor = oldBlendCloneBInfo.material.color; // material.color changes the color for real , but materialBC will still be the old one so

                            //mix types of solution in dropper
                            oldBlendCloneBInfo.water = (interactBlend.GetComponent<BlendInfo>().water || oldBlendCloneBInfo.water); // mixing :D means any true here or there is stronger
                            oldBlendCloneBInfo.starch = (interactBlend.GetComponent<BlendInfo>().starch || oldBlendCloneBInfo.starch);
                            oldBlendCloneBInfo.IKI = (interactBlend.GetComponent<BlendInfo>().IKI || oldBlendCloneBInfo.IKI);
                        }
                        else
                        {
                            oldBlendCloneBInfo.material.color = interactBlend.GetComponent<BlendInfo>().materialblendColor;
                            oldBlendCloneBInfo.materialblendColor = oldBlendCloneBInfo.material.color;
                            oldBlendCloneBInfo.water = interactBlend.GetComponent<BlendInfo>().water ; 
                            oldBlendCloneBInfo.starch = interactBlend.GetComponent<BlendInfo>().starch ;
                            oldBlendCloneBInfo.IKI = interactBlend.GetComponent<BlendInfo>().IKI; 

                        }


                        if (!oldBlendClone.activeInHierarchy)
                            oldBlendClone.SetActive(true);

                        oldBlendClone.transform.SetParent(thisTransform);

                    }



                    thisDropperBlClone_skMeshRderer = oldBlendCloneBInfo.skMsRndr;
                    if (thisDropperBlClone_skMeshRderer)
                        thisDropperBlClone_skMeshRderer.SetBlendShapeWeight(9, TotalEndInDropper);
                    //thisDropperBlClone_skMeshRderer.SetBlendShapeWeight(9, (prevAmountOfSolinthisDrpr + ChoosenAmountOfSolute));




                    //for next absorbing time
                    prevAmountOfSolinthisDrpr += ChoosenAmountOfSolute; //cus done now change prev

                }
                else
                {
                    //print("dropper is nearly full , you can only absorb the amount: " + (100f - prevAmountOfSolinthisDrpr));
                }



                GeneralMethods.sing.PlaySound(GeneralMethods.sing.absorbsound);
                isEmpty = false;
            }
            else
            {
                //print("dropper is pumping  ");
                if (ChoosenAmountOfSolute > 3)
                {
                    UIObjsHandler.instance.AlertString = "Wrong , 3 ml only required , Set in Wanted Amount Panel";
                }

                thisDropperBlClone_skMeshRderer.SetBlendShapeWeight(9, 0);
                prevAmountOfSolinthisDrpr = 0;//reset is a must

                if (oldBlendClone.activeInHierarchy)
                    oldBlendClone.SetActive(false);

                GeneralMethods.sing.PlaySound(GeneralMethods.sing.pump_sound);

            }
        }





    }

}