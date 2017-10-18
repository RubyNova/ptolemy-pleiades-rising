using UnityEngine;
using System.Collections;

public class DeadlyPlatform : MonoBehaviour {
	private float _cooldown;
	public bool _topDown;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		_cooldown += Time.deltaTime;
	}

	void OnCollisionStay2D(Collision2D coll)
	{
		if (!_topDown)
		{
			if (coll.gameObject.tag == "Player" && _cooldown > 3f)
			{
				coll.gameObject.GetComponent<PlatformControls>()._health -= 150;
				_cooldown = 0f;
			}
		}
		else
		{
			if (coll.gameObject.tag == "Player" && _cooldown > 3f)
			{
				coll.gameObject.GetComponent<TopDownControls>()._health -= 150;
				_cooldown = 0f;
			}
		}
	}
}
