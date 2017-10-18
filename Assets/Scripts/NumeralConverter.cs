using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class NumeralConverter
{
	private static Dictionary<char, int> _numDict;

	static NumeralConverter()
	{
		_numDict = new Dictionary<char, int>();
		_numDict.Add('I', 1);
		_numDict.Add('V', 5);
		_numDict.Add('X', 10);
		_numDict.Add('L', 50);
		_numDict.Add('C', 100);
		_numDict.Add('D', 500);
		_numDict.Add('M', 1000);
	}

	public static string ConvertDecimalToRoman(string val)
	{

		decimal resultNum = 0;

		if (decimal.TryParse(val, out resultNum))
		{


			int num = (int)Convert.ToDecimal(resultNum);
			List<char> chars = new List<char>();
			List<KeyValuePair<char, int>> reverseDict = _numDict.Reverse().ToList();
			for (int i = 0; i < reverseDict.Count(); i++)
			{
				if (num > 0)
				{
					int result = num / reverseDict[i].Value;
					char[] currNum = num.ToString().ToCharArray();
					if (currNum.First() == '9' && result != 0)
					{
						if (i + 1 <= reverseDict.Count() && i - 1 != -1)
						{
							char upper = reverseDict[i - 1].Key;
							char lower = reverseDict[i + 1].Key;
							chars.AddRange(new List<char> { lower, upper });
							num -= reverseDict[i - 1].Value - reverseDict[i + 1].Value;
						}
					}
					else if (currNum.First() == '4' && result != 0)
					{
						if (i + 1 <= reverseDict.Count() && i - 1 != -1)
						{
							chars.AddRange(new List<char> { reverseDict[i].Key, reverseDict[i - 1].Key });
							num -= reverseDict[i - 1].Value - reverseDict[i].Value;
						}

					}
					else
					{
						for (int j = 0; j < result; j++)
						{
							chars.Add(reverseDict[i].Key);
						}
						num -= reverseDict[i].Value * result;
					}


				}
				else
				{
					break;
				}
			}
			StringBuilder sb = new StringBuilder();
			foreach (char numeral in chars)
			{
				sb.Append(numeral);

			}
			return sb.ToString();
		}
		else
		{
			return "";
		}
	}

	public static string ConvertRomanToDecimal(string val)
	{
		char[] charArr = val.ToUpper().ToCharArray();
		if (ValidateArray(charArr))
		{


			List<int> nums = charArr.Select(i => _numDict[i]).ToList();
			if (nums.Count > 1)
			{
				int total = nums.Sum();
				for (int i = 0; i < nums.Count; i++)
				{
					if (!(i + 1 == nums.Count))
					{
						if (nums[i] < nums[i + 1])
						{
							total -= nums[i];
							total -= nums[i];
						}
					}
				}

				return total.ToString();
			}
			else
			{
				return nums[0].ToString();
			}
		}
		else
		{
			return "0";
		}
	}

	private static bool ValidateArray(char[] charArr)
	{

		foreach (char numeral in charArr)
		{
			if (!_numDict.Keys.Any(x => x == numeral))
			{
				return false;
			}
		}
		return true;
	}
}
