using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ArabicSupport;

public class InputArabicFix : MonoBehaviour
{
	// Use this for initialization
	InputField textfield;
	Text fakeDisplay;

	public static bool arabic = false;
	void Start()
	{
		textfield = GetComponent<InputField>();
		textfield.textComponent.color = Color.clear;
		fakeDisplay = (Text)GameObject.Instantiate(textfield.textComponent, textfield.textComponent.transform.localPosition, textfield.textComponent.transform.localRotation);
		fakeDisplay.transform.SetParent(textfield.transform, false);
		fakeDisplay.color = Color.black;
		textfield.onValueChanged.AddListener(delegate { FixNewText(); });
	}

	public string text
	{
		get
		{
			return textfield.text;
		}
	}
	// Update is called once per frame
	void Update()
	{
	}

	void FixNewText()
	{
		if (arabic)
		{
			fakeDisplay.text = ArabicFixer.Fix(textfield.text);
		}
		else
		{
			fakeDisplay.text = textfield.text;
		}
	}
}
