using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

	public float _bulletSpeed;
	public bool _topdown;

	void Update()
	{
		transform.Translate(new Vector3(0, _bulletSpeed * Time.deltaTime, 0));
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			if (!_topdown)
			{
				coll.gameObject.GetComponent<PlatformControls>()._health -= 50;
			}
			else
			{
				coll.gameObject.GetComponent<TopDownControls>()._health -= 50;
			}
			Destroy(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void OnBecameInvisible()
	{
		Destroy(gameObject);
	}

}
