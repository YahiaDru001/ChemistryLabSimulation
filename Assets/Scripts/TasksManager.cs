using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YahiaDroubiTask
{
    public class TasksManager : MonoBehaviour
    {
        //since they are just simple tasks not actual missions, no need to create an OOP full system 

        //###2 : if we want to save anything we could use simple (PlayerPrefs) or any db system 
       /* public void SetInt(string KeyName, int Value)
        {
            PlayerPrefs.SetInt(KeyName, Value);
        }

        public int Getint(string KeyName)
        {
            return PlayerPrefs.GetInt(KeyName);
        }*/


        static int _missionID =1 ;
        public static int MissionID
        {

            get
            {
                return _missionID;
            }
            set
            {
                if (_missionID != value)
                {//new Change
                    if (_missionID >= 0)
                    {
                        if (value == 1)
                        {
                            _missionID = value;//someone restarted the missions
                        }

                        

                        if (value == _missionID + 1) //(stability)
                        { 
                            //so if byaccedint in realtime setting (mission=6) if sth happened , and that happens while in mission 2 , then it will be prevented ---(Security and Stability)
                            _missionID = value;
                            GeneralMethods.sing.myMissionEventObserver?.Invoke();//if there any subscribed methods wanted to be called
                                                                                 //


                            if (value == 6 && CylindersManager.remaningCylnds == 0)
                            {
                                //means player put cylinders On Fire already so accept that 
                                MissionID++;//to re enter here again but with mission 6 passed
                                //print("Bonus: Passed mission 6 without even reading the instruction, good job");
                            }

                        }
                        else if(value == _missionID - 1)
                        {
                            _missionID = value; //if mission failed and needed to be reapeated

                        }
                        else
                        {
                            //print("am not allwing jumping between missions , but allowing Restart");
                        }
                    }
                }

            }
        }

        public void RestartButton()
        {

            MissionID = 1;

            //reset
            GeneralMethods.sing.ResetNowObserver?.Invoke();


        }







        // Start is called before the first frame update
        void Start()
        {
            ////print("MissionID: " + MissionID);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}