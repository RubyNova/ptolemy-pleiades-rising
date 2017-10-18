using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class DoorControl : MonoBehaviour {

	public bool IsOpen = false;
	public string _randNumeral;
	public int _randInteger;
	private List<NumeralFragmentController> _fragments;
	public bool _isSolved = false;
	// Use this for initialization
	void Start () {
		_fragments = FindObjectsOfType<NumeralFragmentController>().ToList();
		char[] result;
		_fragments.Reverse();
		while (true)
		{
			System.Random rand = new System.Random();
			_randInteger = rand.Next(1000, 3999);
			_randNumeral = NumeralConverter.ConvertDecimalToRoman(_randInteger.ToString());
			result = _randNumeral.ToCharArray();
			if ((result.Length % _fragments.Count) == 0 && result.Length != 0)
			{
				break;
			}
			else if (result.Length == 0)
			{
				return;
			}
		}
		int indexSkipper = 0;
		foreach (NumeralFragmentController controller in _fragments)
		{
			int portionResult = result.Length / _fragments.Count;
			for (int i = 0 + indexSkipper; i < portionResult + indexSkipper; i++)
			{
				controller._numerals.Add(result[i]);
			}
			indexSkipper += portionResult;
		}

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void Open()
	{
		gameObject.GetComponent<Renderer>().enabled = false;
		Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
		foreach (Collider2D coll in colliders)
		{
			if (!coll.isTrigger)
			{
				coll.enabled = false;
				break;
			}
		}
		IsOpen = true;

	}

	public void Close()
	{
		gameObject.GetComponent<Renderer>().enabled = true;
		Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
		foreach (Collider2D coll in colliders)
		{
			if (!coll.isTrigger)
			{
				coll.enabled = true;
				break;
			}
		}
		IsOpen = false;
	}
}
