using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using UnityEngine.UI;

public class NumeralFragmentController : MonoBehaviour {
	public List<char> _numerals;
	public Canvas _HUDCanvas;
	private PauseMenuController _PauseController;
	public int _passcode;
	public bool _makePasscode;
	public GameObject _previousFragment;
	// Use this for initialization
	void Start () {
		_PauseController = _HUDCanvas.GetComponent<PauseMenuController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_makePasscode && _passcode == 0)
		{
			MakePasscode();
		}
	
	}

	void OnCollisionEnter2D(Collision2D coll)
	{

		if (coll.gameObject.tag == "Player" && !_makePasscode)
		{
			TopDownControls playerController = coll.gameObject.GetComponent<TopDownControls>();
			playerController._numeralData.AddRange(_numerals);
			StringBuilder sb = new StringBuilder();
			foreach (char numeral in _numerals)
			{
				sb.Append(numeral);
			}
            _PauseController._txtRomanNumeralData.GetComponent<Text>().text += sb.ToString();
			_PauseController.GenericPause("You found some Numerals in a memory fragment. Maybe this is a clue to get to the core? \r\n" + sb.ToString());
			Destroy(gameObject);

		}
		else if (coll.gameObject.tag == "Player" && _makePasscode)
		{
			_PauseController.ShowPasscodeScreen(this);
		}

	}

	void MakePasscode()
	{
		List<char> numerals = _previousFragment.GetComponent<NumeralFragmentController>()._numerals;
		StringBuilder sb = new StringBuilder();
		foreach (char numeral in numerals)
		{
			sb.Append(numeral);
		}

		string result = sb.ToString();
		string decimalResult = NumeralConverter.ConvertRomanToDecimal(result);
		_passcode = Convert.ToInt32(decimalResult);
	}
}
