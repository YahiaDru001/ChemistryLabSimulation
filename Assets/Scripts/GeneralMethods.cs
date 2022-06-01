using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YahiaDroubiTask
{
    public class GeneralMethods : MonoBehaviour
    {

        public AudioClip[] instructions_EngSounds;
        public AudioSource instructuionsAudioSource;

        public Text instructionText;

        public static GeneralMethods sing;//singelton pattern


        public SkinnedMeshRenderer Pipe2FutureStarchSkRndrer;


        static int _wrongCounts = 0;
        public static int WrongCounts
        {
            get { return _wrongCounts; }
            set
            {
                _wrongCounts = value;
                UIObjsHandler.instance.WrongCounterTxt.text = "Wrongs: " + value;
            }
        }

        public void IncreaseWrongCounts()
        {
            WrongCounts++;
        }

        public void PlayAlertSound()
        {
            if (AlertClip)
                PlaySound(AlertClip);
        }


        //observer pattern
        public delegate void MyObserverDelegete();
        public MyObserverDelegete myMissionEventObserver;
        public MyObserverDelegete myAlertsEventObserver;
        public MyObserverDelegete ResetNowObserver;

        public bool TimerEnabled = false;
        private float startWatch;
        bool TimeEnds = false;
        public void EnableTimer()
        {
            if (toWaitTime != 12)
            {
                if (ParameterFixingGlitchWaitTime == 0)
                {
                    UIObjsHandler.instance.AlertString = "Wrong , Set Timer time first";
                }
                else
                {
                    UIObjsHandler.instance.AlertString = "Wrong timer set , correct it with 120";
                }

                return;
            }

            if (TasksManager.MissionID < 7)
            {
                UIObjsHandler.instance.AlertString = "Finish previous nessecessary tasks first";
                return;
            }

            if (TasksManager.MissionID == 7)
            {

                if (FireBunsenHandler.singelton.myfireState != FireBunsenHandler.FireState.InFire)
                {
                    UIObjsHandler.instance.AlertString = "Kidding? Turn The Fire On First";
                    return;
                }
            }

            TimeEnds = false;
            TimerEnabled = true;
            startWatch = Time.time;
        }



        float ParameterFixingGlitchWaitTime = 0;//if using toWaitTime directly ,, (out) will make a glitch where any num is accepted while Timer already running
        public void OnEndEdit(string time)
        {
            if (float.TryParse(time, out ParameterFixingGlitchWaitTime))
            {
                if (ParameterFixingGlitchWaitTime != 120)
                {
                    UIObjsHandler.instance.AlertString = "Wrong , 2 minutes required";

                }
                else
                {
                    toWaitTime = ParameterFixingGlitchWaitTime / 10; //faking the time for not let user wait so long

                }

            }
            else
            {
                UIObjsHandler.instance.AlertString = "Wrong , Enter valid seconds";

            }

        }



        float spentTime;
        float toWaitTime = 0;
        private void Update()
        {
            if (TimerEnabled)
            {
                spentTime = Time.time - startWatch;
                if (spentTime >= toWaitTime)
                {
                    TimeEnds = true;
                    PlaySound(end_timerClip);
                    TimerEnabled = false;//must 

                    if (TasksManager.MissionID == 7)
                    {
                        //change nasha color to Purble now
                        if (Pipe2FutureStarchSkRndrer)
                            Pipe2FutureStarchSkRndrer.material.color = new Color32(143, 0, 254, 255);

                        TasksManager.MissionID++;
                    }
                }
                else
                {
                    UIObjsHandler.instance.TimerTxt.text = (spentTime * 10f).ToString();//faking 12 seconds to be 120
                }

            }
        }


        public AudioSource audioSource;
        public AudioClip pump_sound, absorbsound, end_timerClip, mission_passed, AlertClip;

        public void PlaySound(AudioClip clip)
        {
            if (!audioSource)
                return;


            audioSource.clip = clip;
            audioSource.Play();
        }
        public void OnValueChanged(float val)
        {


            //one UIHandler is all across the scene accessable by singelton instance
            UIObjsHandler.instance.amountofSolInputSTR = val.ToString(); // this is a proberty that affects mutlible elemnts

        }


        public bool ArabicChosen = false;
        public void OnValueChanged(Int32 chosenlang)
        {

            if (chosenlang == 1)
            {
                ArabicChosen = true;
                SetArabicInstructions();
                instructuionsAudioSource.clip = null;

            }
            else
            {
                ArabicChosen = false;
                SetEnglishInstructions();
                instructuionsAudioSource.clip = instructions_EngSounds[0];//read english instruction


            }

        }

        public string ArabicFx(string x)
        {

            return ArabicSupport.ArabicFixer.Fix(x, false, false);
        }


        void SetArabicInstructions()
        {
            if (instructionText)
            {
                instructionText.alignment = TextAnchor.UpperRight;
                instructionText.lineSpacing = -1;
            }


            TheInstructions[0] = " ";
            TheInstructions[1] = "  سنتعرف في هذه التجربة على طريقة للكشف عن الكربوهيدرات عديدة التسكر ,,  : الادوات : بعض الماء , محلول نشا , محلول لوغول  _____ المهمة الاولى : تعاليم السلامة , اضغط على الزر في الاعلى لرؤيتها";
            TheInstructions[2] = "أولاً أشعل موقد الغاز لاعداد حمام مائي اثناء تحضير انابيب التجربة";
            TheInstructions[3] = "يتوفر لديك 3 سحاحات , استخدم احداهن واضف 3 مل من الماء في الانبوب الاول. لا تنسى ضبط رقم القياس المطلوب في الخانة المناسبة ";
            TheInstructions[4] = "نضع 3 مل من محلول النشا في الانبوب الثاني (( نفس الطريقة في القسم الاول)) ";
            TheInstructions[5] = " نضيف محلول لوغول بكمية مناسبة";
            TheInstructions[6] = "نقوم بوضع الانبوبين في حمام ساخن لمدة دقيقتين ";
            TheInstructions[7] = " انقر على زر الانتظار بعد انهاء الخطوات السابقة - ضبط المؤقت : دقيقتين";
            TheInstructions[8] = " نقوم باخراج الانابيب وملاحظة النتيجة :  ";
            TheInstructions[9] = " تحول محلول النشا الى اللون البنفسجي ";
        }

        void SetEnglishInstructions()
        {
            if (instructionText)
            {

                instructionText.alignment = TextAnchor.MiddleLeft;
                instructionText.lineSpacing = 1;
            }
            TheInstructions[0] = " ";
            TheInstructions[1] = "In this experiment, we'll learn how to detect carbohydrates that are too intoxicating.\n    Tools: some water, starch solution, logul solution \n        Mission: Safety teachings, click the button at the top";
            TheInstructions[2] = "Light the gas stove to prepare a water bath while preparing the experiment pipes";
            TheInstructions[3] = "You have 3 burettes available, use one of them and add 3 ml of water to the first tube. \n Do not forget to set the required measurement number in the appropriate field ";
            TheInstructions[4] = "We put 3 ml of starch solution in the second tube (same way in the first section)";
            TheInstructions[5] = " Add logul solution in a suitable amount ";
            TheInstructions[6] = "We put the tubes in a hot bath for two minutes. ";
            TheInstructions[7] = " Click the wait button after you finish the previous steps - timer: 2 minutes";
            TheInstructions[8] = "We take out the tubes and note the result: ";
            TheInstructions[9] = " Starch solution turned violet , Why? ";
        }


        private void Awake()
        {
            sing = this;

            audioSource = GetComponent<AudioSource>();


            //instruction must be on awake because TotalInstructionSteps changed on start based on its length
            TheInstructions = new string[10];
            if (ArabicChosen)
            {
                SetArabicInstructions();
            }
            else
            {
                SetEnglishInstructions();


            }
        }

        void Start()
        {


            ResetNowObserver += ResetGeneral;



            //missionPassed observer
            myMissionEventObserver += MissionChangedSoundEffect;
            myMissionEventObserver += GoToMissionInstruction;

            //alerts opserver
            myAlertsEventObserver += PlayAlertSound;
            myAlertsEventObserver += IncreaseWrongCounts;



        }

        public string[] TheInstructions;

        public void NextInstruction()
        {
            UIObjsHandler.instance.StepTxtDeal++;
            if (ArabicChosen)
                UIObjsHandler.instance.TxtOfInstr = ArabicFx(TheInstructions[UIObjsHandler.instance.StepTxtDeal]);
            else
                UIObjsHandler.instance.TxtOfInstr = TheInstructions[UIObjsHandler.instance.StepTxtDeal];

        }
        public void GoToMissionInstruction()
        {

            UIObjsHandler.instance.StepTxtDeal = TasksManager.MissionID;
            if (ArabicChosen)
                UIObjsHandler.instance.TxtOfInstr = ArabicFx(TheInstructions[TasksManager.MissionID]);
            else
            {
                UIObjsHandler.instance.TxtOfInstr = TheInstructions[TasksManager.MissionID];

                instructuionsAudioSource.clip = instructions_EngSounds[TasksManager.MissionID-1];//read english instruction
                instructuionsAudioSource.Play();
            }
        }
        public void PrevInstruction()
        {

            UIObjsHandler.instance.StepTxtDeal--;
            if (ArabicChosen)
                UIObjsHandler.instance.TxtOfInstr = ArabicFx(TheInstructions[UIObjsHandler.instance.StepTxtDeal]);
            else
                UIObjsHandler.instance.TxtOfInstr = TheInstructions[UIObjsHandler.instance.StepTxtDeal];

        }

        public void ToggleInstructionPanel()
        {
            UIObjsHandler.instance.InstructionPanel.SetActive(!UIObjsHandler.instance.InstructionPanel.activeInHierarchy);
            UIObjsHandler.instance.ChooseAmountPanel.SetActive(!UIObjsHandler.instance.ChooseAmountPanel.activeInHierarchy);

        }

        public void ResetGeneral()
        {
            WrongCounts = 0;
            TimerEnabled = false;
            toWaitTime = 0;
            UIObjsHandler.instance.TimerTxt.text = "0.0";

            //print("general reset");

        }


        private void OnDisable()
        {
            myMissionEventObserver -= MissionChangedSoundEffect;
            myMissionEventObserver -= GoToMissionInstruction;



            myAlertsEventObserver -= PlayAlertSound;
            myAlertsEventObserver -= IncreaseWrongCounts;


            ResetNowObserver -= ResetGeneral;



        }

        private void MissionChangedSoundEffect()
        {
            PlaySound(mission_passed);

        }


    }
}