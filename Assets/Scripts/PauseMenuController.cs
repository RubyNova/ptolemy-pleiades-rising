using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Audio;

public class PauseMenuController : MonoBehaviour {
	private bool _isPaused = false;
	private GameObject _pauseMenu;
	private GameObject _genericPauseMenu;
	private GameObject _passcodeMenu;
	private DoorControl _doorInstance;
	private NumeralFragmentController _fragmentInstance;
	private GameObject _gameOverMenu;
    private bool _loadTopDown;
    private HUDTracker _HUDTracker;
    private GameObject _TxtOnOff;
    public GameObject _txtRomanNumeralData;


    // Use this for initialization
    void Start () {
		_pauseMenu = transform.Find("PauseMenu").gameObject;
		_genericPauseMenu = transform.Find("GenericPauseMenu").gameObject;
		_passcodeMenu = transform.Find("PasscodeMenu").gameObject;
		_gameOverMenu = transform.Find("GameOverMenu").gameObject;
        _HUDTracker = GetComponent<HUDTracker>();
        _TxtOnOff = transform.Find("OptionsMenu").Find("TxtOnOff").gameObject;
        _txtRomanNumeralData = transform.Find("RomanNumeralData").gameObject;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
		{
			Pause();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && _isPaused)
		{
			Unpause();
		}
	}

    public void MuteUnmute()
    {
        if (AudioListener.pause)
        {
            AudioListener.pause = false;
            _TxtOnOff.GetComponent<Text>().text = "ON";
        }
        else
        {
            AudioListener.pause = true;
            _TxtOnOff.GetComponent<Text>().text = "OFF";
        }

    }    

    public void LevelSelectSwitch (bool currentMenuIsLevelSelect)
    {
        GameObject btnNewGame = transform.Find("MainMenu").Find("BtnNewGame").gameObject;
        GameObject btnLevelSelect = transform.Find("MainMenu").Find("BtnLevelSelect").gameObject;
        GameObject btnOptions = transform.Find("MainMenu").Find("BtnOptions").gameObject;
        GameObject subMenu = transform.Find("MainMenu").Find("SubMenu").gameObject;

        if (!currentMenuIsLevelSelect)
        {
            btnLevelSelect.SetActive(false);
            btnNewGame.SetActive(false);
            btnOptions.SetActive(false);
            subMenu.SetActive(true);
        }
        else
        {
            btnLevelSelect.SetActive(true);
            btnNewGame.SetActive(true);
            btnOptions.SetActive(true);
            subMenu.SetActive(false);
        }
    }

    public void SwitchMenu (bool currentMenuIsOptions)
    {
        if (currentMenuIsOptions)
        {
            transform.Find("OptionsMenu").gameObject.SetActive(false);
            if (_HUDTracker._MainMenu)
            {
                transform.Find("MainMenu").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("PauseMenu").gameObject.SetActive(true);
            }

        }
        else
        {
            if (_HUDTracker._MainMenu)
            {
                transform.Find("MainMenu").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("PauseMenu").gameObject.SetActive(false);
            }

            transform.Find("OptionsMenu").gameObject.SetActive(true);
        }

    }

	public void GameOver()
	{
		Time.timeScale = 0f;
		_gameOverMenu.SetActive(true);
	}

	public void SetScene(string sceneName)
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(sceneName);
	}

	public void Pause()
	{
		_isPaused = true;
		_pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}

	public void Unpause()
	{
		_isPaused = false;
		_pauseMenu.SetActive(false);
		Time.timeScale = 1f;
	}

	public void GenericUnpause()
	{
		_isPaused = false;
		_genericPauseMenu.SetActive(false);
		Time.timeScale = 1f;
        if (_loadTopDown)
        {
            _loadTopDown = false;
            _HUDTracker._endReached = true;
            //ChangeScene("TopDown");
        }
	}

	public void GenericPause(string messageToDisplay, bool loadTopDown = false)
	{
        _loadTopDown = loadTopDown;
		_isPaused = true;
		Time.timeScale = 0f;
		_genericPauseMenu.SetActive(true);
		_genericPauseMenu.transform.Find("GenericText").gameObject.GetComponent<Text>().text = messageToDisplay;
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void ChangeScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}

    public void ResetScene()
    {
        ChangeScene(SceneManager.GetActiveScene().name);
    }

	public void ShowPasscodeScreen(DoorControl doorControl)
	{
		_doorInstance = doorControl;
		_isPaused = true;
		_passcodeMenu.SetActive(true);
	}


	public void ShowPasscodeScreen(NumeralFragmentController fragmentControl)
	{
		_fragmentInstance = fragmentControl;
		_isPaused = true;
		_passcodeMenu.SetActive(true);
	}

	public void ValidateAnswer()
	{
		string answer = _passcodeMenu.transform.Find("TxtPasscode").GetComponent<InputField>().text;

        int intAnswer = 0;
		bool isValid = int.TryParse(answer, out intAnswer);
		if (_doorInstance != null)
		{

			if (intAnswer == _doorInstance._randInteger && isValid)
			{
				_doorInstance._isSolved = true;
				_isPaused = false;
				_passcodeMenu.SetActive(false);
				_doorInstance.Open();
				_doorInstance = null;
			}
			else
			{
				_passcodeMenu.SetActive(false);
				GenericPause("Wrong answer");
			}
		}
		else if (_fragmentInstance != null)
		{
			int result = Convert.ToInt32(NumeralConverter.ConvertRomanToDecimal(answer));
			if (result == _fragmentInstance._passcode)
			{
				_passcodeMenu.SetActive(false);
				StringBuilder sb = new StringBuilder();
				foreach (char numeral in _fragmentInstance._numerals)
				{
					sb.Append(numeral);
				}

				string numeralResult = sb.ToString();
                _txtRomanNumeralData.GetComponent<Text>().text += sb.ToString();
                Destroy(_fragmentInstance.gameObject);
				_fragmentInstance = null;
				GenericPause("You found some Numerals in a memory fragment. Maybe this is a clue to get to the core? \r\n" + numeralResult);

			}
			else
			{
				_passcodeMenu.SetActive(false);
				GenericPause("Wrong answer");
			}
		}
        _passcodeMenu.transform.Find("TxtPasscode").GetComponent<InputField>().text = "";
    }
}
