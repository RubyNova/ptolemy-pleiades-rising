using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FallDetectorController : MonoBehaviour 
{
	public GameObject _player;
	List<PlatformDrop> _fallingPlatforms;
	List<GameObject> _enemies;
	// Use this for initialization
	void Start () 
	{
		_fallingPlatforms = GameObject.FindObjectsOfType<PlatformDrop>().ToList();
		_enemies = GameObject.FindObjectsOfType<EnemyBehaviour> ().Select (x => x.gameObject).ToList();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (_player != null)
		{
			if (_player.transform.position.y < transform.position.y)
			{
				_player.transform.position = new Vector2(_player.GetComponent<PlatformControls>()._lastPlatform.transform.position.x, _player.GetComponent<PlatformControls>()._lastPlatform.transform.position.y + 5);
				_player.GetComponent<PlatformControls>()._health -= 100;
				foreach (PlatformDrop dropControl in _fallingPlatforms)
				{
					dropControl.Reset(_player.transform.position);
				}
			}
		}
		List<GameObject> result = _enemies.Where (x => x == null).ToList ();
		foreach (GameObject enemy in result) 
		{
			_enemies.Remove(enemy);
		}

		List<GameObject> fallenEnemies = _enemies.Where (x => x.transform.position.y < transform.position.y).ToList ();

		foreach (GameObject enemy in fallenEnemies) 
		{
			Destroy(enemy);
			_enemies.Remove(enemy);
		}
	}
}
