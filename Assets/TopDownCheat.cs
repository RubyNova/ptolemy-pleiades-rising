using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TopDownCheat : MonoBehaviour 
{
	
	private List<GameObject> _enemies;
	private List<GameObject> _deadlyTiles;
	// Use this for initialization
	void Start () 
	{
		_enemies = GameObject.FindObjectsOfType<EnemyBehaviour>().Select(x => x.gameObject).ToList();
		_deadlyTiles = GameObject.FindObjectsOfType<DeadlyPlatform>().Select(x => x.gameObject).ToList();
	}
	
	// Update is called once per frame
	void Update () 
	{
		List<GameObject> nullEnemies = _enemies.Where(x => x == null).ToList();
		foreach (GameObject enemy in nullEnemies) 
		{
			_enemies.Remove (enemy);
		}

		if (Input.GetKey(KeyCode.L)) 
		{
			foreach (GameObject enemy in _enemies) 
			{
				Destroy(enemy);
			}
			foreach (GameObject tile in _deadlyTiles) 
			{
				Destroy (tile);
			}
		}
	
	}
}
