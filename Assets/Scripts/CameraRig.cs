using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YahiaDroubiTask
{
    public class CameraRig : MonoBehaviour
    {


        public Transform NewTarget_SafetBoard;
        public Quaternion InitialRotation;

        bool LookAtSafety = false;

        public void ToggleLookAtSafety()
        {
            LookAtSafety = !LookAtSafety;

            if (TasksManager.MissionID == 1)
                TasksManager.MissionID++;

        }


        // Start is called before the first frame update
        void Start()
        {
            InitialRotation = transform.rotation;

            GeneralMethods.sing.ResetNowObserver += ResetSafetyLookSitu;

        }

        // Update is called once per frame
        void Update()
        {
            if (LookAtSafety)
            {
                if (NewTarget_SafetBoard)
                    RotateToTargetPos(NewTarget_SafetBoard.position);
            }
            else LookAtInitial();
        }

        private void LookAtInitial()
        {
            if (transform.rotation != InitialRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, InitialRotation, speed * Time.deltaTime);

            }
        }

        float speed = 5f;
        public void RotateToTargetPos(Vector3 target)
        {

            Vector3 dir = target - transform.position;
            dir.y = 0; // horizontal always
            Quaternion rot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);





        }







        public void ResetSafetyLookSitu()
        {
            LookAtSafety = false;
        }



        private void OnDisable()
        {

            GeneralMethods.sing.ResetNowObserver -= ResetSafetyLookSitu;


        }
    }
}