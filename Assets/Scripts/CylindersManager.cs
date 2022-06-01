using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YahiaDroubiTask
{                                       //inherits class and interface
    public class CylindersManager : Movables, Interface_interactbles
    {
        [SerializeField] GameObject Blend;

        SkinnedMeshRenderer thisCylndr_skMshRenderer;
        BlendInfo myBlendInfo;

        Material thisPipeBlendMaterial;
        public static int remaningCylnds = 2;//must be global for both

        protected override void Awake()
        {
            base.Awake();

            if (!Blend)
                Blend = thisTransform.Find("Blend").gameObject; //search for child with name Blend



            // Caching Components
            if (Blend.GetComponent<BlendInfo>())
            {
                myBlendInfo = Blend.GetComponent<BlendInfo>();

            }


            if (myBlendInfo.skMsRndr)
                thisCylndr_skMshRenderer = myBlendInfo.skMsRndr;
            else//if this awake were faster BlendInfo Awake
                thisCylndr_skMshRenderer = Blend.GetComponent<SkinnedMeshRenderer>();





        }


        static float amountinCylinder1 = 0f, amountinCylinder2 = 0f;



        public bool Solu_Interact(float AmountDrpNeeds, float amountDrpHave, BlendInfo inDropBlendInfo, out bool isDropperAbsorb)
        {
            isDropperAbsorb = false;

            if (!isDropperAbsorb)
                return FillBlend(amountDrpHave, inDropBlendInfo); // will try to fill and return done or not
            else
                return false; //but will never be here , we are controlling boolean 
        }

        bool FillBlend(float amouDrpHave, BlendInfo newblendInfo)
        {
            if (!Blend || !thisCylndr_skMshRenderer)
            {
                ////print(thisTransform.name + ": Hello Yahia, plz attach the blend first");
                return false;
            }



            float AlreadyInCylinderAmount = thisCylndr_skMshRenderer.GetBlendShapeWeight(9);

            //previous amount plus the new
            float total_fill_amount = (AlreadyInCylinderAmount + amouDrpHave);

            if (total_fill_amount > 0)
            {
                if (!Blend.activeInHierarchy)
                    Blend.SetActive(true);
            }
            else
            {
                return false;
            }

            if ((total_fill_amount) <= 100)
            {


                if (amouDrpHave > 0)
                {
                    if ((total_fill_amount) > 6 && amouDrpHave <= 3)
                    {   //alert if amouDrpHave> 3 in dropper side
                        UIObjsHandler.instance.AlertString = "Wrong , Thats too much , 3ml of IKI and 3ml of solution are enough";
                    }

                    ////print("addition .color: " + ((thisPipeBlendMaterial.color * (AlreadyInCylinderAmount / AlreadyInCylinderAmount+amouDrpHave) ) + (newblendInfo.materialblendColor * (amouDrpHave / AlreadyInCylinderAmount+amouDrpHave) )));
                    if (AlreadyInCylinderAmount > 0)
                    {                                                                                               //last parameter is percentage of seconds color
                        thisPipeBlendMaterial.color = Color.Lerp(thisPipeBlendMaterial.color, newblendInfo.materialblendColor, (amouDrpHave / total_fill_amount));



                        //mixing type getting by method with type already in cylinder
                        myBlendInfo.water = (myBlendInfo.water || newblendInfo.water);//stronger bool is accpted
                        myBlendInfo.starch = (myBlendInfo.starch || newblendInfo.starch);
                        myBlendInfo.IKI = (myBlendInfo.IKI || newblendInfo.IKI);

                    }
                    else
                    {
                        thisPipeBlendMaterial.color = newblendInfo.materialblendColor;


                        myBlendInfo.water = newblendInfo.water;
                        myBlendInfo.starch = newblendInfo.starch;
                        myBlendInfo.IKI = newblendInfo.IKI;

                        ////print("its 0 , first time?");
                    }
                    thisCylndr_skMshRenderer.SetBlendShapeWeight(9, total_fill_amount);

                    //mission 3 must be Water in cylinder 1
                    if (Cylind_id == 0) //cylinder 1 id = 0
                    {
                        amountinCylinder1 = total_fill_amount;//static float for using it globally
                        if (TasksManager.MissionID == 3 && total_fill_amount == 3)
                        {
                            //if(wateronly)
                            if (myBlendInfo.water && !myBlendInfo.starch && !myBlendInfo.IKI)
                                TasksManager.MissionID++;
                            else
                            {
                                UIObjsHandler.instance.AlertString = "ur solution must be water only";
                            }
                        }

                        if (TasksManager.MissionID == 5 && total_fill_amount == 6 && (amountinCylinder2 == 6))
                        {//so this way the Arrangment(too) of pumping in mission 5 will be no problem (amountinc2 is static)

                            if (myBlendInfo.water && myBlendInfo.IKI && !myBlendInfo.starch)
                                TasksManager.MissionID++;
                            else
                            {
                                UIObjsHandler.instance.AlertString = "ur soulution in 1st pipe must be only water and IKI";
                                ////print("ur soulution in 1st pipe must be only water and IKI");
                            }

                        }



                    }
                    else if (Cylind_id == 1)//cylinder 2 id = 1
                    {
                        amountinCylinder2 = total_fill_amount;//static float for using it globally


                        if (TasksManager.MissionID == 4 && total_fill_amount == 3)
                        {
                            //if(startch only)
                            if (myBlendInfo.starch && !myBlendInfo.IKI && !myBlendInfo.water)
                                TasksManager.MissionID++;
                            else
                            {
                                UIObjsHandler.instance.AlertString = "Starch Only firstly on pipe 2";
                            }
                        }

                        //if am last cylinder absorbed I must check whats also in the other
                        if (TasksManager.MissionID == 5 && total_fill_amount == 6 && (amountinCylinder1 == 6))
                        {//so this way the Arrangment(too) of pumping in mission 5 will be no problem (amountinCylinder1 is static)

                            if (myBlendInfo.starch && myBlendInfo.IKI && !myBlendInfo.water)
                                TasksManager.MissionID++;
                            else
                            {
                                UIObjsHandler.instance.AlertString = "ur soulution in 2nd pipe must be only starch and IKI";
                            }

                        }



                    }
                    return true;//done pumping correctly 
                }
                else
                {
                    return false;//ur dropper is empty
                }
            }
            else
            {
                ////print(thisTransform.name + ": your trying to fill it above the limit: new+mine=" + total_fill_amount + " /UR NEW amouDrpHave: " + amouDrpHave);
                return false;//your trying to fill it above the limit
            }





        }

        public override void MakeAnotherSeperatedAction()
        {
            //throw new System.NotImplementedException();
        }

        public GameObject GetBlend()
        {

            return Blend;

        }





        private void OnTriggerEnter(Collider other)
        {   //must a rigid body be on one of the objects 


            if (other.gameObject.layer != LayerMask.NameToLayer("Flammable"))
                return;

            if (other.gameObject.tag.Equals("WarmWater"))
            {
                if (inAction)
                {
                    ////print(thisTransform.name + " : in warm water");

                    InWarmWaterActive = true;
                    newLerpingTransform = other.transform;




                    //so any arrangment is acceptable
                    if (remaningCylnds == 1)
                    {
                        if (TasksManager.MissionID == 6)
                            TasksManager.MissionID++;

                        remaningCylnds--;//becoming 0 will help if in mission 7 we get one out, it will be consider 1 not 2 cus if we get to miss7 with 1 while no remaning in reality makes glitches
                    }
                    else if (remaningCylnds == 2)
                    {
                        remaningCylnds--;
                    }



                    /*                    if (TasksManager.MissionID == 6)
                                        {//so any arrangment is acceptable
                                            if (remaningCylnds == 1)
                                            {
                                                TasksManager.MissionID++;
                                                remaningCylnds--;//becoming 0 will help if in mission 7 we get one out, it will be consider 1 not 2 cus if we get in 7 with 1 while no remaning in reality makes glitches
                                            }
                                            else if (remaningCylnds == 2)
                                            {
                                                remaningCylnds--;
                                            }
                }
                    */
                }


            }

        }

        private void OnTriggerExit(Collider other)
        {

            if (other.gameObject.layer != LayerMask.NameToLayer("Flammable"))
                return;

            if (other.gameObject.tag.Equals("WarmWater"))
            {

                ////print(thisTransform.name + " :out from warm water");

                InWarmWaterActive = false;
                newLerpingTransform = null;
                StartLerpToNewTrans = false;//reset for next time

                if (inAction)
                {
                    if (remaningCylnds < 2)//for mission 6
                        remaningCylnds++;

                    if (TasksManager.MissionID == 7)
                    {
                        if (GeneralMethods.sing.TimerEnabled)
                        {
                            GeneralMethods.sing.TimerEnabled = false;
                        }
                        UIObjsHandler.instance.AlertString = "Wrong , you must waif for whole 2 minutes";
                        TasksManager.MissionID--;//allowed in the proberety

                    }
                }

            }

        }






        private void LateUpdate() //late update : last of the frame
        {

        }


        public void ResetCylindersSitu()
        {
            InWarmWaterActive = false;
            newLerpingTransform = null;
            StartLerpToNewTrans = false;
            remaningCylnds = 2;

            thisCylndr_skMshRenderer.SetBlendShapeWeight(9, 0);
            Blend.SetActive(false); //empty at first

        }

        private void Start()
        {
            base.Start();
            GeneralMethods.sing.ResetNowObserver += ResetCylindersSitu;
            Blend.SetActive(false); //empty at first
            thisPipeBlendMaterial = thisCylndr_skMshRenderer.material;
            remaningCylnds = 2;


            if (Cylind_id == 1)
            {//pipe 2
                GeneralMethods.sing.Pipe2FutureStarchSkRndrer = thisCylndr_skMshRenderer;
            }


        }
        private void OnDisable()
        {

            GeneralMethods.sing.ResetNowObserver -= ResetCylindersSitu;


        }


    }
}