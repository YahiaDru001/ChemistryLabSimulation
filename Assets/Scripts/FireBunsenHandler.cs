using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YahiaDroubiTask
{
    public class FireBunsenHandler : MonoBehaviour
    {
        public static FireBunsenHandler singelton;//one firebunsen in the scene so no problems
        public GameObject TheFlame;
        public enum FireState
        {   //could use scriptable objects instead of enum but not nessecarry here

            GasClosed,
            GasOpened,
            InFire
        }
        public FireState myfireState;

        //caching components
        public Transform GasSwitchTransfrom;

        public float GasSwitchOpenPosZ;
        float GasSwitchClosePosZ;

        Vector3 GasSwitchClosePos, GasSwitchOpenPos;
        Vector3 initialVectorVal = new Vector3(9999, 9999, 9999);


        private void Awake()
        {
            if(!GasSwitchTransfrom)//attched by inspector 
                GasSwitchTransfrom = transform.GetChild(0).GetChild(0); 


            GasSwitchClosePos = initialVectorVal;
            GasSwitchOpenPos = initialVectorVal;

            singelton = this;

        }


        private void OnMouseDown()
        {
            if (GasSwitchTransfrom != null)
            {
                SwitchTheGasState();


            }
            else
            {
                GasSwitchTransfrom = transform.GetChild(0).GetChild(0); // to be sure its linked

                SetZofSwitchStates();//update will be late so its here in this situation
                SwitchTheGasState();

            }

        }

        public void SwitchTheGasState()
        {
            if (myfireState.Equals(FireState.GasClosed))
            {
                myfireState = FireState.GasOpened;
                GasSwitchTransfrom.localPosition = GasSwitchOpenPos;
                StartCoroutine("AlertAndAutoOffifLateFire");

            }
            else
            { //if in fire or open

                CloseGasSwitch();

                StopCoroutine("AlertAndAutoOffifLateFire");//to prevent contradiction
            }
        }

        void CloseGasSwitch()
        {
            myfireState = FireState.GasClosed;
            GasSwitchTransfrom.localPosition = GasSwitchClosePos;
            TurnTheFlameOff();

            if (TasksManager.MissionID == 7)
            {
                if (GeneralMethods.sing.TimerEnabled)
                {
                    GeneralMethods.sing.TimerEnabled = false;
                }
                UIObjsHandler.instance.AlertString = "Wrong , you must waif for whole 2 minutes";
                //TasksManager.MissionID--;//allowed in the proberety

            }




        }



        IEnumerator AlertAndAutoOffifLateFire()
        {


            yield return new WaitForSeconds(10);
            if (myfireState.Equals(FireState.GasOpened))
            {//its ok if fired
                UIObjsHandler.instance.AlertString = "Gas Leak , Auto Gas Off , be careful next time";
                CloseGasSwitch();
            }


        }

        // Update is called once per frame
        void Update()
        {

            SetZofSwitchStates();//calculations happens once not everyframe, check code 



        }

        public void SetZofSwitchStates()
        {
            if (GasSwitchTransfrom)
            {
                if (GasSwitchOpenPos.Equals(initialVectorVal))
                {
                    GasSwitchOpenPos = GasSwitchTransfrom.localPosition;
                    GasSwitchOpenPos.z = GasSwitchOpenPosZ;
                }

                if (GasSwitchClosePos.Equals(initialVectorVal))
                {
                    GasSwitchClosePos = GasSwitchTransfrom.localPosition;
                    GasSwitchClosePos.z = GasSwitchClosePosZ;

                }

            }
        }

        public void FireTheFlame()
        {
            if (TasksManager.MissionID <= 1)
            {
                UIObjsHandler.instance.AlertString = " Check Safety Rules first ";
                return;
            }


            if (myfireState == FireState.GasClosed)
            {
                ////print("Open Gas First");
                UIObjsHandler.instance.AlertString = "Wrong , Open Gas Switch first";

                return;
            }

            if (!TheFlame)
                return; //guard clause

            if (myfireState != FireState.InFire)
            {

                TheFlame.SetActive(true);
                myfireState = FireState.InFire;
                
                if(TasksManager.MissionID == 2)
                    TasksManager.MissionID++;
            }
            else
            {
                //print("Flame is running already");

            }

        }

        void TurnTheFlameOff()
        {
            if (!TheFlame)
                return; //guard clause

            TheFlame.SetActive(false);


        }








        public void ResetFireSitu()
        {
            CloseGasSwitch();
            StopAllCoroutines();
        }

        public void OnEnable()
        {
            GeneralMethods.sing.ResetNowObserver += ResetFireSitu;
        }

        private void OnDisable()
        {

            GeneralMethods.sing.ResetNowObserver -= ResetFireSitu;


        }
    }
}