using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YahiaDroubiTask
{

    public abstract class Movables : MonoBehaviour
    {
        protected bool inAction = false;

        [SerializeField] GameObject ActionPrefab;
        public Transform ActionPosTransf;
        float mZCoordinate;
        Vector3 mOffset;
        Camera _mainCamera;
        protected Transform thisTransform;

        public Vector3 InitialPosOfGO;

        private void OnMouseDown()
        {
            inAction = true;

            mZCoordinate = _mainCamera.WorldToScreenPoint(thisTransform.position).z;

            mOffset = thisTransform.position - MouseWorldPos();


            TurnActionOnBeforeMoving();


            MakeAnotherSeperatedAction();


        }

        bool StartLerpingToInitial = false;

        protected virtual void OnMouseUp()
        {
            inAction = false;

            if (actionObj_Instnt && actionObj_Instnt.activeInHierarchy)
                actionObj_Instnt.SetActive(false); //for (object pooling) , 

            if (!newLerpingTransform && !InWarmWaterActive)
                StartLerpingToInitial = true;
            else
                StartLerpToNewTrans = true;

        }

        public int Cylind_id =1 ;
        public bool InWarmWaterActive = false;
        public bool StartLerpToNewTrans = false;
        public Transform newLerpingTransform;
        void Update()
        {

            if (!newLerpingTransform && !InWarmWaterActive)
            {

                if (StartLerpingToInitial)
                {
                    if (thisTransform.position != InitialPosOfGO)
                    {
                        //(Time.deltaTime) to take diff of performance on different hardware in calculations
                        thisTransform.position = Vector3.Lerp(thisTransform.position, InitialPosOfGO, 2f * Time.deltaTime);
                        //lerp to not change pos imediately & must be in (multiple-called pattern)
                    }
                    else
                    {
                        StartLerpingToInitial = false;//end and reset for next time
                    }
                }



            }
            else
            {
                if (StartLerpToNewTrans)
                {
                    if (InWarmWaterActive)
                    {

                        if (Vector3.Distance(thisTransform.position , (newLerpingTransform.position + (newLerpingTransform.up * 0.015f) + (newLerpingTransform.right * 0.02f * Cylind_id))) > 0)
                        {

                            thisTransform.position = Vector3.Lerp(thisTransform.position, (newLerpingTransform.position+ (newLerpingTransform.up*0.015f) + (newLerpingTransform.right * 0.02f * Cylind_id)), 2f * Time.deltaTime);
                        }
                        else
                        {

                            StartLerpToNewTrans = false;

                        }


                    }
                }



            }










        }




        GameObject actionObj_Instnt;
        public void TurnActionOnBeforeMoving()
        {
            if (!ActionPrefab) //Guard Clause
            {
                return;
            }


            if (!actionObj_Instnt)
            {                           //using clone here ,made pre-original de/activating GOs in another interactables to vary 
                actionObj_Instnt = GameObject.Instantiate(ActionPrefab, ActionPosTransf.position, ActionPosTransf.rotation);
                actionObj_Instnt.transform.SetParent(thisTransform);
            }
            else
            {
                if (actionObj_Instnt && !actionObj_Instnt.activeInHierarchy) //for (object pooling) , to not instaniate then destroy everytime
                {

                    actionObj_Instnt.SetActive(true);


                    //actionObj_Instnt.transform.position = ActionPosTransf.position; //just in case its related to more objects, and not always here 
                    //actionObj_Instnt.transform.rotation = ActionPosTransf.rotation;
                    //actionObj_Instnt.transform.SetParent(thisTransform);
                }
            }



        }


        public abstract void MakeAnotherSeperatedAction();

        private void OnMouseDrag()
        {
            thisTransform.position = MouseWorldPos() + mOffset;
        }

        public Vector3 MouseWorldPos()
        {

            //x & y of mouse
            Vector3 mousePoint = Input.mousePosition;

            //the mouse is in 2D panel ,  but sceneobejcts in 3D world
            //we must get the z from the gameobject Pos
            mousePoint.z = mZCoordinate;

            return _mainCamera.ScreenToWorldPoint(mousePoint);


        }

        protected virtual void Awake()
        {

            // (Caching) to not search everytime
            _mainCamera = Camera.main;
            thisTransform = this.transform;
            InitialPosOfGO = thisTransform.position;

        }

        protected void SetTransform(Transform t)
        {
            thisTransform = t;
        }






        public void ResetMovablesSitu()
        {
            ////print(thisTransform.name+"hello reset "); // 6 movable objects
            thisTransform.position = InitialPosOfGO;
        }

        public void Start()
        {
            GeneralMethods.sing.ResetNowObserver += ResetMovablesSitu;
        }

        private void OnDisable()
        {

            GeneralMethods.sing.ResetNowObserver -= ResetMovablesSitu;


        }




    }
}