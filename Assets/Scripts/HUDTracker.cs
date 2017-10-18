using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HUDTracker : MonoBehaviour {
	public GameObject _player;
	public string  _pickupTag;
	public string _enemyTag;
	private List<GameObject> _pickups;
	private int _maxAmountOfPickups;
	private Text _healthText;
	private Text _enemiesLeftText;
	private Text _pickupsCollectedText;
	private PlatformControls _playerController;
	private TopDownControls _topDownController;
	private List<GameObject> _enemies;
	private Text _gameOverText;
	private GameObject _btnRetry;
	private GameObject _btnContinue;
    public bool _endReached;
    public bool _MainMenu;
    private GameObject _txtTimer;
    private float _countdown = 300f;
    public bool _shouldTimeDemise;

    // Use this for initialization
    void Start () {
		_pickups = GameObject.FindGameObjectsWithTag(_pickupTag).ToList();
		_healthText = transform.Find("HealthText").gameObject.GetComponent<Text>();
		_enemiesLeftText = transform.Find("EnemiesLeftText").gameObject.GetComponent<Text>();
		_pickupsCollectedText = transform.Find("PickupsCollectedText").gameObject.GetComponent<Text>();
		_gameOverText = transform.Find("GameOverText").gameObject.GetComponent<Text>();
		_btnRetry = transform.Find("BtnRetry").gameObject;
		_btnContinue = transform.Find("BtnContinue").gameObject;
        _txtTimer = transform.Find("TimerText").gameObject;
		_maxAmountOfPickups = _pickups.Count;
		_playerController = _player.GetComponent<PlatformControls>();
		if (_playerController == null)
		{
			_topDownController = _player.GetComponent<TopDownControls>();
		}
		_enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(x => x.transform.parent == null).ToList();
	}
	
	// Update is called once per frame
	void Update () {
        if (!_MainMenu)
        {
            if (_shouldTimeDemise)
            {
                _countdown -= Time.deltaTime;
                _txtTimer.GetComponent<Text>().text = "Seconds remaining: " + _countdown;
                if (_countdown <= 0f)
                {
                    _topDownController._health = 0;
                }
            }
            if (_pickups != null)
            {
                List<GameObject> nullObjects = _pickups.Where(x => x == null).ToList();
                foreach (GameObject obj in nullObjects)
                {
                    _pickups.Remove(obj);
                }
            }

            if (_enemies != null)
            {
                List<GameObject> nullEnemies = _enemies.Where(x => x == null).ToList();
                foreach (GameObject obj in nullEnemies)
                {
                    _enemies.Remove(obj);
                }
            }



            if (_endReached)
            {
                if (_pickupsCollectedText.enabled)
                {
                    _pickupsCollectedText.gameObject.SetActive(false);
                    _healthText.gameObject.SetActive(false);
                    _enemiesLeftText.gameObject.SetActive(false);
                    _gameOverText.gameObject.SetActive(true);
                    string powerUpsGained;
                    if (_maxAmountOfPickups != _pickups.Count)
                    {
                        powerUpsGained = (int)(((_maxAmountOfPickups - _pickups.Count) / (decimal)_maxAmountOfPickups) * 100m) + "%";
                    }
                    else if (_maxAmountOfPickups == 0)
                    {
                        powerUpsGained = "100%";
                    }
                    else
                    {
                        powerUpsGained = "0%";
                    }
                    _gameOverText.text = "Summary\r\n" + "Power-ups gained: " + powerUpsGained + "\r\n" + "Enemies Remaining: " + _enemies.Count;
                    _btnContinue.SetActive(true);
                }
            }
            else
            {
                if (_player != null)
                {
                    if (_maxAmountOfPickups != _pickups.Count)
                    {
                        decimal result = (_maxAmountOfPickups - _pickups.Count) / (decimal)_maxAmountOfPickups;
                        _pickupsCollectedText.text = "Power-ups gained: " + (int)(result * 100m) + "%";
                    }
                    else if (_maxAmountOfPickups == 0)
                    {
                        _pickupsCollectedText.text = "Power-ups gained: " + "100%";
                    }
                    else
                    {
                        _pickupsCollectedText.text = "Power-ups gained: " + "0%";
                    }
                    if (_playerController != null)
                    {
                        _healthText.text = "Health: " + _playerController._health;
                    }
                    else
                    {
                        _healthText.text = "Health: " + _topDownController._health;
                    }

                    _enemiesLeftText.text = "Enemies Remaining: " + _enemies.Count;
                }
                else
                {
                    _pickupsCollectedText.enabled = false;
                    _healthText.enabled = false;
                    _enemiesLeftText.enabled = false;
                    _gameOverText.enabled = true;
                    _btnRetry.SetActive(true);
                    _gameOverText.gameObject.SetActive(true);
                }
            }
        }

	}
}