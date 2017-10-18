using UnityEngine;
using System.Collections;

public class HandleRangedDamage : MonoBehaviour {
	public GameObject _bladePrefab;
	public int _limit;
	public int _currentAmount = 0;
	private AudioSource _audioControl;

	// Use this for initialization
	void Start () {
		_audioControl = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FireBlade()
	{
		if (_currentAmount < _limit)
		{
			_audioControl.Play();
			Instantiate(_bladePrefab, transform.position, transform.rotation);
			_currentAmount += 1;
		}

	}
}
