using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YahiaDroubiTask
{
    public class UIObjsHandler : MonoBehaviour
    {
        public Text WrongCounterTxt;
        public Text AlertTxt;
        public GameObject AlertGO;
        public GameObject MissionPassGO;

        string _alertString;
        public String AlertString
        {
            get { return _alertString; }
            set
            {
                _alertString = value;
                AlertTxt.text = value;
                GeneralMethods.sing.myAlertsEventObserver?.Invoke();
            }
        }


        public static UIObjsHandler instance; // singelton

        public InputField AmountofSolInput;
        public Text TimerTxt;

        public Slider AmountofSolSlider;
        
        public String amountofSolInputSTR //Property 
        {
            get {

                if (AmountofSolSlider)
                {
                    return AmountofSolSlider.value.ToString();
                }
                else return AmountofSolInput.text.ToString();
            
            }
            set
            {
                int parsedVal;
                if (!Int32.TryParse(value,out parsedVal))
                {
                    AlertString = "Wrong Input , Enter Valid Number";
                    //Debug.Log("Enter Valid Number");
                    return;
                }


                if (parsedVal < 0)
                {
                    parsedVal = 0;
                    value = "0";
                }else if(parsedVal > 100)
                {
                    parsedVal = 100;
                    value = "100";
                }

                if(AmountofSolSlider)
                    AmountofSolSlider.value = parsedVal;
                if(AmountofSolInput)
                    AmountofSolInput.text = value;

                //send it to dropper (static) var
                DropperManager.ChoosenAmountOfSolute = parsedVal;

            }
        }


        [SerializeField] Text StepNumTxt;
        [SerializeField] int TotalSteps = 9;

        int _stepnow = 1;
        public int StepTxtDeal //Property 
        {
            get { return _stepnow; }
            set
            {
                if (value <= TotalSteps && value > 0) // self-guard
                {
                    _stepnow = value;
                    StepNumTxt.text = _stepnow.ToString() + " / " + TotalSteps;
                }else if(value > TotalSteps)
                {
                    _stepnow = 1;
                    StepNumTxt.text = _stepnow.ToString() + " / " + TotalSteps;

                }else if(value <= 0)
                {
                    _stepnow = 1;
                    StepNumTxt.text = _stepnow.ToString() + " / " + TotalSteps;

                }
            }
        }


        [SerializeField] Text TheInsText;
        public string TxtOfInstr   //Property 
        {
            get { return TheInsText.text; }
            set
            {
                if (TheInsText.text != value)
                {
                    //Debug.Log("YahiaDroubi: This log to TRACE from where the change come, by console");
                    TheInsText.text = value;
                }

            }
        }




        public GameObject InstructionPanel;
        public GameObject ChooseAmountPanel;



        void Awake()
        {

            // singelton design pattern 
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
                instance = this;
            }



        }

        public void Start()
        {
            //Adds a listener to the input field    
            AmountofSolInput.onValueChanged.AddListener((string x) => { InputAmountChanged(x); } );
                                        //and invokes a method when the value changes ( delegateAction using Lambda Expression).
                                        //ValueChangeCheck(); like a method subscribed to delegate but here direct 



            TotalSteps = GeneralMethods.sing.TheInstructions.Length-1;


            StepTxtDeal = 1;
            if (GeneralMethods.sing.ArabicChosen)
            {

                TxtOfInstr = ArabicSupport.ArabicFixer.Fix(GeneralMethods.sing.TheInstructions[StepTxtDeal]);
            }
            else
            {
                TxtOfInstr = GeneralMethods.sing.TheInstructions[StepTxtDeal];

            }


            //generaly this is being onenable method (but it could run before awake)
            GeneralMethods.sing.myAlertsEventObserver += ShowAlertPanel;
            GeneralMethods.sing.myMissionEventObserver += ShowMissionPassGO;
            GeneralMethods.sing.ResetNowObserver += ResetUI;

        }

        public void ResetUI()
        {

            StepTxtDeal = 1;
            if (GeneralMethods.sing.ArabicChosen)
            {
                TxtOfInstr = ArabicSupport.ArabicFixer.Fix(GeneralMethods.sing.TheInstructions[StepTxtDeal], false, false);
            }
            else
            {
                TxtOfInstr = GeneralMethods.sing.TheInstructions[StepTxtDeal];
            }

            if (AlertGO)
            {
                AlertGO.SetActive(false);
            }
            if (MissionPassGO)
            {
                MissionPassGO.SetActive(false);
            }


                StopAllCoroutines();

        }

        public void InputAmountChanged(string val)
        {
            amountofSolInputSTR = val;
        }




        // Update is called once per frame
        void Update()
        {

        }

        //Important note : On Enable of object stms runs before awake of other object (not as expected) so be carefull
/*        private void OnEnable()
        {
            GeneralMethods.sing.myAlertsEventObserver += ShowAlertPanel;
            
            
            GeneralMethods.sing.myMissionEventObserver += ShowMissionPassGO;


        }
*/
        private void OnDisable()
        {
            GeneralMethods.sing.myAlertsEventObserver -= ShowAlertPanel;

            GeneralMethods.sing.myMissionEventObserver -= ShowMissionPassGO;

            GeneralMethods.sing.ResetNowObserver -= ResetUI;


        }





        public void ShowAlertPanel()
        {
            if (AlertGO)
            {
                AlertGO.SetActive(true);
                newTimeCount++;
                StartCoroutine(DisableGameObjAfterSeconds(AlertGO, 5f,newTimeCount));
            }
        }
        int newTimeCount=0;
        public IEnumerator DisableGameObjAfterSeconds(GameObject GO,float time,int prevTimeCount = 9999999)
        {

            yield return new WaitForSeconds(time);
            if (GO && GO.activeInHierarchy)
            {
                if (prevTimeCount != 9999999)//means caller uses contr-preventer
                {
                    if (prevTimeCount == newTimeCount) //to prevent contradictions between future-now coroutines
                        GO.SetActive(false);
                }else
                {
                    GO.SetActive(false);
                }
            }
        }


        public void ShowMissionPassGO()
        {
            if (MissionPassGO)
            {
                MissionPassGO.SetActive(true);
                StartCoroutine(DisableGameObjAfterSeconds(MissionPassGO, 5f));
            }
        }



    }
}